using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public float SpawningTime;
    public GameObject ToSpawn1, ToSpawn2, ToSpawn3, ToSpawn4, ToSpawn5, ToSpawn6;
    public int pickObject;

    public GameObject mainCam;

    public float camX;
    public float camY;

    void Start()
    {
        SpawningTime = Random.Range(2.5f, 5.5f);
    }

    void SpawnNow()
    {
        camX = mainCam.transform.position.x;
        camY = mainCam.transform.position.z;

        Quaternion rotation = Quaternion.Euler(90f, 0, 0);

        if (pickObject == 1)
        {
            float x = Random.Range(camX + 10f, camX - 10f);
            float y = Random.Range(camY + 10f, camY - 10f);
            Instantiate(ToSpawn1, new Vector3(x, 4, y), rotation);
        }
        if (pickObject == 2)
        {
            float x = Random.Range(camX + 10f, camX - 10f);
            float y = Random.Range(camY + 10f, camY - 10f);
            Instantiate(ToSpawn2, new Vector3(x, 4, y), rotation);
        }
        if (pickObject == 3)
        {
            float x = Random.Range(camX + 10f, camX - 10f);
            float y = Random.Range(camY + 10f, camY - 10f);
            Instantiate(ToSpawn3, new Vector3(x, 4, y), rotation);
        }
        if (pickObject == 4)
        {
            float x = Random.Range(camX + 10f, camX - 10f);
            float y = Random.Range(camY + 10f, camY - 10f);
            Instantiate(ToSpawn4, new Vector3(x, 4, y), rotation);
        }
        if (pickObject == 5)
        {
            float x = Random.Range(camX + 10f, camX - 10f);
            float y = Random.Range(camY + 10f, camY - 10f);
            Instantiate(ToSpawn5, new Vector3(x, 4, y), rotation);
        }
        if (pickObject == 6)
        {
            float x = Random.Range(camX + 10f, camX - 10f);
            float y = Random.Range(camY + 10f, camY - 10f);
            Instantiate(ToSpawn6, new Vector3(x, 4, y), rotation);
        }
    }

    void Update()
    {
        SpawningTime -= Time.deltaTime;
        if (SpawningTime <= 0)
        {
            pickObject = Random.Range(1, 7);
            SpawnNow();
            SpawningTime = Random.Range(2.5f, 5.5f);
        }
    }
}
