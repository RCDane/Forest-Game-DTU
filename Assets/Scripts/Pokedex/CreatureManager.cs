using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    [SerializeField]
    private CreatureInstance[] creatures;

    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private PokedexManager pokedexManager;
    public bool TryTakePicture(Camera cam)
    {
        foreach (CreatureInstance creature in creatures)
        {
            if (CheckIfInsideCamera(creature,cam))
            {
                pokedexManager.PicturetakenOff(creature);
                print("Picture taken");
                return true;
            }
        }
        return false;
    }


    private bool CheckIfInsideCamera(CreatureInstance creature, Camera camera)
    {
        Vector3 cameraPosition = camera.transform.position;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        Bounds bounds = new Bounds(creature.transform.position + creature.Offset, Vector3.one * creature.Radius);

        foreach (var p in planes)
        {
            float distance = p.GetDistanceToPoint(creature.transform.position + creature.Offset);
            if (distance < -creature.Radius)
                return false;
        }

        // sphere cast towards the creature, if there is something inbetween then we don't want to take a picture
        RaycastHit hit;
        Vector3 direction = (creature.transform.position + creature.Offset) - cameraPosition;
        if (Physics.SphereCast(cameraPosition, creature.Radius / 2.0f, direction, out hit, Vector3.Distance(cameraPosition, creature.transform.position + creature.Offset)))
        {
            print(hit.transform.name);
            // Check if the hit object is not the creature itself
            if (hit.collider.gameObject != creature.gameObject)
            {
                return false;
            }
        }
        return true;
    }
}
