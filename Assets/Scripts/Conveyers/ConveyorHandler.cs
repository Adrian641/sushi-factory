using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ConveyorHandler : MonoBehaviour
{
    public PlaceConveyors conveyors;

    Transform conveyorBelts;

    int numberConveyorGroups = 0;
    int currentConveyorGroupsLength = 0;
    int allConveyorsNb = 0;
    public Vector2[] start;
    public Vector2[] end;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 Hitpoint = Vector2.zero;

    public Vector2[] beltPositions;

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
                beltPositions = DecriptConveyorBelts(numberConveyorGroups, i);
                Deconstruct(beltPositions, i);
                allConveyorsNb += currentConveyorGroupsLength;
                MergeBeltGroups(beltPositions, i, currentConveyorGroupsLength, allConveyorsNb);
            }
            //Debug.Log(allConveyorsNb);
            conveyors.isHandling = false;
            allConveyorsNb = 0;

            start = new Vector2[1];
            end = new Vector2[1];
        }
        if (conveyors.isHoldingMouse1)
        {
            DeleteObjects(beltPositions);
        }
    }

    public Vector2[] DecriptConveyorBelts(int nbOfConveyorGroups, int i)
    {
        Vector2[] allActiveBelts = new Vector2[conveyors.arrayLimits];
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
                if (Char.IsDigit(BeltName[k]))
                {
                    component += BeltName[k];
                }
                else if (isFirstComponent)
                {
                    //Debug.Log(component);
                    allActiveBelts[j].x = int.Parse(component);
                    component = "";
                    isFirstComponent = false;
                }
                else
                {
                    //Debug.Log(component);
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

    public void Deconstruct(Vector2[] array, int counter)
    {
        Transform conveyorGroupX = conveyorBelts.transform.GetChild(counter);
        int conveyorGroupXNbChild = conveyorGroupX.childCount;
        GameObject NewConveyorGroupX;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i + 1] == Vector2.zero)
                break;

            if (array[i].x != array[i + 1].x && array[i].y != array[i + 1].y)
            {
                NewConveyorGroupX = new GameObject($"conveyorGroup{i}");
                NewConveyorGroupX.transform.parent = this.transform;
            }
        }
    }

    public void MergeBeltGroups(Vector2[] array, int counter, int currentArrayLength, int arrayLength)
    {
        start = Append(start, Vector2.zero);
        end = Append(end, Vector2.zero);

        int type1 = 0;
        int type2 = 0;

        if (counter > 0)
        {
            start[counter] = array[0];
            end[counter] = array[currentArrayLength - 1];
            for (int i = 0; i < start.Length - 1; i++)
            {
                if (end[counter] == start[i] - Vector2.up || end[counter] == start[i] - Vector2.down ||
                    end[counter] == start[i] - Vector2.left || end[counter] == start[i] - Vector2.right)
                {
                    Transform conveyorGroup1 = conveyorBelts.transform.GetChild(counter);
                    Transform beltsTransform = conveyorGroup1.transform.GetChild(conveyorGroup1.childCount - 1);
                    Transform typeOfBelt1 = beltsTransform.transform.GetChild(0);
                    type1 = GetType(typeOfBelt1.name);

                    Transform conveyorGroup2 = conveyorBelts.transform.GetChild(i);
                    beltsTransform = conveyorGroup2.transform.GetChild(0);
                    Transform typeOfBelt2 = beltsTransform.transform.GetChild(0);
                    type2 = GetType(typeOfBelt2.name);

                    MergeEgdes(type1, type2, typeOfBelt1, typeOfBelt2, conveyorGroup1, conveyorGroup2);
                    //Debug.Log($"{type1} to {type2}");
                }
                else if (start[counter] == end[i] - Vector2.up || start[counter] == end[i] - Vector2.down ||
                         start[counter] == end[i] - Vector2.left || start[counter] == end[i] - Vector2.right)
                {

                }
            }
        }
        else
        {
            start[counter] = array[0];
            end[counter] = array[currentArrayLength - 1];
        }
        //Debug.Log($"{end[counter]}{start[counter]}");
    }

    static int GetType(string name)
    {
        string typeString = "";
        for (int i = 0; 0 < name.Length; i++)
        {
            if (!Char.IsDigit(name[i]))
                break;
            else
                typeString += name[i];
        }
        int type = int.Parse(typeString);
        return type;
    }

    static Vector2[] Append(Vector2[] array, Vector2 posToAdd)
    {
        Vector2[] newArray = new Vector2[array.Length + 1];
        for (int i = 0; i < array.Length; i++)
            newArray[i] = array[i];
        newArray[newArray.Length - 1] = posToAdd;
        return newArray;
    }

    public void MergeEgdes(int type1, int type2, Transform beltType1, Transform beltType2, Transform HomeGroup1, Transform HomeGroup2)
    {
        if (type1 == type2)
        {
            CopyAllChildren(HomeGroup2, HomeGroup1);
        }


        //if (type1 == 01 && type2 == 02 || type1 == 03 && type2 == 04 ||
        //type1 == 02 && type2 == 01 || type1 == 04 && type2 == 03)
        //{
        //    typeOfBelt = beltsTransform.transform.GetChild(0);
        //}
    }

    void CopyAllChildren(Transform sourceTr, Transform targetTr)
    {
        GameObject source = sourceTr.gameObject;
        GameObject target = targetTr.gameObject;

        // Iterate through all direct children of the source GameObject
        for (int i = 0; i < source.transform.childCount; i++)
        {
            Transform childTransform = source.transform.GetChild(i);

            // Instantiate a copy of the child GameObject
            GameObject copiedChild = Instantiate(childTransform.gameObject);
            copiedChild.name = childTransform.name;

            // Set the target GameObject as the parent of the copied child
            copiedChild.transform.SetParent(target.transform, false); // 'false' maintains local position/rotation relative to the new parent
                                                                      // 'true' would maintain world position/rotation
        }
        DestroyImmediate(source);
    }

    public void DeleteObjects(Vector2[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array.Contains(conveyors.Hitpoint))
            {
                
            }
        }
    }
}
