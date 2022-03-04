using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ReadFromTxt : MonoBehaviour
{
    private string dialogueFileName = "dialogue.txt";
    private string shortQuestFileName = "shortQuest.txt";
    private string mediumQuestFileName = "mediumQuest.txt";
    private string longQuestFileName = "longQuest.txt";
    private Dialogue d;
    private StreamReader sr;
    [SerializeField]
    QuestManager qm;

    //Send all files to the reader to be added to appropriate arrays and lists
    void Awake()
    {
        d = gameObject.GetComponent<Dialogue>();
        readFile(dialogueFileName);
        readFile(shortQuestFileName);
        readFile(mediumQuestFileName);
        readFile(longQuestFileName);
    }

    //Read the files that have been passed in and add them to the correct array/list
    private void readFile(string fileName)
    {
        sr = new StreamReader(Application.dataPath + "/TxtFiles/" + fileName);
        string tmp = sr.ReadToEnd();
        string[] lines = tmp.Split("\n"[0]);
        foreach (string line in lines)
        {
            if (fileName.Equals("dialogue.txt"))
            {
                d.AddToDialogueArray(line);
            }
            else if (fileName.Equals("shortQuest.txt"))
            {
                string[] s = line.Split(new char[] { ',' });
                qm.AddShortQuest(s[0], s[1], s[2], int.Parse(s[3]), s[4], s[5], s[6]);
            }
            else if (fileName.Equals("mediumQuest.txt"))
            {
                string[] s = line.Split(new char[] { ',' });
                qm.AddMediumQuest(s[0], s[1], s[2], int.Parse(s[3]), s[4], s[5], s[6]);
            }
            else if (fileName.Equals("longQuest.txt"))
            {
                string[] s = line.Split(new char[] { ',' });
                qm.AddLongQuest(s[0], s[1], s[2], int.Parse(s[3]), s[4], s[5], s[6]);
            }
        }
        sr.Close();
    }
}
