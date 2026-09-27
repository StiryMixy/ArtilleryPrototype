using TMPro;
using UnityEngine;

namespace ArtillerySimulator
{
    public class UIDistancePopulator : MonoBehaviour
    {
        private BallisticMaster v_ballistic_master_script;
        private InputTaker v_input_taker_script;
        private TextMeshProUGUI v_target_text;

        void Start()
        {
            v_ballistic_master_script = FindFirstObjectByType<BallisticMaster>();
            v_input_taker_script = FindFirstObjectByType<InputTaker>();
            v_target_text = this.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (v_input_taker_script.v_local_vertical_aim_angle < 90.0f)
            {
                v_target_text.text = (Vector3.Distance(Vector3.zero, v_ballistic_master_script.v_target_gameobject_end_transform.position)).ToString();
            }
            else
            {
                v_target_text.text = "What are you doing!?";
            }
        }
    }
}