using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChecker : MonoBehaviour
{
    public Vector3 target;
    public GameObject redSoldier;
    public GameObject blueSoldier;
    public GameObject life;
    public GameObject plane;
    public LayerMask hitLayers;
    public int maxSoldiers;
    public int maxLifes;
    List<GameObject> blueSoldiers = new List<GameObject>();
    List<GameObject> redSoldiers = new List<GameObject>();
    List<GameObject> lifes = new List<GameObject>();
    private float timer = 0.0f;
    private float lifeSpawnTime = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > lifeSpawnTime && lifes.Count < maxLifes)
        {
            timer = 0.0f;
            var size = plane.GetComponent<Collider>().bounds.size;
            var auxObject = Instantiate(life, new Vector3(Random.Range(-size.x/3, size.x/3), 1.18f, Random.Range(-size.z/3, size.z/3)), Quaternion.identity);
            auxObject.SetActive(true);
            lifes.Add(auxObject);
            foreach (GameObject soldier in redSoldiers)
            {
                soldier.GetComponent<CharacterBehaviour>().lifes = lifes;
            }
            foreach (GameObject soldier in blueSoldiers)
            {
                soldier.GetComponent<CharacterBehaviour>().lifes = lifes;
            }
        }
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

                if (target.z > 0 && inBounds(target) && redSoldiers.Count< maxSoldiers)
                {
                    target.y = 0.25f;
                    var auxObject = Instantiate(redSoldier, target, Quaternion.identity);
                    auxObject.SetActive(true);
                    redSoldiers.Add(auxObject);
                    auxObject.GetComponent<CharacterBehaviour>().life = 500;
                    auxObject.GetComponent<CharacterBehaviour>().maxLife = 200;
                    auxObject.GetComponent<CharacterBehaviour>().lifePercentage = 0.5f;
                    auxObject.GetComponent<CharacterBehaviour>().damage =1;
                    auxObject.GetComponent<CharacterBehaviour>().enemies = blueSoldiers;
                    auxObject.GetComponent<CharacterBehaviour>().lifes = lifes;
                    foreach (GameObject soldier in blueSoldiers)
                    {
                        soldier.GetComponent<CharacterBehaviour>().enemies = redSoldiers;
                    }
                    foreach (GameObject soldier in redSoldiers)
                    {
                        soldier.GetComponent<CharacterBehaviour>().us = redSoldiers;
                    }
                } else if (target.z <= 0 && blueSoldiers.Count < maxSoldiers)
                {
                    var auxObject = Instantiate(blueSoldier, target, Quaternion.identity);
                    auxObject.SetActive(true);
                    blueSoldiers.Add(auxObject);
                    auxObject.GetComponent<CharacterBehaviour>().life = 500;
                    auxObject.GetComponent<CharacterBehaviour>().maxLife = 200;
                    auxObject.GetComponent<CharacterBehaviour>().lifePercentage = 0.5f;
                    auxObject.GetComponent<CharacterBehaviour>().damage = 1;
                    auxObject.GetComponent<CharacterBehaviour>().attackRadius = 2;
                    auxObject.GetComponent<CharacterBehaviour>().enemies = redSoldiers;
                    auxObject.GetComponent<CharacterBehaviour>().lifes = lifes;
                    foreach (GameObject soldier in redSoldiers)
                    {
                        soldier.GetComponent<CharacterBehaviour>().enemies = blueSoldiers;
                    }
                    foreach (GameObject soldier in blueSoldiers)
                    {
                        soldier.GetComponent<CharacterBehaviour>().us = blueSoldiers;
                    }
                }
            }


        }
    }

    public bool inBounds( Vector3 pos)
    {
        var size = plane.GetComponent<Collider>().bounds.size;
        if (pos.x > -size.x/2 && pos.x < size.x/2 && pos.z > -size.z/2 && pos.z < size.z / 2)
        {
            return true;
        }
        return false;
    }
}
