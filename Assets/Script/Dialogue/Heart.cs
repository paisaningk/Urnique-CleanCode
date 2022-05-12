using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private Animator animator;
    private int heart;
    private static readonly int Heart1 = Animator.StringToHash("Heart");

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        heart = ((Ink.Runtime.IntValue) DialogueManager.GetInstance().GetVariableState("RinHeart")).value;
        animator.SetInteger(Heart1,heart);
    }
}
