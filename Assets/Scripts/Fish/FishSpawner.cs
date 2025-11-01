using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public float SpawningTime;
    public GameObject salmon, tuna, orangeFish, sharky, jelly, smallJelly;
    public int pickObject;

    public GameObject mainCam;

    public float camX;
    public float camY;

    void Start()
    {
        SpawningTime = Random.Range(1.5f, 3.5f);
    }

    void SpawnNow()
    {
        camX = mainCam.transform.position.x;
        camY = mainCam.transform.position.z;

        Quaternion rotation = Quaternion.Euler(90f, 0, 0);

        if (pickObject == 1)
        {
            float x = Random.Range(camX + 20f, camX + 20f);
            float y = Random.Range(camY + 20f, camY + 20f);

            if (x > -10f + camX && x <= 0f + camX)
                x = Random.Range(-20f, -10f) + camX;
            else if (x > 0f + camX && x < 10f + camX)
                x = Random.Range(20f, 10f) + camX;
            if (y > -10f + camY && y <= 0f + camY)
                y = Random.Range(-20f, -10f) + camY;
            else if (y > 0f + camY && y < 10f + camY)
                y = Random.Range(20f, 10f) + camY;

            Instantiate(salmon, new Vector3(x, 6, y), rotation);
        }
        if (pickObject == 2)
        {
            float x = Random.Range(camX + 20f, camX + 20f);
            float y = Random.Range(camY + 20f, camY + 20f);

            if (x > -10f + camX && x <= 0f + camX)
                x = Random.Range(-20f, -10f) + camX;
            else if (x > 0f + camX && x < 10f + camX)
                x = Random.Range(20f, 10f) + camX;
            if (y > -10f + camY && y <= 0f + camY)
                y = Random.Range(-20f, -10f) + camY;
            else if (y > 0f + camY && y < 10f + camY)
                y = Random.Range(20f, 10f) + camY;

            Instantiate(tuna, new Vector3(x, 6, y), rotation);
        }
        if (pickObject == 3)
        {
            float x = Random.Range(camX + 20f, camX + 20f);
            float y = Random.Range(camY + 20f, camY + 20f);

            if (x > -10f + camX && x <= 0f + camX)
                x = Random.Range(-20f, -10f) + camX;
            else if (x > 0f + camX && x < 10f + camX)
                x = Random.Range(20f, 10f) + camX;
            if (y > -10f + camY && y <= 0f + camY)
                y = Random.Range(-20f, -10f) + camY;
            else if (y > 0f + camY && y < 10f + camY)
                y = Random.Range(20f, 10f) + camY;

            Instantiate(orangeFish, new Vector3(x, 4, y), rotation);
        }
        if (pickObject == 4)
        {
            float x = Random.Range(camX + 20f, camX + 20f);
            float y = Random.Range(camY + 20f, camY + 20f);

            if (x > -10f + camX && x <= 0f + camX)
                x = Random.Range(-20f, -10f) + camX;
            else if (x > 0f + camX && x < 10f + camX)
                x = Random.Range(20f, 10f) + camX;
            if (y > -10f + camY && y <= 0f + camY)
                y = Random.Range(-20f, -10f) + camY;
            else if (y > 0f + camY && y < 10f + camY)
                y = Random.Range(20f, 10f) + camY;

            Instantiate(sharky, new Vector3(x, 4, y), rotation);
        }
        if (pickObject == 5)
        {
            float x = Random.Range(camX + 20f, camX + 20f);
            float y = Random.Range(camY + 20f, camY + 20f);

            if (x > -10f + camX && x <= 0f + camX)
                x = Random.Range(-20f, -10f) + camX;
            else if (x > 0f + camX && x < 10f + camX)
                x = Random.Range(20f, 10f) + camX;
            if (y > -10f + camY && y <= 0f + camY)
                y = Random.Range(-20f, -10f) + camY;
            else if (y > 0f + camY && y < 10f + camY)
                y = Random.Range(20f, 10f) + camY;

            Instantiate(jelly, new Vector3(x, 4, y), rotation);
        }
        if (pickObject == 6)
        {
            float x = Random.Range(camX + 20f, camX + 20f);
            float y = Random.Range(camY + 20f, camY + 20f);

            if (x > -10f + camX && x <= 0f + camX)
                x = Random.Range(-20f, -10f) + camX;
            else if (x > 0f + camX && x < 10f + camX)
                x = Random.Range(20f, 10f) + camX;
            if (y > -10f + camY && y <= 0f + camY)
                y = Random.Range(-20f, -10f) + camY;
            else if (y > 0f + camY && y < 10f + camY)
                y = Random.Range(20f, 10f) + camY;

            Instantiate(smallJelly, new Vector3(x, 4, y), rotation);
        }
    }

    void Update()
    {
        SpawningTime -= Time.deltaTime;
        if (SpawningTime <= 0)
        {
            pickObject = Random.Range(1, 7);
            SpawnNow();
            SpawningTime = Random.Range(1.5f, 3.5f);
        }
    }
}
