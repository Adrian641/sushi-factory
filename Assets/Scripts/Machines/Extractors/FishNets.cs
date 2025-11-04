using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNets : MonoBehaviour
{
    public PlaceFishExtractor placeFishExtractors;

    public Camera mainCam;
    public RaycastHit RayHit;
    public Ray ray;
    public Vector2 hitPoint = Vector2.zero;

    public Vector3 cagePos;

    public GameObject fishNet;

    private Transform cage;

    void Update()
    {
        if (placeFishExtractors.isFishNet && mainCam.ScreenToWorldPoint(Input.mousePosition) != cagePos)
        {
            cage = Instantiate(fishNet.transform);
            cagePos = new Vector3(mainCam.ScreenToWorldPoint(Input.mousePosition).x, 12f, mainCam.ScreenToWorldPoint(Input.mousePosition).y);
            cage.transform.position = cagePos;
            Debug.Log(mainCam.ScreenToWorldPoint(Input.mousePosition));
            cage.LookAt(mainCam.transform);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] RayHit = Physics.RaycastAll(ray);

                for (int i = 0; i < RayHit.Length; i++)
                {
                    if (RayHit[i].collider.CompareTag("Fish"))
                    {
                        if (RayHit[i].transform.name[0] == 's')
                            placeFishExtractors.fishType = 1;
                        if (RayHit[i].transform.name[0] == 't')
                            placeFishExtractors.fishType = 2;

                        placeFishExtractors.isFishNet = false;
                        Destroy(fishNet);
                    }
                }
            }
        }
        else
        {
            Destroy(cage);
        }
    }
}
