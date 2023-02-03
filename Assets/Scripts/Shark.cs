using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class Shark : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float sickDuration;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Rect moveBoundary = new Rect(-6f, -4f, 6f, 4f);
    [SerializeField]
    private Animator animator;


    [Header("UI")]
    [SerializeField]
    private TMPro.TMP_Text scoreText;




    public event System.EventHandler OnSick;

    private Vector3 velocity;
    private float score;
    private bool isSick;
    private int size = 0;

    private void Start()
    {
        score = 0;
    }

    private void Update()
    {
        // Get player input to control shark
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        velocity = new Vector3(playerInput.x, playerInput.y, 0);


        // Check if shark position is within restricted area
        Vector3 newPosition = transform.localPosition + velocity * Time.deltaTime * speed;
        if(!moveBoundary.Contains(new Vector2(newPosition.x, newPosition.y)))
        {
            newPosition.x =
                Mathf.Clamp(newPosition.x, moveBoundary.xMin, moveBoundary.xMax);
            newPosition.y =
                Mathf.Clamp(newPosition.y, moveBoundary.yMin, moveBoundary.yMax);
        }
        transform.localPosition = newPosition;
    }

    private void LateUpdate()
    {
        // flip sprite based on shark velocity
        if(velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSick)
            return;

        // Detect if collider with fish
        if(other.gameObject.CompareTag("Fish"))
        {
            Fish fish = other.GetComponent<Fish>();

            if(!fish.IsHarmful)
            {
                AddScore(fish.Point);
            }
            else
            {
                StartCoroutine(SetSickCoroutine());
                OnSick?.Invoke(this, System.EventArgs.Empty);
            }
            fish.Kill();
        }
    }

    private void AddScore(float value)
    {
        score += value;
        scoreText.SetText(string.Format("Score: {0}", score));

        // For every 100 points, increase the scale of shark
        int newSize = (int)score / 25;
        if(newSize > size)
        {
            transform.localScale *= 1.1f;
            size = newSize;

            // Lower the speed of shark when it gets bigger
            speed = Mathf.Lerp(speed, speed / 2, (float)size / 10);
        }
    }

    private IEnumerator SetSickCoroutine()
    {
        isSick = true;

        WaitForSeconds waitForSick = new WaitForSeconds(sickDuration);

        animator.SetTrigger("Sick");

        yield return waitForSick;

        animator.SetTrigger("Normal");

        isSick = false;
    }
}
