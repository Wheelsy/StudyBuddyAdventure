using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToDo : MonoBehaviour
{
    public GameObject[] tasks;
    public List<bool> completed = new List<bool>();
    public Sprite[] checkBoxes;
    public TMP_InputField txtField;

    //Get the user input and find the next inactive task in the array
    //Set the text of the task to the user input and set the task to active
    public void AddToList(TextMeshProUGUI text)
    {
        if (text.text != "")
        {
            GameObject task = FindNextTask();

            if (task != null)
            {
                task.SetActive(true);
                task.GetComponentInChildren<TextMeshProUGUI>().text = text.text;             
                txtField.text = "";
            }
        }
        else
        {
            return;
        }
    }

    //Loop through the task array and return the first task that is inactive
    //Else return null
    private GameObject FindNextTask()
    {
        for(int i = 0; i< tasks.Length; i++)
        {
            if (!tasks[i].activeInHierarchy)
            {
                return tasks[i];
            }
        }
        return null;
    }

    public void CompleteTask(int index, Image image)
    {
        if (completed[index])
        {
            completed[index] = false;
            image.sprite = checkBoxes[0];
        }
        else
        {
            completed[index] = true;
            image.sprite = checkBoxes[1];
        }
    }

    //Clear text and deactivate object in heirachy
    public void DeleteTask(GameObject task)
    {
        task.GetComponentInChildren<TextMeshProUGUI>().text = "";
        task.SetActive(false);
        ReorganiseTasks();
    }

    //Get the index of the empty task
    //Check if the next obj is active
    //If so move the content from the active task to the inactve one
    //The loop goes throught the remaining tasks to check for active objs and continues the process of moving them up
    private void ReorganiseTasks()
    {
        int emptyIndex = 6;
        for (int i = 0; i < tasks.Length; i++)
        {
            if (!tasks[i].activeInHierarchy)
            {
                emptyIndex = i;
                break;
            }
        }

        if (emptyIndex != 6)
        {
            if (tasks[emptyIndex + 1].activeInHierarchy)
            {
                for(int i = (emptyIndex+1); i < tasks.Length; i++)
                {
                    if (tasks[i].activeInHierarchy)
                    {
                        tasks[i - 1].GetComponentInChildren<TextMeshProUGUI>().text = tasks[i].GetComponentInChildren<TextMeshProUGUI>().text;
                        tasks[i - 1].SetActive(true);
                        tasks[i].SetActive(false);
                    }
                }
            }
        }
    }

    //Get todo info for saving
    public int GetNumTasksToSave()
    {
        int numTasks = 0;
        for(int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].activeSelf)
            {
                numTasks++;
            }
        }
        return numTasks;
    }

    public List<string> GetTaskValueToSave()
    {
        List<string> values = new List<string>();
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].activeSelf)
            {
                values.Add(tasks[i].GetComponentInChildren<TextMeshProUGUI>().text);
            }
        }
        return values;
    }

    public List<bool> GetTaskCompletedToSave()
    {
        List<bool> completeds = new List<bool>();
        for (int i = 0; i < GetNumTasksToSave(); i++)
        {
            completeds.Add(completed[i]);
        }
        return completeds;
    }
}
