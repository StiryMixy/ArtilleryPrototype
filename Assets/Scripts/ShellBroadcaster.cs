using UnityEngine;

namespace ArtillerySimulator
{
    public class ShellBroadcaster : MonoBehaviour
    {
        [Header("Destruction Timer Setup")]
        [SerializeField] public float v_destruction_timer_max;
        [Header("Camera Zoom Proxy")]
        [SerializeField] public float v_camera_zoom_proxy_adjustment;
        [Header("Explosion Adjustments")]
        [SerializeField] public float v_explosion_force;
        [SerializeField] public float v_explosion_up_modifier;

        private InputTaker v_input_taker_script;
        private CameraHandler v_camera_handler_script;
        private ColliderReader v_collider_reader_script;
        private PhysicsTimeConstrainer v_physics_time_constrainer_script;
        private Rigidbody v_rigid_body;
        private MeshRenderer[] v_mesh_renderers;
        private bool v_initiate_desctruction_gate = false;
        private float v_destruction_timer = 0.0f;
        private Collider[] v_detected_colliders;

        [HideInInspector] public GameObject v_target_explosion;
        [HideInInspector] public float v_explosion_size = 100.0f;

        void Start()
        {
            v_input_taker_script = FindFirstObjectByType<InputTaker>();
            v_camera_handler_script = FindFirstObjectByType<CameraHandler>();

            v_collider_reader_script = this.GetComponent<ColliderReader>();
            v_physics_time_constrainer_script = this.GetComponent<PhysicsTimeConstrainer>();

            v_rigid_body = this.GetComponent<Rigidbody>();
            v_mesh_renderers = this.GetComponentsInChildren<MeshRenderer>();

            v_input_taker_script.v_has_existing_shell = true;
            v_camera_handler_script.v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gate = true;
            v_camera_handler_script.v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject = this.gameObject;
        }

        void Update()
        {
            if (v_collider_reader_script.v_collider_current_collisions_list.Count > 0)
            {
                if (!v_initiate_desctruction_gate)
                {
                    v_detected_colliders = Physics.OverlapSphere(transform.position, v_explosion_size);

                    foreach (Collider tv_confirmed_hit in v_detected_colliders)
                    {
                        Rigidbody tv_rigid_body = tv_confirmed_hit.GetComponent<Rigidbody>();
                        if (tv_rigid_body != null)
                        {
                            tv_rigid_body.constraints = RigidbodyConstraints.None;
                            tv_rigid_body.AddExplosionForce(v_explosion_force, transform.position, v_explosion_size, v_explosion_up_modifier, ForceMode.Impulse);
                        }
                    }

                    v_physics_time_constrainer_script.enabled = false;

                    v_rigid_body.velocity = Vector3.zero;
                    v_rigid_body.Sleep();
                    v_rigid_body.isKinematic = true;

                    foreach (MeshRenderer sv_target_mesh_renderer in v_mesh_renderers)
                    {
                        sv_target_mesh_renderer.enabled = false;
                    }

                    v_camera_handler_script.v_camera_eagle_view_position_setup.v_camera_eagle_view_position_references.v_eagle_view_camera_distance_mult_proxy = v_camera_zoom_proxy_adjustment;

                    if (v_target_explosion)
                    {
                        Instantiate(v_target_explosion, this.transform);
                    }

                    v_initiate_desctruction_gate = true;
                }
            }

            if (v_initiate_desctruction_gate)
            {
                if (v_destruction_timer < v_destruction_timer_max)
                {
                    v_destruction_timer += Time.deltaTime;
                }
                else
                {
                    v_camera_handler_script.v_camera_mode = venum_tags_camera_mode_list.EagleView;

                    v_input_taker_script.v_has_existing_shell = false;
                    v_camera_handler_script.v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gate = false;
                    v_camera_handler_script.v_camera_shell_focus_setup.v_camera_focus_references.v_shell_focus_gameobject = null;
                    v_camera_handler_script.v_camera_eagle_view_position_setup.v_camera_eagle_view_position_references.v_eagle_view_camera_distance_mult_proxy = 0.0f;

                    foreach (Collider tv_confirmed_hit in v_detected_colliders)
                    {
                        if ((tv_confirmed_hit.gameObject.layer != 3) && (tv_confirmed_hit.gameObject.layer != 6) && (tv_confirmed_hit.gameObject.layer != 7))
                        {
                            Destroy(tv_confirmed_hit.gameObject, 1.0f);
                        }
                    }

                    Destroy(this.gameObject);
                }
            }
        }
    }
}