﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteeringBehaviour
{
    public Vector3 target;
    public LayerMask hitLayers;



    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (positionToGo != null)//If the player has left clicked
        {
            target = positionToGo.Value;
            target.y = 0.25f;
            positionToGo = null;
        }
    }

    public override Vector3 NextDirection()
    {
        target.y = 0.25f;
        return Vector3.Normalize(target - transform.position);
    }
}
