using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossIdle : IState
{
    private BossController _bossfunc;
    public BossIdle(BossController boss) { _bossfunc = boss; }
    public void Tick() { }
    public void OnEnter() { Debug.Log("Entered Idle"); _bossfunc.MoveToCenter(); }

    public void OnExit() { }
}



