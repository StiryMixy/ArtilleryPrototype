using TMPro;
using UnityEngine;

namespace ArtillerySimulator
{
    public class UIElevationPopulator : MonoBehaviour
    {
        private InputTaker v_input_taker_script;
        private TextMeshProUGUI v_target_text;

        void Start()
        {
            v_input_taker_script = FindFirstObjectByType<InputTaker>();
            v_target_text = this.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            float tv_elevation = (v_input_taker_script.v_input_taker_setup.v_input_taker_configurables.v_player_aim_y_target.transform.localEulerAngles.x + 360f) % 360f;
            
            v_target_text.text = (tv_elevation).ToString();
        }
    }
}