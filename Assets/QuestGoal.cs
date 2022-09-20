using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;
    public int requiredAmount;
    public int currentAmount;
    public MonsterType monsterToKill = MonsterType.None;
    
    public QuestGoal(GoalType goal, int requiredAmount, MonsterType monster)
    {
        goalType = goal;
        this.requiredAmount = requiredAmount;
        currentAmount = 0;
        monsterToKill = monster;
    }
    public bool IsCompleted()
    {
        return currentAmount >= requiredAmount;
    }
}

public enum GoalType
{
    Kill = 0,
    KillBoss = 1,
    Find = 2
}