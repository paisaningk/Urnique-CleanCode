using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rin : MonoBehaviour
{
    public Animator Animator;
    public float StarMove;
    public float StarMoveslowe;
    
    
    public IEnumerator ForStart()
    {
        var a = Random.Range(StarMove,StarMoveslowe);
        yield return new WaitForSeconds(a);
        Animator.SetBool("Test",true);
    }
    
    public IEnumerator ForEnd()
    {
        var a = Random.Range(StarMove,StarMoveslowe);
        yield return new WaitForSeconds(a);
        Animator.SetBool("Test",false);
    }
}
