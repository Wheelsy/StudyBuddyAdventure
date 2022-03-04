using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questName;
    [SerializeField]
    private TextMeshProUGUI reward;
    [SerializeField]
    private TextMeshProUGUI difficulty;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private QuestManager qm;
    private Quest openQuest;

    public Quest OpenQuest { get => openQuest; set => openQuest = value; }

    //Load the text associated with chosen quest
    public void LoadView(string questName)
    {
        this.questName.text = questName;
        Quest quest = qm.FindQuestByName(questName);

        if(quest.Name != null)
        {
            OpenQuest = quest;
            this.reward.text = "Reward: " + quest.Reward;
            this.difficulty.text = "Difficulty: " + quest.Difficulty;
            this.description.text = "Description: " + quest.Description;
        }
    }
}
