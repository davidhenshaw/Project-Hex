using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : PersistentSingleton<AudioManager>
{

    [Header("Sources")]

    [SerializeField]
    AudioSource sfxSource;
    [SerializeField]
    AudioSource musicSource;
    [SerializeField]
    AudioSource ambienceSource;

    [Space]

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip meadowAmbience;

    [Header("Bee sounds")]
    public AudioClip beeMoved;
    public AudioClip beeDied;
    public AudioClip beeLand;
    public AudioClip beeEnteredHive;
    
    [Header("Flower sounds")]
    public AudioClip flowerPollenated;
    public AudioClip allFlowersPollenated;

    [Header("Gameplay sounds")]
    public AudioClip levelComplete;

    private string[] _snapshotNames = { "Main", "Only Ambience", "Silent" };
    private AudioMixerSnapshot[] _snapshots;

    [Space]

    [SerializeField]
    AudioMixer _mixer;

    void Start()
    {
        LoadSnapshots();
    }

    public static void PlayOneShot(AudioClip sfx)
    {
        if(!Instance)
        {
            Debug.LogError("Cannot play audio. Audio Manager instance does not exist");
            return;
        }

        if (!Instance.sfxSource)
            return;

        Instance.sfxSource.PlayOneShot(sfx);
    }

    void LoadSnapshots()
    {
        _snapshots = new AudioMixerSnapshot[_snapshotNames.Length];

        for(int i = 0; i < _snapshotNames.Length; i++)
        {
            _snapshots[i] = _mixer.FindSnapshot(_snapshotNames[i]);
        }
    }

}
