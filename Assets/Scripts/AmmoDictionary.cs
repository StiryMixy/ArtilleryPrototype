using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArtillerySimulator
{
    public enum venum_tags_ammo_type_list
    {
        BasicShell,
        ElectrifiedShell,
        IncendiaryShell,
        CryogenicShell,
        SpeedyShell,
        ScatterShell,
        HeavyShell,
        EnergyShell,
        CupidArrow
    };

    [Serializable]
    public class vpc_ammo_type_definition
    {
        [Header("Target Ammmo Type")]
        [SerializeField] public venum_tags_ammo_type_list v_ammo_type = venum_tags_ammo_type_list.BasicShell;
        [Header("Target Ammo Adjustment Values")]
        [SerializeField] public GameObject v_ammo_type_prefab;
        [SerializeField] public GameObject v_ammo_type_explosion;
        [SerializeField] public float v_ammo_type_launch_force = 250.0f;
        [SerializeField] public float v_ammo_type_mass = 1.0f;
        [SerializeField] public float v_ammo_type_drag = 0.1f;
        [SerializeField] public float v_ammo_type_explosion_size = 500.0f;
        [Header("Target Ammo Adjustment Interfacial")]
        [SerializeField] public string v_ammo_type_description = "";
    }

    public class AmmoDictionary : MonoBehaviour
    {
        [Header("Active Ammo")]
        [SerializeField] public vpc_ammo_type_definition v_ammo_type_active = new vpc_ammo_type_definition();
        [Header("Ammmo Dictionary")]
        [SerializeField] public List<vpc_ammo_type_definition> v_ammo_type_setup = new List<vpc_ammo_type_definition>();

        void Update()
        {
            foreach (vpc_ammo_type_definition sv_target_ammo in v_ammo_type_setup)
            {
                if (sv_target_ammo.v_ammo_type.Equals(v_ammo_type_active.v_ammo_type))
                {
                    v_ammo_type_active.v_ammo_type_prefab = sv_target_ammo.v_ammo_type_prefab;
                    v_ammo_type_active.v_ammo_type_explosion = sv_target_ammo.v_ammo_type_explosion;
                    v_ammo_type_active.v_ammo_type_launch_force = sv_target_ammo.v_ammo_type_launch_force;
                    v_ammo_type_active.v_ammo_type_mass = sv_target_ammo.v_ammo_type_mass;
                    v_ammo_type_active.v_ammo_type_drag = sv_target_ammo.v_ammo_type_drag;
                    v_ammo_type_active.v_ammo_type_explosion_size = sv_target_ammo.v_ammo_type_explosion_size;
                    v_ammo_type_active.v_ammo_type_description = sv_target_ammo.v_ammo_type_description;
                    break;
                }
            }
        }
    }
}