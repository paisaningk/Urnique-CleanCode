using System;
using System.Collections;
using Script.Base;
using Script.Player;
using Script.Sound;
using UnityEngine;

namespace Script.UI
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] ammoGameObjects;
        [SerializeField] private GameObject AmmoTextGameObject;
        private PlayerDate playerDate;

        private void Start()
        {
            SetupComponent();
            SetController();
        }

        private void Update()
        {
            CheckAmmo();
        }
        
        private void SetupComponent()
        {
            playerDate = GetComponent<PlayerDate>();
        }

        private void SetController()
        {
            PlayerController.playerInput.PlayerAction.Reload.performed += context => CanReload();
        }

        private void CanReload()
        {
            if (CheckIsAmmoLessThanZero())
            {
                StartCoroutine(nameof(Reload));
            }
        }

        private void CheckAmmo()
        {
            if (!CheckIsPlayerTypeGun()) return;
            var ammo = playerDate.playerAttackRanged.ammo;
            if (ammo < 5)
            {
                DisableOneAmmoGameObjects(ammo);
            }
            else
            {
                DisableOneAmmoGameObjects(ammo);
                StartCoroutine(nameof(Reload));
            }
        }

        private bool CheckIsPlayerTypeGun()
        {
            return playerDate.playerCharacter.PlayerType == PlayerType.Gun;
        }

        private bool CheckIsAmmoLessThanZero()
        {
            return playerDate.playerAttackRanged.ammo < 0;
        }

        private void DisableOneAmmoGameObjects(int ammo)
        {
            ammoGameObjects[ammo-1].SetActive(false);
        }
        
        IEnumerator Reload()
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Reload);
            
            DisableAll();
            
            yield return new WaitForSeconds(playerDate.playerAttackRanged.reloadTime);

            EnableAll();
        }

        private void EnableAll()
        {
            EnableAmmoTextGameObject();
            EnableAllAmmoGameObjects();
        }

        private void DisableAll()
        {
            DisableAllAmmoGameObjects();
            DisableAmmoTextGameObject();
        }

        private void EnableAmmoTextGameObject()
        {
            AmmoTextGameObject.SetActive(true);
        }

        private void DisableAmmoTextGameObject()
        {
            AmmoTextGameObject.SetActive(false);
        }
        private void EnableAllAmmoGameObjects()
        {
            foreach (var ammoGameObject in ammoGameObjects)
            {
                ammoGameObject.SetActive(true);
            }
        }

        private void DisableAllAmmoGameObjects()
        {
            foreach (var variable in ammoGameObjects)
            {
                variable.SetActive(false);
            }
        }
    }
}