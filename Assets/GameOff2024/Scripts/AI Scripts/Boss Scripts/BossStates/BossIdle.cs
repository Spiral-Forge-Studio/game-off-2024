using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : IState
{

    public BossIdle() { }
    public void Tick() { }
    public void OnEnter() { Debug.Log("Entered Idle"); }

    public void OnExit() { }
}
