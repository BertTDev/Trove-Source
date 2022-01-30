using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class doorPickup : MonoBehaviour
{
    public GameObject pointsText;
    public Color pointsColor;
    // Start is called before the first frame update
    public AudioSource source;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(PersistentManager._instance.soundOn)source.Play();
            PersistentManager._instance.newLevel();
            var txt = Instantiate(pointsText, (this.transform.position), Quaternion.identity);
            PersistentManager._instance.score += 10000;
            PersistentManager._instance.gManager.scoreText.text = "SCORE - " + PersistentManager._instance.score.ToString();
            txt.GetComponent<TextMeshPro>().text = "+10000";
            txt.GetComponent<TextMeshPro>().color = pointsColor;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
