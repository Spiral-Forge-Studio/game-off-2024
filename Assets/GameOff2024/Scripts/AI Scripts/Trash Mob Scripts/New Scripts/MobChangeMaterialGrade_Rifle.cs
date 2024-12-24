using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobChangeMaterialGrade_Rifle : MobChangeMaterialGrade
{
    public SkinnedMeshRenderer body;
    public SkinnedMeshRenderer gunBody;
    public SkinnedMeshRenderer gunMount;
    List<Material> materials = new List<Material>();

    public override void ChangeMaterials(EMobGrade mobGrade)
    {
        int matIndex = ((int)mobGrade);
        
        materials.Clear();

        materials.Add(primaryMaterialsList[matIndex]);
        materials.Add(secondaryMaterialsList[matIndex]);

        //Debug.Log("[MAT]: " + primaryMaterialsList[matIndex].name);

        body.SetMaterials(materials);
        gunMount.SetMaterials(materials);
        materials.Reverse();
        gunBody.SetMaterials(materials);

        //body.materials[0] = primaryMaterialsList[matIndex];
        //body.materials[1] = secondaryMaterialsList[matIndex];

        //gunBody.materials[1] = primaryMaterialsList[matIndex];
        //gunBody.materials[0] = secondaryMaterialsList[matIndex];

        //gunMount.materials[0] = primaryMaterialsList[matIndex];
        //gunMount.materials[1] = secondaryMaterialsList[matIndex];
    }
}
