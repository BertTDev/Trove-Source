using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class colorChange : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float changeTime = 1;
    float cTimer;
    public Color[] colors;
    int cState = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cTimer += Time.deltaTime;
        if (cTimer > changeTime)
        {
            cState++;
            if (cState > (colors.Length-1)) cState = 0;
            text.color = colors[cState];
            cTimer = 0;
        }
    }
}
