using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;

    private bool stopTyping = false;
    private Image speechBubble;
    private float timer = 0;

    [SerializeField]
    private List<string> dialogues = new List<string>();

    public void AddToDialogueArray(string text)
    {
        dialogues.Add(text);
    }

    void Start()
    {
        speechBubble = gameObject.GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer%20 <= 0.01f && timer%20 >= -0.01f)
        {
            int index = Random.Range(0, dialogues.Count);
            WriteText(index);
        }
    }

    public void WriteText(int textIndex)
    {
        stopTyping = false;
        speechBubble.enabled = true;
        StartCoroutine(TextWriter(textIndex));
    }

    IEnumerator TextWriter(int textIndex)
    {
        text.text = "";

        for (int i = 0; i < dialogues[textIndex].Length; i++)
        {
            if (!stopTyping)
            {
                text.text += dialogues[textIndex][i].ToString();
            }
            else
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.06f);
        }

        yield return new WaitForSeconds(1f);

        speechBubble.enabled = false;
        text.text = "";
    }

    public void StopSpeech()
    {
        stopTyping = true;
        speechBubble.enabled = false;
        text.text = "";
    }
}
