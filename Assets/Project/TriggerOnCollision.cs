using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnCollision : MonoBehaviour
{
    public GameObject collisionObject;
    public UnityEvent events;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == collisionObject)
        {
            events.Invoke();
        }
    }
}
