using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    public ItemsSystem itemsSystem;

    public string code;
    public Vector2 dir;

    public int error;

    public GameObject greenCut01;
    public GameObject coralCut02;

    void Start()
    {
        dir = GetObjectDir();
    }

    void FixedUpdate()
    {
        RaycastHit[] RayHit = Physics.RaycastAll(new Vector3(transform.position.x - (dir.x / 2), 10f, transform.position.z - (dir.y / 2)), Vector3.down, 20f);
        for (int i = 0; i < RayHit.Length; i++)
        {
            if (RayHit[i].collider.CompareTag("Item"))
            {
                error = 0;
                code = GetType(RayHit[i].collider.gameObject.name);

                GameObject itemCut = Cut(code);
                Destroy(RayHit[i].collider.gameObject);
                if (itemCut != null)
                {
                    GameObject item = Instantiate(itemCut, new Vector3(transform.position.x - (dir.x / 1.2f), 0f, transform.position.z - (dir.y / 1.2f)), Quaternion.identity);
                    item.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

                    ItemsSystem.ConveyorBeltItem newConveyorItem = new ItemsSystem.ConveyorBeltItem();
                    newConveyorItem.item = item.transform;
                    newConveyorItem.currentLerp = 0f;
                    newConveyorItem.startPoint = 0;


                    bool isConnected = false;
                    RaycastHit[] raycastHits = Physics.RaycastAll(new Vector3(transform.position.x + dir.x, 10f, transform.position.z + dir.y), Vector3.down, 20f);
                    for (int j = 0; j < raycastHits.Length; ++j)
                    {
                        if (raycastHits[j].collider.CompareTag("Belt"))
                        {
                            Transform conveyorPos = raycastHits[j].collider.transform.parent;
                            Transform conveyorGroup = conveyorPos.parent;
                            itemsSystem = conveyorGroup.gameObject.GetComponent<ItemsSystem>();
                            isConnected = true;
                        }
                    }
                    if (!isConnected)
                    {
                        Destroy(item);
                        error = -1;
                    }
                    else
                        itemsSystem.items.Add(newConveyorItem);
                }
                else
                {
                    error = -1;
                }
            }
        }
    }

    static string GetType(string name)
    {
        string typeString = "";
        for (int i = 0; i < name.Length; i++)
        {
            if (!Char.IsDigit(name[i]))
                break;
            else
                typeString += name[i];
        }
        string type = typeString;
        return type;
    }

    Vector2 GetObjectDir()
    {
        Vector2 dir = Vector2.zero;
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

        return dir;
    }

    GameObject Cut(string name)
    {
        if (name[name.Length - 2] == '0')
        {
            if (name == "01")
                return greenCut01;
            if (name == "02")
                return coralCut02;
        }
        return null;
    }
}
