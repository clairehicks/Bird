using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const string CanUseClaw = "CanUseClaw";
    public const string animBoolName = "isOpen_Obj_";

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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check beak drop
        if (Input.GetKeyDown(KeyCode.B) && currentAction == BeakAndClawStatus.BeakFull)
        {
            Drop();
        }

        //move bird
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
        Debug.Log("Enter");
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
        Debug.Log("Stay");

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (collision.gameObject.tag == BirdSeedController.SeedTag)
            {
                SeedActions(collision);
            }
            else if (collision.gameObject == CageDoorController.gameObject)
            {
                CageDoorController.Open();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClawActions(collision);
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
                holding.transform.parent = gameObject.transform;
                Physics.IgnoreCollision(holding.GetComponents<Collider>().First(x => !x.isTrigger), gameObject.GetComponent<SphereCollider>());
            }
            else if (movementType == PlayerMoveType.Walking && seedScript.Eat())
            {
                StartCoroutine(Eating());
            }
        }
    }

    private void ClawActions(Collision collision)
    {
        if (collision.gameObject.CompareTag(CanUseClaw))
        {
            return;
        }

        MoveableObject moveable = collision.gameObject.GetComponent<MoveableObject>();
        if (moveable == null) { return; }

        Animator anim = moveable.GetComponentInParent<Animator>();

        if (anim == null) { return; }

        string nameOfAnim = animBoolName + moveable.objectNumber;
        anim.SetBool(nameOfAnim, !anim.GetBool(nameOfAnim));
        anim.enabled = true;
    }

    private void Drop()
    {
        BirdSeedController seedScript = holding.gameObject.GetComponent<BirdSeedController>();
        if (seedScript != null)
        {
            seedScript.Drop();
        }
        currentAction = BeakAndClawStatus.Empty;
        holding.transform.parent = null;
        Physics.IgnoreCollision(holding.GetComponents<Collider>().First(x => !x.isTrigger), gameObject.GetComponent<SphereCollider>(),false);
        holding = null;
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
