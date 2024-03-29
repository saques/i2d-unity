﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{

    Grid GridReference;//For referencing the grid class
    public float v;
    public float threshold;
    public ISteeringBehaviour currentBehaviour;
    public List<GameObject> enemies;
    public List<GameObject> us;
    public List<GameObject> lifes;
    CharacterController controller;
    public Vector3? lastPositionToGo;
    public float attackRadius;
    public int damage;
    public int life;
    public int maxLife;
    public float lifePercentage;
    private GameObject targetObject;
    private GameObject closestEnemy;
    public string state;
    public LayerMask hitLayers;
    int times;
    Vector3 lastPosition;



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
        if (life < 0)
        {
            foreach (GameObject soldier in enemies)
            {
                soldier.GetComponent<CharacterBehaviour>().enemies.Remove(gameObject);
            }
            Destroy(gameObject);
        }

        if (life < (int)(lifePercentage * maxLife))
        {
            state = "getLife";
            targetObject = GetClosest(lifes);
            lastPosition = transform.position;
            currentBehaviour.positionToGo = targetObject.transform.position;
        }
        else if (life >= (int)(lifePercentage * maxLife) )
        {
            state = "attack";
            targetObject = GetClosest(enemies);
        }
        closestEnemy = GetClosest(enemies);
        if (targetObject != null && state == "attack" &&(lastPositionToGo == null || lastPositionToGo.Value != targetObject.transform.position))
        {
            currentBehaviour.positionToGo = targetObject.transform.position;
            lastPositionToGo = targetObject.transform.position;

        }

        if (closestEnemy != null && Vector3.Distance(closestEnemy.transform.position, transform.position) < threshold)
        {
            closestEnemy.GetComponent<CharacterBehaviour>().life -= damage;
        }
        if (targetObject != null && Vector3.Distance(targetObject.transform.position, transform.position) < threshold && state == "getLife" )
        {
            GetComponent<CharacterBehaviour>().life = GetComponent<CharacterBehaviour>().maxLife;
            foreach (GameObject soldier in enemies)
            {
                soldier.GetComponent<CharacterBehaviour>().lifes.Remove(targetObject);
            }
            foreach (GameObject soldier in us)
            {
                soldier.GetComponent<CharacterBehaviour>().lifes.Remove(targetObject);
            }
            Destroy(targetObject);
        }
        if (targetObject != null)
        {
            Vector3 nextDir = currentBehaviour.NextDirection();
            controller.Move(nextDir * Time.deltaTime * v);
            transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
        }
    }

    GameObject GetClosest(List<GameObject> enemies)
    {
        GameObject gOMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject go in enemies)
        {
            float dist = Vector3.Distance(go.transform.position, currentPos);
            if (dist < minDist)
            {
                gOMin = go;
                minDist = dist;
            }
        }
        return gOMin;
    }


}
