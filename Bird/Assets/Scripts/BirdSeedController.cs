using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSeedController : MonoBehaviour
{
    [SerializeField] BirdSeedStatus status;
    [SerializeField] Rigidbody rigidbody;

    public const string SeedTag = "BirdSeed";
    public const string SeedPrefabPath = "Prefabs/seedbox";
//    public const string SeedPrefabPath = "Prefabs/BirdSeed";
    public const float Mass = 1.0f;
    public const float CarryMass = 0.0f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public BirdSeedStatus GetStatus()
    {
        return status;
    }

    public bool Lift()
    {
        if (status == BirdSeedStatus.Full)
        {
            status = BirdSeedStatus.Carried;
            rigidbody.mass = CarryMass;
            return true;
        }
        return false;
    }

    public void Drop()
    {
        status = BirdSeedStatus.Full;
        rigidbody.mass = Mass;
    }


    public bool Eat()
    {
        //start eat animation
        if (status == BirdSeedStatus.Spilled)
        {
            status = BirdSeedStatus.Empty;
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("rel velocity " + collision.relativeVelocity.magnitude);
        if (status == BirdSeedStatus.Full && collision.rigidbody == null && collision.relativeVelocity.magnitude > 0.1)
        {
            //start spill animation
            status = BirdSeedStatus.Spilled;
        }
    }

}

public enum BirdSeedStatus
{
    Full,
    Carried,
    Spilled,
    Empty
}
