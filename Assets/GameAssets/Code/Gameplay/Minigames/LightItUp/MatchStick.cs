using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatchStick : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    [Header("Matchbox")]
    [SerializeField] [Range(0f, 10f)] private float maxAngle = 5f;
    [SerializeField] [Range(1000f, 3000f)] private float minVelocity = 2000f;

    private Animator animator;

    private DateTime timeEntered;
    private Vector3 positionEntered;

    private Rigidbody2D rbody;

    private bool isLit = false;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        timeEntered = DateTime.UtcNow;
        positionEntered = transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Candle" || !isLit) return;
        double seconds = (DateTime.UtcNow - timeEntered).TotalSeconds;
        Candle candle = collision.GetComponent<Candle>();
        if (seconds < candle.MinTimeToLightCandle) return;
        candle.LightCandle();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Matchbox")
        {
            HandleMatchbox();
        }
    }

    private void HandleMatchbox()
    {
        double seconds = (DateTime.UtcNow - timeEntered).TotalSeconds;
        Vector3 direction = positionEntered - transform.position;

        float distance = direction.magnitude;
        double velocity = distance / seconds;

        Vector3 directionNorm = direction.normalized;
        Vector3 desiredAngle = new Vector3(Mathf.Sign(directionNorm.x), 0, 0);

        float angle = Vector3.Angle(direction.normalized, desiredAngle);
        if (angle <= maxAngle && velocity >= minVelocity)
        {
            animator.SetTrigger("Lit");
			eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.MatchLight));
            isLit = true;
        } else
        {
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.MatchStrike));
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rbody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rbody.bodyType = RigidbodyType2D.Kinematic;
    }
}
