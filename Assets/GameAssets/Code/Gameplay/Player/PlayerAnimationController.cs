using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private AIPath aiPath;

    void Start()
    {
        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
    }

    void Update()
    {
        animator.SetBool("IsMoving", !aiPath.reachedEndOfPath && aiPath.canMove);
        if (aiPath.reachedEndOfPath) return;
        Vector2 playerVector = aiPath.NormalizedVector2d;
        animator.SetFloat("Horizontal", playerVector.x);
        animator.SetFloat("Vertical", playerVector.y);
    }
}
