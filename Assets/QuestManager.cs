using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class QuestManager : MonoBehaviour
{
    public event Action QuestCompleteAction;
    MainStats playerStats;
    public List<Quest> receivedQuests = new List<Quest>();
    int[] questToRemoveIndex = new int[] {-1, -1, -1 };
    private void Start()
    {
        playerStats = GetComponent<MainStats>();    
    }
    public void UpdateKillQuest(MonsterType monster)
    {
        int removeIndex = 0;
        for (int i = 0; i < receivedQuests.Count; i++)
        {
            if (receivedQuests[i].goal.monsterToKill != MonsterType.None &&
                receivedQuests[i].goal.monsterToKill == monster)
            {
                receivedQuests[i].goal.currentAmount++;
                if (QuestCompleteAction != null)
                    QuestCompleteAction.Invoke();
                bool isCompleted = receivedQuests[i].goal.IsCompleted();
                if (isCompleted)
                {
                    OnQuestComplete(receivedQuests[i]);
                    questToRemoveIndex[removeIndex] = i;
                    removeIndex++;
                }
            }
        }
        for (int i = questToRemoveIndex.Length - 1; i >= 0; i--)
        {
            if (questToRemoveIndex[i] >= 0)
                receivedQuests.RemoveAt(questToRemoveIndex[i]);
        }
        if (QuestCompleteAction != null)
            QuestCompleteAction.Invoke();
        ResetIndex();
    }

    public void OnQuestComplete(Quest completedQuest)
    {
        playerStats.GetComponent<Level>().currentExp += completedQuest.experienceReward;
        playerStats.GetComponent<Level>().UpdateLevel();
    }

    void ResetIndex()
    {
        for (int i = 0; i < questToRemoveIndex.Length; i++)
        {
            questToRemoveIndex[i] = -1;
        }
    }
}
