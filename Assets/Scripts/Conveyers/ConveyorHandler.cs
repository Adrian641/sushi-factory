using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorHandler : MonoBehaviour
{
    Transform conveyorBelts;

    void Start()
    {
        conveyorBelts = this.transform;
    }

    void Update()
    {
        int numberConveyorGroups = conveyorBelts.childCount;

        Debug.Log(numberConveyorGroups);
    }
}
