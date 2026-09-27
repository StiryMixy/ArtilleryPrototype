using UnityEngine;
using TMPro;

namespace ArtillerySimulator
{
    public class UITextHandler : MonoBehaviour
    {
        [Header("Master Switch")]
        [SerializeField] public bool v_master_fade_switch = true;
        [Header("Target Color")]
        [SerializeField] public Color32 v_target_text_color = new Color32(255, 255, 255, 255);
        [SerializeField] public byte v_target_text_color_step = 5;
        [SerializeField] public byte v_target_text_color_max = 255;
        [Header("Reference Alpha")]
        [SerializeField] public byte v_stored_alpha = 255;

        [HideInInspector] public bool v_force_disable = false;
        private TextMeshProUGUI v_target_text;

        void Start()
        {
            v_target_text = this.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if ((v_master_fade_switch) && (!v_force_disable))
            {
                if (v_stored_alpha < v_target_text_color_max)
                {
                    v_stored_alpha = (byte)Mathf.Clamp((v_stored_alpha + v_target_text_color_step), 0, v_target_text_color_max);
                    v_target_text_color = new Color32(v_target_text_color.r, v_target_text_color.g, v_target_text_color.b, v_stored_alpha);
                }
                else if (v_stored_alpha > v_target_text_color_max)
                {
                    v_stored_alpha = (byte)Mathf.Clamp((v_stored_alpha - v_target_text_color_step), 0, v_target_text_color_max);
                    v_target_text_color = new Color32(v_target_text_color.r, v_target_text_color.g, v_target_text_color.b, v_stored_alpha);
                }
            }
            else
            {
                if (v_stored_alpha > 0)
                {
                    v_stored_alpha = (byte)Mathf.Clamp((v_stored_alpha - v_target_text_color_step), 0, v_target_text_color_max);
                    v_target_text_color = new Color32(v_target_text_color.r, v_target_text_color.g, v_target_text_color.b, v_stored_alpha);
                }
            }

            v_target_text.color = v_target_text_color;
        }
    }
}