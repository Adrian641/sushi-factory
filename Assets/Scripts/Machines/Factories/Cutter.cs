using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    public string code;
    public Vector2 dir;

    void Start()
    {
        dir = GetObjectDir();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("smt is touching me without consent");
        if (other.CompareTag("Item"))
        {
            code = GetType(other.gameObject.name);
        }
    }

    static string GetType(string name)
    {
        string typeString = "";
        for (int i = 0; i < name.Length; i++)
        {
            if (!Char.IsDigit(name[i]))
                break;
            else
                typeString += name[i];
        }
        string type = typeString;
        return type;
    }

    Vector2 GetObjectDir()
    {
        Vector2 dir = Vector2.zero;
        Transform child = gameObject.transform.GetChild(0);
        if (child.name[child.name.Length - 1] == 'U')
            dir = Vector2.up;
        else if (child.name[child.name.Length - 1] == 'D')
            dir = Vector2.down;
        else if (child.name[child.name.Length - 1] == 'L')
            dir = Vector2.left;
        else if (child.name[child.name.Length - 1] == 'R')
            dir = Vector2.right;
        else
            Destroy(gameObject);

        return dir;
    }
}
