using UnityEngine;

namespace ArtillerySimulator
{
    public class UIPanelHandler : MonoBehaviour
    {
        [Header("Master Switch")]
        [SerializeField] public bool v_master_fade_switch = true;
        [Header("Target Color")]
        [SerializeField] public Color v_target_image_color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        [SerializeField] public float v_target_image_color_step = 0.01f;
        [SerializeField] public float v_target_image_color_max = 1.0f;
        [Header("Reference Alpha")]
        [SerializeField] public float v_stored_alpha = 1.0f;

        [HideInInspector] public bool v_force_disable = false;
        private UnityEngine.UI.Image v_target_panel_image;

        void Start()
        {
            v_target_panel_image = this.GetComponent<UnityEngine.UI.Image>();
        }

        void Update()
        {
            if ((v_master_fade_switch) && (!v_force_disable))
            {
                if (v_stored_alpha < v_target_image_color_max)
                {
                    v_stored_alpha = Mathf.Clamp((v_stored_alpha + v_target_image_color_step), 0.0f, v_target_image_color_max);
                    v_target_image_color = new Color(v_target_image_color.r, v_target_image_color.g, v_target_image_color.b, v_stored_alpha);
                }
                else if (v_stored_alpha > v_target_image_color_max)
                {
                    v_stored_alpha = Mathf.Clamp((v_stored_alpha - v_target_image_color_step), 0.0f, v_target_image_color_max);
                    v_target_image_color = new Color(v_target_image_color.r, v_target_image_color.g, v_target_image_color.b, v_stored_alpha);
                }
            }
            else
            {
                if (v_stored_alpha > 0.0f)
                {
                    v_stored_alpha = Mathf.Clamp((v_stored_alpha - v_target_image_color_step), 0.0f, v_target_image_color_max);
                    v_target_image_color = new Color(v_target_image_color.r, v_target_image_color.g, v_target_image_color.b, v_stored_alpha);
                }
            }

            v_target_panel_image.color = v_target_image_color;
        }
    }
}