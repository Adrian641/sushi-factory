using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ItemsSystem : MonoBehaviour
{
    public ConveyorHandler conveyors;

    private float updatePerSec = 0.1f;
    private float dt = 0;

    public Vector3[] allCornerAndIndex = new Vector3[500];

    public Transform ConveyorBeltsTr;

    private void FixedUpdate()
    {
        dt += Time.fixedDeltaTime;
    }

    void Update()
    {
        if (dt > updatePerSec)
        {
            dt = 0;

            for (int i = 0; i < conveyors.allEndPoints.Count; i++)
            {
                //Debug.Log(conveyors.allEndPoints[i]);
                //Debug.Log(conveyors.allStartPoints[i]);
            }
            for (int i = 0; i < ConveyorBeltsTr.childCount; i++)
            {
                Transform group = ConveyorBeltsTr.transform.GetChild(i);
                for (int j = 0; j <  group.childCount; j++)
                {
                    Transform pos = group.transform.GetChild(j);
                    if (pos.childCount != 0)
                    {
                        Transform type = pos.transform.GetChild(0);
                        if (GetType(type.gameObject.name) > 5)
                        {
                            Vector2 cornerPos = GetPos(pos.gameObject.name);
                            allCornerAndIndex[i] = new Vector3(cornerPos.x, i, cornerPos.y);
                            Debug.Log(allCornerAndIndex[i]);
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
