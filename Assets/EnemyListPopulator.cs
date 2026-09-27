using TMPro;
using UnityEngine;

namespace ArtillerySimulator
{
    public class EnemyListPopulator : MonoBehaviour
    {
        private EnemyMaster v_enemy_master_script;
        private TextMeshProUGUI v_target_text;

        void Start()
        {
            v_enemy_master_script = FindFirstObjectByType<EnemyMaster>();
            v_target_text = this.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (v_enemy_master_script.gameObject.transform.childCount > 0)
            {
                int tv_enemy_counter = 1;
                v_target_text.text = "";
                foreach (Transform tv_detected_enemy in v_enemy_master_script.gameObject.transform)
                {
                    Vector3 tv_direction = tv_detected_enemy.transform.position - Vector3.zero;
                    float tv_angle = Mathf.Atan2(tv_direction.x, tv_direction.z) * Mathf.Rad2Deg;
                    float tv_bearing = (tv_angle + 360f) % 360f;

                    v_target_text.text +=
                        "[Target " + tv_enemy_counter + " has a distance of " + Vector3.Distance(Vector3.zero, tv_detected_enemy.transform.position) + ", with a bearing of " + tv_bearing
                        +
                        "]\n\n";

                    tv_enemy_counter++;
                }
            }
            else
            {
                v_target_text.text = "";
            }
        }
    }
}