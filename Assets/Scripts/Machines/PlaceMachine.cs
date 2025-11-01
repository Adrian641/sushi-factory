using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlaceMachine : MonoBehaviour
{
    public SelectItems selectItems;

    public int TypeOfMachine;

    public SpriteRenderer exctractorRenderer;
    public GameObject exctractor_Up;
    public GameObject exctractor_Down;
    public GameObject exctractor_Left;
    public GameObject exctractor_Right;

    public string tagToPlaceOn;

    private bool isSelected;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 Hitpoint = Vector2.zero;

    void Update()
    {
        if (!isSelected)
        {
            if (selectItems.SelectExtractor_2 && TypeOfMachine == 2)
                isSelected = true;
            else if (selectItems.SelectFishNet_3 && TypeOfMachine == 3)
                isSelected = true;
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
        }
        else
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RayHit))
            {
                GameObject hit = RayHit.transform.gameObject;
                if (hit.CompareTag(tagToPlaceOn))
                {
                    Hitpoint = new Vector2(MathF.Round(RayHit.point.x), MathF.Round(RayHit.point.z));
                    Instantiate(exctractor_Up, HitPoint, Quaternion.EulerRotation, gameObject, )
                    Debug.Log(Hitpoint);
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
            }
        }
    }
}
