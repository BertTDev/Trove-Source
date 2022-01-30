using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class classicinfomanager : MonoBehaviour
{
    public TextMeshProUGUI info;
    bool mode;
    public string classicText = "- CLASSIC -";
    public string mainText = "- OVER 700 PROCEDURAL LEVELS! -";
    void Start()
    {
        mode = PersistentManager._instance.isClassicMode;
        if (PersistentManager._instance.isClassicMode) info.text = classicText;
        else info.text = mainText;
    }

    // Update is called once per frame
    void Update()
    {
        if(mode != PersistentManager._instance.isClassicMode)
        {
            if (PersistentManager._instance.isClassicMode) info.text = classicText;
            else info.text = mainText;
        }
        mode = PersistentManager._instance.isClassicMode;
    }
}
