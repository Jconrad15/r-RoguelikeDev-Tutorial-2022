using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource effectAudioSource;

    public void Initialize()
    {
        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(Entity player)
    {
        player.RegisterOnEntityMoved(OnPlayerMoved);
    }

    private void OnPlayerMoved(Entity player)
    {
        PlayAudioClipByName(
            "playerMove",
            Random.Range(0.7f, 0.8f),
            Random.Range(0.8f, 1.2f));
    }

    private AudioClip LoadAudioClip(string name)
    {
        return (AudioClip)Resources.Load(
            "Audio/" + name, typeof(AudioClip));
    }

    private void PlayAudioClipByName(
        string name, float volume)
    {
        effectAudioSource.pitch = 1;

        AudioClip clip = LoadAudioClip(name);
        effectAudioSource.PlayOneShot(clip, volume);
    }

    private void PlayAudioClipByName(
        string name, float volume, float pitch)
    {
        effectAudioSource.pitch = pitch;

        AudioClip clip = LoadAudioClip(name);
        effectAudioSource.PlayOneShot(clip, volume);
    }


}
