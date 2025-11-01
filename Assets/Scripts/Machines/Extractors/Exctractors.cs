using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exctractors : MonoBehaviour
{
    public ItemsSystem itemsSystem;

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
    void FixedUpdate()
    {
        dt += Time.deltaTime;

        if (dt > SpawnSpeed)
        {
            dt = 0;
            GameObject objectToSpawn = null;
            if (itemToProduce == 1)
                objectToSpawn = Algae;
            else if (itemToProduce == 2)
                objectToSpawn = Green;
            else if (itemToProduce == 3)
                objectToSpawn = Coral;

            GameObject item = Instantiate(objectToSpawn, new Vector3(transform.position.x - (dir.x / 1.2f), 0f, transform.position.z + (dir.y / 1.2f)), Quaternion.identity);
            item.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            ItemsSystem.ConveyorBeltItem newConveyorItem = new ItemsSystem.ConveyorBeltItem();
            newConveyorItem.item = item.transform;
            newConveyorItem.currentLerp = 0f;
            newConveyorItem.startPoint = 0;

            bool isConnected = false;
            RaycastHit[] raycastHits = Physics.RaycastAll(new Vector3(transform.position.x - dir.x, 10f, transform.position.z + dir.y), Vector3.down, 20f);
            for (int i = 0; i < raycastHits.Length; ++i)
            {
                if (raycastHits[i].collider.CompareTag("Belt"))
                {
                    Transform conveyorPos = raycastHits[i].collider.transform.parent;
                    Transform conveyorGroup = conveyorPos.parent;
                    itemsSystem = conveyorGroup.gameObject.GetComponent<ItemsSystem>();
                    isConnected = true;
                }
            }
            if (!isConnected)
                DestroyImmediate(item);
            else 
                itemsSystem.items.Add(newConveyorItem);
        }
    }
}
