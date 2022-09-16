using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortKeyManager : MonoBehaviour
{
    public SpellManager spellManager;
    public PlayerController playerRef;
    public InventorySO usableItemInventory;
    public KeyCode keyCode;
    public int indexForSlot;//Index to the item in the Equipment InventorySO
    InventoryItem refToItemInSlot;
    MainStats playerMainStat;
    // Start is called before the first frame update
    void Start()
    {
        playerMainStat = playerRef.GetComponent<MainStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (usableItemInventory.GetSlots[indexForSlot].inventoryItem.ID >= 0)
        {
            refToItemInSlot = usableItemInventory.GetSlots[indexForSlot].inventoryItem;
        }
        else if(usableItemInventory.GetSlots[indexForSlot].inventoryItem.ID < 0)
        {
            refToItemInSlot = null;
        }
            
        if (Input.GetKeyDown(keyCode) && 
            refToItemInSlot != null &&
            playerMainStat.IsEnoughMana(refToItemInSlot.spell.manaCost) &&
            !playerRef.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BaseAnimation.MainHandCast"))
        {
            spellManager.spellPrefab = usableItemInventory.GetSlots[indexForSlot].ItemSO.spellPrefab;
            SpellBehaviour.speed = refToItemInSlot.spell.speed;
            SpellBehaviour.lifeDuration = playerMainStat.DurationAfterDexBonus(refToItemInSlot.spell.lifeDuration);
            SpellBehaviour.minDamage = playerMainStat.DamageAfterStrBonus(refToItemInSlot.spell.minBaseDamage);
            SpellBehaviour.maxDamage = playerMainStat.DamageAfterStrBonus(refToItemInSlot.spell.maxBaseDamage);
            SpellBehaviour.AOE = playerMainStat.AOEAfterIntBonus(refToItemInSlot.spell.AOE);
            playerMainStat.UseMana(refToItemInSlot.spell.manaCost);
            playerRef.CastXSpellToMousePos("mainSpellCast");
        }
    }
}
