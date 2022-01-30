using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class chooseText : MonoBehaviour
{
    public TextMeshProUGUI info;
    bool wasTransitioning = false;
    public string[] motivators;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!wasTransitioning && PersistentManager._instance.transitioning)
        {
            info.text = motivators[Random.Range(0, motivators.Length)];
            if (PersistentManager._instance.sleepMode) info.text = "ZZZ...";
            if (!PersistentManager._instance.playerAlive) info.text = "";
        }
        wasTransitioning = PersistentManager._instance.transitioning;
    }
}
