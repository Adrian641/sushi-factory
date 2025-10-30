using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro.Examples;
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
                if (i != currentConveyorGroupsLength - 1 && array[i] + Vector2.up != array[i + 1] && array[i] + Vector2.down != array[i + 1] &&
                    array[i] + Vector2.left != array[i + 1] && array[i] + Vector2.right != array[i + 1] && !inCopyMode)
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

        //Debug.Log(conveyorBelts.childCount);
        for (int i = 0; i < conveyorBelts.childCount; i++)
        {
            Transform sourceGroup = conveyorBelts.transform.GetChild(i);
            if (sourceGroup.childCount != 0)
            {
                Transform startPos = sourceGroup.transform.GetChild(0);
                Transform startType = startPos.transform.GetChild(0);
                Transform endPos = sourceGroup.transform.GetChild(sourceGroup.childCount - 1);
                Transform endType = endPos.transform.GetChild(0);

                int startCode = GetType(startType.name);
                int endCode = GetType(endType.name);

                if (startCode / 10 != 0)
                    ChangeEgde(startCode / 10, startType, new Vector2(startType.transform.position.x, startType.transform.position.z));
                else if (endCode / 10 != 0)
                    ChangeEgde(endCode, endType, new Vector2(endType.transform.position.x, endType.transform.position.z));

                for (int j = 0; j < start.Length - 1; j++)
                {
                    Debug.Log("--");
                }
                //Debug.Log(start.Length - 1);
                //Debug.Log($"{startCode / 10}{endCode / 10}");
            }
        }
        //    for (int i = 0; i < counter; i++)
        //    {
        //        Transform sourceGroup = conveyorBelts.transform.GetChild(i);

        //        Transform startPos = sourceGroup.transform.GetChild(0);
        //        Transform startType = startPos.transform.GetChild(0);
        //        Transform endPos = sourceGroup.transform.GetChild(sourceGroup.childCount - 1);
        //        Transform endType = endPos.transform.GetChild(0);

        //        int startCodeI = GetType(startType.name);
        //        int endCodeI = GetType(endType.name);

        //        Debug.Log($"{start[i]} + {end[i]}");
        //        for (int j = 0; j < counter; j++)
        //        {
        //            homeGroup = conveyorBelts.GetChild(j);
        //            Transform currentStartPos = homeGroup.transform.GetChild(0);
        //            Transform currentStartType = currentStartPos.transform.GetChild(0);
        //            Transform currentEndPos = homeGroup.transform.GetChild(homeGroup.childCount - 1);
        //            Transform currentEndType = currentEndPos.transform.GetChild(0);
        //            int currentStartCode = GetType(currentStartType.name);
        //            int currentEndCode = GetType(currentEndType.name);

        //            if (j == i)
        //                j++;
        //            if (j == start.Length)
        //                break;

        //            //Debug.LogError($"{start[i]}{end[i]} - {start[j]}{end[j]} --- {startCodeI}{endCodeI} - {currentEndCode}{currentStartCode}");
        //            if (start[i].x == end[j].x && (end[j] + Vector2.up == start[i] || end[j] + Vector2.down == start[i]))
        //                CopyAllChildren(homeGroup, sourceGroup);
        //else if (start[i].y == end[j].y && startCodeI == currentEndCode && (end[j] + Vector2.left == start[i] || end[j] + Vector2.right == start[i]))
        //    CopyAllChildren(homeGroup, sourceGroup);
        //else if (end[i].x == start[j].x && endCodeI == currentStartCode && (start[j] + Vector2.up == end[i] || start[j] + Vector2.down == end[i]))
        //    CopyAllChildren(homeGroup, sourceGroup);
        //else if (end[i].y == start[j].y && endCodeI == currentStartCode && (start[j] + Vector2.left == end[i] || start[j] + Vector2.right == end[i]))
        //    CopyAllChildren(homeGroup, sourceGroup);
        //}
        //Debug.Log($"{startCode} - {endCode} - {currentStartCode} - {currentEndCode}");
        //for (int j = i; j < start.Length - 1 || homeGroup != null || sourceGroup != null; i++)
        //{
        //    if (i >= start.Length)
        //        break;
        //    if (start[j].x == end[i].x && (start[j] + Vector2.up == end[i] || start[j] + Vector2.down == end[i]))
        //    {
        //        CopyAllChildren(homeGroup, sourceGroup);
        //        //break;
        //    }
        //    else if (start[i].y == end[j].y && (start[j] + Vector2.left == end[i] || start[j] + Vector2.right == end[i]))
        //    {
        //        CopyAllChildren(homeGroup, sourceGroup);
        //        //MergeEgdes(endCode, currentStartCode, currentStartType, endType, counter);
        //        //break;
        //    }

        //    if (homeGroup == null || sourceGroup == null)
        //    {
        //        break;
        //    }
        //    else if (start[i].x == end[j].x && (end[j] + Vector2.up == start[i] || end[j] + Vector2.down == start[i]))
        //    {
        //        CopyAllChildren(sourceGroup, homeGroup);
        //        //break;
        //    }
        //    else if (start[i].y == end[j].y && (end[j] + Vector2.left == start[i] || end[j] + Vector2.right == start[i]))
        //    {
        //        CopyAllChildren(sourceGroup, homeGroup);
        //        //break;
        //    }
        //}
        //    }
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
        if (array == null) array = new Vector2[0];
        if (arrayToAdd == null) arrayToAdd = new Vector2[0];

        Vector2[] newArray = new Vector2[array.Length + arrayToAdd.Length];
        for (int i = 0; i < array.Length; i++)
            newArray[i] = array[i];
        for (int j = 0; j < arrayToAdd.Length; j++)
            newArray[array.Length + j] = arrayToAdd[j];

        return newArray;
    }


    public void ChangeEgde(int type, Transform beltTypeToChange, Vector2 pos)
    {
        type = type % 10;
        Debug.Log(type);
        if (type != 0)
        {
            InstantiateBelt(type, beltTypeToChange.parent.gameObject, pos);
            DestroyImmediate(beltTypeToChange.gameObject);
        }
    }

    void CopyAllChildren(Transform sourceTr, Transform targetTr)
    {
        if (sourceTr != null && targetTr != null)
        {
            Debug.Log("asdasdasda");
            GameObject source = sourceTr.gameObject;
            GameObject target = targetTr.gameObject;

            for (int i = 0; i < source.transform.childCount; i++)
            {
                Transform childTransform = source.transform.GetChild(i);
                GameObject copiedChild = Instantiate(childTransform.gameObject);
                copiedChild.name = childTransform.name;

                copiedChild.transform.SetParent(target.transform, false);
            }
            DestroyImmediate(source);
        }
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
        else
            PrefabToSpawn = PrefabBelt_Up;

        GameObject ConveyerSprite = Instantiate(PrefabToSpawn, new Vector3(pos.x, 0f, pos.y), Quaternion.Euler(90f, 0f, 0f), parent.transform);

    }
}
