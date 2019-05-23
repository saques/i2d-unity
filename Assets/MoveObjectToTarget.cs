using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectToTarget : MonoBehaviour
{

    Grid GridReference;//For referencing the grid class
    public int i;
    public float v;
    public float threshold;
    public ISteeringBehaviour currentBehaviour;
    CharacterController controller;

    private void Start()
    {
        i = -1;
        GridReference = GetComponent<Grid>();//Get a reference to the game manager
        controller = GetComponent<CharacterController>();
    }

    private void Awake()//When the program starts
    {
    }

    private void Update()//Every frame
    {
        //if (i != -1)
        //{

        //    float deltaT = Time.deltaTime;

        //    Vector3 target = GridReference.FinalPath[i].vPosition;

        //    if(Vector3.Distance(target, StartPosition.position) < threshold)
        //    {
        //        if (GridReference.FinalPath.Count - 1 == i)
        //        {
        //            i = -1;
        //        }
        //        else
        //        {
        //            i++;
        //        }
        //        return;
        //    }

        //    StartPosition.position += Vector3.Normalize(target - StartPosition.position)*deltaT*v;

        //}
        controller.Move(currentBehaviour.NextDirection() * Time.deltaTime * v);
    }

    
}
