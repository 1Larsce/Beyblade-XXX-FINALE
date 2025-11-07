using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    [Header("Assign your background track here")]
    public AudioClip musicClip;
    public float volume = 0.5f;

    [Header("Scenes where the music should play")]
    public string[] allowedScenes = { "Menu", "Level1", "FinalScene" };

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Audio setup
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.volume = volume;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // Play immediately if starting in an allowed scene
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isAllowed = false;

        foreach (var s in allowedScenes)
        {
            if (scene.name == s)
            {
                isAllowed = true;
                break;
            }
        }

        if (isAllowed)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
