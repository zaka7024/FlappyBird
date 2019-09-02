using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class soundManager
{
    public static void playSound(AudioClip audioClip)
    {
        GameObject soundPlayer = new GameObject("soundPlayer", typeof(AudioSource));
        var audioSource = soundPlayer.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
    }
} 
