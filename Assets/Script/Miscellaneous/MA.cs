using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class MA : MonoBehaviour
{
    public GameObject Talk;

    public void Update()
    {
        if (ChooseCharacter.IsSelect)
        {
            Talk.SetActive(true);
        }
    }
}
