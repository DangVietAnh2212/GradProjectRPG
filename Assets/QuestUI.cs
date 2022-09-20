using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public GameObject collapsibleBody;
    public GameObject questTextBox;
    public GameObject playerRef;
    List<Quest> quests;
    TextMeshProUGUI questTitle;
    TextMeshProUGUI questDes;
    private void Awake()
    {
        playerRef.GetComponent<QuestManager>().QuestCompleteAction += UpdateQuestUI;
        quests = playerRef.GetComponent<QuestManager>().receivedQuests;   
    }

    public void UpdateQuestUI()
    {
        for (int i = collapsibleBody.transform.childCount - 1; i >= 0; i--)
        {
            Transform transform = collapsibleBody.transform.GetChild(i);
            transform.SetParent(null);
            Destroy(transform.gameObject);
        }
        for (int i = 0; i < quests.Count; i++)
        {
            GameObject newQuestBox = Instantiate(questTextBox);
            questTitle = newQuestBox.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            questDes = newQuestBox.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            questTitle.text = quests[i].title;
            questDes.text = quests[i].description;
            questDes.text += $"\nCurrent Kill: {quests[i].goal.currentAmount}";
            questDes.text += $"\nReward: {quests[i].experienceReward} EXP";
            newQuestBox.transform.SetParent(collapsibleBody.transform, false);
        }
    }
}
