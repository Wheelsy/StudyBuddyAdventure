using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    //Structs created form text file on game start
    private List<Quest> shortQuests = new List<Quest>();
    private List<Quest> mediumQuests = new List<Quest>();
    private List<Quest> longQuests = new List<Quest>();
    //Available quests chosen from full list of quests
    private List<Quest> availableShortQuests = new List<Quest>();
    private List<Quest> availableMediumQuests = new List<Quest>();
    private List<Quest> availableLongQuests = new List<Quest>();
    //Active quest
    private Quest activeQuest;
    //Quest texts in game
    [SerializeField]
    TextMeshProUGUI[] shortQuestNames;
    [SerializeField]
    TextMeshProUGUI[] shortQuestDifficulties;
    [SerializeField]
    TextMeshProUGUI[] mediumQuestNames;
    [SerializeField]
    TextMeshProUGUI[] mediumQuestDifficulties;
    [SerializeField]
    TextMeshProUGUI[] longQuestNames;
    [SerializeField]
    TextMeshProUGUI[] longQuestDifficulties;
    private int index;
    private List<int> validIndexes = new List<int>();

    //Quest that has been accepted
    public Quest ActiveQuest { get => activeQuest; set => activeQuest = value; }

    //Generate a random number and add the corresponding quest from full list to available list
    private void Start()
    {
        SetValidIndexes();
        for (int i = 0; i < 3; i++)
        {
            //Get a random quest
            index = GetRandomIndex();
            //Add to available list
            availableShortQuests.Add(shortQuests[index]);
            availableMediumQuests.Add(shortQuests[index]);
            availableLongQuests.Add(shortQuests[index]);
            //Set in game quest text to quest name
            shortQuestNames[i].text = shortQuests[index].Name;
            mediumQuestNames[i].text = mediumQuests[index].Name;
            longQuestNames[i].text = longQuests[index].Name;
            shortQuestDifficulties[i].text = shortQuests[index].Difficulty;
            mediumQuestDifficulties[i].text = mediumQuests[index].Difficulty;
            longQuestDifficulties[i].text = longQuests[index].Difficulty;
            //remove that quest from the pool
            RemoveIndex(index);
        }
    }

    //create a list of indexes to use to fetch random quests
    private void SetValidIndexes(){
        for(int i = 0; i < shortQuests.Count; i++)
        {
            validIndexes.Add(i);
        }
    }

    //Get an index for a random quest
    private int GetRandomIndex()
    {
        return validIndexes[Random.Range(0, validIndexes.Count+1)];
    }

    //Remove an index from valid indexes so the quest wont show again
    private void RemoveIndex(int index)
    {
        validIndexes.Remove(index);
    }

    //Create the struct and add to short quests list
    public void AddShortQuest(string name, string reward, string difficulty, int time, string description, string victory, string defeat)
    {
        Quest q = new Quest
        {
            Name = name,
            Reward = reward,
            Difficulty = difficulty,
            Time = time,
            Description = description,
            Victory = victory,
            Defeat = defeat
        };
        shortQuests.Add(q);
    }

    //create the struct and add to medium quests list
    public void AddMediumQuest(string name, string reward, string difficulty, int time, string description, string victory, string defeat)
    {
        Quest q = new Quest
        {
            Name = name,
            Reward = reward,
            Difficulty = difficulty,
            Time = time,
            Description = description,
            Victory = victory,
            Defeat = defeat
        };
        mediumQuests.Add(q);
    }

    //create the struct and add to long quests list
    public void AddLongQuest(string name, string reward, string difficulty, int time, string description, string victory, string defeat)
    {
        Quest q = new Quest
        {
            Name = name,
            Reward = reward,
            Difficulty = difficulty,
            Time = time,
            Description = description,
            Victory = victory,
            Defeat = defeat
        };
        longQuests.Add(q);
    }

    //Seach all quests by name and return Quest if matched
    //Else return an empty Quest
    public Quest FindQuestByName(string questName)
    {
        List<Quest> allQuests = new List<Quest>();
        allQuests.AddRange(shortQuests);
        allQuests.AddRange(mediumQuests);
        allQuests.AddRange(longQuests);
        foreach (Quest q in allQuests)
        {
            if(q.Name.Equals(questName))
            {
                return q;
            }
        }
        Quest none = new Quest();
        return none;
    }

    //keep a record of which quest is currently being undertaken
    public void SetActiveQuest(string questName)
    {
        Quest q = FindQuestByName(questName);
        if (q.Name != null)
        {
            ActiveQuest = q;
        }
    }
}

//Quest struct
public struct Quest
{
    private string name;
    private string reward;
    private string difficulty;
    private int time;
    private string description;
    private string victory;
    private string defeat;

    public string Name { get => name; set => name = value; }
    public string Reward { get => reward; set => reward = value; }
    public string Difficulty { get => difficulty; set => difficulty = value; }
    public int Time { get => time; set => time = value; }
    public string Description { get => description; set => description = value; }
    public string Victory { get => victory; set => victory = value; }
    public string Defeat { get => defeat; set => defeat = value; }
}

