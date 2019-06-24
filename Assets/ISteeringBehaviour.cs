using UnityEngine;

public abstract class ISteeringBehaviour: MonoBehaviour
{
    public Vector3? positionToGo;
    public abstract Vector3 NextDirection();
    public float threshold;


}
