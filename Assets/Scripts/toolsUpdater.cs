using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class toolsUpdater : MonoBehaviour
{
    public GameManager manager;

    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI timePlayedText;
    public TextMeshProUGUI fireballsText;
    public TextMeshProUGUI gemsText;

    public float fpsAcca = 0;
    public int counter = 0;
    public float TimerMax = 0.1f;
    float timer = 0;
    void Start()
    {
        fpsAcca = 0;
        counter = 0;
        timer = 0;
    }

    private void Awake()
    {
        fpsAcca = 0;
        counter = 0;
        timer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        fpsAcca += 1.0f / Time.deltaTime;
        counter++;

        if (timer > TimerMax)
        {
            fpsText.text = (fpsAcca / counter).ToString("F1");
            fpsAcca = 0;
            counter = 0;
            timePlayedText.text = (Time.time- PersistentManager._instance.startTime).ToString("F1");
            fireballsText.text = PersistentManager._instance.fireballCount.ToString();
            gemsText.text = PersistentManager._instance.allGems.ToString();
            timer -= TimerMax;
        }
        timer += Time.deltaTime;
    }
}
