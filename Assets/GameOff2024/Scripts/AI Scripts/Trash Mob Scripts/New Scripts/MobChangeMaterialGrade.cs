using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MobChangeMaterialGrade : MonoBehaviour
{
    public Material[] primaryMaterialsList;
    public Material[] secondaryMaterialsList;

    private void Start()
    {
        int numOfMobGrades = Enum.GetNames(typeof(EMobGrade)).Length;

        if (primaryMaterialsList.Length != numOfMobGrades || secondaryMaterialsList.Length != numOfMobGrades)
        {
            Debug.LogError("[MobChangeMaterial] Materials list does not match mob grade amount");
            Debug.Break();
        }
    }

    public abstract void ChangeMaterials(EMobGrade mobGrade);
}
