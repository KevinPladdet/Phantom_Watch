using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource[] _audioSources;
    private int _currentIndex;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _audioSources = GetComponentsInChildren<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = 1f, bool randomizePitch = false, Vector3? position = null, float spatialBlend = 0f)
    {
        // Volume
        _audioSources[_currentIndex].volume = volume;

        // Pitch
        if (randomizePitch)
        {
            _audioSources[_currentIndex].pitch = Random.Range(0.85f, 1.15f);
        }
        else
        {
            _audioSources[_currentIndex].pitch = 1f;
        }

        // Space
        if (position == null)
        {
            position = Camera.main.transform.position;
        }
        _audioSources[_currentIndex].transform.position = (Vector3)position;
        _audioSources[_currentIndex].spatialBlend = spatialBlend;

        // Play
        _audioSources[_currentIndex].PlayOneShot(clip);

        _currentIndex = _currentIndex >= _audioSources.Length - 1 ? 0 : _currentIndex + 1;
    }
}
