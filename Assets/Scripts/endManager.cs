using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class endManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        finalScoreText.text = "FINAL SCORE - " + PersistentManager._instance.score.ToString();
        highScoreText.text = "HI-SCORE - " + PersistentManager._instance.highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            PersistentManager._instance.fireballCount = 0;
            PersistentManager._instance.allGems = 0;
            PersistentManager._instance.score = 0;
            PersistentManager._instance.levelCount = 0;
            PersistentManager._instance.audioTime = 0;

            if (PersistentManager._instance.isClassicMode) SceneManager.LoadScene(1);
            else SceneManager.LoadScene(3);


        }
        if (Input.GetButtonDown("Fire1"))
        {
            PersistentManager._instance.fireballCount = 0;
            PersistentManager._instance.allGems = 0;
            PersistentManager._instance.score = 0;
            PersistentManager._instance.levelCount = 0;
            SceneManager.LoadScene(0);
        }
    }
}
