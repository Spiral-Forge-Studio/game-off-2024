using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobChangeMaterialGrade_ROcket : MobChangeMaterialGrade
{
    public SkinnedMeshRenderer body;
    public SkinnedMeshRenderer turret;

    List<Material> materials = new List<Material>();

    public override void ChangeMaterials(EMobGrade mobGrade)
    {
        int matIndex = ((int)mobGrade);
        
        materials.Clear();

        materials.Add(primaryMaterialsList[matIndex]);
        materials.Add(secondaryMaterialsList[matIndex]);

        body.SetMaterials(materials);
        turret.SetMaterials(materials);
    }
}
