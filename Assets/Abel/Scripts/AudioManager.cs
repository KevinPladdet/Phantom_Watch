using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private GameObject _sfxPool;
    private AudioSource[] _sfxSources;
    private int _currentIndex;

    [Header("Ambiance")]
    [SerializeField] private AudioSource _officeAmbiance;
    private float _officeVolume;
    [SerializeField] private AudioLowPassFilter _officeFilter;
    [SerializeField] private float _filterCutoffFrequency = 500f;
    private float _filterDefault;

    [Space(10)]
    [SerializeField] private AudioSource _cameraAmbiance;
    private float _cameraVolume;

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

        _sfxSources = _sfxPool.GetComponentsInChildren<AudioSource>();

        _officeVolume = _officeAmbiance.volume;
        _cameraVolume = _cameraAmbiance.volume;
        _filterDefault = _officeFilter.cutoffFrequency;
        SetAmbiance(false, false);
    }

    public void PlaySound(AudioClip clip, float volume = 1f, bool randomizePitch = false, Vector3? position = null, float spatialBlend = 0f)
    {
        // Volume
        _sfxSources[_currentIndex].volume = volume;

        // Pitch
        if (randomizePitch)
        {
            _sfxSources[_currentIndex].pitch = Random.Range(0.85f, 1.15f);
        }
        else
        {
            _sfxSources[_currentIndex].pitch = 1f;
        }

        // Space
        if (position == null)
        {
            position = Camera.main.transform.position;
        }
        _sfxSources[_currentIndex].transform.position = (Vector3)position;
        _sfxSources[_currentIndex].spatialBlend = spatialBlend;

        // Play
        _sfxSources[_currentIndex].PlayOneShot(clip);

        _currentIndex = _currentIndex >= _sfxSources.Length - 1 ? 0 : _currentIndex + 1;
    }

    public void SetAmbiance(bool officeActive, bool cameraActive)
    {
        _officeAmbiance.volume = officeActive ? _officeVolume : 0f;
        _officeFilter.cutoffFrequency = _filterDefault;
        _cameraAmbiance.volume = cameraActive ? _cameraVolume : 0f;

        // Office Low Pass filter is only applied if both Office and Camera ambiances are active.
        if (officeActive && cameraActive)
        {
            _officeFilter.cutoffFrequency = _filterCutoffFrequency;
            _officeAmbiance.volume = _officeVolume / 2;
        }
    }

    public void PauseAmbiance()
    {
        if (_officeAmbiance.isPlaying && _cameraAmbiance.isPlaying)
        {
            _officeAmbiance.Pause();
            _cameraAmbiance.Pause();
        }
    }
    public void ResumeAmbiance()
    {
        if (!_officeAmbiance.isPlaying && !_cameraAmbiance.isPlaying)
        {
            _officeAmbiance.UnPause();
            _cameraAmbiance.UnPause();
        }
    }
}
