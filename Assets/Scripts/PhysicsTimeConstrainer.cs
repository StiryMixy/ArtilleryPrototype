using UnityEngine;

namespace ArtillerySimulator
{
    public class PhysicsTimeConstrainer : MonoBehaviour
    {
        private Rigidbody v_rigid_body;
        private TimeLevel v_target_time_level;
        private Timekeeper v_timekeeper;
        private Vector3 v_stored_velocity;

        void Start()
        {
            v_rigid_body = this.GetComponent<Rigidbody>();
            v_target_time_level = this.GetComponent<TimeLevel>();
            v_timekeeper = FindFirstObjectByType<Timekeeper>();
        }

        void FixedUpdate()
        {
            if (v_rigid_body)
            {
                float sv_target_rate = v_timekeeper.f_time_level_rate_get(v_target_time_level.v_tags_timer_level);
                if (sv_target_rate <= 0)
                {
                    v_stored_velocity = v_rigid_body.velocity;
                    v_rigid_body.velocity = Vector3.zero;
                    v_rigid_body.Sleep();
                }
                else
                {
                    if(v_rigid_body.IsSleeping())
                    {
                        v_rigid_body.WakeUp();
                        v_rigid_body.velocity = v_stored_velocity;
                        v_stored_velocity = Vector3.zero;
                    }
                    else
                    {
                        v_rigid_body.velocity *= Mathf.Pow(sv_target_rate, Time.fixedDeltaTime);
                    }
                }
            }
        }
    }
}