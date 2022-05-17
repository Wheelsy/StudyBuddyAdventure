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
    private bool shuffle = false;
    private bool repeatAll = false;
    private bool repeatOne = false;
    private float timePlaying;
    private int currentSongIndex;

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (source.isPlaying)
        {
            timePlaying += Time.deltaTime;

            if (timePlaying >= source.clip.length)
            {
                if (repeatOne)
                {
                    PlaySong(currentSongIndex);
                }
                else if (repeatAll)
                {
                    if ((currentSongIndex + 1) < library.Count)
                    {
                        PlaySong(currentSongIndex + 1);
                    }
                    else
                    {
                        PlaySong(0);
                    }
                }
                else if (shuffle)
                {
                    int song = Random.Range(0, library.Count);
                    PlaySong(song);
                }
                else
                {
                    source.Stop();
                    timePlaying = 0;
                }
            }
        }
    }

    public void AddToLibrary(AudioClip clip)
    {
        library.Add(clip);
        songNames[songCount].text = clip.name;
        songCount++;
    }

    public void PlaySong(int index)
    {
        if (index <= library.Count && index >= 0)
        {
            timePlaying = 0;
            currentSongIndex = index;
            source.clip = library[index];
            source.Play();
        }
    }

    public void Shuffle()
    {
        repeatAll = false;
        repeatOne = false;
        shuffle = true;
    }

    public void RepeatAll()
    {
        repeatOne = false;
        shuffle = false;
        repeatAll = true;
    }

    public void RepeatOne()
    {
        repeatAll = false;
        shuffle = false;
        repeatOne = true;
    }
}
