using UnityEngine;

namespace ArtillerySimulator
{
    public class ExplosionSergeant : MonoBehaviour
    {
        [Header("Explosion Configuration")]
        [SerializeField] public float v_target_expansion_alpha_step;
        [SerializeField] public float v_target_expansion_max;
        [SerializeField] public float v_target_expansion_speed;

        private MeshRenderer v_mesh_renderer;

        void Start()
        {
            v_mesh_renderer = GetComponent<MeshRenderer>();
        }

        void Update()
        {
            if (v_mesh_renderer.material.color.a > 0.0f)
            {
                v_mesh_renderer.material.color = new Color(v_mesh_renderer.material.color.r, v_mesh_renderer.material.color.g, v_mesh_renderer.material.color.b, v_mesh_renderer.material.color.a - v_target_expansion_alpha_step);
            }

            this.transform.localScale = new Vector3
                (
                    Mathf.Clamp(Mathf.Lerp(this.transform.localScale.x, v_target_expansion_max, (v_target_expansion_speed * Time.deltaTime)), 0.0f, v_target_expansion_max),
                    Mathf.Clamp(Mathf.Lerp(this.transform.localScale.y, v_target_expansion_max, (v_target_expansion_speed * Time.deltaTime)), 0.0f, v_target_expansion_max),
                    Mathf.Clamp(Mathf.Lerp(this.transform.localScale.z, v_target_expansion_max, (v_target_expansion_speed * Time.deltaTime)), 0.0f, v_target_expansion_max)
                );
        }
    }
}