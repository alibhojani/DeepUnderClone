﻿using UnityEngine;
using Extensions;

public class LargeBoidsFish : BoidsFish {

    private float IdleMin = 6f;
    private float IdleMax = 13f;
    private float AbsoluteMax = 20f;

    public override STATE State
	{
		get				{ return this.state; }
		protected set
		{
			if (this.state != value)
				StateTimer = 0;

			this.state = value;
			if (value == STATE.EATING)
                { this.MinSpeed = this.MaxSpeed = this.IdleMin; }
			else if (value == STATE.FLEEING)
				{ this.MinSpeed = this.MaxSpeed = this.AbsoluteMax; }
			else if (value == STATE.IDLE || value == STATE.SWIMMING)
            {
                this.state = STATE.IDLE;
                this.MinSpeed = this.IdleMin;
                this.MaxSpeed = this.IdleMax;
            }
			/*else if (value == STATE.SWIMMING)
            {
                this.MinSpeed = this.SwimMin;
                this.MaxSpeed = this.SwimMax;
            }*/
			else if (value == STATE.HUNTING)
				{ this.MinSpeed = this.MaxSpeed = this.AbsoluteMax; }
		}
	}

	protected override void Start()
    {
        base.Start();
        this.EnforceLayerMembership("Large Fish");
        this.Size = SIZE.LARGE;
	}

    protected override Vector3 CalculateVelocity()
	{
		// Handle rigidbody velocity updates
		Vector3 separation = this.VectorAwayFromNeighbours();
		Vector3 target = this.VectorTowardsTarget();
        Vector3 avoid = this.VectorAwayFromPredators();

		// Glue all the stages together
		Vector3 updatedVelocity = this.transform.forward * BoidsSettings.Instance.LargeFish_IdleMin;     // Fish is always moving a minimum speed
        updatedVelocity += target;
		updatedVelocity += separation;
        updatedVelocity += avoid;
		updatedVelocity *= BoidsSettings.Instance.FishSpeedMultiplier;
		updatedVelocity = Vector3.Slerp(this.RigidBody.velocity, updatedVelocity, 2*Time.fixedDeltaTime);

		return updatedVelocity;
	}

#if UNITY_EDITOR
    protected override void FixedUpdate()
    {
        this.State = this.State;
        this.IdleMin = BoidsSettings.Instance.LargeFish_IdleMin;
        this.IdleMax = BoidsSettings.Instance.LargeFish_IdleMax;
        // this.SwimMin = BoidsSettings.Instance.LargeFish_SwimMin;
        // this.SwimMax = BoidsSettings.Instance.LargeFish_SwimMax;
        this.AbsoluteMax = BoidsSettings.Instance.LargeFish_AbsoluteMax;
        base.FixedUpdate();
    }
#endif
}
