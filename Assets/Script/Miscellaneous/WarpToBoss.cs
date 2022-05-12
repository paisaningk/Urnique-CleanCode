using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class WarpToBoss : MonoBehaviour
{
    public MMFeedbacks Warp;
    private bool GO;
    private bool Play = true;

    // Update is called once per frame
    private void Update()
    {
        GO = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("RoomBoss")).value;
        if (GO != true || !Play) return;
        Warp.PlayFeedbacks();
        Play = false;
    }
}
