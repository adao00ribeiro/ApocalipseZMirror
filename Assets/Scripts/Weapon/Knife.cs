using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{

    [RequireComponent(typeof(AudioSource))]
    public class Knife : MonoBehaviour, IWeapon
    {
        [SerializeField] private DataArmsWeapon weaponSetting;

        [SerializeField] private string weaponName;
        public string WeaponName { get => weaponName; }
        private Sway sway;

        private Animator Animator;
        private float nextFireTime;
        private AudioClip shotSFX;
        private Recoil recoilComponent;
        private int currentAmmo;
        public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
        public DataArmsWeapon WeaponSetting => weaponSetting;
        private bool setAim = false;
        public bool SetAim => setAim;
        private AudioClip meleeHitFX;
        private float distanceToPlayer;
        private SoundManager soundManager;
        // Start is called before the first frame update
        void Start()
        {
            weaponSetting = GameController.Instance.DataManager.GetArmsWeapon(weaponName);
            //DataParticles DataParticles = GameController.Instance.DataManager.GetDataParticles("FlashParticles");
            soundManager = GameController.Instance.SoundManager;
            shotSFX = GameController.Instance.DataManager.GetDataAudio(weaponSetting.shotSFX).Audio;
            meleeHitFX = GameController.Instance.DataManager.GetDataAudio(weaponSetting.meleeHitFX).Audio;
            sway = transform.GetComponentInParent<Sway>();
            recoilComponent = GameObject.FindObjectOfType<Recoil>();

            if (GetComponentInChildren<Animator>())
                Animator = GetComponentInChildren<Animator>();

        }

        // Update is called once per frame
        public bool Fire()
        {
            if (Time.time > nextFireTime) //Allow fire statement
            {

                soundManager.Stop();
                soundManager.PlayOneShot(transform.position, shotSFX);

                Animator.Play("Attack");
                Invoke("MeleeHit", weaponSetting.meleeHitTime);
                recoilComponent.AddRecoil(weaponSetting.recoil);
                nextFireTime = Time.time + weaponSetting.meleeAttackRate;

                return true;
            }

            return false;
        }

        public void ReloadBegin()
        {

        }

        public bool Aim(bool v)
        {
            return false;
        }
        public void MeleeHit()
        {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponSetting.meleeAttackDistance))
            {
                HitFXManager manager = GameController.Instance.HitFXManager;
                soundManager.PlayOneShot(transform.position, meleeHitFX);
                IStats stats = hit.collider.GetComponent<IStats>();
                if (stats != null)
                {
                    stats.TakeDamage(weaponSetting.meleeDamagePoints);
                }
                manager.ApplyFX(hit);

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(weaponSetting.meleeRigidbodyHitForce * Camera.main.transform.forward, hit.point);
                }

                if (hit.collider.CompareTag("Target"))
                {
                    hit.rigidbody.isKinematic = false;
                    hit.rigidbody.AddForceAtPosition(weaponSetting.meleeRigidbodyHitForce * Camera.main.transform.forward, hit.point);
                }
            }

        }

        bool IWeapon.ReloadBegin()
        {
            return false;
        }

        public void InvokeRealodEnd()
        {

        }
    }

}
