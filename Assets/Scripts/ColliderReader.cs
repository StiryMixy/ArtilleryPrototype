using System.Collections.Generic;
using UnityEngine;

namespace ArtillerySimulator
{
    public class ColliderReader : MonoBehaviour
    {
        [Header("Reference Variables")]
        [SerializeField] public List<GameObject> v_collider_current_collisions_list;

        private void OnCollisionEnter(UnityEngine.Collision sv_other_object)
        {
            if (!v_collider_current_collisions_list.Contains(sv_other_object.gameObject))
            {
                v_collider_current_collisions_list.Add(sv_other_object.gameObject);
            }
        }

        private void OnCollisionStay(UnityEngine.Collision sv_other_object)
        {
            if (!v_collider_current_collisions_list.Contains(sv_other_object.gameObject))
            {
                v_collider_current_collisions_list.Add(sv_other_object.gameObject);
            }
        }

        private void OnCollisionExit(UnityEngine.Collision sv_other_object)
        {
            if (v_collider_current_collisions_list.Count > 0)
            {
                if (v_collider_current_collisions_list.Contains(sv_other_object.gameObject))
                {
                    v_collider_current_collisions_list.Remove(sv_other_object.gameObject);
                }
            }
        }
        private void OnTriggerEnter(UnityEngine.Collider sv_other_object)
        {
            if (!v_collider_current_collisions_list.Contains(sv_other_object.gameObject))
            {
                v_collider_current_collisions_list.Add(sv_other_object.gameObject);
            }
        }

        private void OnTriggerStay(UnityEngine.Collider sv_other_object)
        {
            if (!v_collider_current_collisions_list.Contains(sv_other_object.gameObject))
            {
                v_collider_current_collisions_list.Add(sv_other_object.gameObject);
            }
        }

        private void OnTriggerExit(UnityEngine.Collider sv_other_object)
        {
            if (v_collider_current_collisions_list.Count > 0)
            {
                if (v_collider_current_collisions_list.Contains(sv_other_object.gameObject))
                {
                    v_collider_current_collisions_list.Remove(sv_other_object.gameObject);
                }
            }
        }
    }
}