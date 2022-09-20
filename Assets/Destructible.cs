
using UnityEngine;

public class Destructible : Interactable
{
    public string soundName;
    public GameObject brokenPref;
    public GameObject groundItemPref;
    public ItemDatabaseSO itemDatabaseSO;
    public override void Interact()
    {
        FindObjectOfType<AudioManager>().Play(soundName);
        CreateItem(1);
        Instantiate(brokenPref, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void CreateItem(int number)
    {
        Camera cam = Camera.main;
        for (int i = 0; i < number; i++)
        {
            int randomID = Random.Range(0, (int)(itemDatabaseSO.itemSOs.Length * 1.5f));
            if (randomID < itemDatabaseSO.itemSOs.Length)
            {
                GroundItem groundItem = Instantiate(groundItemPref, transform.position + Vector3.up * 1.5f, Quaternion.identity).GetComponent<GroundItem>();
                groundItem.item = itemDatabaseSO.itemSOs[randomID];
                groundItem.gameObject.GetComponent<BillBoard>().cam = cam;
            }
        }
    }


}
