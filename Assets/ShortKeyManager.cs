using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortKeyManager : MonoBehaviour
{
    public GameObject healingPref;
    public GameObject manaHealPref;
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
            
        if (!PauseMenu.gameIsPause &&
            Input.GetKeyDown(keyCode) && 
            refToItemInSlot != null &&
            !refToItemInSlot.isStackable &&
            playerMainStat.IsEnoughMana(refToItemInSlot.spell.manaCost) &&
            !playerRef.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BaseAnimation.MainHandCast"))
        {
            spellManager.spellPrefab = usableItemInventory.GetSlots[indexForSlot].ItemSO.spellPrefab;
            SpellBehaviour.parent = playerRef.gameObject;
            SpellBehaviour.speed = refToItemInSlot.spell.speed;
            SpellBehaviour.lifeDuration = playerMainStat.DurationAfterDexBonus(refToItemInSlot.spell.lifeDuration);
            SpellBehaviour.minDamage = playerMainStat.DamageAfterStrBonus(refToItemInSlot.spell.minBaseDamage);
            SpellBehaviour.maxDamage = playerMainStat.DamageAfterStrBonus(refToItemInSlot.spell.maxBaseDamage);
            SpellBehaviour.AOE = playerMainStat.AOEAfterIntBonus(refToItemInSlot.spell.AOE);
            playerMainStat.UseMana(refToItemInSlot.spell.manaCost);
            playerRef.CastXSpellToMousePos("mainSpellCast");
        }

        if(Input.GetKeyDown(keyCode) &&
            refToItemInSlot != null &&
            refToItemInSlot.isStackable)
        {
            switch (usableItemInventory.GetSlots[indexForSlot].ItemSO.itemType)
            {
                case ItemType.LifeRecover:
                    Instantiate(healingPref, playerRef.transform.position, Quaternion.identity).transform.parent = playerRef.transform;
                    FindObjectOfType<AudioManager>().Play("PotionUseHeal");
                    playerMainStat.currentHealth += refToItemInSlot.healValue;
                    usableItemInventory.GetSlots[indexForSlot].AddAmount(-1);
                    if (usableItemInventory.GetSlots[indexForSlot].amount <= 0)
                        usableItemInventory.GetSlots[indexForSlot].RemoveItem();
                    break;
                case ItemType.ManaRecover:
                    Instantiate(manaHealPref, playerRef.transform.position, Quaternion.identity).transform.parent = playerRef.transform;
                    FindObjectOfType<AudioManager>().Play("PotionUseMana");
                    playerMainStat.currentMana += refToItemInSlot.healValue;
                    usableItemInventory.GetSlots[indexForSlot].AddAmount(-1);
                    if (usableItemInventory.GetSlots[indexForSlot].amount <= 0)
                        usableItemInventory.GetSlots[indexForSlot].RemoveItem();
                    break;
            }
        }
    }
}
