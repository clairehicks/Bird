using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageDoorController : MonoBehaviour
{
    public FixedJoint joint;
    private State state = State.Closed;
    [SerializeField] BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Open()
    {
        if (joint != null)
        {
            Destroy(joint);
            state = State.Open;
            StartCoroutine(DestroyCollider());
        }
    }

    IEnumerator DestroyCollider()
    {
        yield return new WaitForSeconds(1);
            Destroy(collider);
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
