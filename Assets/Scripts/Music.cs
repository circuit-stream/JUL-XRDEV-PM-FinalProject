using System;
using DG.Tweening;
using UnityEngine;

public class Music : MonoBehaviour
{
    [Serializable]
    public class Track
    {
        public AudioSource[] _orbitalStems;
        public AudioSource[] _surfaceStems;
    }

    [SerializeField] private Track[] _tracks;
    [SerializeField] private float _musicVolume;
    [SerializeField] private bool _musicDisabled;

    private int _currentTrackIndex;

    void Update()
    {
        if (_musicDisabled)
            return;
        
        // TESTING: Manual music controls
        if (Input.GetKeyDown("1"))
        {
            FadeOutTrack(3f);
            Debug.Log($"Playing track 1");
            StartTrack(0, 3f);
        }
        if (Input.GetKeyDown("2"))
        {
            FadeOutTrack(3f);
            Debug.Log($"Playing track 2");
            StartTrack(1, 3f);
        }
        if (Input.GetKeyDown("3"))
        {
            FadeOutTrack(3f);
            Debug.Log($"Playing track 3");
            StartTrack(2, 3f);
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FadeInSurfaceStems(3f);
            Debug.Log($"Fading in surface stems");
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FadeOutSurfaceStems(3f);
            Debug.Log($"Fading out surface stems");
        }
    }

    public void FadeOutTrack(float fadeSeconds)
    {
        if (_musicDisabled)
            return;
        
        // Fade out all the orbital stems
        foreach (var orbitalStem in _tracks[_currentTrackIndex]._orbitalStems)
        {
            orbitalStem.DOFade(0, fadeSeconds);
        }
        
        // Fade out all the surface stems
        foreach (var surfaceStem in _tracks[_currentTrackIndex]._surfaceStems)
        {
            surfaceStem.DOFade(0, fadeSeconds);
        }
    }

    public void StartTrack(int trackIndex, float fadeSeconds)
    {
        if (_musicDisabled)
            return;
        
        // Store the new current track index
        _currentTrackIndex = trackIndex;
        
        // Restart all the orbital stems and fade them in
        foreach (var orbitalStem in _tracks[trackIndex]._orbitalStems)
        {
            orbitalStem.volume = 0f;
            orbitalStem.Play();
            orbitalStem.DOFade(_musicVolume, fadeSeconds);
        }
        
        // Start all the surface stems, but silence them
        foreach (var surfaceStem in _tracks[trackIndex]._surfaceStems)
        {
            surfaceStem.volume = 0f;
            surfaceStem.Play();
        }
    }

    public void FadeOutSurfaceStems(float fadeSeconds)
    {
        if (_musicDisabled)
            return;
        
        // Fade out all the surface stems
        foreach (var surfaceStem in _tracks[_currentTrackIndex]._surfaceStems)
        {
            surfaceStem.DOFade(0f, fadeSeconds);
        }
    }
    
    public void FadeInSurfaceStems(float fadeSeconds)
    {
        if (_musicDisabled)
            return;
        
        // Fade in all the surface stems
        foreach (var surfaceStem in _tracks[_currentTrackIndex]._surfaceStems)
        {
            surfaceStem.DOFade(_musicVolume, fadeSeconds);
        }
    }
}
