using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

public class ItemsSystem : MonoBehaviour
{
    public ConveyorHandler conveyors;

    public float updatePerSec = 5f;
    private float dt = 0;

    public Vector3[] allCornerAndIndex = new Vector3[500];
    private int amountOfCorners = 0;

    public Transform ConveyorBeltsTr;

    public Vector2[] itemsPos = new Vector2[10000];
    public float distanceBeforeEnd = 0;

    private void FixedUpdate()
    {
        dt += Time.fixedDeltaTime;
    }

    void Update()
    {
        if (dt > updatePerSec)
        {
            dt = 0;
            amountOfCorners = 0;

            for (int i = 0; i < conveyors.allEndPoints.Count; i++)
            {
                //Debug.Log(conveyors.allEndPoints[i]);
                //Debug.Log(conveyors.allStartPoints[i]);
            }
            for (int i = 0; i < allCornerAndIndex.Length; i++)
                if (allCornerAndIndex[i].y != 0)
                    allCornerAndIndex[i] = Vector3.zero;
            for (int i = 0; i < ConveyorBeltsTr.childCount; i++)
            {
                Transform group = ConveyorBeltsTr.transform.GetChild(i);
                //Debug.Log(group.childCount);
                for (int j = 0; j < group.childCount; j++)
                {
                    Transform pos = group.transform.GetChild(j);
                    if (pos.childCount != 0)
                    {

                        Transform type = pos.transform.GetChild(0);
                        //Debug.Log(GetType(type.gameObject.name));
                        if (GetType(type.gameObject.name) > 5)
                        {
                            Vector2 cornerPos = GetPos(pos.gameObject.name);
                            allCornerAndIndex[amountOfCorners] = new Vector3(cornerPos.x, i + 1, cornerPos.y);
                            amountOfCorners++;
                        }
                    }
                }
            }
            bool hasSameX = false;
            bool hasSameY = false;
            for (int i = 0; itemsPos[i] != Vector2.zero; i++)
            {
                Vector2 pointToGo = Vector2.zero;
                for (int j = 0; allCornerAndIndex[j].y != 0 ; j++)
                {
                    if (itemsPos[i].x == allCornerAndIndex[j].x)
                    {
                        hasSameX = true;

                    }
                    if (itemsPos[i].y == allCornerAndIndex[j].y)
                        hasSameY = true;
                }
                if (!hasSameX && !hasSameY)
                {
                    //Delete Object
                }
                //distanceBeforeEnd = 
                
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
