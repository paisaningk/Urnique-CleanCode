using Script.Sound;
using UnityEngine;

namespace Script.Player
{
    public class GunController : MonoBehaviour
    {
        [SerializeField]private Transform startFire;
        private float bulletSpeed = 3;
        private int maxAmmo = 5;
        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void ShootBullet(Vector2 direction, float rotationZ,int ammo)
        {
            var bullet = ObjectPool.SharedInstance.GetPooledObject("Bullet");
            bullet.SetActive(true);
            bullet.transform.position = startFire.transform.position;
            bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

            PlaySoundShoot();
            PlaySoundLastAmmo(ammo);
        }
        
        private void PlaySoundShoot()
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Shoot);
        }
        
        private void PlaySoundLastAmmo(int ammo)
        {
            if (ammo == maxAmmo)
            {
                SoundManager.Instance.PlaySound(SoundManager.Sound.Ammo);
            }
        }

        public void PlayAnimationShoot()
        {
            animator.SetBool("Fire",true);
        }
        
        public void StopAnimationShoot()
        {
            animator.SetBool("Fire",false);
        }

        public void PlayAnimationReload()
        {
            animator.SetBool("Reload",true);
        }
        
        public void StopAnimationReload()
        {
            animator.SetBool("Reload",false);
        }
    }
}
