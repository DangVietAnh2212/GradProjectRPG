using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiver : Interactable
{
    int maxQuestNumber = 3;

    Quest questToGive;

    public GameObject questWindow;

    public GameObject questFullWindow;
    public override void Interact()
    {
        if(player != null)
        {
            StartCoroutine(FaceTarget());
            QuestGenerator();
            OpenQuestWindow();
        }
    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        questWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = questToGive.title;
        questWindow.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = questToGive.description;
        questWindow.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Reward: " + questToGive.experienceReward + " EXP";
    }

    public void CloseQuestWindow()
    {
        questWindow.SetActive(false);
    }

    IEnumerator FaceTarget()
    {
        while(player != null)
        {
            Vector3 directionToTarget = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            //using new Vector3 to avoid looking up and down
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            //5f is the speed of the rotation (smooth the rotation)
            yield return null;
        }
        CloseQuestWindow();
        StopAllCoroutines();
    }

    void QuestGenerator()
    {
        List<Quest> playerAcceptedQuests;
        if (player.GetComponent<QuestManager>() == null)
            return;
        playerAcceptedQuests = player.GetComponent<QuestManager>().receivedQuests;
        GoalType questType = (GoalType)Random.Range(0, 2);
        for (int i = 0; i < playerAcceptedQuests.Count; i++)
        {
            if (playerAcceptedQuests[i].goal.monsterToKill == MonsterType.Boss)
                questType = GoalType.Kill;
            if (playerAcceptedQuests[i].goal.goalType == GoalType.Find)
                questType = GoalType.Kill;
        }
        questToGive = new Quest();
        switch (questType)
        {
            case GoalType.Kill:
                questToGive.title = "Monster Hunt";
                int numberTokill = Random.Range(10, 20);
                questToGive.goal = new QuestGoal(questType, numberTokill, MonsterType.Zombie);
                questToGive.description = $"I need you to kill {numberTokill} Zombies";
                questToGive.experienceReward = Random.Range(300, 501);
                break;
            /*case GoalType.Find:
                break;*/
            case GoalType.KillBoss:
                questToGive.title = "Overthrow Overlord";
                questToGive.description = "I need you to kill this extremely dangerous monster has been lurking around for 100 years!!!";
                questToGive.experienceReward = Random.Range(1000, 1501);
                questToGive.goal = new QuestGoal(questType, 1, MonsterType.Boss);
                break;
        }
    }

    public void QuestAccepted()
    {
        List<Quest> playerAcceptedQuests = player.GetComponent<QuestManager>().receivedQuests;
        if (playerAcceptedQuests.Count >= maxQuestNumber)
        {
            questFullWindow.SetActive(true);
            return;
        }
        playerAcceptedQuests.Add(questToGive);
    }
}
