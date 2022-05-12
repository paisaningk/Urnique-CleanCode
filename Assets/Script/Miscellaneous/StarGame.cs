using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

//this Script use in Start Game Scene
public class StarGame : MonoBehaviour
{
    public MMFeedbacks Scene;
    void Update()
    {
        //Press mouse 0 button to skip movie.
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Scene?.PlayFeedbacks();
        }
    }

    void A()
    {
        //if movie end load scene main menu
        Scene?.PlayFeedbacks();
    }
}
