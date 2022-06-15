using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ReadFromTxt : MonoBehaviour
{
    private string dialogueFileName = "dialogue";
    private string shortQuestFileName = "shortQuest";
    private string mediumQuestFileName = "mediumQuest";
    private string longQuestFileName = "longQuest";
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
        TextAsset ta = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
        string tmp = ta.text;
        string[] lines = tmp.Split("\n"[0]);
        foreach (string line in lines)
        {
           // Debug.Log("new line");
            if (fileName.Equals("dialogue"))
            {
                d.AddToDialogueArray(line);
            }
            else if (fileName.Equals("shortQuest"))
            {
                string[] s = line.Split(new char[] { ',' });
                qm.AddShortQuest(s[0], s[1], s[2], int.Parse(s[3]), s[4], s[5], s[6]);
            }
            else if (fileName.Equals("mediumQuest"))
            {
                string[] s = line.Split(new char[] { ',' });
                qm.AddMediumQuest(s[0], s[1], s[2], int.Parse(s[3]), s[4], s[5], s[6]);
            }
            else if (fileName.Equals("longQuest"))
            {
                string[] s = line.Split(new char[] { ',' });
                qm.AddLongQuest(s[0], s[1], s[2], int.Parse(s[3]), s[4], s[5], s[6]);
            }
        }
        //sr.Close();
    }
}
