using UnityEngine;

public class DumbAnimal : MonoBehaviour
{
    public Transform center;
    public float radius = 10f;
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    public float pauseTime = 2f;

    [Header("Animation Names")]
    public string walkAnim = "Walk";
    public string runAnim = "Run";
    public string idleAnim = "Idle";
    public string grazeAnim = "Graze";

    private Animator animator;
    private Vector3 target;
    private bool moving = false;
    private float pauseTimer = 0f;
    private float speed = 1.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        PickTarget();
        moving = true;
        PlayMoveAnimation();
    }

    void Update()
    {
        if (moving)
        {
            Vector3 toTarget = target - transform.position;
            toTarget.y = 0f;

            if (toTarget.magnitude > 0.5f)
            {
                Vector3 moveDir = toTarget.normalized;
                transform.position += moveDir * speed * Time.deltaTime;

                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 2f);
            }
            else
            {
                moving = false;
                pauseTimer = 0f;
                PlayIdleAnimation();
            }
        }
        else
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseTime)
            {
                PickTarget();
                moving = true;
                PlayMoveAnimation();
            }
        }

        GroundClamp();
    }

    void PickTarget()
    {
        Vector2 offset = Random.insideUnitCircle * radius;
        target = center.position + new Vector3(offset.x, 0f, offset.y);

        // Randomly choose walk or run
        bool doRun = Random.value < 0.3f; // 30% chance to run
        speed = doRun ? runSpeed : walkSpeed;
    }

    void PlayMoveAnimation()
    {
        if (speed == runSpeed)
            animator.Play(runAnim);
        else
            animator.Play(walkAnim);
    }

    void PlayIdleAnimation()
    {
        bool doGraze = Random.value < 0.3f; // 30% chance to graze
        animator.Play(doGraze ? grazeAnim : idleAnim);
    }

    void GroundClamp()
    {
        Vector3 origin = transform.position + Vector3.up * 2f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 5f))
        {
            Vector3 corrected = transform.position;
            corrected.y = hit.point.y;
            transform.position = Vector3.Lerp(transform.position, corrected, Time.deltaTime * 10f);
        }
    }
}
