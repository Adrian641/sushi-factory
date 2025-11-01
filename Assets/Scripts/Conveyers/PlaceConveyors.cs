using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class PlaceConveyors : MonoBehaviour
{
    public bool isHoldingLeftShift = false;
    public bool isHoldingMouse0 = false;
    public bool isStartClickingMouse0 = false;
    public bool isReleasingMouse0 = false;
    public bool toggleFlip = false;
    public bool isHoldingMouse1 = false;

    public bool isHandling = false;

    public int mousePositionIndex = 0;
    public Vector2[] mousePositions;
    public int arrayLimits = 1000;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 Hitpoint = Vector2.zero;

    public GameObject SeclectedTile;
    private GameObject SeclectedTileGroup;

    public Vector2[] conveyorLinePath;
    public int conveyorGroupNumber = 0;

    LayerMask beltLayer;

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

    public GameObject ConveyorBelts;

    public GameObject ConveyorBeltsGroup;

    void Start()
    {
        mousePositions = new Vector2[arrayLimits];
    }

    void Update()
    {
        float dt = 0;
        dt += Time.deltaTime;
        if (dt > 1)
        {
            isHandling = true;
            dt = 0;
        }
        CheckUsersInputs();

        if (isStartClickingMouse0)
        {
            SeclectedTileGroup = new GameObject("SelectedTilesGroup");
            SeclectedTileGroup.transform.parent = ConveyorBelts.transform;
        }

        if (isReleasingMouse0)
        {
            DestroyImmediate(SeclectedTileGroup);
            isHandling = true;

            if (!isHoldingLeftShift)
                conveyorLinePath = CreateConveyorLine(mousePositions, mousePositionIndex);
            else
                conveyorLinePath = CreateConveyorLine(conveyorLinePath, conveyorLinePath.Length);

            if(conveyorLinePath != null)
            {
                DeleteObjects(conveyorLinePath);
                if (conveyorLinePath[0] != new Vector2(-1f, -1f))
                {
                    GameObject ConveyorGroup = Instantiate(ConveyorBeltsGroup);
                    ConveyorGroup.transform.parent = ConveyorBelts.transform;
                    conveyorGroupNumber++;
                    for (int i = 0; i < conveyorLinePath.Length / 2; i++)
                    {
                    
                        GameObject ConveyerPos = new GameObject($"{conveyorLinePath[i].x},{conveyorLinePath[i].y},");
                        ConveyerPos.transform.parent = ConveyorGroup.transform;
                        InstantiateBelt(conveyorLinePath[conveyorLinePath.Length / 2 + i].x, ConveyerPos, conveyorLinePath[i]);
                    }
                }
                PutToZero(mousePositions);
                mousePositionIndex = 0;

                if (toggleFlip)
                    toggleFlip = false;
                isHandling = true;
            }
        }

        if (isHoldingMouse0 || isHoldingMouse1)
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RayHit))
                Hitpoint = new Vector2(MathF.Round(RayHit.point.x), MathF.Round(RayHit.point.z));
        }

        if (isHoldingMouse0 && mousePositionIndex < arrayLimits)
        {

            if (isHoldingLeftShift)
            {
                DestroyImmediate(SeclectedTileGroup);
                conveyorLinePath = CreateStraightConveyorLine(mousePositions[0], Hitpoint);
                SeclectedTileGroup = new GameObject("SelectedTilesGroup");
                SeclectedTileGroup.transform.parent = ConveyorBelts.transform;
                HighLightConveyorPath(conveyorLinePath, mousePositionIndex);
            }
            if (!mousePositions.Contains(Hitpoint))
            {
                mousePositions[mousePositionIndex] = Hitpoint;
                if (!isHoldingLeftShift)
                    HighLightConveyorPath(mousePositions, mousePositionIndex);
                mousePositionIndex++;
            }
        }
        if (isHoldingMouse1)
            DeleteObjects();
    }
    void HighLightConveyorPath(Vector2[] arrayToHighLight, int arrayIndex)
    {
        if (!isHoldingLeftShift)
        {
            Instantiate(SeclectedTile, new Vector3(arrayToHighLight[arrayIndex].x, 0f, arrayToHighLight[arrayIndex].y), Quaternion.identity, SeclectedTileGroup.transform);
        }
        else if (isHoldingLeftShift)
        {
            for (int i = 0; i < arrayToHighLight.Length; i++)
            {
                Instantiate(SeclectedTile, new Vector3(arrayToHighLight[i].x, 0f, arrayToHighLight[i].y), Quaternion.identity, SeclectedTileGroup.transform);
            }
        }
    }

    Vector2[] CreateConveyorLine(Vector2[] mousePositions, int conveyorLineLength)
    {
        Vector2[] ContinuousConveyorLine = new Vector2[conveyorLineLength * 2];
        Vector2[] ErrorArray = new Vector2[1];
        Vector2 lastDir = Vector2.zero;
        Vector2 Dir = Vector2.zero;

        if (conveyorLineLength > 2)
        {
            for (int i = 0; i < conveyorLineLength; i++)
            {
                lastDir = Dir;
                if (i + 1 == conveyorLineLength)
                {
                    ContinuousConveyorLine[i] = mousePositions[i];
                }
                else if (mousePositions[i].x == mousePositions[i + 1].x || mousePositions[i].y == mousePositions[i + 1].y)
                {
                    ContinuousConveyorLine[i] = mousePositions[i];
                    ContinuousConveyorLine[i + 1] = mousePositions[i + 1];
                    Dir = mousePositions[i + 1] - mousePositions[i];
                    ContinuousConveyorLine[ContinuousConveyorLine.Length / 2 + i] = new Vector2(FindConveyorType(lastDir, Dir), -1f);
                }
                else
                {
                    ErrorArray[0] = new Vector2(-1f, -1f);
                    return ErrorArray;
                }
            }

            ContinuousConveyorLine[ContinuousConveyorLine.Length / 2].x = ContinuousConveyorLine[ContinuousConveyorLine.Length / 2 + 1].x % 10;

            if (ContinuousConveyorLine[ContinuousConveyorLine.Length - 2].x > 10 && ContinuousConveyorLine.Length >= 2)
                ContinuousConveyorLine[ContinuousConveyorLine.Length - 1] = new Vector2((ContinuousConveyorLine[ContinuousConveyorLine.Length - 2].x / 10) - ((ContinuousConveyorLine[ContinuousConveyorLine.Length - 2].x % 10) / 10), -1f);
            else
                ContinuousConveyorLine[ContinuousConveyorLine.Length - 1] = new Vector2(ContinuousConveyorLine[ContinuousConveyorLine.Length - 2].x, -1f);
        }
        else if (conveyorLineLength == 2)
        {
            ContinuousConveyorLine[0] = mousePositions[0];
            ContinuousConveyorLine[1] = mousePositions[1];
            Dir = mousePositions[1] - mousePositions[0];
            ContinuousConveyorLine[2] = new Vector2(FindConveyorType(Dir, Dir), -1f);
            ContinuousConveyorLine[3] = ContinuousConveyorLine[2];
        }
        else if (conveyorLineLength == 1)
        {
            ContinuousConveyorLine[0] = mousePositions[0];
            ContinuousConveyorLine[1] = new Vector2(1f, -1f);
        }

        return ContinuousConveyorLine;
    }
    Vector2[] CreateStraightConveyorLine(Vector2 AnchorPoint, Vector2 HitPoint)
    {
        Vector2[] ConveyorLine = { };
        Vector2[] ConveyorLineTypes = { };

        int conveyorSize = (int)(MathF.Abs(HitPoint.x - AnchorPoint.x) + MathF.Abs(HitPoint.y - AnchorPoint.y) + 1);
        ConveyorLine = new Vector2[conveyorSize];
        ConveyorLine[0] = AnchorPoint;

        if (toggleFlip)
        {
            for (int i = 1; i < conveyorSize; i++)
            {
                if (ConveyorLine[i - 1].y! < Hitpoint.y)
                    ConveyorLine[i] = new Vector2(AnchorPoint.x, AnchorPoint.y + i);
                else if (ConveyorLine[i - 1].y! > Hitpoint.y)
                    ConveyorLine[i] = new Vector2(AnchorPoint.x, AnchorPoint.y - i);
                else if (ConveyorLine[i - 1].x! < Hitpoint.x)
                    ConveyorLine[i] = new Vector2(ConveyorLine[i - 1].x + 1, Hitpoint.y);
                else if (ConveyorLine[i - 1].x! > Hitpoint.x)
                    ConveyorLine[i] = new Vector2(ConveyorLine[i - 1].x - 1, Hitpoint.y);
            }
        }
        else
        {
            for (int i = 1; i < conveyorSize; i++)
            {
                if (ConveyorLine[i - 1].x! < Hitpoint.x)
                    ConveyorLine[i] = new Vector2(AnchorPoint.x + i, AnchorPoint.y);
                else if (ConveyorLine[i - 1].x! > Hitpoint.x)
                    ConveyorLine[i] = new Vector2(AnchorPoint.x - i, AnchorPoint.y);
                else if (ConveyorLine[i - 1].y! < Hitpoint.y)
                    ConveyorLine[i] = new Vector2(Hitpoint.x, ConveyorLine[i - 1].y + 1);
                else if (ConveyorLine[i - 1].y! > Hitpoint.y)
                    ConveyorLine[i] = new Vector2(Hitpoint.x, ConveyorLine[i - 1].y - 1);
            }
        }
        return ConveyorLine;
    }

    public void DeleteObjects()
    {

        if (Physics.Raycast(ray, out RayHit))
        {
            GameObject hit = RayHit.transform.gameObject;
            if (hit.layer != beltLayer)
            {
                GameObject hitParent = hit.transform.parent.gameObject;
                Destroy(hitParent);
                isHandling = true;
            }
        }
    }
    public void DeleteObjects(Vector2[] Pos)
    {
        for (int i = 0; i < Pos.Length; i++)
        {
            if (Physics.Raycast(new Vector3(Pos[i].x, 0.5f, Pos[i].y), Vector3.down, out RayHit))
            {
                GameObject hit = RayHit.transform.gameObject;
                if (hit.layer != beltLayer && hit.CompareTag("Belt"))
                {
                    GameObject hitParent = hit.transform.parent.gameObject;
                    Destroy(hitParent);
                    isHandling = true;
                }
            }
        }
    }

    public bool AlreadyContained(Vector2[] array, Vector2 value, int currentArrayLength)
    {
        for (int i = 0; i < array.Length - currentArrayLength; i++)
        {
            if (array[i] == value)
                return true;
        }
        return false;
    }

    int FindConveyorType(Vector2 lastDir, Vector2 dir)
    {
        if (lastDir == Vector2.zero)
            return 0;
        else if (lastDir == Vector2.up && dir == Vector2.up)
            return 1;
        else if (lastDir == Vector2.down && dir == Vector2.down)
            return 2;
        else if (lastDir == Vector2.left && dir == Vector2.left)
            return 3;
        else if (lastDir == Vector2.right && dir == Vector2.right)
            return 4;
        else if (lastDir == Vector2.up && dir == Vector2.left)
            return 31;
        else if (lastDir == Vector2.up && dir == Vector2.right)
            return 41;
        else if (lastDir == Vector2.down && dir == Vector2.left)
            return 32;
        else if (lastDir == Vector2.down && dir == Vector2.right)
            return 42;
        else if (lastDir == Vector2.left && dir == Vector2.up)
            return 13;
        else if (lastDir == Vector2.left && dir == Vector2.down)
            return 23;
        else if (lastDir == Vector2.right && dir == Vector2.up)
            return 14;
        else if (lastDir == Vector2.right && dir == Vector2.down)
            return 24;
        return 0;
    }
    static Vector2[] PutToZero(Vector2[] array)
    {
        for (int i = 0; i < array.Length; i++)
            array[i] = Vector2.zero;
        return array;
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

    void CheckUsersInputs()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            isHoldingLeftShift = true;
        else
            isHoldingLeftShift = false;
        if (Input.GetKey(KeyCode.Mouse0))
            isHoldingMouse0 = true;
        else
            isHoldingMouse0 = false;
        if (Input.GetKeyDown(KeyCode.Mouse0))
            isStartClickingMouse0 = true;
        else
            isStartClickingMouse0 = false;
        if (Input.GetKeyUp(KeyCode.Mouse0))
            isReleasingMouse0 = true;
        else
            isReleasingMouse0 = false;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!toggleFlip)
                toggleFlip = true;
            else if (toggleFlip)
                toggleFlip = false;
        }

        if (Input.GetKey(KeyCode.Mouse1))
            isHoldingMouse1 = true;
        else
            isHoldingMouse1 = false;
    }
}
