using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCombiner
{
    public readonly Dictionary<int, Transform> boneRootDictionary = new Dictionary<int, Transform>();
    private readonly Transform[] boneTransforms = new Transform[67];

    private readonly Transform playerTransform;

    public BoneCombiner(GameObject rootObj)
    {
        playerTransform = rootObj.transform;
        TraverseHierarchy(playerTransform);
    }

    public Transform AddLimb(GameObject bonedObj)
    {
        var limb = ProcessBonedObj(bonedObj.GetComponent<SkinnedMeshRenderer>());
        limb.parent = playerTransform;
        return limb;
    }

    private Transform ProcessBonedObj(SkinnedMeshRenderer renderer)
    {
        var bonedObject = new GameObject().transform;//Create our own object

        var meshRenderer = bonedObject.gameObject.AddComponent<SkinnedMeshRenderer>();

        var bones = renderer.bones;

        for (int i = 0; i < bones.Length; i++)
        {
            boneTransforms[i] = boneRootDictionary[bones[i].name.GetHashCode()];
        }//Assemble bones' structure

        meshRenderer.bones = boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;
        //Assemble renderer
        return bonedObject;
    }

    private void TraverseHierarchy(Transform transform)
    {
        foreach (Transform child in transform)
        {
            boneRootDictionary.Add(child.name.GetHashCode(), child);
            TraverseHierarchy(child);
        }
    }
}
