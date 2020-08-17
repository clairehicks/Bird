using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const string CanUseClaw= "CanUseClaw";

    public Rigidbody Rigidbody;
    public CageDoorController CageDoorController;
    public float TurnSpeed = 10f;
    public float ForwardForce = 3f;
    public float MaxForwardSpeed = 5f;

    public float FlapForce = 0f;
    public float MaxFlapForce = 0.6f;
    public float MaxFlapSpeed = 5f;
    public float FlapIncrement = 0.005f;
    public float SlowFall = 0.3f;


    [SerializeField] PlayerMoveType movementType;
    public BeakAndClawStatus currentAction;
    public GameObject holding = null;
    [SerializeField] Vector3 currentVelocity;
    [SerializeField] FixedJoint joint = null;

    // Start is called before the first frame update
    void Start()
    {
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyMotionFromInput();
    }

    private void ApplyMotionFromInput()
    {
        //forward
        float currentForwardMaxSpeed = (movementType == PlayerMoveType.Walking ? 0.5f : 1.0f) * MaxForwardSpeed;
        float currentForwardForce = (movementType == PlayerMoveType.Walking ? 0.5f : 1.0f) * ForwardForce;
        if (Rigidbody.velocity.z > currentForwardMaxSpeed)
        {
            Rigidbody.AddForce(transform.forward * -currentForwardForce);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            Rigidbody.AddForce(transform.forward * currentForwardForce);
        }


        //turn
        var horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, Time.deltaTime * TurnSpeed * horizontalInput);

        //apply flap force
        var verticalInput = Input.GetAxis("Vertical");
        FlapForce += verticalInput * 0.01f;

        if (FlapForce > MaxFlapForce)
        {
            FlapForce = MaxFlapForce;
        }
        else if (FlapForce < 0)
        {
            FlapForce = 0;
        }

        Rigidbody.AddForce(Vector3.up * FlapForce);

        //cap vertical speed
        if (Rigidbody.velocity.y < -MaxFlapSpeed)
        {
            Rigidbody.AddForce(Vector3.up * 0.3f);
        }

        currentVelocity = Rigidbody.velocity;
    }

    //Todo update animations for flying/walking
    public void SetMovementType(PlayerMoveType moveType)
    {
        if (movementType == moveType)
        {
            return;
        }

        movementType = moveType;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (contactPoint.normal == Vector3.up && collision.rigidbody == null)
            {
                SetMovementType(PlayerMoveType.Walking);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody == null)
        {
            SetMovementType(PlayerMoveType.Flying);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetKeyDown(KeyCode.B) && collision.gameObject.tag == BirdSeedController.SeedTag)
        {
            SeedActions(collision);
        }
    }

    private void SeedActions(Collision collision)
    {
        if (currentAction == BeakAndClawStatus.Empty)
        {
            BirdSeedController seedScript = collision.gameObject.GetComponent<BirdSeedController>();
            //We are picking up seed
            if (seedScript.Lift())
            {
                currentAction = BeakAndClawStatus.BeakFull;
                holding = collision.gameObject;

                joint = gameObject.AddComponent<FixedJoint>();
                joint.anchor = collision.contacts[0].point;
                joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
                joint.massScale = 1/Rigidbody.mass;
                joint.connectedMassScale = 1/joint.connectedBody.mass;
            }
            else if (movementType == PlayerMoveType.Walking && seedScript.Eat())
            {
                StartCoroutine(Eating());


            }
        }
        else if (currentAction == BeakAndClawStatus.BeakFull)
        {
            BirdSeedController seedScript = holding.gameObject.GetComponent<BirdSeedController>();

            seedScript.Drop();
            currentAction = BeakAndClawStatus.Empty;
            holding = null;
            Destroy(joint);
            joint = null;
        }
    }

    IEnumerator Eating()
    {
        {
            currentAction = BeakAndClawStatus.Eating;
            yield return new WaitForSeconds(5);
            currentAction = BeakAndClawStatus.Empty;
        }
    }
}

public enum PlayerMoveType
{
    Walking,
    Flying
}

public enum BeakAndClawStatus
{
    Empty,
    BeakFull,
    ClawFull,
    Eating,
    Washing
}
