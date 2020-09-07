using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSeedController : MonoBehaviour
{
    [SerializeField] BirdSeedStatus status;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] Healthbar hunger;
    [SerializeField] AudioSource audio;

    public const string SeedTag = "BirdSeed";
    public const string SeedPrefabPath = "Prefabs/seedbox";
    public const int DropTime = 2;
    public const int EatingTime = 3;
    public const float SeedFill = 100f;

    // Start is called before the first frame update
    void Start()
    {
        status = BirdSeedStatus.Full;
        hunger = GameObject.Find("HungerBar").GetComponent<Healthbar>();
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
            rigidbody.isKinematic = true;
            return true;
        }
        return false;
    }

    public void Drop()
    {
        status = BirdSeedStatus.Full;
        rigidbody.isKinematic = false;
    }


    public bool Eat()
    {
        //start eat animation
        if (status == BirdSeedStatus.Spilled)
        {
            status = BirdSeedStatus.Empty;
            particleSystem.Play();
            hunger.GainHealth(SeedFill/PlayerData.Difficulty);
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (status == BirdSeedStatus.Full && collision.rigidbody == null && collision.relativeVelocity.magnitude > 0.1)
        {
            //start spill animation
            status = BirdSeedStatus.Spilled;
            StartCoroutine(SeedDrop());
        }
    }

    IEnumerator SeedDrop()
    {
        audio.Stop();
        audio.Play();
        particleSystem.Play();
        yield return new WaitForSeconds(DropTime);
        particleSystem.Pause();
    }

}


public enum BirdSeedStatus
{
    Full,
    Carried,
    Spilled,
    Empty
}
