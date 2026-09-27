using UnityEngine;

namespace ArtillerySimulator
{
    public class ShellTranscriptor : MonoBehaviour
    {
        private ShellBroadcaster v_shell_broadcaster_script;

        private Rigidbody v_target_rigidbody;

        public void f_transcript_physics_values(vpc_ammo_type_definition sv_target_ammo_type)
        {
            v_target_rigidbody = this.GetComponent<Rigidbody>();
            v_shell_broadcaster_script = this.GetComponent<ShellBroadcaster>();
            if (v_target_rigidbody)
            {
                v_target_rigidbody.mass = sv_target_ammo_type.v_ammo_type_mass;
                v_target_rigidbody.drag = sv_target_ammo_type.v_ammo_type_drag;
            }
            if (v_shell_broadcaster_script)
            {
                v_shell_broadcaster_script.v_target_explosion = sv_target_ammo_type.v_ammo_type_explosion;
                v_shell_broadcaster_script.v_explosion_size = sv_target_ammo_type.v_ammo_type_explosion_size;
            }
        }
    }
}