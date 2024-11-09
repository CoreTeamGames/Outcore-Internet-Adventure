using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject objectForTp;
    public Vector2 position;
    public void Teleport()
    {
        objectForTp.transform.position = position;
    }
}
