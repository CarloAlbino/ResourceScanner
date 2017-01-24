using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>{

    [SerializeField, Tooltip("Background audio")]
    private AudioSource m_backgroundAudio;

    [SerializeField, Tooltip("Background audio")]
    private AudioClip m_clickSFX, m_scanSFX, m_extractSFX;

    private AudioSource m_audioSource;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Click()
    {
        m_audioSource.clip = m_clickSFX;
        PlayAudio();
    }

    public void Scan()
    {
        m_audioSource.clip = m_scanSFX;
        PlayAudio();
    }

    public void Extract()
    {
        m_audioSource.clip = m_extractSFX;
        PlayAudio();
    }

    public void Pause()
    {
        m_backgroundAudio.volume = 0.2f;
    }

    public void UnPause()
    {
        m_backgroundAudio.volume = 0.5f;
    }

    private void PlayAudio()
    {
        m_audioSource.Play();
    }

}
