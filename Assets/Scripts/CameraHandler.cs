using System;
using UnityEngine;

namespace ArtillerySimulator
{
    public enum venum_tags_camera_mode_list
    {
        EagleView,
        FirstPerson,
        None
    };

    [Serializable]
    public class vpc_camera_first_person_position_element_configurables
    {
        [Header("Target Position Values")]
        [SerializeField] public GameObject v_first_person_target_gameobject;
        [Header("Target Adjustment Values")]
        [SerializeField] public float v_first_person_adjust_speed = 1.0f;
        [SerializeField] public float v_first_person_rotation_lerp_speed = 1.0f;
    }

    [Serializable]
    public class vpc_camera_first_person_position_element_references
    {
        [Header("Position Check")]
        [SerializeField] public bool v_first_person_capsule_position_check = false;
        [SerializeField] public bool v_first_person_capsule_rotation_check = false;
        [SerializeField] public bool v_first_person_camera_position_check = false;
        [SerializeField] public bool v_first_person_camera_rotation_check = false;
    }

    [Serializable]
    public class vpc_camera_first_person_position_element
    {
        [Header("Configurable Variables")]
        [SerializeField] public vpc_camera_first_person_position_element_configurables v_camera_first_person_position_configurables = new vpc_camera_first_person_position_element_configurables();
        [Header("Reference Variables")]
        [SerializeField] public vpc_camera_first_person_position_element_references v_camera_first_person_position_references = new vpc_camera_first_person_position_element_references();
    }

    [Serializable]
    public class vpc_camera_eagle_view_position_element_configurables
    {
        [Header("Target Rotation Values")]
        [SerializeField] public Vector3 v_eagle_view_capsule_target_rotation = Vector3.zero;
        [SerializeField] public Vector3 v_eagle_view_camera_target_local_rotation = Vector3.zero;
        [Header("Target Adjustment Values")]
        [SerializeField] public float v_eagle_view_camera_adjust_speed = 1.0f;
        [SerializeField] public float v_eagle_view_general_rotation_lerp_speed = 1.0f;
        [SerializeField] public float v_eagle_view_camera_distance_mult = 25.0f;
    }

    [Serializable]
    public class vpc_camera_eagle_view_position_element_references
    {
        [Header("Position Storage")]
        [SerializeField] public Vector3 v_eagle_view_camera_position_target = Vector3.zero;
        [Header("Target Adjustment Values")]
        [SerializeField] public float v_eagle_view_camera_distance_mult_proxy = 0.0f;
    }

    [Serializable]
    public class vpc_camera_eagle_view_position_element
    {
        [Header("Configurable Variables")]
        [SerializeField] public vpc_camera_eagle_view_position_element_configurables v_camera_eagle_view_position_configurables = new vpc_camera_eagle_view_position_element_configurables();
        [Header("Reference Variables")]
        [SerializeField] public vpc_camera_eagle_view_position_element_references v_camera_eagle_view_position_references = new vpc_camera_eagle_view_position_element_references();
    }

    [Serializable]
    public class vpc_camera_variable_element
    {
        [Header("Configurable Variables")]
        [SerializeField] public Camera v_actual_camera;
        [SerializeField] public GameObject v_capsule;
        [SerializeField] public GameObject v_cube;
    }

    [Serializable]
    public class vpc_camera_focus_element_configurables
    {
        [Header("Focus Point")]
        [SerializeField] public GameObject v_focus_gameobject;
        [Header("Focus Values")]
        [SerializeField] public float v_focus_lerp_speed = 1.0f;
    }

    [Serializable]
    public class vpc_camera_focus_element_references
    {
        [Header("Focus Check")]
        [SerializeField] public bool v_focus_check = false;
    }

    [Serializable]
    public class vpc_camera_focus_element
    {
        [Header("Configurable Variables")]
        [SerializeField] public vpc_camera_focus_element_configurables v_camera_focus_configurables = new vpc_camera_focus_element_configurables();
        [Header("Reference Variables")]
        [SerializeField] public vpc_camera_focus_element_references v_camera_focus_references = new vpc_camera_focus_element_references();
    }

    [Serializable]
    public class vpc_camera_shell_focus_element_configurables
    {
        [Header("Shell Focus Timer")]
        [SerializeField] public float v_shell_focus_timer_max = 2.0f;
        [Header("Shell Focus Damp Time Limit")]
        [SerializeField] public float v_shell_focus_damp_smooth_time = 0.0f;
    }

    [Serializable]
    public class vpc_camera_shell_focus_element_references
    {
        [Header("Shell Focus Values")]
        [SerializeField] public bool v_shell_focus_gate = false;
        [Header("Shell Focus Point")]
        [SerializeField] public GameObject v_shell_focus_gameobject;
        [Header("Shell Focus Timer")]
        [SerializeField] public float v_shell_focus_timer = 0.0f;
        [Header("Shell Focus Damping Velocity")]
        [SerializeField] public Vector3 v_shell_focus_velocity = Vector3.zero;
    }

    [Serializable]
    public class vpc_camera_shell_focus_element
    {
        [Header("Configurable Variables")]
        [SerializeField] public vpc_camera_shell_focus_element_configurables v_camera_focus_configurables = new vpc_camera_shell_focus_element_configurables();
        [Header("Reference Variables")]
        [SerializeField] public vpc_camera_shell_focus_element_references v_camera_focus_references = new vpc_camera_shell_focus_element_references();
    }

    public class CameraHandler : MonoBehaviour
    {
        [Header("Camera Mode")]
        [SerializeField] public venum_tags_camera_mode_list v_camera_mode = venum_tags_camera_mode_list.EagleView;
        [Header("Camera Eagle View Position Configuration")]
        [SerializeField] public vpc_camera_shell_focus_element v_camera_shell_focus_setup = new vpc_camera_shell_focus_element();
        [Header("Camera Eagle View Position Configuration")]
        [SerializeField] public vpc_camera_eagle_view_position_element v_camera_eagle_view_position_setup = new vpc_camera_eagle_view_position_element();
        [Header("Camera First Person Position Configuration")]
        [SerializeField] public vpc_camera_first_person_position_element v_camera_first_person_position_setup = new vpc_camera_first_person_position_element();
        [Header("Camera Focus Setup")]
        [SerializeField] public vpc_camera_focus_element v_camera_focus_setup = new vpc_camera_focus_element();
        [Header("Camera Elements Setup")]
        [SerializeField] public vpc_camera_variable_element v_camera_elements_setup = new vpc_camera_variable_element();

        void LateUpdate()
        {
            if (v_camera_elements_setup.v_actual_camera)
            {
                if (v_camera_mode.Equals(venum_tags_camera_mode_list.EagleView))
                {
                    if ((v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gate) && (v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject))
                    {
                        if (v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_timer < v_camera_shell_focus_setup.v_camera_focus_configurables.v_shell_focus_timer_max)
                        {
                            v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_timer += Time.deltaTime;
                        }
                        else
                        {
                            v_camera_elements_setup.v_capsule.transform.position = Vector3.SmoothDamp
                                (
                                    v_camera_elements_setup.v_capsule.transform.position,
                                    v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject.transform.position,
                                    ref v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_velocity,
                                    v_camera_shell_focus_setup.v_camera_focus_configurables.v_shell_focus_damp_smooth_time
                                );

                            v_camera_elements_setup.v_actual_camera.transform.eulerAngles = Quaternion.Slerp
                                (
                                    Quaternion.Euler(v_camera_elements_setup.v_actual_camera.transform.eulerAngles),
                                    Quaternion.LookRotation(v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject.transform.position - v_camera_elements_setup.v_actual_camera.transform.position),
                                    v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_general_rotation_lerp_speed * Time.deltaTime
                                ).eulerAngles;
                        }
                    }
                    else
                    {
                        v_camera_elements_setup.v_capsule.transform.position = this.transform.position;

                        v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_timer = 0.0f;
                    }

                    f_camera_eagle_view_calculate_position(v_camera_elements_setup.v_cube.transform.parent.InverseTransformDirection(v_camera_elements_setup.v_cube.transform.forward));

                    v_camera_elements_setup.v_actual_camera.gameObject.transform.localPosition = Vector3.Lerp
                        (
                            v_camera_elements_setup.v_actual_camera.gameObject.transform.localPosition,
                            v_camera_eagle_view_position_setup.v_camera_eagle_view_position_references.v_eagle_view_camera_position_target,
                            v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_camera_adjust_speed * Time.deltaTime
                        );

                    //==//

                    v_camera_elements_setup.v_capsule.transform.eulerAngles = Quaternion.Slerp
                        (
                            Quaternion.Euler(v_camera_elements_setup.v_capsule.transform.eulerAngles),
                            Quaternion.Euler(v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_capsule_target_rotation), 
                            v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_general_rotation_lerp_speed * Time.deltaTime
                        ).eulerAngles;

                    v_camera_elements_setup.v_cube.transform.localEulerAngles = Quaternion.Slerp
                        (
                            Quaternion.Euler(v_camera_elements_setup.v_cube.transform.localEulerAngles),
                            Quaternion.Euler(v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_camera_target_local_rotation),
                            v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_general_rotation_lerp_speed * Time.deltaTime
                        ).eulerAngles;

                    if (!((v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gate) && (v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject)))
                    {
                        v_camera_elements_setup.v_actual_camera.transform.eulerAngles = Quaternion.Slerp
                            (
                                Quaternion.Euler(v_camera_elements_setup.v_actual_camera.transform.eulerAngles),
                                Quaternion.LookRotation(v_camera_elements_setup.v_cube.transform.position - v_camera_elements_setup.v_actual_camera.transform.position),
                                v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_general_rotation_lerp_speed * Time.deltaTime
                            ).eulerAngles;
                    }
                }
                else if (v_camera_mode.Equals(venum_tags_camera_mode_list.FirstPerson))
                {
                    if ((v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gate) && (v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject))
                    {
                        if (v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_timer < v_camera_shell_focus_setup.v_camera_focus_configurables.v_shell_focus_timer_max)
                        {
                            v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_timer += Time.deltaTime;
                        }
                        else
                        {
                            v_camera_elements_setup.v_capsule.transform.position = Vector3.SmoothDamp
                                (
                                    v_camera_elements_setup.v_capsule.transform.position,
                                    v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject.transform.position,
                                    ref v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_velocity,
                                    v_camera_shell_focus_setup.v_camera_focus_configurables.v_shell_focus_damp_smooth_time
                                );

                            f_camera_eagle_view_calculate_position(v_camera_elements_setup.v_cube.transform.parent.InverseTransformDirection(v_camera_elements_setup.v_cube.transform.forward));

                            v_camera_elements_setup.v_actual_camera.gameObject.transform.localPosition = Vector3.Lerp
                                (
                                    v_camera_elements_setup.v_actual_camera.gameObject.transform.localPosition,
                                    v_camera_eagle_view_position_setup.v_camera_eagle_view_position_references.v_eagle_view_camera_position_target,
                                    v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_camera_adjust_speed * Time.deltaTime
                                );

                            //==//

                            v_camera_elements_setup.v_capsule.transform.eulerAngles = Quaternion.Slerp
                                (
                                    Quaternion.Euler(v_camera_elements_setup.v_capsule.transform.eulerAngles),
                                    Quaternion.Euler(v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_capsule_target_rotation),
                                    v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_general_rotation_lerp_speed * Time.deltaTime
                                ).eulerAngles;

                            v_camera_elements_setup.v_cube.transform.localEulerAngles = Quaternion.Slerp
                                (
                                    Quaternion.Euler(v_camera_elements_setup.v_cube.transform.localEulerAngles),
                                    Quaternion.Euler(v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_camera_target_local_rotation),
                                    v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_general_rotation_lerp_speed * Time.deltaTime
                                ).eulerAngles;

                            v_camera_elements_setup.v_actual_camera.transform.eulerAngles = Quaternion.Slerp
                                (
                                    Quaternion.Euler(v_camera_elements_setup.v_actual_camera.transform.eulerAngles),
                                    Quaternion.LookRotation(v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject.transform.position - v_camera_elements_setup.v_actual_camera.transform.position),
                                    v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_general_rotation_lerp_speed * Time.deltaTime
                                ).eulerAngles;
                        }
                    }
                    else
                    {
                        v_camera_elements_setup.v_capsule.transform.position = this.transform.position;

                        v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_timer = 0.0f;

                        v_camera_elements_setup.v_actual_camera.transform.position = Vector3.Lerp
                            (
                                v_camera_elements_setup.v_actual_camera.transform.position,
                                v_camera_first_person_position_setup.v_camera_first_person_position_configurables.v_first_person_target_gameobject.transform.position,
                                v_camera_first_person_position_setup.v_camera_first_person_position_configurables.v_first_person_adjust_speed * Time.deltaTime
                            );

                        v_camera_elements_setup.v_actual_camera.transform.eulerAngles = Quaternion.Slerp
                            (
                                Quaternion.Euler(v_camera_elements_setup.v_actual_camera.transform.eulerAngles),
                                Quaternion.Euler(v_camera_first_person_position_setup.v_camera_first_person_position_configurables.v_first_person_target_gameobject.transform.rotation.eulerAngles),
                                v_camera_first_person_position_setup.v_camera_first_person_position_configurables.v_first_person_rotation_lerp_speed * Time.deltaTime
                            ).eulerAngles;
                    }
                }
            }
            if (v_camera_focus_setup.v_camera_focus_configurables.v_focus_gameobject)
            {
                transform.position = Vector3.Lerp
                    (
                        transform.position,
                        v_camera_focus_setup.v_camera_focus_configurables.v_focus_gameobject.transform.position,
                        v_camera_focus_setup.v_camera_focus_configurables.v_focus_lerp_speed * Time.deltaTime
                    );
            }
        }

        public void f_camera_eagle_view_calculate_position(Vector3 sv_target_vector)
        {
            float tv_target_mult = v_camera_eagle_view_position_setup.v_camera_eagle_view_position_configurables.v_eagle_view_camera_distance_mult;

            if (v_camera_eagle_view_position_setup.v_camera_eagle_view_position_references.v_eagle_view_camera_distance_mult_proxy > 0.0f)
            {
                tv_target_mult = v_camera_eagle_view_position_setup.v_camera_eagle_view_position_references.v_eagle_view_camera_distance_mult_proxy;
            }

            v_camera_eagle_view_position_setup.v_camera_eagle_view_position_references.v_eagle_view_camera_position_target = 
                    (sv_target_vector) 
                    * 
                    (-tv_target_mult);
        }
    }
}
