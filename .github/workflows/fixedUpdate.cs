protected void GroundedLogic()
        {
            // prime a tempVelocity value with Vector3.zero - Mind this tempVelocity is unique to the grounded scope
            // If Levi/Petra or attack1/attack2 (basic attack) - this.baseR.AddForce(this.baseG.transform.forward * 200f); otherwise zero out a temp velocity (mikasa)

            // If just grounded
            // And not attacking
            // If directional inputs are both 0 and no hooks active, and not refilling
            // State = Land and crossfade dash_land
            // Else
            // release attack
            // And if not attacking, not refilling, and RVel.x^2 + RVel.z^2 > RunSpeed^2 * 1.5
            // Slide,
            //this.facingDirection = Mathf.Atan2(this.baseR.velocity.x, this.baseR.velocity.z) * 57.29578f;
            //this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);

            // If just grounded - continued
            // set justgrounded to false and the tempVelocity to the RVelocity

            // If we are in state ground dodge, set tempvelocity value to -this.baseG.transform.forward * 2.4f * RunSpeed if animation time is between 0.2 and 0.8, otherwise 0.9f

            // If we are in state idle, handle logic for idle (can start walking from this)

            // If we are in state land, tempVelocity = RVelocity * 0.96f

            // If we are in state slide, tempVelocity = RVelocity * 0.99f

            // After all that, still in grounded scope, find the velocity change of the horizontal plane (ignores vertical since grounded)
            // clamp to maxVelocityChange
            // Handle jump
            // Handle horse geton
            // if not attacking or not using a gun, add the tempVelocity (clamped) to the rigidbody with mode velocity change
            // Also set the R rotation to be Lerp of T rotation towards Quaternion.Euler(0f, this.facingDirection, 0f) with smoothing 10
        }

        protected void NonGroundedLogic()
        {
            // If we're in state IDLE and fuck you:
            // !this.baseA.IsPlaying("dash") && !this.baseA.IsPlaying("wallrun") && !this.baseA.IsPlaying("toRoof") && !this.baseA.IsPlaying("horse_geton")
            // && !this.baseA.IsPlaying("horse_getoff") && !this.baseA.IsPlaying("air_release") && !this.isMounted && (!this.baseA.IsPlaying("air_hook_l_just")
            // || this.baseA["air_hook_l_just"].normalizedTime >= 1f) && (!this.baseA.IsPlaying("air_hook_r_just") || this.baseA["air_hook_r_just"].normalizedTime >= 1f))
            // || this.baseA["dash"].normalizedTime >= 0.99f
            // ... if (!this.isLeftHandHooked && !this.isRightHandHooked && (this.baseA.IsPlaying("air_hook_l") || this.baseA.IsPlaying("air_hook_r")
            // || this.baseA.IsPlaying("air_hook")) && this.baseR.velocity.y > 20f)
            // ... play air release
            // else
            // ... else if we're too slow and vertical velocity is negative, play airfall, otherwise play air_rise
            // ... else if we're not hooked, based on the horizontal velocity's angle and the y rotation, play a different animation depending on the difference of the two.
            //  ... x < 45, air2
            //  ... 0 < x < 135f, air2_right
            //  ... -135 < x < 0, air2_left
            //  ... else air2_backward
            // if we're using a gun, play different animations based on what hand is hooked
            // if not, if right hand is not hooked, play air_hook_l
            // if left hand is not hooked, play air_hook_r
            // if both hooked, play air_hook


            // Finally free from the idle state check
            // if we're playing air_rise, based on tthe animation time and state, change animation

            //Handle to roof logic which includes a check for walljump

            // If we're idle and holding input towards hero, and not a bunch of garbage, start wallrun animation and set wallruntime to zero
            // Garbage: else if (this.state == HERO_STATE.Idle && this.isPressDirectionTowardsHero(sidewaysDir, forwardDir)
            // && !FengCustomInputs.Inputs.isInput[InputCode.jump] && !FengCustomInputs.Inputs.isInput[InputCode.leftRope] && !FengCustomInputs.Inputs.isInput[InputCode.rightRope]
            // && !FengCustomInputs.Inputs.isInput[InputCode.bothRope] && this.IsFrontGrounded() && !this.baseA.IsPlaying("wallrun") && !this.baseA.IsPlaying("dodge")

            // else if we're playing wallrun, add force upwards and keep track of time,  if time > 1 or we let go of directional keys, jump off.

            //IsFrontGrounded - This seems interesting to check - to roof and airfall

            // If we're not using a special, dashing, jumping, or firing TS

            // Get the global facing direction from our input keys
            // calculate the global facing vector based on the getGlobalFacingDirection and getGlobaleFacingVector3
            // multiply this by the clampedInputAngle and the ACL stat * 1f / 10f * 2f -> figure out order of operands

            // If we're not holding a direction and we attack, reset the globalFacingVector to zero and if we were not attacking, set horizontalAngleDirection to -874f as a flag

            // if the state wasnt attack, set facing direction equal to horizontalAngleDirection and set targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);

            /// HERES THE PART THAT BROKE :^) !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // if neither left is hooked and snagged or right is hooked and snagged
            // and if directional input is given, this.baseR.AddForce(globalFacingVector3, ForceMode.Acceleration);
            // but if no directional input is given, this.baseR.AddForce(this.baseT.forward * globalFacingVector3.magnitude, ForceMode.Acceleration);
            // regardless of that, based on the first if here, set jumpHookedBothAndSnagged to true

        }

        protected void OtherDifferences()
        {
            // Dash check and application is in Update (seems bad)

            // Attack animation picked is in update

        }

        protected void FixedUpdateFixed()
        {
            // Body lean

            // Set current speed

            // If using a gun, lerp T rotation towards shot with smoothing 30

            // Otherwise if not using a special (honestly maybe we want this for some) set R rotation to Lerp of T rotation towards target rotation with smoothing 6

            // If grabbed, AddForce to cancel out velocity with VelocityChange

            // Set Grounded and JustGrounded values

            // If we hook someone and we're 2 units away, AddForce(targetToMe.normalized * Mathf.Pow(targetToMe.magnitude, 0.15f) * 30f - this.baseR.velocity * 0.95f, ForceMode.VelocityChange);

            // If we're hooked, this.baseR.AddForce(hookerToMe.normalized * Mathf.Pow(hookerToMe.magnitude, 0.15f) * 0.2f, ForceMode.Impulse);

            // Read directional inputs (sideways = -1,0,1) and (forward = -1, 0, 1)

            // Handle hook logic (use gas if held, otherwise clear hook if hook launch time is up)
            // Also set bool reel snag values for left, right, and both hooks - has weird AddForce logic here.

            // If Grounded
            GroundedLogic();

            // else
            NonGroundedLogic();

            // For both

            // If we're hooked (at all), compute average hooked position to T position
            // Store speedPriorPlusSome = speed + 0.1f
            // this.baseR.AddForce(-this.baseR.velocity, ForceMode.VelocityChange); zero out velocity
            // Read reel direction (1, 0, -1) for 1 being reelout, 0 none, -1 reeling in
            // Clamp reel direction to 0.2f to 1.8f

            // Vector3 absoluteBullshitReelCode = Vector3.RotateTowards(avgHookToMe, this.baseR.velocity, 1.53938f * reelDirection, 1.53938f * reelDirection);

            // this.baseR.velocity = absoluteBullshitReelCode * speedPriorPlusSome;

            // Handle levi special logic

            // if there is a hook with the position.y above the player, this.baseR.AddForce(new Vector3(0f, -10f * this.baseR.mass, 0f));
            // else this.baseR.AddForce(new Vector3(0f, -this.gravity * this.baseR.mass, 0f));

            // Update fov based on speed
        }
