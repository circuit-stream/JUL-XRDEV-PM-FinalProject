using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sounds : MonoBehaviour
{
    public enum Type
    {
        Impact,
        Alarm,
        Pickup,
        ExtinguishFire,
        WarpOut,
        WarpIn,
    }

    [Serializable]
    public class Sound
    {
        public Type _type;
        public AudioClip[] _clips;
    }

    [SerializeField] private Sound[] _sounds;
    [SerializeField] private AudioSource _audioSourcePrefab;
    
    public void PlayAtSource(Type type, Transform source, float volume = 1f)
    {
        // Get a random sound of the given type
        var randomSound = GetRandomSound(type);
        if (randomSound == null)
        {
            Debug.LogError($"Couldn't find a sound of the type {type}");
            return;
        }
        
        // Spawn an audio source at the position of the sound
        var audioSource = Instantiate(_audioSourcePrefab, source.position, Quaternion.identity, source);
        
        // Play a random sound of the given type at the given volume from the audio source
        audioSource.PlayOneShot(randomSound, volume);
        Debug.Log($"Playing {randomSound.name} with volume {volume}");
        
        // Destroy the audio source once the sound has finished
        Destroy(audioSource.gameObject, randomSound.length);
    }

    private AudioClip GetRandomSound(Type type)
    {
        // For each available sound
        foreach (var sound in _sounds)
        {
            // If this is the right type of sound
            if (sound._type == type)
                // Return a random audio clip for the sound
                return sound._clips[Random.Range(0, sound._clips.Length)];
        }

        // Couldn't find the sound
        return null;
    }

    public float GetSoundLength(Type type)
    {
        return GetRandomSound(type).length;
    }
}
