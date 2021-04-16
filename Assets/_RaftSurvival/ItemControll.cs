using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControll : MonoBehaviour
{

    public Vector3 moveDirection;

    public float speed;

    public List<Item> itemPrefabs = new List<Item>();

    public Vector3 startSpawnPos;
    public Vector3 endSpawnPos;
    public Vector3 offset;
    public float step;
    public float timerToSpawn;
    float _timerToSpawn;

    private void OnValidate()
    {
        startSpawnPos.x = offset.x;
        endSpawnPos.x = offset.x;
    }

    private void Awake()
    {
        _timerToSpawn = timerToSpawn;

        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveItems();
        SpawnItems();
    }

    void SpawnItems()
    {
        if(timerToSpawn > 0)
        {
            timerToSpawn -= Time.deltaTime;
            if(timerToSpawn <= 0)
            {
                timerToSpawn = _timerToSpawn;

                Vector3 randomSpawnPos = new Vector3(startSpawnPos.x, 1, Random.Range(startSpawnPos.z, endSpawnPos.z));
                Instantiate(itemPrefabs[Random.Range(0,itemPrefabs.Count)], randomSpawnPos,Quaternion.identity);
            }
        }
    }

    void MoveItems()
    {
        foreach(var item in Item.allItems)
        {
            if (item.isGrabed) continue;
            item.transform.position = item.transform.position += moveDirection.normalized * speed * Time.deltaTime;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(startSpawnPos, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(endSpawnPos, Vector3.one);
    }

}
