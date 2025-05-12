using UnityEngine;
using System.Collections;

public class BirbPatrol : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 1.5f;
    public float tiltAngle = 25f;
    public float tiltSpeed = 2f;
    public float pauseDuration = 2f;
    public float patrolRadius = 5f;
    public Transform center;

    private Vector3 target;
    private bool moving = false;

    void Start()
    {
        PickNewTarget();
        StartCoroutine(PatrolLoop());
    }

    void Update()
    {
        // Face target
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
        }

        // Apply forward tilt if moving
        Quaternion upright = transform.rotation;
        Quaternion tilted = upright * Quaternion.Euler(tiltAngle, 0, 0);
        Quaternion targetTilt = moving ? tilted : upright;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetTilt, Time.deltaTime * tiltSpeed);
    }

    IEnumerator PatrolLoop()
    {
        while (true)
        {
            moving = true;
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                Vector3 moveDir = (target - transform.position).normalized;
                transform.position += moveDir * moveSpeed * Time.deltaTime;
                yield return null;
            }

            moving = false;
            yield return new WaitForSeconds(pauseDuration);
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        Vector2 offset = Random.insideUnitCircle * patrolRadius;
        target = center.position + new Vector3(offset.x, Random.Range(-1f, 1f), offset.y);
    }
}
