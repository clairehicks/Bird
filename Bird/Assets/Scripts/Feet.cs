using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    public PlayerController player;
    public CapsuleCollider playerCollider;
    public Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(collider, playerCollider);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            player.SetMovementType(PlayerMoveType.Walking);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            player.SetMovementType(PlayerMoveType.Flying);
        }
    }
}
