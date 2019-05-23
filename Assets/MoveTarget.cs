using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    public LayerMask hitLayers;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))//If the player has left clicked
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // create a plane at 0,0,0 whose normal points to +Y:
            Plane hPlane = new Plane(Vector3.up, Vector3.zero);
            // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
            float distance = 0;

            RaycastHit hit;

            // if the ray hits the plane...
            if (hPlane.Raycast(ray, out distance) && !Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayers)) {
                // get the hit point:
                this.transform.position = ray.GetPoint(distance);
            }

        }
    }
}

