using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArtillerySimulator
{
    [Serializable]
    public class vpc_input_taker_element_configurables
    {
        [Header("Input Actions")]
        [SerializeField] public InputAction v_player_first_person_aim;
        [SerializeField] public InputAction v_player_eagle_view_aim;
        [SerializeField] public InputAction v_player_camera_pan;
        [SerializeField] public InputAction v_player_camera_pan_mode;
        [SerializeField] public InputAction v_player_camera_switch;
        [SerializeField] public InputAction v_player_fire_shell;
        [SerializeField] public InputAction v_ui_toggle_switch;
        [Header("Target Game Objects")]
        [SerializeField] public GameObject v_player_aim_x_target;
        [SerializeField] public GameObject v_player_aim_y_target;
        [Header("First Person Adjustment Values")]
        [SerializeField] public float v_player_first_person_aim_clamp_x = 12.0f;
        [SerializeField] public float v_player_first_person_aim_clamp_y = 16.0f;
        [SerializeField] public float v_player_first_person_aim_horizontal_sensitivity = 0.03f;
        [SerializeField] public float v_player_first_person_aim_vertical_sensitivity = 0.015f;
        [Header("Eagle View Adjustment Values")]
        [SerializeField] public float v_player_eagle_view_aim_horizontal_sensitivity = 0.12f;
        [SerializeField] public float v_player_eagle_view_aim_vertical_sensitivity = 0.08f;
        [Header("Camera Pan Adjustment Values")]
        [SerializeField] public float v_player_camera_pan_horizontal_sensitivity = 0.025f;
        [SerializeField] public float v_player_camera_pan_vertical_sensitivity = 0.075f;
        [SerializeField] public float v_player_camera_pan_horizontal_sensitivity_shell = 0.075f;
        [SerializeField] public float v_player_camera_pan_vertical_sensitivity_shell = 0.225f;
    }

    [Serializable]
    public class vpc_input_taker_element_references
    {
        [Header("Raw Delta Values")]
        [SerializeField] public Vector2 v_player_aim_delta = Vector2.zero;
        [SerializeField] public Vector2 v_player_pan_delta = Vector2.zero;
    }

    [Serializable]
    public class vpc_input_taker_element
    {
        [Header("Configurable Variables")]
        [SerializeField] public vpc_input_taker_element_configurables v_input_taker_configurables = new vpc_input_taker_element_configurables();
        [Header("Reference Variables")]
        [SerializeField] public vpc_input_taker_element_references v_input_taker_references = new vpc_input_taker_element_references();
    }

    public class InputTaker : MonoBehaviour
    {
        [Header("Input Setup")]
        [SerializeField] public vpc_input_taker_element v_input_taker_setup;

        private CameraHandler v_camera_handler;
        private InterfaceMaster v_interface_master_script;
        private AmmoDictionary v_ammo_dictionary_script;
        private BallisticMaster v_ballistic_master_script;
        private float v_local_horizontal_pan_angle = 0.0f;
        private float v_local_vertical_pan_angle = 0.0f;
        [HideInInspector] public float v_local_vertical_aim_angle = 0.0f;
        [HideInInspector] public bool v_has_existing_shell = false;

        void Awake()
        {
            v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim = InputSystem.actions.FindAction("FirstPersonAim");
            v_input_taker_setup.v_input_taker_configurables.v_player_eagle_view_aim = InputSystem.actions.FindAction("EagleViewAim");
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan = InputSystem.actions.FindAction("CameraPan");
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan_mode = InputSystem.actions.FindAction("CameraPanMode");
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_switch = InputSystem.actions.FindAction("CameraSwitch");
            v_input_taker_setup.v_input_taker_configurables.v_player_fire_shell = InputSystem.actions.FindAction("FireShell");
            v_input_taker_setup.v_input_taker_configurables.v_ui_toggle_switch = InputSystem.actions.FindAction("ToggleMasterUI");
        }

        void Start()
        {
            v_camera_handler = FindFirstObjectByType<CameraHandler>();
            v_interface_master_script = FindFirstObjectByType<InterfaceMaster>();
            v_ammo_dictionary_script = FindFirstObjectByType<AmmoDictionary>();
            v_ballistic_master_script = FindFirstObjectByType<BallisticMaster>();

            v_local_vertical_aim_angle = v_input_taker_setup.v_input_taker_configurables.v_player_aim_y_target.transform.localEulerAngles.x;
        }

        private void OnEnable()
        {
            v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim.Enable();
            v_input_taker_setup.v_input_taker_configurables.v_player_eagle_view_aim.Enable();
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan.Enable();
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan_mode.Enable();
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_switch.Enable();
            v_input_taker_setup.v_input_taker_configurables.v_player_fire_shell.Enable();
            v_input_taker_setup.v_input_taker_configurables.v_ui_toggle_switch.Enable();
        }

        private void OnDisable()
        {
            v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim.Disable();
            v_input_taker_setup.v_input_taker_configurables.v_player_eagle_view_aim.Disable();
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan.Disable();
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan_mode.Disable();
            v_input_taker_setup.v_input_taker_configurables.v_player_camera_switch.Disable();
            v_input_taker_setup.v_input_taker_configurables.v_player_fire_shell.Disable();
            v_input_taker_setup.v_input_taker_configurables.v_ui_toggle_switch.Disable();
        }

        void Update()
        {
            if (v_camera_handler)
            {
                if (v_input_taker_setup.v_input_taker_configurables.v_player_camera_switch.triggered)
                {
                    if (v_camera_handler.v_camera_mode.Equals(venum_tags_camera_mode_list.FirstPerson))
                    {
                        v_camera_handler.v_camera_mode = venum_tags_camera_mode_list.EagleView;
                    }
                    else if (v_camera_handler.v_camera_mode.Equals(venum_tags_camera_mode_list.EagleView))
                    {
                        v_camera_handler.v_camera_mode = venum_tags_camera_mode_list.FirstPerson;
                    }
                }

                //==//

                if (v_input_taker_setup.v_input_taker_configurables.v_ui_toggle_switch.triggered)
                {
                    v_interface_master_script.v_interface_master_switch = !v_interface_master_script.v_interface_master_switch;
                }

                //==//

                if ((v_input_taker_setup.v_input_taker_configurables.v_player_fire_shell.triggered) && (!v_has_existing_shell))
                {
                    GameObject tv_fired_shell = Instantiate(v_ammo_dictionary_script.v_ammo_type_active.v_ammo_type_prefab, v_ballistic_master_script.gameObject.transform.position, Quaternion.identity);
                    ShellTranscriptor tv_shell_transcriptor_script = tv_fired_shell.GetComponent<ShellTranscriptor>();
                    if (tv_shell_transcriptor_script)
                    {
                        tv_shell_transcriptor_script.f_transcript_physics_values(v_ammo_dictionary_script.v_ammo_type_active);
                        Rigidbody tv_shell_rigidbody = tv_fired_shell.GetComponent<Rigidbody>();
                        if (tv_shell_rigidbody)
                        {
                            tv_shell_rigidbody.AddForce(v_ballistic_master_script.gameObject.transform.forward * v_ammo_dictionary_script.v_ammo_type_active.v_ammo_type_launch_force, ForceMode.Impulse);
                        }
                    }
                }

                //==//

                float tv_target_horizontal_sensitivity = 1.0f;
                float tv_target_vertical_sensitivity = 1.0f;
                float tv_target_read_x = 0.0f;
                float tv_target_read_y = 0.0f;

                if (v_camera_handler.v_camera_mode.Equals(venum_tags_camera_mode_list.FirstPerson))
                {
                    v_input_taker_setup.v_input_taker_references.v_player_aim_delta = v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim.ReadValue<Vector2>();
                    tv_target_horizontal_sensitivity = v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim_horizontal_sensitivity;
                    tv_target_vertical_sensitivity = v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim_vertical_sensitivity;
                    tv_target_read_x = Mathf.Clamp(v_input_taker_setup.v_input_taker_references.v_player_aim_delta.x, -v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim_clamp_x, v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim_clamp_x);
                    tv_target_read_y = Mathf.Clamp(v_input_taker_setup.v_input_taker_references.v_player_aim_delta.y, -v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim_clamp_y, v_input_taker_setup.v_input_taker_configurables.v_player_first_person_aim_clamp_y);
                }
                else if (v_camera_handler.v_camera_mode.Equals(venum_tags_camera_mode_list.EagleView))
                {
                    v_input_taker_setup.v_input_taker_references.v_player_aim_delta = v_input_taker_setup.v_input_taker_configurables.v_player_eagle_view_aim.ReadValue<Vector2>();
                    tv_target_horizontal_sensitivity = v_input_taker_setup.v_input_taker_configurables.v_player_eagle_view_aim_horizontal_sensitivity;
                    tv_target_vertical_sensitivity = v_input_taker_setup.v_input_taker_configurables.v_player_eagle_view_aim_vertical_sensitivity;
                    tv_target_read_x = v_input_taker_setup.v_input_taker_references.v_player_aim_delta.x;
                    tv_target_read_y = v_input_taker_setup.v_input_taker_references.v_player_aim_delta.y;
                }

                if ((v_camera_handler.v_camera_mode.Equals(venum_tags_camera_mode_list.EagleView)) || (v_has_existing_shell))
                {
                    float tv_target_horizontal_pan_sensitivity = v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan_horizontal_sensitivity;
                    float tv_target_vertical_pan_sensitivity = v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan_vertical_sensitivity;

                    if (v_has_existing_shell)
                    {
                        tv_target_horizontal_pan_sensitivity = v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan_horizontal_sensitivity_shell;
                        tv_target_vertical_pan_sensitivity = v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan_vertical_sensitivity_shell;
                    }

                    if (v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan_mode.IsPressed())
                    {
                        v_input_taker_setup.v_input_taker_references.v_player_pan_delta = v_input_taker_setup.v_input_taker_configurables.v_player_camera_pan.ReadValue<Vector2>();

                        v_local_horizontal_pan_angle = v_camera_handler.v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_capsule_target_rotation.y;
                        v_local_horizontal_pan_angle += (-v_input_taker_setup.v_input_taker_references.v_player_pan_delta.x * tv_target_horizontal_pan_sensitivity);

                        v_local_vertical_pan_angle = v_camera_handler.v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_camera_target_local_rotation.x;
                        v_local_vertical_pan_angle += (-v_input_taker_setup.v_input_taker_references.v_player_pan_delta.y * tv_target_vertical_pan_sensitivity);
                        v_local_vertical_pan_angle = Mathf.Clamp(v_local_vertical_pan_angle, 10.0f, 90.0f);

                        v_camera_handler.v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_capsule_target_rotation.y = v_local_horizontal_pan_angle;
                        v_camera_handler.v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_camera_target_local_rotation.x = v_local_vertical_pan_angle;
                    }
                }

                if (!v_has_existing_shell)
                {
                    v_input_taker_setup.v_input_taker_configurables.v_player_aim_x_target.transform.localRotation *= Quaternion.Euler
                        (
                            0,
                            (tv_target_read_x * tv_target_horizontal_sensitivity),
                            0
                        );

                    v_local_vertical_aim_angle += (tv_target_read_y * tv_target_vertical_sensitivity);
                    v_local_vertical_aim_angle = Mathf.Clamp(v_local_vertical_aim_angle, 27.5f, 152.5f);

                    v_input_taker_setup.v_input_taker_configurables.v_player_aim_y_target.transform.localRotation = Quaternion.Euler
                        (
                            v_local_vertical_aim_angle,
                            0,
                            -90
                        );
                }
            }
        }
    }
}
