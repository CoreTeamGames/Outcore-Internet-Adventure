using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxBackgroundMove : MonoBehaviour
{
    [SerializeField] float parallaxFactor;
    [SerializeField] float startpos;
    [SerializeField] GameObject cam;

    void Start()
    {
        startpos = transform.position.x;

    }
    public void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxFactor);
        float distance = cam.transform.position.x * -parallaxFactor;

        Vector3 newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        transform.position = newPosition;



    }
}
