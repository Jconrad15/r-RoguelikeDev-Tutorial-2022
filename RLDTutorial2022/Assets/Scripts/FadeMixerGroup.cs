using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
public static class FadeMixerGroup
{
    public static IEnumerator StartFade(
        AudioMixer audioMixer, string exposedParam,
        float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(
                currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(
                exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }

    public static IEnumerator StartFadeOutFadeIn(
        AudioMixer audioMixer, string exposedParam,
        float duration)
    {
        Debug.Log("StartFadeOutFadeIn");
        float currentTime = 0;
        audioMixer.GetFloat(exposedParam, out float currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);

        float maxVol = currentVol;
        float halfDuration = duration / 2f;

        // Fade out
        float targetValue = Mathf.Clamp(0, 0.0001f, 1);
        while (currentTime < halfDuration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(
                currentVol, targetValue, currentTime / halfDuration);
            audioMixer.SetFloat(
                exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        // Fade in
        targetValue = Mathf.Clamp(maxVol, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(
                currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(
                exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        yield break;
    }
}