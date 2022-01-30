using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class gemPickup : MonoBehaviour
{
    public AudioSource sfx;
    public SpriteRenderer rend;
    public Collider2D col;
    public GameObject pointsText;
    public Color pointsColor;
    public ParticleSystem pSystem;


    [Header("Animation")]
    public float ySpeed = 0.1f;
    public float yTop = 0.2f;
    bool goingUp = true;
    public Vector3 startPos;

    private void Start()
    {
        startPos = this.transform.position;
        yTop += startPos.y;
        this.transform.localPosition += new Vector3(0,Random.Range(0,0.2f),0); 
    }

    private void Update()
    {
        Vector3 pos = this.transform.localPosition;
        if (pos.y >= yTop && goingUp) goingUp = false;
        else if (pos.y <= startPos.y && !goingUp) goingUp = true;

        if (goingUp) this.transform.localPosition += new Vector3(0, ySpeed*Time.deltaTime, 0);
        else this.transform.localPosition -= new Vector3(0, ySpeed*Time.deltaTime, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            pSystem.Play();
            rend.enabled = false;
            col.enabled = false;
            var txt = Instantiate(pointsText, (this.transform.position),Quaternion.identity);

            PersistentManager._instance.gManager.addScore();

            if (PersistentManager._instance.soundOn)
            {
                /*sfx.pitch = .95f;*/
                sfx.PlayOneShot(sfx.clip);
            }

            txt.GetComponent<TextMeshPro>().text = "+" + PersistentManager._instance.gManager.AddOnScore.ToString();
            txt.GetComponent<TextMeshPro>().color = pointsColor;
            Invoke("goodbye", 1f);
            

        }
    }
    void goodbye()
    {
        Destroy(gameObject);
    }
}
