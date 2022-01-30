using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playManager : MonoBehaviour
{
    public Scene gameScene;
    public float waitTime = 1;
    public GameObject instructions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
                instructions.SetActive(true);
                Invoke("loadPlay", waitTime);
                PersistentManager._instance.firstPlay = false;           
        }
    }

    void loadPlay()
    {
        if(PersistentManager._instance.isClassicMode) SceneManager.LoadScene(1);
        else SceneManager.LoadScene(3);
    }
}
