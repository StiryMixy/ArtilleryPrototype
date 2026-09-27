using UnityEngine;

namespace ArtillerySimulator
{
    public class ArtilleryDeployer : MonoBehaviour
    {
        [Header("Configurable Variables")]
        [SerializeField] public int v_target_trigger_index;
        [SerializeField] public int v_timer_threshold;
        [Header("Reference Variables")]
        [SerializeField] public int v_timer_counter = 0;

        private AnimatorHandler v_target_animator_handler;
        private ColliderReader v_target_collider_reader;
        private Terrain v_target_terrain;
        private TimeLevel v_target_time_level;
        private Timekeeper v_timekeeper;
        private bool v_deployed = false;

        void Start()
        {
            v_target_animator_handler = this.GetComponent<AnimatorHandler>();
            v_target_collider_reader = this.GetComponent<ColliderReader>();
            v_target_time_level = this.GetComponent<TimeLevel>();
            v_target_terrain = FindFirstObjectByType<Terrain>();
            v_timekeeper = FindFirstObjectByType<Timekeeper>();
        }

        void Update()
        {
            if (!v_deployed)
            {
                if (v_target_collider_reader)
                {
                    if (v_target_terrain)
                    {
                        if (v_target_collider_reader.v_collider_current_collisions_list.Contains(v_target_terrain.gameObject))
                        {
                            if (v_timekeeper.f_time_level_gate_get(v_target_time_level.v_tags_timer_level))
                            {
                                v_timer_counter += 1;
                            }
                        }
                        else
                        {
                            if (v_timer_counter > 0)
                            {
                                v_timer_counter -= 1;
                            }
                        }
                    }
                }

                if (v_timer_counter >= v_timer_threshold)
                {
                    if (v_target_animator_handler)
                    {
                        v_target_animator_handler.v_target_animator.SetTrigger(v_target_animator_handler.v_animator_trigger_string_list[v_target_trigger_index]);
                    }
                    v_deployed = true;
                    Destroy(this.GetComponent<ArtilleryDeployer>());
                }
            }
        }
    }
}
