using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArtillerySimulator
{
    [Serializable]
    public class vpc_enemy_management_definition_configurables
    {
        [Header("Spawn Adjustment Values")]
        [SerializeField] public float v_spawn_interval = 0.0f;
        [SerializeField] public float v_spawn_radius_min = 0.0f;
        [SerializeField] public float v_spawn_radius_max = 1.0f;
        [SerializeField] public int v_spawn_max_entities = 1;
    }

    [Serializable]
    public class vpc_enemy_management_definition_references
    {
        [Header("Spawn Adjustment References")]
        [SerializeField] public float v_spawn_interval_counter = 0.0f;
        [SerializeField] public Vector3 v_spawn_target_random_direction = Vector3.zero;
        [SerializeField] public Vector3 v_spawn_target_random_position = Vector3.zero;
    }

    [Serializable]
    public class vpc_enemy_management_definition
    {
        [Header("Configurable Variables")]
        [SerializeField] public vpc_enemy_management_definition_configurables v_repository_configurables = new vpc_enemy_management_definition_configurables();
        [Header("Reference Variables")]
        [SerializeField] public vpc_enemy_management_definition_references v_repository_references = new vpc_enemy_management_definition_references();
    }

    public class EnemyMaster : MonoBehaviour
    {
        [Header("Catalogue Configuration")]
        [SerializeField] public List<GameObject> v_enemy_catalogue;
        [Header("Live Enemy Management")]
        [SerializeField] public vpc_enemy_management_definition v_existing_enemy_repository = new vpc_enemy_management_definition();

        private Terrain v_target_terrain;


        void Start()
        {
            v_target_terrain = FindFirstObjectByType<Terrain>();
        }

        void Update()
        {
            if (this.transform.childCount < v_existing_enemy_repository.v_repository_configurables.v_spawn_max_entities)
            {
                if (v_existing_enemy_repository.v_repository_references.v_spawn_interval_counter < v_existing_enemy_repository.v_repository_configurables.v_spawn_interval)
                {
                    v_existing_enemy_repository.v_repository_references.v_spawn_interval_counter += Time.deltaTime;
                }
                else
                {
                    v_existing_enemy_repository.v_repository_references.v_spawn_target_random_direction = UnityEngine.Random.insideUnitCircle.normalized * (UnityEngine.Random.Range(v_existing_enemy_repository.v_repository_configurables.v_spawn_radius_min, v_existing_enemy_repository.v_repository_configurables.v_spawn_radius_max));
                    v_existing_enemy_repository.v_repository_references.v_spawn_target_random_position = transform.position + new Vector3(v_existing_enemy_repository.v_repository_references.v_spawn_target_random_direction.x, 0, v_existing_enemy_repository.v_repository_references.v_spawn_target_random_direction.y);

                    v_existing_enemy_repository.v_repository_references.v_spawn_target_random_position = new Vector3
                        (
                            v_existing_enemy_repository.v_repository_references.v_spawn_target_random_position.x,
                            v_target_terrain.SampleHeight(v_existing_enemy_repository.v_repository_references.v_spawn_target_random_position) + 5.0f,
                            v_existing_enemy_repository.v_repository_references.v_spawn_target_random_position.z
                        );

                    Instantiate(v_enemy_catalogue[UnityEngine.Random.Range(0, (v_enemy_catalogue.Count - 1))], v_existing_enemy_repository.v_repository_references.v_spawn_target_random_position, Quaternion.LookRotation(Vector3.zero - v_existing_enemy_repository.v_repository_references.v_spawn_target_random_position), this.transform);
                    
                    v_existing_enemy_repository.v_repository_references.v_spawn_interval_counter = 0.0f;
                }
            }

        }
    }
}