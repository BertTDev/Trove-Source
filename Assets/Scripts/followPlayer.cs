using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;
public class followPlayer : MonoBehaviour
{
    Transform player;
    public Rigidbody2D rb;
    public LayerMask playerLayer;
    public AudioSource sfxSource;
    public AudioClip whooshSfx;
    public AudioClip fireNoise;
    public float whooshDistance = 4;
    public float movementSpeed = 2;
    public float movementStrength = 2;
    public float DirectionStrength = 1;
    public float RandomStrength = 2;
    Vector2 Movement;

    public Shaker camShake;
    public ShakePreset fbShake;
    public float shakeDistance = 1.5f;
    public float TimeAlive = 0;
    float dist = 100;
    bool isSet = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        player = playerObj.transform;
        GameObject mainCam = GameObject.Find("Main Camera");
        camShake = mainCam.GetComponent<Shaker>();
    }
    void Update()
    {
        if (dist < whooshDistance && PersistentManager._instance.soundOn)
        {
            float vol = ((whooshDistance - dist)/ whooshDistance) *0.5f;
            sfxSource.volume = vol;
            
        } else
        {
            sfxSource.volume = 0;
        }
        if(dist < shakeDistance)
        {
            camShake.Shake(fbShake);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        dist = Vector3.Distance(rb.position, player.position);

        Vector2 Dir = new Vector2((player.position.x - rb.position.x), (player.position.y - rb.position.y));
        float mag = Dir.magnitude;
        Dir = Dir / mag;
        Movement = ((Dir * DirectionStrength) + (Movement * movementStrength) + (new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f))*RandomStrength)) / (movementStrength + DirectionStrength);
        rb.velocity = Movement * movementSpeed;
        if (PersistentManager._instance.transitioning) rb.velocity = new Vector2(0, 0);
    }
}
