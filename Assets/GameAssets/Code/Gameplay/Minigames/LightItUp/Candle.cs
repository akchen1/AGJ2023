using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public bool IsLit { get; private set; } = false;

    [SerializeField] private GameObject fire;
    [field: SerializeField][field: Range(0f, 5f)] public float MinTimeToLightCandle { get; private set; } = 3f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void LightCandle()
    {
        animator.SetTrigger("Lit");
        IsLit = true;
    }
}
