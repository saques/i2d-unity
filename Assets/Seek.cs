using System.Collections;
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

        if (Input.GetMouseButtonDown(0))//If the player has left clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // create a plane at 0,0,0 whose normal points to +Y:
            Plane hPlane = new Plane(Vector3.up, Vector3.zero);
            // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
            float distance = 0;

            RaycastHit hit;

            // if the ray hits the plane...
            if (hPlane.Raycast(ray, out distance) && !Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayers))
            {
                // get the hit point:
                //this.transform.position = ray.GetPoint(distance);

                target = ray.GetPoint(distance);
            }


        }


    }

    public override Vector3 NextDirection()
    {
        return Vector3.Normalize(target - transform.position);
    }
}
