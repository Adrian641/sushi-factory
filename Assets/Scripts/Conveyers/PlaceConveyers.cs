using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.XR;

public class PlaceConveyers : MonoBehaviour
{
    public int mousePositionIndex = 0;
    public Vector2[] mousePositions;
    public int arrayLimits = 1000;
    public float dt = 0;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 Hitpoint = Vector2.zero;

    public GameObject SeclectedTile;
    private GameObject SeclectedTileGroup;

    void Start()
    {
        mousePositions = new Vector2[arrayLimits];
    }

    void Update()
    {

        dt += 0.01f;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            SeclectedTileGroup = new GameObject("SelectedTilesGroup");
        if (Input.GetKeyUp(KeyCode.Mouse0))
            DestroyImmediate(SeclectedTileGroup);

        if (dt > 0.05 && Input.GetKey(KeyCode.Mouse0) && mousePositionIndex < arrayLimits)
        {
            dt = 0;
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RayHit))
            {
                Hitpoint = new Vector2(MathF.Round(RayHit.point.x), MathF.Round(RayHit.point.z));
                mousePositions[mousePositionIndex] = Hitpoint;
                HighlightPath(mousePositions, mousePositionIndex, SeclectedTile, SeclectedTileGroup);
                mousePositionIndex++;
            }
        }
    }

    public void HighlightPath(Vector2[] array, int arrayIndex, GameObject Tile, GameObject TileGroup)
    {
        Vector2[] highlightedSquares = new Vector2[arrayLimits];

        if (!highlightedSquares.Contains(array[arrayIndex]))
        {
            highlightedSquares[arrayIndex] = array[arrayIndex];
            Instantiate(Tile, new Vector3(array[arrayIndex].x, 0f, array[arrayIndex].y), Quaternion.identity, TileGroup.transform);
            Debug.Log(highlightedSquares[arrayIndex]);
        }
    }
}
