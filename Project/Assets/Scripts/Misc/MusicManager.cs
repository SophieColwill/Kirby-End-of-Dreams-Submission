using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public void ChangeCurrentSong(AudioClip newSong)
    {
        musicSource.clip = newSong;
        musicSource.Play();
    }
}
