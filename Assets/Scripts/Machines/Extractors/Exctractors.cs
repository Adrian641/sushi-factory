using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exctractors : MonoBehaviour
{
    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 hitPoint = Vector2.zero;

    public Vector3 dir;

    public int itemToProduce;

    public GameObject Algae;
    public GameObject Green;
    public GameObject Coral;

    float dt = 0f;
    public int SpawnSpeed = 5;
    void Start()
    {
        Transform child = gameObject.transform.GetChild(0);
        if (child.name[child.name.Length - 1] == 'U')
            dir = Vector2.up;
        else if (child.name[child.name.Length - 1] == 'D')
            dir = Vector2.down;
        else if (child.name[child.name.Length - 1] == 'L')
            dir = Vector2.left;
        else if (child.name[child.name.Length - 1] == 'R')
            dir = Vector2.right;
        else
            Destroy(gameObject);

        // x + 1567.53
        // z - 4.21025
        //realPos = new Vector3(transform.position.x + 1567.53f, 0.15f, transform.position.z - 4.21025f);
        RaycastHit[] RayHit = Physics.RaycastAll(new Vector3(transform.position.x, 10f, transform.position.z), Vector3.down, 20f);
        for (int i = 0; i < RayHit.Length; i++)
        {
            if (RayHit[i].collider.CompareTag("Algea"))
                itemToProduce = 1;
            if (RayHit[i].collider.CompareTag("Green"))
                itemToProduce = 2;
            if (RayHit[i].collider.CompareTag("Coral"))
                itemToProduce = 3;
        }
    }
    void Update()
    {
        dt += Time.deltaTime;

        if (dt > SpawnSpeed)
        {
            GameObject objectToSpawn = null;
            if (itemToProduce == 1)
                objectToSpawn = Algae;
            else if (itemToProduce == 2)
                objectToSpawn = Green;
            else if (itemToProduce == 3)
                objectToSpawn = Coral;

            GameObject item = Instantiate(objectToSpawn,new Vector3(transform.position.x - dir.x, 0.2f, transform.position.z + dir.y), Quaternion.identity);
            item.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
