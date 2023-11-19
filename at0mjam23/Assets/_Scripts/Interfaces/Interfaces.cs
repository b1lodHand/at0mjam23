using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IActivator
{
    public bool IsActive();
}

public interface IInteractable
{
    public void Interact(params object[] data);
}

public interface IPatrol
{
    public void Activate();
    public void Deactivate();
    public void Pause();
    public void Resume();
    public PatrolState GetState();
    public void AppendOnWaitingEnds(Action actionWillBeAppended);
}

public interface IBreakable
{
    public bool Break(float duration);
    public void Recover();
    public bool IsBroken();
    public Transform GetTransform();
}