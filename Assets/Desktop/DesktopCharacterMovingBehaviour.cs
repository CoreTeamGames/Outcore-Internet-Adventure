using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DesktopCharacterMovingBehaviour : MonoBehaviour
{
    public NavMeshAgent characterAgent;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveTo(Vector2 endPosVector)
    {
        characterAgent.SetDestination(endPosVector);
    }
}
