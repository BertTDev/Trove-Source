using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager _instance;
    public GameManager gManager;
    public AudioSource backgroundAudio;
    
    public float audioTime = 0;
    public float gamesPlayed = 0;
    public int score = 0;
    public int highScore = 0;
    public AudioSource backAudioMenu;
    [Header("Cheats + Options")]
    public bool toolsOn = false;
    public bool invincOn = false;
    public bool invincBeenOn = false;
    public bool musicOn = true;
    public bool soundOn = true;
    public bool sleepMode = false;
    public bool firstPlay = true;
    public float startTime = 0;
    public bool isClassicMode = false;
    public bool wallJumping = false;
    public bool isPaused = false;
    public GameObject pauseMenu;
    [Header("Transitions")]
    public Animator transitions;
    public float transitionTimeEntry = 0.5f;
    public float transitionTimeExit = 1;
    public bool transitioning = false;
    public chooseText infoText;

    bool notPlayedDeath = true;
    public AudioClip deathClip;
    [Header("In Game")]
    public int allGems = 0;
    public int fireballCount = 0;
    public int levelCount = 0;
    public bool playerAlive = true;

    public static PersistentManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        startTime = Time.time;
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void notTransitioning()
    {
        transitioning = false;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Invoke("notTransitioning", transitionTimeExit);
        infoText.info.text = "";
        GameObject gmObj = GameObject.Find("GameManager");
        if (gmObj != null) gManager = gmObj.GetComponent<GameManager>();
        gamesPlayed++;
        if( score > highScore && !sleepMode && !invincBeenOn &&scene.buildIndex == 2)
        {
            highScore = score;
        }

        if (scene.buildIndex == 1 || scene.buildIndex == 3)
        {
            notPlayedDeath = true;
            invincBeenOn = false;
            levelCount++;
            if (levelCount == 1) startTime = Time.time;

                transitions.SetTrigger("sceneLoaded");
            if (musicOn && (!backgroundAudio.isPlaying))
            {
                backgroundAudio.time = audioTime;
                backgroundAudio.Play();
            }

        }
        else
        {
            isPaused = false;
            backgroundAudio.Stop();
            GameObject camAudio = GameObject.Find("Main Camera");
            if (camAudio != null) backAudioMenu = camAudio.GetComponent<AudioSource>();
            if (!musicOn) backAudioMenu.Stop();
            backAudioMenu.time = audioTime;
        }
        if (scene.buildIndex == 2)
        {
            transitions.SetTrigger("sceneLoaded");
            if (notPlayedDeath)
            {
                GameObject camAudio = GameObject.Find("Main Camera");
                if (camAudio != null) backAudioMenu = camAudio.GetComponent<AudioSource>();
                if (soundOn) backAudioMenu.PlayOneShot(deathClip);
                notPlayedDeath = false;
            }
        }
    }
    // Start is called before the first frame update
    
    public void newLevel()
    {
        PersistentManager._instance.audioTime = PersistentManager._instance.backgroundAudio.time;
        StartCoroutine(reloadScene(3,transitionTimeEntry));
    }

    public IEnumerator reloadScene(int index, float waitTime)
    {
        transitions.SetTrigger("nextSceneComing");
        transitioning = true;
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(index);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            playerAlive = gManager.PlayerController.getIsAlive();
            if (Input.GetButtonDown("Pause"))
            {
                isPaused = !isPaused;
                if (isPaused) 
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0;    
                }

                else
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }
        
    }
}
