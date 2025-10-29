using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConveyorHandler : MonoBehaviour
{
    public PlaceConveyors conveyors;

    Transform conveyorBelts;

    int numberConveyorGroups = 0;
    int currentConveyorGroupsLength = 0;
    int allConveyorsNb = 0;
    public Vector2Int[] start;
    public Vector2Int[] end;


    void Start()
    {
        conveyorBelts = this.transform;
        conveyors = FindObjectOfType<PlaceConveyors>();
    }

    void Update()
    {
        if (conveyors.isHandling)
        {
            numberConveyorGroups = conveyorBelts.childCount;
            for (int i = 0; i < numberConveyorGroups; i++)
            {
                Vector2Int[] beltPositions;
                beltPositions = DecriptConveyorBelts(numberConveyorGroups, i);
                Deconstruct(beltPositions, i);
                allConveyorsNb += currentConveyorGroupsLength;
                MergeBeltGroups(beltPositions, i, currentConveyorGroupsLength, allConveyorsNb);
            }
            Debug.Log(allConveyorsNb);
            conveyors.isHandling = false;
            allConveyorsNb = 0;
        }
    }

    public Vector2Int[] DecriptConveyorBelts(int nbOfConveyorGroups, int i)
    {
        Vector2Int[] allActiveBelts = new Vector2Int[conveyors.arrayLimits];
        string BeltName;
        string component = "";
        bool isFirstComponent = true;
        Transform conveyorGroupX = conveyorBelts.transform.GetChild(i);
        int conveyorGroupXNbChild = conveyorGroupX.childCount;

        for (int j = 0; j < conveyorGroupXNbChild; j++)
        {
            Transform BeltsTransform = conveyorGroupX.transform.GetChild(j);
            GameObject BeltsGameObject = BeltsTransform.gameObject;
            BeltName = BeltsGameObject.name;
            for (int k = 0; k < BeltName.Length; k++)
            {
                if (BeltName[k] != ',' && k != BeltName.Length - 1)
                {
                    component += BeltName[k];
                }
                else if (isFirstComponent)
                {
                    allActiveBelts[j].x = int.Parse(component);
                    component = "";
                    isFirstComponent = false;
                }
                else
                {
                    component += BeltName[k];
                    allActiveBelts[j].y = int.Parse(component);
                    component = "";
                    isFirstComponent = true;
                }
            }
            //Debug.Log(allActiveBelts[j]);
        }
        currentConveyorGroupsLength = conveyorGroupXNbChild;
        return allActiveBelts;
    }

    public void Deconstruct(Vector2Int[] array, int counter)
    {
        Transform conveyorGroupX = conveyorBelts.transform.GetChild(counter);
        int conveyorGroupXNbChild = conveyorGroupX.childCount;
        GameObject NewConveyorGroupX;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i + 1] == Vector2Int.zero)
                break;

            if (array[i].x != array[i + 1].x && array[i].y != array[i + 1].y)
            {
                NewConveyorGroupX = new GameObject($"conveyorGroup{i}");
                NewConveyorGroupX.transform.parent = this.transform;
            }
        }
    }

    public void MergeBeltGroups(Vector2Int[] array, int counter, int currentArrayLength, int arrayLength)
    {
        start = new Vector2Int[counter + 1];
        end = new Vector2Int[counter + 1];
        

        if (counter > 0)
        {
            
            for (int i = 0; i < start.Length - 1; i++)
            {
                if (array[arrayLength - currentArrayLength]  == start[i] || array[arrayLength - currentArrayLength] == end[i])
                {
                    //Debug.Log($"{end[i]}{start[i]}{array[arrayLength - currentArrayLength]}");
                }
                if (array[counter] == start[i] && array[counter] == end[i])
                {
                    //Debug.Log($"{array[counter]}");
                }
            }
        }
        else
        {
            start[counter] = array[0];
            end[counter] = array[currentArrayLength - 1];
            //Debug.Log($"{end[counter]}{start[counter]}{array[arrayLength - currentArrayLength]}");
        }
    }
}
