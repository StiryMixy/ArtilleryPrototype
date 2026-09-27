using UnityEngine;
using TMPro;

namespace ArtillerySimulator
{
    public class UITextCameraModePopulator : MonoBehaviour
    {
        [Header("Desired Strings")]
        [SerializeField] public string v_populator_shell_exist;
        [SerializeField] public string v_populator_first_person;
        [SerializeField] public string v_populator_eagle_view;

        private CameraHandler v_camera_handler;
        private TextMeshProUGUI v_target_text;

        void Start()
        {
            v_camera_handler = FindFirstObjectByType<CameraHandler>();
            v_target_text = this.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (v_camera_handler)
            {
                if ((v_camera_handler.v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gate) && (v_camera_handler.v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject))
                {
                    v_target_text.text = v_populator_shell_exist;
                }
                else if (v_camera_handler.v_camera_mode.Equals(venum_tags_camera_mode_list.FirstPerson))
                {
                    v_target_text.text = v_populator_first_person;
                }
                else if (v_camera_handler.v_camera_mode.Equals(venum_tags_camera_mode_list.EagleView))
                {
                    v_target_text.text = v_populator_eagle_view;
                }
                else
                {
                    v_target_text.text = "null";
                }
            }
        }
    }
}