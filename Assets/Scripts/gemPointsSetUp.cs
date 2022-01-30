using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gemPointsSetUp : MonoBehaviour
{
    public float destroyTime = 1.5f;
    public float riseSpeed = 1;
    public Vector3 pointsOffset;
    void Start()
    {
        transform.localPosition += pointsOffset;
        Destroy(this.gameObject, destroyTime);
    }
    void Update()
    {
        transform.position += new Vector3(0,riseSpeed, 0);
    }
}
