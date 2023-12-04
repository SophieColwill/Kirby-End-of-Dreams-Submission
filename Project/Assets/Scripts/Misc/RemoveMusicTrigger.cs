using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMusicTrigger : MonoBehaviour
{
    MusicManager musicManager;

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KirbyHealth target = collision.GetComponent<KirbyHealth>();

        if (target != null)
        {
            musicManager.ChangeCurrentSong(null);
        }
    }
}
