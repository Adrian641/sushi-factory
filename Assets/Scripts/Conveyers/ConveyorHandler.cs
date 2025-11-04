using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorHandler : MonoBehaviour
{
    public PlaceConveyors conveyors;

    Transform conveyorBelts;

    int numberConveyorGroups = 0;
    public int currentConveyorGroupsLength = 0;
    int allConveyorsNb = 0;
    public Vector2[] start;
    public List<Vector2> allStartPoints = new List<Vector2>();
    public Vector2[] end;
    public List<Vector2> allEndPoints = new List<Vector2>();

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

    public GameObject ConveyorBeltsGroup;

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
                        CheckCorners(beltPositions, i);
                    }
                }
                numberConveyorGroups = conveyorBelts.childCount;
            }
            beltPositions = new Vector2[currentConveyorGroupsLength];
            allPos = Append(allPos, beltPositions);

            conveyors.isHandling = false;
            allConveyorsNb = 0;

            allEndPoints.Clear();
            allStartPoints.Clear();
            for (int j = 0; j < end.Length; j++)
            {
                allEndPoints.Add(end[j]);
                allStartPoints.Add(start[j]);
            }

            start = new Vector2[0];
            end = new Vector2[0];
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

            GameObject NewConveyorGroupX = Instantiate(ConveyorBeltsGroup);
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
                if (i == counter)
                    i++;
                if (i + 1 < conveyorBelts.childCount)
                {
                    if (startCode == 1 || endCode == 1)
                    {
                        if (start[i] == end[counter] + Vector2.up)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.up)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.up)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.up)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.up)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.up)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.up)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.up)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                    }
                    if (startCode == 2 || endCode == 2)
                    {
                        if (start[i] == end[counter] + Vector2.down)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.down)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.down)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.down)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.down)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.down)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                    }
                    if (startCode == 3 || endCode == 3)
                    {
                        if (start[i] == end[counter] + Vector2.left)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.left)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.left)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.left)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.left)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.left)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                    }
                    if (startCode == 4 || endCode == 4)
                    {
                        if (start[i] == end[counter] + Vector2.right)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.right)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.right)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.right)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                        if (start[i] == end[counter] + Vector2.right)
                            CopyAllChildren(sourceGroup, conveyorBelts.transform.GetChild(counter));
                        else if (end[i] == start[counter] - Vector2.right)
                            CopyAllChildren(conveyorBelts.transform.GetChild(counter), sourceGroup);
                    }
                }
            }
        }
    }
    void CheckCorners(Vector2[] array, int counter)
    {
        Transform group = conveyorBelts.transform.GetChild(counter);
        int[] allType = new int[group.childCount];
        int newType = 0;
        bool switchCorners = false;
        for (int i = 0; i < group.childCount; i++)
        {
            Transform pos = group.transform.GetChild(i);
            Transform type = pos.transform.GetChild(0);
            allType[i] = GetType(type.gameObject.name);
        }
        for (int i = 0; i < allType.Length - 1; i++)
        {

            if (allType[i] != allType[i + 1] && allType[i] / 10 == 0 && allType[i + 1] / 10 == 0)
            {
                newType = allType[i] * 10 + allType[i + 1];

                if (newType / 10 == 1 && array[i] + Vector2.up != array[i + 1])
                    switchCorners = true;
                else if (newType / 10 == 2 && array[i] + Vector2.down != array[i + 1])
                    switchCorners = true;
                else if (newType / 10 == 3 && array[i] + Vector2.left != array[i + 1])
                    switchCorners = true;
                else if (newType / 10 == 4 && array[i] + Vector2.right != array[i + 1])
                    switchCorners = true;

                if (switchCorners)
                {
                    Transform pos = group.transform.GetChild(i);
                    Transform type = pos.transform.GetChild(0);
                    ChangeIntoEgde(newType, type, array[i]);
                }
                else
                {
                    Transform pos = group.transform.GetChild(i + 1);
                    Transform type = pos.transform.GetChild(0);
                    ChangeIntoEgde(newType, type, array[i + 1]);
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
        if (type != 0)
        {
            InstantiateBelt(type, beltTypeToChange.parent.gameObject, pos);
            DestroyImmediate(beltTypeToChange.gameObject);
        }
    }
    public void ChangeIntoEgde(int type, Transform beltTypeToChange, Vector2 pos)
    {
        InstantiateBelt(type, beltTypeToChange.parent.gameObject, pos);
        DestroyImmediate(beltTypeToChange.gameObject);
    }

    void CopyAllChildren(Transform sourceTr, Transform targetTr)
    {
        if (sourceTr != null && targetTr != null)
        {
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
