using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioBackgroundManager : MonoBehaviour
{
    private AudioSource[] audioSources;
    private int toggle = 0;

    [SerializeField]
    private AudioClip[] audioClips;

    [SerializeField]
    private AudioMixer audioMixer;

    private double nextStartTime;

    private int nextClip = 0;

    private readonly float dimmedVolume = -20f;
    private readonly float baseVolume = 1;

    public void OnEnable()
    {
        // Shuffle audioclips
        Utility.ShuffleArray(audioClips);

        audioSources = GetComponents<AudioSource>();
        nextStartTime = AudioSettings.dspTime + 0.2;

        ScheduleNextClip();
        
        // Start bg music at low volume
        audioMixer.SetFloat("BackgroundVolume", dimmedVolume);

        // Fade in the background music
        _ = StartCoroutine(
            FadeMixerGroup.StartFade(
                audioMixer,
                "BackgroundVolume",
                5,
                baseVolume));
    }

    private void Update()
    {
        if (AudioSettings.dspTime > nextStartTime - 1)
        {
            ScheduleNextClip();
        }

    }

    private void ScheduleNextClip()
    {
        AudioClip clipToPlay = audioClips[nextClip];
        // Loads the next Clip to play
        // and schedules when it will start
        audioSources[toggle].clip = clipToPlay;
        audioSources[toggle].PlayScheduled(nextStartTime);
        // Checks how long the Clip will last
        // and updates the Next Start Time with a new value
        double duration =
            (double)clipToPlay.samples /
            clipToPlay.frequency;
        nextStartTime += duration;
        // Switches the toggle to use the other Audio Source next
        toggle = 1 - toggle;
        // Increase the clip index number,
        // reset if it runs out of clips
        nextClip = nextClip < audioSources.Length - 1 ?
            nextClip + 1 : 0;
    }
}
