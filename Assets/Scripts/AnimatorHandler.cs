using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArtillerySimulator
{
    public class AnimatorHandler : MonoBehaviour
    {
        [Header("Animator Trigger List")]
        [SerializeField] public List<string> v_animator_trigger_string_list = new List<string>();
        [SerializeField] public int v_animator_trigger_string_list_target_index = 0;

        [HideInInspector] public Animator v_target_animator;

        private InputTaker v_input_taker;
        private InterfaceMaster v_interface_master;
        private BallisticMaster v_ballistic_master;

        void Start()
        {
            v_target_animator = this.GetComponent<Animator>();
            v_input_taker = this.GetComponent<InputTaker>();
            v_interface_master = FindFirstObjectByType<InterfaceMaster>();
            v_ballistic_master = FindFirstObjectByType<BallisticMaster>();
        }

        void Update()
        {
            v_animator_trigger_string_list_target_index = Mathf.Clamp(v_animator_trigger_string_list_target_index, 0, (v_animator_trigger_string_list.Count - 1));
        }

        public void f_animator_disable()
        {
            v_target_animator.enabled = false;
        }

        public void f_component_enable()
        {
            v_input_taker.enabled = true;
            v_interface_master.v_interface_master_switch = true;
            v_ballistic_master.enabled = true;
        }
    }
}
