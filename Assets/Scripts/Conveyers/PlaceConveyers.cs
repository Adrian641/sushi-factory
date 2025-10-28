using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class PlaceConveyors : MonoBehaviour
{
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
        mousePositions = new Vector2[arrayLimits];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            SeclectedTileGroup = new GameObject("SelectedTilesGroup");
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DestroyImmediate(SeclectedTileGroup);
            conveyorLinePath = CreateConveyorLine(mousePositions, mousePositionIndex);
            if (conveyorLinePath[0] != new Vector2(-1f, -1f))
            {
                GameObject ConveyorGroup = new GameObject($"conveyorGroup{conveyorGroupNumber}");
                conveyorGroupNumber++;
                for (int i = 0; i < conveyorLinePath.Length / 2; i++)
                {
                    GameObject ConveyerPos = new GameObject($"{conveyorLinePath[i].x}, {conveyorLinePath[i].y}");
                    ConveyerPos.transform.parent = ConveyorGroup.transform;
                    InstantiateBelt(conveyorLinePath[conveyorLinePath.Length / 2 + i].x, ConveyerPos, conveyorLinePath[i]);
                }
            }
            PutToZero(mousePositions);
            mousePositionIndex = 0;
        }

        if (Input.GetKey(KeyCode.Mouse0) && mousePositionIndex < arrayLimits)
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RayHit))
            {
                Hitpoint = new Vector2(MathF.Round(RayHit.point.x), MathF.Round(RayHit.point.z));
                if (!mousePositions.Contains(Hitpoint))
                {
                    mousePositions[mousePositionIndex] = Hitpoint;
                    Instantiate(SeclectedTile, new Vector3(mousePositions[mousePositionIndex].x, 0f, mousePositions[mousePositionIndex].y), Quaternion.identity, SeclectedTileGroup.transform);
                    mousePositionIndex++;
                }
            }
        }
    }
    Vector2[] CreateConveyorLine(Vector2[] mousePositions, int conveyorLineLength)
    {
        Vector2[] ContinuousConveyorLine = new Vector2[conveyorLineLength * 2];
        Vector2[] ErrorArray = new Vector2[1];
        Vector2 lastDir = Vector2.zero;
        Vector2 Dir = Vector2.zero;

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
                Debug.Log(FindConveyorType(lastDir, Dir));
            }
            else
            {
                ErrorArray[0] = new Vector2(-1f, -1f);
                return ErrorArray;
            }
        }
        ContinuousConveyorLine[ContinuousConveyorLine.Length / 2].x = ContinuousConveyorLine[ContinuousConveyorLine.Length / 2 + 1].x % 10;
        ContinuousConveyorLine[ContinuousConveyorLine.Length - 1] = new Vector2(Mathf.Round(ContinuousConveyorLine[ContinuousConveyorLine.Length - 2].x % 10), -1f);
        return ContinuousConveyorLine;

    }

    int FindConveyorType(Vector2 lastDir,  Vector2 dir)
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
        GameObject PrefabToSpawn = null;
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

        GameObject ConveyerSprite = Instantiate(PrefabToSpawn, new Vector3(pos.x, 0f, pos.y), Quaternion.Euler(90f, 0f, 0f), parent.transform);
        
    }
}
