using Cinemachine;
using UnityEngine;

namespace Script.Controller
{
    //this Script use in gameplay Scene
    public class CameraController : MonoBehaviour
    {
        private new CinemachineVirtualCamera camera;
        private Transform player;
        
        //set camera follow player
        private void Start()
        {
            camera = GetComponent<CinemachineVirtualCamera>();
            player = GameObject.FindWithTag("Player").transform;
            camera.Follow = player;
        }
    }
}