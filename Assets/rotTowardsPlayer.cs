using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotTowardsPlayer : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = target.position - transform.position;
        
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 5, 0.0f);
        
        transform.rotation = Quaternion.LookRotation(newDirection);
        
    }
}
