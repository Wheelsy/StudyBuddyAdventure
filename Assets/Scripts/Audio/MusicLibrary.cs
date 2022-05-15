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

    public void AddToLibrary(AudioClip clip)
    {
        Debug.Log(clip.name);
        library.Add(clip);
        songNames[songCount].text = clip.name;
        songCount++;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
