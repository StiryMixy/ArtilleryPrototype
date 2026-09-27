using TMPro;
using UnityEngine;

namespace ArtillerySimulator
{
    public class UIBearingPopulator : MonoBehaviour
    {
        private BallisticMaster v_ballistic_master_script;
        private TextMeshProUGUI v_target_text;

        void Start()
        {
            v_ballistic_master_script = FindFirstObjectByType<BallisticMaster>();
            v_target_text = this.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            Vector3 tv_direction = v_ballistic_master_script.v_target_gameobject_end_transform.position - Vector3.zero;
            float tv_angle = Mathf.Atan2(tv_direction.x, tv_direction.z) * Mathf.Rad2Deg;
            float tv_bearing = (tv_angle + 360f) % 360f;
            
            v_target_text.text = ((int)tv_bearing).ToString();
        }
    }
}