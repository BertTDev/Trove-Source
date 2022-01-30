using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using MilkShake;

public class GameManager : MonoBehaviour
{
    public playerController PlayerController;
    public Transform player;
    [Header("Respawn")]
    public float waitDeath = 0.3f;
    public float waitRespawn = 1f;
    public Transform[] gemSpawnpoints;
    public GameObject[] gemPrefabs;
    public Transform fireballSpawnpoint;
    public GameObject fireballPrefab;

    [Header("Scoring")]
    public GameObject inGameScore;
    public GameObject finalScore;
    public TextMeshPro scoreText;
    public TextMeshProUGUI finalScoreText;
    public int AddOnScore = 0;
    public static int NoOfGems = 4;
    public int GemCount = 4;


    [Header("Camera Shake")]
    public Shaker camShake;
    public ShakePreset fbShake;
    public AudioSource sfxSource;
    public AudioClip roarSfx;

    [Header("Development Tools")]
    public GameObject devToolsWindow;
    bool toolsIsOn = false;
    GameObject[] fireballs = new GameObject[25];


    [Header("Level Creation")]
    public GameObject lmanager;
    public GameObject door;
    public int newLevelGemAmount = 3;

    public static GameManager _instance;
    // Start is called before the first frame update
    void Start()
    {
        if ((PersistentManager._instance.isClassicMode == false) && (PersistentManager._instance.levelCount > 0)) Invoke("createLevel", 0.01f);
        else levelUp();
        
        
        if (PersistentManager._instance.toolsOn) devToolsWindow.SetActive(true);
    }

    private void createLevel()
    {
        loadNewGems();
        if (PersistentManager._instance.levelCount == 1) PersistentManager._instance.fireballCount++;
        scoreText.text = "SCORE - " + PersistentManager._instance.score.ToString();
        if (PersistentManager._instance.levelCount == 1)
        {
            scoreText.text = "SCORE - 0000";
            if(PersistentManager._instance.soundOn)sfxSource.PlayOneShot(roarSfx);
        }
        for (int i = 0; i < PersistentManager._instance.fireballCount; i++)
        {
            loadFireball(4, true);
        }
    }

        private void Update()
        {
        if (Input.GetButtonDown("DevTools"))
        {
            if (!PersistentManager._instance.toolsOn)
            {
                PersistentManager._instance.toolsOn = true;
                devToolsWindow.SetActive(true);
            }
            else
            {
                PersistentManager._instance.toolsOn = false;
                devToolsWindow.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Cheats"))
        {
            PersistentManager._instance.invincOn = !PersistentManager._instance.invincOn;
            PlayerController.setinvincColor();
            PersistentManager._instance.invincBeenOn = true;
        }
        }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PlayerController.getIsAlive())
        {
            PersistentManager._instance.backgroundAudio.Pause();
            respawn();

        }
    }
    void showFinalScore()
    {
        inGameScore.SetActive(false);
        finalScore.SetActive(true);
        finalScoreText.text = "FINAL SCORE - " + PersistentManager._instance.score.ToString();
    }
    void respawn()
    {
        PersistentManager._instance.fireballCount = 0;
        GemCount = 4;
        PersistentManager._instance.audioTime = PersistentManager._instance.backgroundAudio.time;
        StartCoroutine(PersistentManager._instance.reloadScene(2,waitRespawn));
    }

    public void addScore()
    {
        AddOnScore = (((NoOfGems + 1) - GemCount) * 1000);
        PersistentManager._instance.score += AddOnScore;
        scoreText.text = "SCORE - " + PersistentManager._instance.score.ToString();
        GemCount--;
        PersistentManager._instance.allGems++;
        if (GemCount < 1)
        {
            if (PersistentManager._instance.isClassicMode) levelUp();
            else loadDoor();
        }
    }
    
    public void loadDoor()
    {
        if (PersistentManager._instance.allGems % newLevelGemAmount == 0)
        {
            shuffleArray(gemSpawnpoints);
            int spawnRef = 0;
            while (Vector3.Distance(gemSpawnpoints[spawnRef].position, player.position) < 2) spawnRef++;
            Instantiate(door,gemSpawnpoints[spawnRef].position,Quaternion.identity);
            loadFireball(0, true);
            PersistentManager._instance.fireballCount++;
        } else
        {
            levelUp();
        }
    }

    public void levelUp()
    { 
        loadFireball(0, true);
        loadNewGems();
        PersistentManager._instance.fireballCount++;
    }

    public void loadFireball(int xVariance, bool playSound)
    {
        if (!PersistentManager._instance.sleepMode)
        {
            Vector3 spawnPosition = fireballSpawnpoint.position + new Vector3(Random.Range(-xVariance, xVariance), 0, 0);
            fireballs[PersistentManager._instance.fireballCount] = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
            camShake.Shake(fbShake);
            if (PersistentManager._instance.soundOn && !sfxSource.isPlaying && playSound) sfxSource.PlayOneShot(roarSfx);
        }
    }

    public void loadNewGems()
    {
        shuffleArray(gemSpawnpoints);
        int spawnRef = 0;
        for (int i = 0; i < NoOfGems; i++)
        {
            if (Vector3.Distance(gemSpawnpoints[i].position, player.position) < 2) spawnRef++;

            Instantiate(gemPrefabs[i], gemSpawnpoints[spawnRef].position, Quaternion.identity);
            spawnRef++;
        }

        GemCount = NoOfGems;
        
    }

    void shuffleArray(Transform[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = (int)Mathf.Floor(Random.Range(0.0f, 1.0f) * (i + 1));
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
