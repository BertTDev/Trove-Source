using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int noOfSections = 6;
    public GameObject[] TopLeft;
    public GameObject[] TopMiddle;
    public GameObject[] TopRight;
    public GameObject[] BottomLeft;
    public GameObject[] BottomMiddle;
    public GameObject[] BottomRight;

    public List<Transform> gemSpawnPoints;
    public Level level;
    public GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        generateSection(TopLeft, new Vector3(-10,5,0));
        generateSection(TopMiddle, new Vector3(0, 5, 0));
        generateSection(TopRight, new Vector3(10, 5, 0));
        generateSection(BottomLeft, new Vector3(-10, -5, 0));
        generateSection(BottomMiddle, new Vector3(0, -5, 0));
        generateSection(BottomRight, new Vector3(10, -5, 0));

        manager.gemSpawnpoints = gemSpawnPoints.ToArray();
    }

    public void generateSection(GameObject[] section, Vector3 pos)
    {
        int identifier = (int)Mathf.Floor(Random.Range(0, section.Length));
        GameObject levelsection = Instantiate(section[identifier], pos, Quaternion.identity);
        level = levelsection.GetComponent<Level>();
        for (int i = 0; i < level.spawnPoints.Length; i++)
        {
            gemSpawnPoints.Add(level.spawnPoints[i]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
