using UnityEngine;

namespace ArtillerySimulator
{
    public class InterfaceMaster : MonoBehaviour
    {
        [Header("Master Switch")]
        [SerializeField] public bool v_interface_master_switch;
        [Header("Detected Elements")]
        [SerializeField] public UIPanelHandler[] v_target_panel_list;
        [SerializeField] public UITextHandler[] v_target_text_list;

        void Start()
        {
            v_target_panel_list = GetComponentsInChildren<UIPanelHandler>();
            v_target_text_list = GetComponentsInChildren<UITextHandler>();
        }

        void Update()
        {
            if (v_interface_master_switch)
            {
                foreach (UIPanelHandler sv_target_panel in v_target_panel_list)
                {
                    sv_target_panel.v_force_disable = false;
                }
                foreach (UITextHandler sv_target_text in v_target_text_list)
                {
                    sv_target_text.v_force_disable = false;
                }
            }
            else
            {
                foreach (UIPanelHandler sv_target_panel in v_target_panel_list)
                {
                    sv_target_panel.v_force_disable = true;
                }
                foreach (UITextHandler sv_target_text in v_target_text_list)
                {
                    sv_target_text.v_force_disable = true;
                }
            }
        }
    }
}
