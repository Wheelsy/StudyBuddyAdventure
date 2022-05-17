using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicLibrary : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] songNames;
    private List<AudioClip> library = new List<AudioClip>();
    private int songCount = 0;
    private AudioSource source;

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    public void AddToLibrary(AudioClip clip)
    {
        Debug.Log(clip.name);
        library.Add(clip);
        songNames[songCount].text = clip.name;
        songCount++;
    }

    public void PlaySong(int index)
    {
        if (index <= library.Count && index >= 0)
        {
            source.clip = library[index];
            source.Play();
        }
    }
}
