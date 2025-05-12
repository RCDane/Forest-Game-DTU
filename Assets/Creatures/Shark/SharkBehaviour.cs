using UnityEngine;
using System.Collections;

public class LandsharkMVP : MonoBehaviour
{
    enum State { Swim, Rear, Descend }
    State state;

    public Animator animator;
    public Transform center;         // Center point for patrol radius
    public float radius = 20f;
    public float swimSpeed = 4f;
    public float diveDepth = 1.5f;

    public float patrolDuration = 6f;
    public float rearDuration = 2f;
    public float descendDuration = 1f;

    private float descendTimer = 0f;
    private float startY = 0f;
    private float stateTimer = 0f;
    private Vector3 direction;

    void Start()
    {
        state = State.Swim;
        PickNewDirection();
        animator.Play("Swim");
    }

    void Update()
    {
        float groundY = center.position.y;
        Vector3 pos = transform.position;

        // Lock rotation to Y only
        Vector3 euler = transform.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        transform.eulerAngles = euler;

        switch (state)
        {
            case State.Swim:
                // Move forward
                Vector3 nextPos = transform.position + direction * swimSpeed * Time.deltaTime;

                // Stay within radius
                Vector2 flatOffset = new Vector2(nextPos.x - center.position.x, nextPos.z - center.position.z);
                if (flatOffset.magnitude > radius)
                {
                    // Nudge back inward and pick a new direction
                    Vector3 pullBack = (center.position - transform.position).normalized;
                    transform.position += pullBack * 0.5f;
                    PickNewDirection();
                    return; // Skip movement this frame
                }

                nextPos.y = groundY - diveDepth;
                transform.position = nextPos;

                stateTimer += Time.deltaTime;
                if (stateTimer >= patrolDuration)
                    SwitchState(State.Rear);
                break;

            case State.Rear:
                pos.y = groundY - diveDepth + 0.5f;
                transform.position = pos;
                // Wait for coroutine to handle transition
                break;

            case State.Descend:
                descendTimer += Time.deltaTime;
                float t = descendTimer / descendDuration;
                float targetY = groundY - diveDepth;
                float newY = Mathf.Lerp(startY, targetY, t);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                if (descendTimer >= descendDuration)
                    SwitchState(State.Swim);
                break;
        }
    }

    void SwitchState(State next)
    {
        state = next;
        stateTimer = 0f;

        switch (next)
        {
            case State.Swim:
                PickNewDirection();
                animator.Play("Swim");
                break;

            case State.Rear:
                StartCoroutine(WaitForRearAnim());
                break;

            case State.Descend:
                startY = transform.position.y;
                descendTimer = 0f;
                animator.Play("Descend");
                break;
        }
    }

    IEnumerator WaitForRearAnim()
    {
        animator.Play("Rear");

        yield return null;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Rear"))
            yield return null;

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        SwitchState(State.Descend);
    }

    void PickNewDirection()
    {
        Vector2 rand = Random.insideUnitCircle.normalized;
        direction = new Vector3(rand.x, 0f, rand.y);

        if (direction.magnitude < 0.1f)
            direction = new Vector3(0f, 0f, 1f); // fallback

        Quaternion look = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Euler(0f, look.eulerAngles.y, 0f);
    }

    void OnDrawGizmosSelected()
    {
        if (center == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(center.position, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + direction * 5f);
    }
}
