using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueBuff : MonoBehaviour
{
    public abstract void InitializeBuff();

    public abstract void UpdateBuff();

    public abstract void RemoveBuff();
}
