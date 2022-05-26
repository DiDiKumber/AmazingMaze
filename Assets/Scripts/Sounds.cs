using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip lose;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioWin()
    {
        audioSource.PlayOneShot(win);
    }

    public void AudioLose()
    {
        audioSource.PlayOneShot(lose);
    }
}
