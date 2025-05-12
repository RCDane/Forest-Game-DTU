using UnityEngine;

public class CreatureInstance : MonoBehaviour
{
    [SerializeField]
    private float radius;

    [SerializeField]
    private Vector3 offset;

    public string creatureName;
    public float Radius
    {
        get => radius;
        set => radius = value;
    }

    public Vector3 Offset
    {
        get => offset;
        set => offset = value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
