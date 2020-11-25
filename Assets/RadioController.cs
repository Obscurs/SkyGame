using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject song1;
    public GameObject song2;
    public GameObject song3;
    int currentSong = 0;

    void Awake()
    {
        song1 = GameObject.Find("song1");
        song2 = GameObject.Find("song2");
        song3 = GameObject.Find("song3");
        song1.GetComponent<AudioSource>().mute = true;
        song2.GetComponent<AudioSource>().mute = true;
        song3.GetComponent<AudioSource>().mute = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSong()
    {
        currentSong += 1;
        if (currentSong > 3)
            currentSong = 0;
        if (currentSong == 0)
        {
            song1.GetComponent<AudioSource>().mute = true;
            song2.GetComponent<AudioSource>().mute = true;
            song3.GetComponent<AudioSource>().mute = true;
        }
        else if(currentSong == 1)
        {
            song1.GetComponent<AudioSource>().mute = false;
            song2.GetComponent<AudioSource>().mute = true;
            song3.GetComponent<AudioSource>().mute = true;
        }
        else if (currentSong == 2)
        {
            song1.GetComponent<AudioSource>().mute = true;
            song2.GetComponent<AudioSource>().mute = false;
            song3.GetComponent<AudioSource>().mute = true;
        }
        else if (currentSong == 3)
        {
            song1.GetComponent<AudioSource>().mute = true;
            song2.GetComponent<AudioSource>().mute = true;
            song3.GetComponent<AudioSource>().mute = false;
        }
    }
}
