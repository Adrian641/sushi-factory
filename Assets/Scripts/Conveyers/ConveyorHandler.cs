using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorHandler : MonoBehaviour
{
    public PlaceConveyors conveyors;

    Transform conveyorBelts;

    int numberConveyorGroups = 0;

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
            DecriptConveyorBelts(numberConveyorGroups);
            conveyors.isHandling = false;
        }

    }

    public Vector2Int[] DecriptConveyorBelts(int nbOfConveyorGroups)
    {
        Vector2Int[] allActiveBelts = new Vector2Int[conveyors.arrayLimits];
        string BeltName;
        string component = "";
        for (int i = 0; i < nbOfConveyorGroups; i++)
        {
            Transform conveyorGroupX = conveyorBelts.transform.GetChild(i);
            int conveyorGroupXNbChild = conveyorGroupX.childCount;
            for (int j = 0; j < conveyorGroupXNbChild; j++)
            {
                Transform BeltsTransform = conveyorGroupX.transform.GetChild(j);
                GameObject BeltsGameObject = BeltsTransform.gameObject;
                BeltName = BeltsGameObject.name;
                for (int k = 0; k < BeltName.Length; k++)
                {
                    bool isFirstComponent = true;
                    if (BeltName[k] != ',')
                    {
                        component += BeltName[k];
                        Debug.Log(component);
                    }
                    else if (isFirstComponent)
                    {
                        allActiveBelts[j].x = int.Parse(component);
                        component = "";
                    }
                    else
                    {
                        allActiveBelts[j].y = int.Parse(component);
                        component = "";
                    }
                    Debug.Log(allActiveBelts[j]);
                }
            }
        }
        return allActiveBelts;
    }
}
