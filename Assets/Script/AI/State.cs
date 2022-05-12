using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public enum StateStatus
    {
        Patrol, Pursue , Attack
    }
   
    public enum EvenState
    {
        Enter,Update,Exit
    }

    public StateStatus name;
    protected EvenState evenstate;
    protected GameObject npc;

    protected Transform player;
    protected State nextState;
    protected TextMesh TextStatus;

    public State(GameObject npc,Transform player,TextMesh textStatus)
    {
        this.npc = npc;
        this.evenstate = EvenState.Enter;
        this.player = player;
        this.TextStatus = textStatus;
    }

    protected virtual void Enter()
    {
        evenstate = EvenState.Update;
    }

    protected virtual void Update()
    {
        evenstate = EvenState.Update;
    }

    protected virtual void Exit()
    {
        evenstate = EvenState.Exit;
    }
   
    public State Process()
    {
        if (evenstate == EvenState.Exit)
        {
            Exit();
            return nextState;
        }
        if (evenstate == EvenState.Enter)
        {
            Enter();
        }
        if (evenstate == EvenState.Update)
        {
            Update();
        }
        return this;
    }
   

    protected float DistancePlayer()
    {
        return Vector3.Distance(npc.transform.position,player.transform.position);
    }
}
