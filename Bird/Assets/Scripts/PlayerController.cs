using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const string CanUseClaw = "CanUseClaw";
    public const string animBoolName = "isOpen_Obj_";
    [SerializeField] BoxCollider FeetCollider;

    public Rigidbody Rigidbody;
    public CageDoorController CageDoorController;
    public lb_Bird animationClass;
    public float TurnSpeed = 10f;
    public float ForwardForce = 2f;
    public float MaxForwardSpeed = 4f;
    public float HopDistance = 0.01f;

    public float FlapForce = 0f;
    public float MaxFlapForce = 0.6f;
    public float MaxFlapSpeed = 5f;
    public float FlapIncrement = 0.005f;
    public float SlowFall = 0.3f;
    public Healthbar hunger;

    public bool UpDownEnabled = true;
    public bool ForwardEnabled = true;
    public bool TurnEnabled = true;
    public bool BeakEnabled = true;
    public bool ClawEnabled = true;
    public bool AllActionsDisabled = false;

    [SerializeField] PlayerMoveType movementType = PlayerMoveType.Walking;
    public BeakAndClawStatus currentAction;
    public GameObject holding = null;
    [SerializeField] Vector3 currentVelocity;
    private BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponents<BoxCollider>().First(c => !c.isTrigger);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check beak drop
        if (Input.GetKeyDown(KeyCode.B) && currentAction == BeakAndClawStatus.BeakFull && !AllActionsDisabled && BeakEnabled)
        {
            Drop();
        }

        //move bird
        ApplyMotionFromInput();
    }

    private void ApplyMotionFromInput()
    {
        if (AllActionsDisabled)
        {
            return;
        }

        //forward
        if (ForwardEnabled)
        {
            if (movementType == PlayerMoveType.Flying)
            {
                float currentForwardMaxSpeed = MaxForwardSpeed;
                float currentForwardForce = ForwardForce;
                if (Rigidbody.velocity.z > currentForwardMaxSpeed)
                {
                    Rigidbody.AddForce(transform.forward * -currentForwardForce);
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    Rigidbody.AddForce(transform.forward * currentForwardForce);
                }
            }else if (Input.GetKey(KeyCode.Space))
            {
                //hop
                animationClass.Hop();
                gameObject.transform.Translate(Vector3.forward * HopDistance);
            }
        }

        //turn
        if (TurnEnabled)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, Time.deltaTime * TurnSpeed * horizontalInput);
        }

        //apply flap force
        if (UpDownEnabled)
        {
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
        }

        Rigidbody.AddForce(Vector3.up * FlapForce);
        hunger.healthPerSecond = 0.1f + FlapForce*PlayerData.Difficulty;

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

        switch (movementType)
        {
            case PlayerMoveType.Flying:
                animationClass.TakeOff();
                break;
            case PlayerMoveType.Walking:
                animationClass.Land();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (contactPoint.thisCollider == FeetCollider)
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
        if (Input.GetKeyDown(KeyCode.B) && !AllActionsDisabled && BeakEnabled)
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

        if (Input.GetKeyDown(KeyCode.C) && !AllActionsDisabled && ClawEnabled)
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
                SetAction(BeakAndClawStatus.BeakFull);
                holding = collision.gameObject;
                holding.transform.parent = gameObject.transform;
                Physics.IgnoreCollision(holding.GetComponents<Collider>().First(x => !x.isTrigger), collider);
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
        SetAction(BeakAndClawStatus.Empty);
        holding.transform.parent = null;
        Physics.IgnoreCollision(holding.GetComponents<Collider>().First(x => !x.isTrigger), collider, false);
        holding = null;
    }
    IEnumerator Eating()
    {
        {
            SetAction(BeakAndClawStatus.Eating);
            AllActionsDisabled = true;
            StartCoroutine(animationClass.Eat());
            yield return new WaitForSeconds(BirdSeedController.EatingTime);
            SetAction(BeakAndClawStatus.Empty);
            AllActionsDisabled = false;
        }
    }

    private void SetAction(BeakAndClawStatus status)
    {
        currentAction = status;
        animationClass.beakAndClawStatus = currentAction;
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
}
