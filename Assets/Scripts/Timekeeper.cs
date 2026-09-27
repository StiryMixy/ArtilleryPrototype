using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArtillerySimulator
{
    public enum venum_tags_timer_level_list
    {
        Basic,
        Trained,
        Mutated,
        Magical,
        Omniscient,
        Metaphysical,
        None
    };

    [Serializable]
    public class vpc_time_handler_list_element_configurable
    {
        [Header("General Switch")]
        [SerializeField] public bool v_timer_enabled = true;
        [Header("Level Type")]
        [SerializeField] public venum_tags_timer_level_list v_timer_level_type;
        [Header("Rate Change Rate")]
        [SerializeField] public float v_timer_rate_increment = 0.1f;
        [Header("Rate Rules")]
        [SerializeField] public bool v_timer_rate_follow_target = true;
        [SerializeField] public bool v_timer_rate_follow_is_instant = false;
        [Header("Rate Values")]
        [SerializeField] public float v_timer_rate_target = 1.0f;
        [SerializeField] public float v_timer_rate_default = 1.0f;
        [SerializeField] public float v_timer_rate = 1.0f;
        [Header("Purge Rules")]
        [SerializeField] public bool v_timer_purge = false;
        [SerializeField] public float v_timer_purge_threshold = 0.0f;
    }

    [Serializable]
    public class vpc_time_handler_list_element_references
    {
        [Header("Actual Time")]
        [SerializeField] public float v_timer = 0.0f;
        [Header("Precise Actual Time")]
        [SerializeField] public int v_timer_precise = 0;
        [Header("Time Gate Values")]
        [SerializeField] public bool v_timer_gate = false;
        [SerializeField] public int v_timer_gate_counter = 0;
        [Header("Time Delta Values")]
        [SerializeField] public float v_timer_delta_rate = 0.0f;
    }

    [Serializable]
    public class vpc_time_handler_list_element
    {
        [Header("Configurable Variables")]
        [SerializeField] public vpc_time_handler_list_element_configurable v_timer_element_configurables;
        [Header("Reference Variables")]
        [SerializeField] public vpc_time_handler_list_element_references v_timer_element_references;
    }

    public class Timekeeper : MonoBehaviour
    {
        [Header("Timekeeper List Setup")]
        [SerializeField] public List<vpc_time_handler_list_element> v_timekeeper_list_setup = new List<vpc_time_handler_list_element>();
        [Header("Timekeeper Level Type Setup")]
        public List<venum_tags_timer_level_list> v_tags_timer_level_list = new List<venum_tags_timer_level_list>();
        [Header("Timekeeper Stop Handler Setup")]
        public bool v_time_is_stopped = false;

        void FixedUpdate()
        {
            if (v_timekeeper_list_setup.Count > 0)
            {
                for (int i = 0; i < v_timekeeper_list_setup.Count; i++)
                {
                    v_timekeeper_list_setup[i].v_timer_element_references.v_timer_delta_rate = Time.fixedDeltaTime * v_timekeeper_list_setup[i].v_timer_element_configurables.v_timer_rate;
                    f_time_level_rate_controller(i);
                    f_time_level_timer(i);
                }
            }
        }

        public bool f_time_level_gate_get(venum_tags_timer_level_list sv_time_level_type)
        {
            return v_timekeeper_list_setup[v_tags_timer_level_list.IndexOf(sv_time_level_type)].v_timer_element_references.v_timer_gate;
        }

        public float f_time_level_rate_get(venum_tags_timer_level_list sv_time_level_type)
        {
            return v_timekeeper_list_setup[v_tags_timer_level_list.IndexOf(sv_time_level_type)].v_timer_element_configurables.v_timer_rate;
        }

        public float f_time_level_delta_get(venum_tags_timer_level_list sv_time_level_type)
        {
            return v_timekeeper_list_setup[v_tags_timer_level_list.IndexOf(sv_time_level_type)].v_timer_element_references.v_timer_delta_rate;
        }

        public void f_time_level_rate_controller(int sv_time_level_index)
        {
            float tv_target_val = 0.0f;
            vpc_time_handler_list_element tv_reference_index = v_timekeeper_list_setup[sv_time_level_index];
            if (tv_reference_index.v_timer_element_configurables.v_timer_rate_follow_target)
            {
                tv_target_val = tv_reference_index.v_timer_element_configurables.v_timer_rate_target;
            }
            else
            {
                tv_target_val = tv_reference_index.v_timer_element_configurables.v_timer_rate_default;
            }

            if (tv_reference_index.v_timer_element_configurables.v_timer_rate_follow_is_instant)
            {
                if (tv_reference_index.v_timer_element_configurables.v_timer_rate != tv_target_val)
                {
                    tv_reference_index.v_timer_element_configurables.v_timer_rate = tv_target_val;
                }
            }
            else
            {
                if (tv_reference_index.v_timer_element_configurables.v_timer_rate != tv_target_val)
                {
                    if (tv_reference_index.v_timer_element_configurables.v_timer_rate > tv_target_val)
                    {
                        if ((tv_reference_index.v_timer_element_configurables.v_timer_rate - tv_reference_index.v_timer_element_configurables.v_timer_rate_increment) < tv_target_val)
                        {
                            tv_reference_index.v_timer_element_configurables.v_timer_rate = tv_target_val;
                        }
                        else
                        {
                            tv_reference_index.v_timer_element_configurables.v_timer_rate -= tv_reference_index.v_timer_element_configurables.v_timer_rate_increment;
                        }
                    }
                    else if (tv_reference_index.v_timer_element_configurables.v_timer_rate < tv_target_val)
                    {
                        if ((tv_reference_index.v_timer_element_configurables.v_timer_rate + tv_reference_index.v_timer_element_configurables.v_timer_rate_increment) > tv_target_val)
                        {
                            tv_reference_index.v_timer_element_configurables.v_timer_rate = tv_target_val;
                        }
                        else
                        {
                            tv_reference_index.v_timer_element_configurables.v_timer_rate += tv_reference_index.v_timer_element_configurables.v_timer_rate_increment;
                        }
                    }
                }
            }

            if (tv_reference_index.v_timer_element_configurables.v_timer_purge)
            {
                if (tv_reference_index.v_timer_element_references.v_timer > tv_reference_index.v_timer_element_configurables.v_timer_purge_threshold)
                {
                    tv_reference_index.v_timer_element_references.v_timer = 0.0f;
                }
                if (tv_reference_index.v_timer_element_references.v_timer_precise > (int)tv_reference_index.v_timer_element_configurables.v_timer_purge_threshold)
                {
                    tv_reference_index.v_timer_element_references.v_timer_precise = 0;
                }
                if (tv_reference_index.v_timer_element_references.v_timer_gate_counter > (int)tv_reference_index.v_timer_element_configurables.v_timer_purge_threshold)
                {
                    tv_reference_index.v_timer_element_references.v_timer_gate_counter = 0;
                }
            }

            if (v_time_is_stopped)
            {
                tv_reference_index.v_timer_element_configurables.v_timer_rate = 0.0f;
            }
        }

        public void f_time_level_timer(int sv_time_level_index)
        {
            vpc_time_handler_list_element tv_reference_index = v_timekeeper_list_setup[sv_time_level_index];
            if (tv_reference_index.v_timer_element_configurables.v_timer_enabled)
            {
                if (!v_time_is_stopped)
                {
                    tv_reference_index.v_timer_element_references.v_timer += tv_reference_index.v_timer_element_references.v_timer_delta_rate;
                }

                if ((int)tv_reference_index.v_timer_element_references.v_timer != tv_reference_index.v_timer_element_references.v_timer_precise)
                {
                    tv_reference_index.v_timer_element_references.v_timer_gate = true;
                    tv_reference_index.v_timer_element_references.v_timer_precise = (int)tv_reference_index.v_timer_element_references.v_timer;
                }
                else
                {
                    tv_reference_index.v_timer_element_references.v_timer_gate = false;
                }

                if (tv_reference_index.v_timer_element_references.v_timer_gate)
                {
                    tv_reference_index.v_timer_element_references.v_timer_gate_counter += 1;
                }
            }
            else
            {
                tv_reference_index.v_timer_element_references.v_timer_gate = false;
            }
        }
    }
}
