using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    //There is a frame at the start of the script that it has not reached the end of the path.
    private bool firstLoad;
    //Which was the last walk animation.
    private string lastWalkAnimation;

    private bool RunOnce;

    void Start()
    {
        animator = GetComponent<Animator>();
        firstLoad = false;
        RunOnce = true;
        lastWalkAnimation = "None";
    }

    void Update()
    {
        if(!firstLoad)
            firstLoad = true;
        else
        {
            if(GetComponent<Pathfinding.AIPath>().reachedEndOfPath)
            {
                if(RunOnce)
                {
                    animator.SetBool("IsMoving",false);
                    RunOnce = false;
                }
            }
            else
            {
                animator.SetBool("IsMoving",true);
                Vector2 playerVector = GetComponent<Pathfinding.AIPath>().NormalizedVector2d;
                animator.SetFloat("Horizontal", playerVector.x);
                animator.SetFloat("Vertical", playerVector.y);
                RunOnce = true;
            }
        }

    }
}
