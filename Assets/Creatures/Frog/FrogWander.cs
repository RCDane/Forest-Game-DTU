using UnityEngine;
using System.Collections;

public class FrogWander : MonoBehaviour
{
    public Animator animator;
    public float hopForce = 2f;
    public float hopCooldown = 2f;
    public float wanderRadius = 5f;
    public Transform wanderCenter;  // Empty GameObject in scene
    public float rotationSpeed;

    Rigidbody rb;
    Vector3 targetPos;
    bool isJumping = false;
    bool isTurning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(HopLoop());
    }

    IEnumerator HopLoop()
    {
        while (true)
        {
            if (!isJumping)
            {
                PickTarget();

                PickTarget();
                animator.Play("Walk");
                isTurning = true;

                yield return StartCoroutine(RotateTowardsTarget());

                isTurning = false;


                animator.Play("Jump");
                yield return new WaitForSeconds(0.5f);  // adjust to match jump launch moment

                DoJump();
                animator.Play("Land");
                yield return new WaitForSeconds(0.8f);  // time to land

                animator.Play("Idle");
                yield return new WaitForSeconds(hopCooldown);
            }
            yield return null;
        }
    }

    void PickTarget()
    {
        Vector2 circle = Random.insideUnitCircle * wanderRadius;
        Vector3 offset = new Vector3(circle.x, 0, circle.y);
        targetPos = wanderCenter.position + offset;
    }

    IEnumerator RotateTowardsTarget()
    {
        Vector3 flatTarget = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        while (true)
        {
            Vector3 dir = (targetPos - transform.position);
            dir.y = 0;  // remove vertical component
            if (dir == Vector3.zero) yield break;

            Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            float angleDiff = Quaternion.Angle(transform.rotation, targetRot);
            if (angleDiff < 1f) break;

            yield return null;
        }
    }

    void FaceTarget()
    {
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void DoJump()
    {
        Vector3 jumpDir = (targetPos - transform.position).normalized;
        rb.AddForce(jumpDir * hopForce + Vector3.up * 3f, ForceMode.Impulse);
    }
}
