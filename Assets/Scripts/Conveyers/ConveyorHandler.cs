using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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

    public GameObject PrefabBelt_Up;
    public GameObject PrefabBelt_Down;
    public GameObject PrefabBelt_Left;
    public GameObject PrefabBelt_Right;
    public GameObject PrefabBelt_Up_Left;
    public GameObject PrefabBelt_Up_Right;
    public GameObject PrefabBelt_Down_Left;
    public GameObject PrefabBelt_Down_Right;
    public GameObject PrefabBelt_Left_Up;
    public GameObject PrefabBelt_Left_Down;
    public GameObject PrefabBelt_Right_Up;
    public GameObject PrefabBelt_Right_Down;

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
                        MergeBeltGroups(beltPositions, i, currentConveyorGroupsLength, allConveyorsNb, conveyorGroupX);
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

    public void MergeBeltGroups(Vector2[] array, int counter, int currentArrayLength, int arrayLength, Transform homeGroup)
    {
        start = Append(start, Vector2.zero);
        end = Append(end, Vector2.zero);

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
        for (int i = 0; i < counter; i++)
        {
            Transform sourceGroup = conveyorBelts.transform.GetChild(i);

            Transform startPos = sourceGroup.transform.GetChild(0);
            Transform startType = startPos.transform.GetChild(0);
            Transform endPos = sourceGroup.transform.GetChild(sourceGroup.childCount - 1);
            Transform endType = endPos.transform.GetChild(0);
            int startCode = GetType(startType.name);
            int endCode = GetType(endType.name);

            Transform currentStartPos = homeGroup.transform.GetChild(0);
            Transform currentStartType = currentStartPos.transform.GetChild(0);
            Transform currentEndPos = homeGroup.transform.GetChild(homeGroup.childCount - 1);
            Transform currentEndType = currentEndPos.transform.GetChild(0);
            int currentStartCode = GetType(currentStartType.name);
            int currentEndCode = GetType(currentEndType.name);

            MergeEgdes(endCode, currentStartCode, currentStartType, endType);
            //Debug.Log($"{startCode} - {endCode} - {currentStartCode} - {currentEndCode}");

            if (start[counter].x == end[i].x && (start[counter] + Vector2.up == end[i] || start[counter] + Vector2.down == end[i]))
                CopyAllChildren(homeGroup, sourceGroup);
            else if (start[i].y == end[counter].y && (start[counter] + Vector2.left == end[i] || start[counter] + Vector2.right == end[i]))
                CopyAllChildren(homeGroup, sourceGroup);
            else if (start[i].x == end[counter].x && (end[counter] + Vector2.up == start[i] || end[counter] + Vector2.down == start[i]))
                CopyAllChildren(sourceGroup, homeGroup);
            else if (start[i].y == end[counter].y && (end[counter] + Vector2.left == start[i] || end[counter] + Vector2.right == start[i]))
                CopyAllChildren(sourceGroup, homeGroup);
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

    public void MergeEgdes(int type, int typeToChange, Transform beltType, Transform beltTypeToChange)
    {
        if (typeToChange - 5 < 0)
            typeToChange *= 10;
        int newType = 0;
        if (typeToChange / 10 == 1)
        {
            if (type % 10 == 3)
                newType = 13;
            else if (type % 10 == 4)
                newType = 14;
        }
        else if (typeToChange / 10 == 2)
        {
            if (type % 10 == 3)
                newType = 23;
            else if (type % 10 == 4)
                newType = 24;
        }
        else if (typeToChange / 10 == 3)
        {
            if (type % 10 == 1)
                newType = 31;
            else if (type % 10 == 2)
                newType = 32;
        }
        else if (typeToChange / 10 == 4)
        {
            if (type % 10 == 1)
                newType = 41;
            else if (type % 10 == 2)
                newType = 42;
        }
        if (newType != 0)
            InstantiateBelt(newType, beltTypeToChange.parent.gameObject, start[0]);
    }

    void CopyAllChildren(Transform sourceTr, Transform targetTr)
    {
        Debug.Log("asdasdasda");
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

    void InstantiateBelt(float beltCode, GameObject parent, Vector2 pos)
    {
        GameObject PrefabToSpawn;
        if (beltCode == 1)
            PrefabToSpawn = PrefabBelt_Up;
        else if (beltCode == 2)
            PrefabToSpawn = PrefabBelt_Down;
        else if (beltCode == 3)
            PrefabToSpawn = PrefabBelt_Left;
        else if (beltCode == 4)
            PrefabToSpawn = PrefabBelt_Right;
        else if (beltCode == 31)
            PrefabToSpawn = PrefabBelt_Up_Left;
        else if (beltCode == 41)
            PrefabToSpawn = PrefabBelt_Up_Right;
        else if (beltCode == 32)
            PrefabToSpawn = PrefabBelt_Down_Left;
        else if (beltCode == 42)
            PrefabToSpawn = PrefabBelt_Down_Right;
        else if (beltCode == 13)
            PrefabToSpawn = PrefabBelt_Left_Up;
        else if (beltCode == 23)
            PrefabToSpawn = PrefabBelt_Left_Down;
        else if (beltCode == 14)
            PrefabToSpawn = PrefabBelt_Right_Up;
        else if (beltCode == 24)
            PrefabToSpawn = PrefabBelt_Right_Down;
        else
            PrefabToSpawn = PrefabBelt_Up;

        GameObject ConveyerSprite = Instantiate(PrefabToSpawn, new Vector3(pos.x, 0f, pos.y), Quaternion.Euler(90f, 0f, 0f), parent.transform);

    }
}
