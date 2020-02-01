using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StickToRandomPoint : MonoBehaviour
{
   RandomPoint rand;
    private void Start()
    {
        rand = GetComponentInParent<RandomPoint>();
    }
    void Update()
    {
        transform.position = rand.target;
    }
}