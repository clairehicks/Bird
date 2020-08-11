using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public float TurnSpeed = 10f;
    public float ForwardForce = 3f;
    public float MaxForwardSpeed = 5f;

    public float FlapForce = 0f;
    public float MaxFlapForce = 0.6f;
    public float MaxFlapSpeed = 5f;
    public float FlapIncrement = 0.005f;
    public float SlowFall = 0.3f;


    [SerializeField] bool onFloor;
    [SerializeField] Vector3 currentVelocity;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMotionFromInput();
    }

    private void ApplyMotionFromInput()
    {
        //forward
        float currentForwardMaxSpeed = (onFloor ? 0.5f : 1.0f) * MaxForwardSpeed;
        float currentForwardForce = (onFloor ? 0.5f : 1.0f) * ForwardForce;
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
    public void SetOnFloor(bool onFloor)
    {
        if (this.onFloor == onFloor)
        {
            return;
        }

        this.onFloor = onFloor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (contactPoint.normal == Vector3.up)
            {
                SetOnFloor(true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        SetOnFloor(false);
    }
}
