using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectToTarget : MonoBehaviour
{

    Grid GridReference;//For referencing the grid class
    public float v;
    public float threshold;
    public ISteeringBehaviour currentBehaviour;
    CharacterController controller;

    private void Start()
    {
        GridReference = GameObject.Find("GameManager").GetComponent<Grid>();//Get a reference to the game manager
        controller = GetComponent<CharacterController>();
    }

    private void Awake()//When the program starts
    {
    }

    private void Update()//Every frame
    {
        controller.Move(currentBehaviour.NextDirection() * Time.deltaTime * v);
    }

    
}
