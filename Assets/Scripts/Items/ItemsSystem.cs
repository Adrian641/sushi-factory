using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ItemsSystem : MonoBehaviour
{
    [System.Serializable]
    public class ConveyorBeltItem
    {
        public Transform item;
        public float currentLerp;
        public int startPoint;
    }

    [SerializeField] private float itemSpacing;
    [SerializeField] private float speed;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] public List<ConveyorBeltItem> items;

    private Transform beltGroup;

    public Vector2[] beltPos;

    private float timeBetweenUpdate = -1f;
    private float dt = 0;

    private void Awake()
    {
        beltGroup = this.transform;
    }

    void FixedUpdate()
    {
        dt += Time.fixedDeltaTime;

        if (dt > timeBetweenUpdate)
        {
            dt = 0;

            int amountOfPos = 2;
            for (int i = 0; i < beltGroup.childCount; i++)
            {
                Transform BeltPos = beltGroup.GetChild(i);
                Transform BeltType = BeltPos.GetChild(0);

                if (GetType(BeltType.name) > 5)
                {
                    amountOfPos++;
                }
            }
            beltPos = new Vector2[amountOfPos];
            int amountOfCorners = 0;
            for (int i = 0; i < beltGroup.childCount; i++)
            {
                Transform BeltPos = beltGroup.GetChild(i);
                Transform BeltType = BeltPos.GetChild(0);
                if (i == 0)
                {
                    int orientation = GetType(BeltType.name);
                    if (orientation == 1)
                        beltPos[0] = GetPos(BeltPos.name) - (Vector2.up / 2);
                    else if (orientation == 2)
                        beltPos[0] = GetPos(BeltPos.name) - (Vector2.down / 2);
                    else if (orientation == 3)
                        beltPos[0] = GetPos(BeltPos.name) - (Vector2.left / 2);
                    else if (orientation == 4)
                        beltPos[0] = GetPos(BeltPos.name) - (Vector2.right / 2);
                }
                if (GetType(BeltType.name) > 5)
                {
                    amountOfCorners++;
                    beltPos[amountOfCorners] = GetPos(BeltPos.name);
                }
                if (i == beltGroup.childCount - 1)
                {
                    int orientation = GetType(BeltType.name);
                    if (orientation == 1)
                        beltPos[amountOfPos - 1] = GetPos(BeltPos.name) + (Vector2.up / 2);
                    else if (orientation == 2)
                        beltPos[amountOfPos - 1] = GetPos(BeltPos.name) + (Vector2.down / 2);
                    else if (orientation == 3)
                        beltPos[amountOfPos - 1] = GetPos(BeltPos.name) + (Vector2.left / 2);
                    else if (orientation == 4)
                        beltPos[amountOfPos - 1] = GetPos(BeltPos.name) + (Vector2.right / 2);
                }
            }


            lineRenderer.positionCount = beltPos.Length;
            for (int i = 0; i < beltPos.Length; i++)
            {
                lineRenderer.SetPosition(i, new Vector3(beltPos[i].x, 0f, beltPos[i].y));
            }

            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    ConveyorBeltItem beltItem = items[i];
                    Transform item = items[i].item;

                    if (item == null)
                    {
                        items.Remove(items[i]);
                        Destroy(item.gameObject);
                    }
                    else
                    {

                        if (i > 0)
                        {
                            if (Vector3.Distance(item.position, items[i - 1].item.position) <= itemSpacing)
                            {
                                continue;
                            }
                        }

                        item.transform.position = Vector3.Lerp(lineRenderer.GetPosition(beltItem.startPoint), lineRenderer.GetPosition(beltItem.startPoint + 1), beltItem.currentLerp);
                        float distance = Vector3.Distance(lineRenderer.GetPosition(beltItem.startPoint), lineRenderer.GetPosition(beltItem.startPoint + 1));
                        beltItem.currentLerp += speed * Time.fixedDeltaTime / distance;

                        if (beltItem.currentLerp >= 1)
                        {
                            if (beltItem.startPoint + 2 < lineRenderer.positionCount)
                            {
                                beltItem.currentLerp = 0;
                                beltItem.startPoint++;
                            }
                            else
                            {
                                //Debug.Log("wallah");
                            }
                        }
                    }

                }
            }
        }
    }

    static int GetType(string name)
    {
        string typeString = "";
        for (int i = 0; i < name.Length; i++)
        {
            if (!Char.IsDigit(name[i]))
                break;
            else
                typeString += name[i];
        }
        int type = int.Parse(typeString);
        return type;
    }
    static Vector2 GetPos(string name)
    {
        Vector2 pos = Vector2.zero;
        bool isFirstComponent = false;
        string component = "";
        for (int k = 0; k < name.Length; k++)
        {
            if (Char.IsDigit(name[k]))
            {
                component += name[k];
            }
            else if (isFirstComponent)
            {
                pos.x = int.Parse(component);
                component = "";
                isFirstComponent = false;
            }
            else
            {
                pos.y = int.Parse(component);
                component = "";
                isFirstComponent = true;
            }
        }
        float memy = pos.y;
        pos.y = pos.x;
        pos.x = memy;
        return pos;
    }
}
