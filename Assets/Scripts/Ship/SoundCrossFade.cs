using System;
using UnityEngine;

internal class SoundCrossFade : MonoBehaviour
{
    public AudioSource soundA;
    public AudioSource soundB;

    private float volume;
    private float crossfade;

    internal void SetVolume(float volume)
    {
        this.volume = volume;

        UpdateSounds();
    }

    internal void SetCrossFade(float amount)
    {
        crossfade = amount;

        UpdateSounds();
    }

    private void UpdateSounds()
    {
        soundA.volume = Mathf.Lerp(volume, 0, crossfade);
        soundB.volume = Mathf.Lerp(0, volume, crossfade);
    }
}