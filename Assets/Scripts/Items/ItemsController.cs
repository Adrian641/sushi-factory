using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    public float updateTime = 0.2f;
    public float dt;
    private void FixedUpdate()
    {
        dt += Time.fixedDeltaTime;
        if (dt > updateTime)
        {
            dt = 0;
            bool isOnBelt = false;
            RaycastHit[] RayHit = Physics.RaycastAll(new Vector3(transform.position.x, 10f, transform.position.z), Vector3.down, 20f);
            for (int i = 0; i < RayHit.Length; i++)
            {
                if (RayHit[i].collider.CompareTag("Belt"))
                {
                    isOnBelt = true;
                }
            }
            if (!isOnBelt)
                DestroyImmediate(gameObject);
        }
    }
}
