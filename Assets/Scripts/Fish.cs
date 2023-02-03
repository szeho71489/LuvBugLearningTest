using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fish : MonoBehaviour
{
    [SerializeField]
    private float point;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool isHarmful;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool isDefaultLeft;

    public float Point { get => point; }
    public bool IsHarmful { get => isHarmful; }

    private Tweener moveTweener;

    private float lifeDuration = 15f;

    private void Start()
    {
        // Auto kill when life cycle ends
        StartCoroutine(AutoKillCoroutine());
    }

    public virtual void Move(Vector3 destinationPosition)
    {
        float distance = Vector3.Distance(transform.position, destinationPosition);
        moveTweener = transform.DOLocalMove(destinationPosition, distance / speed);

        // Set sprite flipX based on move direction
        spriteRenderer.flipX = destinationPosition.x > transform.position.x ? isDefaultLeft : !isDefaultLeft; 
    }

    public void Kill()
    {
        if (moveTweener.IsActive())
            moveTweener.Kill();

        Destroy(gameObject);
    }



    private IEnumerator AutoKillCoroutine()
    {
        WaitForSeconds waitForKill = new WaitForSeconds(lifeDuration);

        yield return waitForKill;

        if (moveTweener.IsActive())
            moveTweener.Kill();
        Destroy(gameObject, lifeDuration);
    }
}
