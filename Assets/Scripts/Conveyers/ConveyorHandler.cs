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
    public int currentConveyorGroupsLength = 0;
    int allConveyorsNb = 0;
    public Vector2[] start;
    public Vector2[] end;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 Hitpoint = Vector2.zero;

    public Vector2[] beltPositions;

    Vector2[] allPos = { };


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
                Transform conveyorGroupX = conveyorBelts.transform.GetChild(i);
                int conveyorGroupXNbChild = conveyorGroupX.childCount;
                if (conveyorGroupXNbChild == 0)
                {
                    DestroyImmediate(conveyorGroupX.gameObject);
                }
                else
                {
                    Transform posChild = conveyorGroupX.transform.GetChild(0);
                    if (posChild.childCount == 0)
                    {
                        DestroyImmediate(conveyorGroupX.gameObject);
                    }
                    else
                    {
                        beltPositions = DecriptConveyorBelts(numberConveyorGroups, i);
                        Deconstruct(beltPositions, i);
                        allConveyorsNb += currentConveyorGroupsLength;
                        //MergeBeltGroups(beltPositions, i, currentConveyorGroupsLength, allConveyorsNb);
                    }
                }
                numberConveyorGroups = conveyorBelts.childCount;
            }
            beltPositions = new Vector2[currentConveyorGroupsLength];
            allPos = Append(allPos, beltPositions);

            conveyors.isHandling = false;
            allConveyorsNb = 0;

            start = new Vector2[1];
            end = new Vector2[1];
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

        if (conveyorGroupXNbChild == 0)
            DestroyImmediate(conveyorGroupX.gameObject);

        if (conveyorGroupXNbChild > 0)
        {
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
                        allActiveBelts[j].x = int.Parse(component);
                        component = "";
                        isFirstComponent = false;
                    }
                    else
                    {
                        allActiveBelts[j].y = int.Parse(component);
                        component = "";
                        isFirstComponent = true;
                    }
                }
            }
            currentConveyorGroupsLength = conveyorGroupXNbChild;
        }
        return allActiveBelts;
    }

    public void Deconstruct(Vector2[] array, int counter)
    {
        Transform conveyorGroupX = conveyorBelts.transform.GetChild(counter);
        int conveyorGroupXNbChild = conveyorGroupX.childCount;
        if (conveyorGroupXNbChild == 0)
            DestroyImmediate(conveyorGroupX.gameObject);

        if (conveyorGroupXNbChild > 0)
        {
            Transform beltsTransform = conveyorGroupX.transform.GetChild(conveyorGroupX.childCount - 1);
            GameObject belt = beltsTransform.gameObject;

            GameObject NewConveyorGroupX = new GameObject($"conveyorGroup{-1}");
            NewConveyorGroupX.transform.parent = this.transform;

            bool inCopyMode = false;
            int posToCopy = 0;


            for (int i = 0; i < currentConveyorGroupsLength; i++)
            {
                belt = beltsTransform.gameObject;
                if (array[i] + Vector2.up != array[i + 1] && array[i] + Vector2.down != array[i + 1] &&
                    array[i] + Vector2.left != array[i + 1] && array[i] + Vector2.right != array[i + 1] && !inCopyMode && i != currentConveyorGroupsLength - 1)
                {
                    inCopyMode = true;
                    posToCopy = i + 1;
                }

                if (inCopyMode && posToCopy < i + 1)
                {
                    beltsTransform = conveyorGroupX.transform.GetChild(posToCopy);
                    beltsTransform.transform.SetParent(NewConveyorGroupX.transform, false);
                }
            }
            if (!inCopyMode)
                DestroyImmediate(NewConveyorGroupX.gameObject);
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
        }
        else
        {
            start[0] = array[0];
            end[0] = array[currentArrayLength - 1];
        }
        for (int i = 1; i < start.Length; i++)
        {

        }
        //Debug.Log($"{start[counter]} + {end[counter]}");
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
    static Vector2[] Append(Vector2[] array, Vector2[] arrayToAdd)
    {
        Vector2[] newArray = new Vector2[array.Length + arrayToAdd.Length];
        for (int i = 0; i < newArray.Length; i++)
            if (i > array.Length)
                for (int j = 0; j < arrayToAdd.Length; j++)
                    newArray[j + array.Length] = arrayToAdd[j];

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
}
