using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType { SMG, SniperRiffle, Pistol, Shotgun, Melee, Grenade }
[CreateAssetMenu ( fileName = "DataWeapon" , menuName = "Weapon Data" , order = 0 )]
public class ScriptableWeapon : ScriptableObject
{
    public string Name;
    public WeaponType Type;

    public Sprite Icon;

    [Header("Weapon stats",order = 0)]

    [Tooltip("Force that be applyed to rigidbody on raycast hit")]
    public float rigidBodyHitForce;
    [Tooltip("Time in seconds to call next fire. 1 means 1 shot per second, 0.5 means 2 shots per second etc")]
    public float fireRate;
    [Tooltip("Bullet spread value. Influence on shooting trajectory in static reticle mod")]
    public float spread = 0.01f;
    [Tooltip("Recoil value to X axis of camera")]
    public Vector3 recoil;
    //---------------------------------------
    [Header("Prefabs Objects",order = 2)]

    [Tooltip("Particle system which being started after calling fire method")]
    public ParticleSystem MuzzleFlashParticlesFX;
    [Tooltip("SFX for each type of weapon state")]
    public AudioClip shotSFX, reloadingSFX, emptySFX;

    [Tooltip("Shell object which drops from the defined point in Weapon() class after calling fire method")]
    public GameObject shell;
    [Tooltip("Maximal amount of shell which being created on Start()")]
    public int shellsPoolSize;
    [Tooltip("Force with shells ejected from weapon")]
    public float shellEjectingForce;
      

    [Header("Scope settings")]

    [Tooltip("Can weapon use scope when aiming?")]
    public bool canUseScope;
    [Tooltip("Zoom FOV when scope is active")]
    public float scopeFOV;
    [Tooltip("Input sensetivity when player aiming")]
    public float scopeSensitivityX;
    [Tooltip("Input sensetivity when player aiming")]
    public float scopeSensitivityY;

    [Header("Melee settings (for melee only!)")]
    public float meleeAttackDistance;
    public float meleeAttackRate = 0.4f;
    public int meleeDamagePoints;
    public float meleeRigidbodyHitForce;
    public float meleeHitTime;
    public AudioClip meleeHitFX;

    [Header("Ballistic settings")]
    [Tooltip("Initial bullet velocity in meters per second. Recomended to take real weapons parameters")]
    public float bulletInitialVelocity = 360;
    [Tooltip("Air resistance force that slow down bullet in air. Default value is 1. If 0 no air resistance applied to bullet")]
    public float airResistanceForce = 1;
    [Tooltip("Projectile prefab used as projectile. Select one from prefabs")]
    public GameObject projectile;
    [Tooltip("Max amount of projectiles which being created on Start()")]
    [Range(1, 100)]
    public int projectilePoolSize = 1;

    [Header("Grenade settings")]
    public GameObject grenadePrefab;
    public float throwForce = 1500;
}
