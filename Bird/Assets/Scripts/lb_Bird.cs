using UnityEngine;
using System.Collections;

//modified significantly for user-controlled bird

public class lb_Bird : MonoBehaviour
{
    enum birdBehaviors
    {
        sing,
        preen,
        ruffle,
        peck,
        hopForward,
    }

    public AudioClip song1;
    public AudioClip song2;
    public AudioClip flyAway1;
    public AudioClip flyAway2;
    public BeakAndClawStatus beakAndClawStatus;

    Animator anim;
    public Rigidbody rb;

    bool paused = false;
    bool idle = true;
    bool flying = false;
    bool landing = false;
    bool onGround = true;
    BoxCollider birdCollider;
    float agitationLevel = .5f;
    float originalAnimSpeed = 1.0f;

    //hash variables for the animation states and animation properties
    int idleAnimationHash;
    int hopIntHash;
    int flyingBoolHash;
    int peckBoolHash;
    int ruffleBoolHash;
    int preenBoolHash;
    int landingBoolHash;
    int singTriggerHash;

    void OnEnable()
    {
        birdCollider = gameObject.GetComponent<BoxCollider>();
        anim = gameObject.GetComponent<Animator>();

        idleAnimationHash = Animator.StringToHash("Base Layer.Idle");
        hopIntHash = Animator.StringToHash("hop");
        flyingBoolHash = Animator.StringToHash("flying");
        peckBoolHash = Animator.StringToHash("peck");
        ruffleBoolHash = Animator.StringToHash("ruffle");
        preenBoolHash = Animator.StringToHash("preen");
        landingBoolHash = Animator.StringToHash("landing");
        singTriggerHash = Animator.StringToHash("sing");
        anim.SetFloat("IdleAgitated", agitationLevel);
    }

    public void PauseBird()
    {
        originalAnimSpeed = anim.speed;
        anim.speed = 0;
        GetComponent<AudioSource>().Stop();
        paused = true;
    }

    public void UnPauseBird()
    {
        anim.speed = originalAnimSpeed;
        paused = false;
    }

    public void TakeOff()
    {
        if (Random.value < .5)
        {
            GetComponent<AudioSource>().PlayOneShot(flyAway1, .1f);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(flyAway2, .1f);
        }
        flying = true;
        landing = false;
        onGround = false;
        anim.applyRootMotion = false;
        anim.SetBool(flyingBoolHash, true);
        anim.SetBool(landingBoolHash, false);
    }


    public void Land()
    {
        //initiate the landing for the bird to finally reach the target
        flying = false;
        onGround = true;
        anim.SetBool(flyingBoolHash, false);

    }

    public void Hop()
    {
        if (idle)
        {
            DisplayBehavior(birdBehaviors.hopForward);
        }
    }

    void OnGroundBehaviors()
    {
        idle = anim.GetCurrentAnimatorStateInfo(0).nameHash == idleAnimationHash ;

        if (idle && beakAndClawStatus==BeakAndClawStatus.Empty)
        {
            //the bird is in the idle animation, lets randomly choose a behavior every 3 seconds
            if (Random.value < Time.deltaTime * .33)
            {
                //bird will display a behavior
                //in the perched state the bird can only sing, preen, or ruffle
                float rand = Random.value;
                if (rand < .5)
                {
                    DisplayBehavior(birdBehaviors.preen);
                }
                else if (rand < .8)
                {
                    DisplayBehavior(birdBehaviors.ruffle);
                }
                else
                {
                    DisplayBehavior(birdBehaviors.sing);
                }
                //lets alter the agitation level of the brid so it uses a different mix of idle animation next time
                anim.SetFloat("IdleAgitated", Random.value);
            }
        }
    }

    void DisplayBehavior(birdBehaviors behavior)
    {
        idle = false;
        switch (behavior)
        {
            case birdBehaviors.sing:
                anim.SetTrigger(singTriggerHash);
                break;
            case birdBehaviors.ruffle:
                anim.SetTrigger(ruffleBoolHash);
                break;
            case birdBehaviors.preen:
                anim.SetTrigger(preenBoolHash);
                break;
            case birdBehaviors.peck:
                anim.SetTrigger(peckBoolHash);
                break;
            case birdBehaviors.hopForward:
                anim.SetInteger(hopIntHash, 1);
                break;
        }
    }

    public IEnumerator Eat()
    {
        DisplayBehavior(birdBehaviors.peck);
        yield return 3f;
    }


    void ResetHopInt()
    {
        anim.SetInteger(hopIntHash, 0);
    }

    void PlaySong()
    {
        if (Random.value < .5)
        {
            GetComponent<AudioSource>().PlayOneShot(song1, 1);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(song2, 1);
        }
    }

    void Update()
    {
        if (paused)
        {
            return;
        }
        else if (rb.velocity.z > 0.1f)
        {
            Debug.Log(rb.velocity.z);
            Hop();
        }
        else
        {
            OnGroundBehaviors();
        }
    }
}
