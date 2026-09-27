using System;
using UnityEngine;

namespace ArtillerySimulator
{
    public struct vs_projectile_properties
    {
        public Vector3 v_direction;
        public Vector3 v_initial_position;
        public float v_launch_force;
        public float v_mass;
        public float v_drag;
    }

    public class BallisticMaster : MonoBehaviour
    {
        [Header("Configurable Variables")]
        [Tooltip("The marker will show where the projectile will hit.")]
        [SerializeField] public Transform v_target_gameobject_end_transform;
        [Range(10, 10000), Tooltip("The maximum number of points the LineRenderer can have.")]
        [SerializeField] public int v_max_points = 50;
        [Range(0.01f, 0.5f), Tooltip("The time increment used to calculate the trajectory.")]
        [SerializeField] public float v_time_increment = 0.025f;
        [Range(1.05f, 2f), Tooltip("The raycast overlap between points in the trajectory, this is a multiplier of the length between points. 2 = twice as long.")]
        [SerializeField] public float v_ray_overlap = 1.1f;

        private vs_projectile_properties v_target_projectile_property = new vs_projectile_properties();
        private AmmoDictionary v_ammo_dictionary_script;
        private LineRenderer v_trajectory_line;

        private void Start()
        {
            v_ammo_dictionary_script = FindFirstObjectByType<AmmoDictionary>();
            v_trajectory_line = this.GetComponent<LineRenderer>();

            f_set_trajectory_visible(true);
        }

        private void Update()
        {
            v_target_projectile_property.v_direction = this.transform.forward;
            v_target_projectile_property.v_initial_position = this.transform.position;
            v_target_projectile_property.v_launch_force = v_ammo_dictionary_script.v_ammo_type_active.v_ammo_type_launch_force;
            v_target_projectile_property.v_mass = v_ammo_dictionary_script.v_ammo_type_active.v_ammo_type_mass;
            v_target_projectile_property.v_drag = v_ammo_dictionary_script.v_ammo_type_active.v_ammo_type_drag;

            f_predict_trajectory(v_target_projectile_property);
        }

        public void f_predict_trajectory(vs_projectile_properties sv_target_projectile)
        {
            Vector3 tv_velocity = sv_target_projectile.v_direction * (sv_target_projectile.v_launch_force / sv_target_projectile.v_mass);
            Vector3 tv_position = sv_target_projectile.v_initial_position;
            Vector3 tv_next_position;
            float tv_overlap = 0.0f;

            f_update_line_render(v_max_points, (0, tv_position));

            for (int i = 1; i < v_max_points; i++)
            {
                // Estimate velocity and update next predicted position.
                tv_velocity = f_calculate_new_velocity(tv_velocity, sv_target_projectile.v_drag, v_time_increment);
                tv_next_position = tv_position + tv_velocity * v_time_increment;

                // Overlap our rays by small margin to ensure we never miss a surface.
                tv_overlap = Vector3.Distance(tv_position, tv_next_position) * v_ray_overlap;

                // When hitting a surface we want to show the surface marker and stop updating our line.
                if (Physics.Raycast(tv_position, tv_velocity.normalized, out RaycastHit tv_hit, tv_overlap))
                {
                    if (tv_hit.transform.gameObject.layer == 3)
                    {
                        f_update_line_render(i, (i - 1, tv_hit.point));
                        f_move_hit_marker(tv_hit);
                        break;
                    }
                }

                //If nothing is hit, continue rendering the arc without a visual marker
                v_target_gameobject_end_transform.gameObject.SetActive(false);
                tv_position = tv_next_position;
                f_update_line_render(v_max_points, (i, tv_position)); //Unneccesary to set count here, but not harmful
            }
        }

        /// <summary>
        /// Allows us to set line count and an individual position at the same time.
        /// </summary>
        /// <param name="sv_count">Number of points in our line.</param>
        /// <param name="pointPos">The position of an individual point.</param>
        private void f_update_line_render(int sv_count, (int sv_point, Vector3 sv_pos) pointPos)
        {
            v_trajectory_line.positionCount = sv_count;
            v_trajectory_line.SetPosition(pointPos.sv_point, pointPos.sv_pos);
        }

        private Vector3 f_calculate_new_velocity(Vector3 sv_velocity, float sv_drag, float sv_increment)
        {
            sv_velocity += Physics.gravity * sv_increment;
            sv_velocity *= Mathf.Clamp01(1f - sv_drag * sv_increment);
            return sv_velocity;
        }

        private void f_move_hit_marker(RaycastHit sv_hit)
        {
            v_target_gameobject_end_transform.gameObject.SetActive(true);

            // Offset marker from surface.
            float tv_offset = 0.025f;
            v_target_gameobject_end_transform.position = sv_hit.point + sv_hit.normal * tv_offset;
            v_target_gameobject_end_transform.rotation = Quaternion.LookRotation(sv_hit.normal, Vector3.up);
        }

        public void f_set_trajectory_visible(bool sv_visible)
        {
            v_trajectory_line.enabled = sv_visible;
            v_target_gameobject_end_transform.gameObject.SetActive(sv_visible);
        }
    }
}