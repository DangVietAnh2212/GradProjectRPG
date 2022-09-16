using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public GameObject basicSpellPrefab;
    public GameObject spellPrefab;
    public Transform spellSpawnPosTransform;

    public void SpellCastBasic()
    {
        Quaternion lookRotation = Quaternion.LookRotation(spellSpawnPosTransform.forward);
        GameObject spell = Instantiate(basicSpellPrefab, spellSpawnPosTransform.position, lookRotation);
    }
    public void SpellCast()
    {
        Quaternion lookRotation = Quaternion.LookRotation(spellSpawnPosTransform.forward);
        GameObject spell = Instantiate(spellPrefab, spellSpawnPosTransform.position, lookRotation);
    }
}
