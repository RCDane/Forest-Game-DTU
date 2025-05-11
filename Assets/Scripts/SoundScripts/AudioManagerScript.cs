using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic; // For gameplay
    public AudioClip menuMusic;       // For main menu
    public AudioClip cameraClick;
    public AudioClip cameraPrint;
    public AudioClip creature1sound;
    public AudioClip creature2sound;

    [Header("Ambient Sounds")]
    [SerializeField] private GameObject ambientSounds;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Start menu music on launch
        SwitchToMenuMusic();
    }

    // Play a one-shot sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // Switch music tracks immediately
    private void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.clip != musicClip)
        {
            musicSource.Stop();
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }

    // Public methods to switch music
    public void SwitchToMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void SwitchToGameplayMusic()
    {
        PlayMusic(backgroundMusic);
    }

    // Volume controls (no saving)
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    // Start all ambient sounds
    public void StartAmbientSounds()
    {
        if (ambientSounds == null)
        {
            Debug.LogWarning("AmbientSounds GameObject not assigned.");
            return;
        }

        AudioSource[] sources = ambientSounds.GetComponentsInChildren<AudioSource>();
        foreach (var source in sources)
        {
            if (!source.isPlaying)
                source.Play();
        }
    }

    // Stop all ambient sounds
    public void StopAmbientSounds()
    {
        if (ambientSounds == null) return;

        AudioSource[] sources = ambientSounds.GetComponentsInChildren<AudioSource>();
        foreach (var source in sources)
        {
            if (source.isPlaying)
                source.Stop();
        }
    }
    

    

    /*
    ===========================
    === Usage Examples ========
    ===========================

    // Play sound effects
    AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.cameraClick);

    // Switch music
    AudioManagerScript.Instance.SwitchToGameplayMusic();
    AudioManagerScript.Instance.SwitchToMenuMusic();

    // Adjust volumes
    AudioManagerScript.Instance.SetMusicVolume(0.6f);
    AudioManagerScript.Instance.SetSFXVolume(1.0f);

    // Start/Stop ambient background audio
    AudioManagerScript.Instance.StartAmbientSounds();
    AudioManagerScript.Instance.StopAmbientSounds();

    // Example when "Play" button is clicked
    void OnPlayButtonPressed()
    {
        AudioManagerScript.Instance.SwitchToGameplayMusic();
        AudioManagerScript.Instance.StartAmbientSounds();

        // Load the gameplay scene...
        SceneManager.LoadScene("GameScene");
    }


    --------------------------------------------------------------------
    //Other example 

    using UnityEngine;

    public class ExampleScript : MonoBehaviour
    {
    public AudioClip clickSound;

    void Start()
    {
        AudioManagerScript.Instance.PlaySFX(clickSound);
        AudioManagerScript.Instance.SwitchToGameplayMusic();
    }

    public void OnSliderChanged(float value)
    {
        AudioManagerScript.Instance.SetMusicVolume(value);
    }
}

    */
}





