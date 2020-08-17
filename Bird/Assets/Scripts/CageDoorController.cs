using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageDoorController : MonoBehaviour
{
    //public Rigidbody doorRB;
    public Quaternion open = Quaternion.Euler(180, 0, 0);
    private Quaternion closed = Quaternion.Euler(0, 0, 0);
    private State state = State.Closed;
    private State? turning = null;
    private float openFraction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (turning.HasValue)
        {
            switch (turning.Value)
            {
                case State.Open:
                    openFraction += Time.deltaTime;
                    if (openFraction > 1)
                    {
                        return;
                    }
                    break;
                case State.Closed:
                    openFraction -= Time.deltaTime;
                    if (openFraction <0)
                    {
                        return;
                    }

                    break;
            }

        gameObject.transform.rotation = Quaternion.Slerp(closed, open, openFraction);
            Debug.Log(gameObject.transform.rotation);
        }
    }

    public void Use(State action)
    {
        if (state == action)
        {
            return;
        }

        turning = action;
        state = action;
    }

    public State GetState()
    {
        return state;
    }

    public enum State
    {
        Closed,
        Open
    }

}
