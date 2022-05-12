using System;
using System.Collections;
using Script.Controller;
using UnityEngine;

namespace Script.Player
{
    public class PlayerAttackRanged : MonoBehaviour
    {
        [SerializeField] private Transform gun;
        private GunController gunController;
        private int maxAmmo = 5;
        private bool canFire = true;
        private int ammo;
        private float reloadTime = 2;
        private Vector3 mousePosition;

        private void Awake()
        {
            gunController = GetComponent<GunController>();
        }

        private void Update()
        {
            if (ammo == maxAmmo)
            {
                StartCoroutine(ReloadAmmo());
            }
        }

        public void GunFollowMouse()
        {
            gun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, GetRotationZ());
        }

        public void ShootBullet()
        {
            if (!canFire) return;
            gunController.PlayAnimationShoot();
            StartCoroutine(FireRate());
            UsingAmmo();
            gunController.ShootBullet(DirectionToShoot(), GetRotationZ(),ammo);
        }

        private void UsingAmmo()
        {
            ammo++;
        }

        private Vector2 DirectionToShoot()
        {
            return (GetDifference() / GetDifference().magnitude).normalized;
        }
        
        private Vector3 GetDifference()
        {
            return mousePosition - transform.position;
        }

        private float GetRotationZ()
        {
            var difference = GetDifference();
            return Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }

        public void SetMousePosition(Vector3 mousePosition)
        {
            this.mousePosition = mousePosition;
        }
        
        private IEnumerator FireRate()
        {
            canFire = false;
            
            yield return new WaitForSeconds(0.2f);
            
            gunController.StopAnimationShoot();
            canFire = true;
        }
        
        private IEnumerator ReloadAmmo()
        {
            gunController.PlayAnimationReload();
            canFire = false;
            
            yield return new WaitForSeconds(reloadTime);
            
            gunController.StopAnimationReload();
            canFire = true;
            ammo = 0;
        }

        public void CheckCanReload()
        {
            if (ammo > 0)
            {
                StartCoroutine(ReloadAmmo());
            }
        }
    }
}