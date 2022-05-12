using Cinemachine;
using MoreMountains.Feedbacks;
using Script.Controller;
using Script.Spawn;
using UnityEngine;

public class SetupSecen : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    public MMFeedbacks Fade;
    public GameObject RoninP;
    public GameObject GunP;
    public GameObject PlayerRonin;
    public GameObject PlayerGun;
    public Transform Spawn;
    private bool Ronin;
    private bool Mark;
    void Start()
    {
        Fade?.PlayFeedbacks();
        Time.timeScale = 1;
        if (SpawnPlayer.instance.PlayerType == PlayerType.SwordMan)
        {
            RoninP.SetActive(false);
            PlayerRonin.SetActive(true);
            PlayerRonin.transform.position = Spawn.position;
            VirtualCamera.m_Follow = PlayerRonin.transform;
        }
        else
        {
            GunP.SetActive(false);
            PlayerGun.SetActive(true);
            PlayerGun.transform.position = Spawn.position;
            VirtualCamera.m_Follow = PlayerGun.transform;
        }
    }

    void Update()
    {
        Mark = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("MarksMan")).value;
        Ronin = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("Ronin")).value;
        CheckAndChangeCharacter();
    }


    private void CheckAndChangeCharacter()
    {
        if (Mark)
        {
            VirtualCamera.Follow = PlayerGun.transform;
                
            RoninP.SetActive(true);
            PlayerRonin.SetActive(false);
            PlayerRonin.transform.position = RoninP.transform.position;

            PlayerGun.SetActive(true);
            GunP.SetActive(false);
            SpawnPlayer.instance.PlayerType = PlayerType.Gun;
        }
        else if (Ronin)
        {
            VirtualCamera.Follow = PlayerRonin.transform;
                
            GunP.SetActive(true);
            PlayerGun.SetActive(false);
            PlayerGun.transform.position =GunP.transform.position;

            PlayerRonin.SetActive(true);
            RoninP.SetActive(false);
            SpawnPlayer.instance.PlayerType = PlayerType.SwordMan;
        }
    }
}
