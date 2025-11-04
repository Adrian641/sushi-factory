using System;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlaceFishExtractor : MonoBehaviour
{
    public SelectItems selectItems;

    public int TypeOfMachine;

    public GameObject tunaHighlightedExctractor_Up;
    public GameObject tunaHighlightedExctractor_Down;
    public GameObject tunaHighlightedExctractor_Left;
    public GameObject tunaHighlightedExctractor_Right;
    public GameObject salmonHighlightedExctractor_Up;
    public GameObject salmonHighlightedExctractor_Down;
    public GameObject salmonHighlightedExctractor_Left;
    public GameObject salmonHighlightedExctractor_Right;

    public GameObject tunaExctractor_Up;
    public GameObject tunaExctractor_Down;
    public GameObject tunaExctractor_Left;
    public GameObject tunaExctractor_Right;
    public GameObject salmonExctractor_Up;
    public GameObject salmonExctractor_Down;
    public GameObject salmonExctractor_Left;
    public GameObject salmonExctractor_Right;

    public string tagToPlaceOn;
    public string factoryTag;

    private bool isSelected;
    public bool isFishNet;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 hitPoint = Vector2.zero;
    public Vector2 mousePos = Vector2.zero;

    public Vector2 selectedPos = Vector2.zero;

    public List<Vector2> factoryPos;
    private int factoryPosIndex = 0;

    private GameObject selectPrefab;
    private GameObject selectedObject;

    public int fishType = 0;

    private void Start()
    {
        selectPrefab = salmonExctractor_Left;
    }
    void Update()
    {
        if (selectItems.SelectExtractor_2 && TypeOfMachine == 2)
            isSelected = true;
        else if (selectItems.SelectFishNet_3 && TypeOfMachine == 3)
            isFishNet = true;
        else if (selectItems.SelectRiceField_4 && TypeOfMachine == 4)
            isSelected = true;
        else if (selectItems.SelectCutter_5 && TypeOfMachine == 5)
            isSelected = true;
        else if (selectItems.SelectStacker_6 && TypeOfMachine == 6)
            isSelected = true;
        else if (selectItems.SelectRoller_7 && TypeOfMachine == 7)
            isSelected = true;
        else
            isSelected = false;
        if (isSelected && !IsMouseOverUi() && !isFishNet)
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] RayHit = Physics.RaycastAll(ray);
            if (RayHit != null)
            {
                for (int i = 0; i < RayHit.Length; i++)
                {
                    if (RayHit[i].collider.CompareTag(tagToPlaceOn))
                    {
                        hitPoint = new Vector2(MathF.Round(RayHit[i].point.x), MathF.Round(RayHit[i].point.z));
                        if (Input.GetKeyDown(KeyCode.R))
                            Rotate();
                        if (hitPoint != selectedPos)
                        {
                            Destroy(selectedObject);

                            selectedPos = hitPoint;
                            selectedObject = Instantiate(selectPrefab, new Vector3(hitPoint.x, 0f, hitPoint.y), Quaternion.identity, gameObject.transform);
                            selectedObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && !factoryPos.Contains(hitPoint) && selectedPos != Vector2.zero)
            {
                GameObject FactoryToPlace = salmonExctractor_Up;

                if (fishType == 1)
                {
                    if (selectPrefab == salmonHighlightedExctractor_Up)
                        FactoryToPlace = salmonExctractor_Up;
                    else if (selectPrefab == salmonHighlightedExctractor_Right)
                        FactoryToPlace = salmonExctractor_Right;
                    else if (selectPrefab == salmonHighlightedExctractor_Down)
                        FactoryToPlace = salmonExctractor_Down;
                    else if (selectPrefab == salmonHighlightedExctractor_Left)
                        FactoryToPlace = salmonExctractor_Left;
                }
                else if (fishType == 2)
                {
                    if (selectPrefab == tunaHighlightedExctractor_Up)
                        FactoryToPlace = tunaExctractor_Up;
                    else if (selectPrefab == tunaHighlightedExctractor_Right)
                        FactoryToPlace = tunaExctractor_Right;
                    else if (selectPrefab == tunaHighlightedExctractor_Down)
                        FactoryToPlace = tunaExctractor_Down;
                    else if (selectPrefab == tunaHighlightedExctractor_Left)
                        FactoryToPlace = tunaExctractor_Left;
                }

                Destroy(selectedObject);
                GameObject Factory = Instantiate(FactoryToPlace, new Vector3(hitPoint.x, 0f, hitPoint.y), Quaternion.identity, gameObject.transform);
                Factory.transform.position = new Vector3(mousePos.x, 0f, mousePos.y);
                Factory.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                factoryPos.Add(new Vector2(Factory.transform.position.x, Factory.transform.position.z));
            }
        }
        if (IsMouseOverUi())
            Destroy(selectedObject);
        if (Physics.Raycast(ray, out RayHit))
        {
            mousePos = new Vector2(MathF.Round(RayHit.point.x), MathF.Round(RayHit.point.z));
            if (mousePos != hitPoint)
            {
                Destroy(selectedObject);
                selectedPos = Vector2.zero;
            }
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            DeleteObjects();
        }
    }

    public void Rotate()
    {
        if (fishType == 1)
        {
            if (selectPrefab == salmonHighlightedExctractor_Up)
                selectPrefab = salmonExctractor_Right;
            else if (selectPrefab == salmonExctractor_Right)
                selectPrefab = salmonExctractor_Down;
            else if (selectPrefab == salmonExctractor_Down)
                selectPrefab = salmonExctractor_Left;
            else if (selectPrefab == salmonExctractor_Left)
                selectPrefab = salmonHighlightedExctractor_Up;
        }
        else if (fishType == 2)
        {
            if (selectPrefab == tunaHighlightedExctractor_Up)
                selectPrefab = tunaHighlightedExctractor_Right;
            else if (selectPrefab == tunaHighlightedExctractor_Right)
                selectPrefab = tunaHighlightedExctractor_Down;
            else if (selectPrefab == tunaHighlightedExctractor_Down)
                selectPrefab = tunaHighlightedExctractor_Left;
            else if (selectPrefab == tunaHighlightedExctractor_Left)
                selectPrefab = tunaHighlightedExctractor_Up;
        }
        Destroy(selectedObject);
        selectedPos = Vector2.zero;
    }

    private bool IsMouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void DeleteObjects()
    {

        if (Physics.Raycast(ray, out RayHit))
        {
            GameObject hit = RayHit.transform.gameObject;
            if (hit.CompareTag(factoryTag))
            {
                Destroy(hit.gameObject);
                factoryPos.Remove(new Vector2(hit.transform.position.x, hit.transform.position.z));
            }
        }
    }
}
