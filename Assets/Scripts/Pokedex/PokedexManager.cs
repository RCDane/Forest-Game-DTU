using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokedexManager : MonoBehaviour
{
    [SerializeField]
    private List<CreatureInfo> creatures;

    [SerializeField]
    private GameObject pokedexCanvasObj;

    [SerializeField]
    private GameObject pokedexEntryPrefab;


    [SerializeField]
    private GameObject pokedexTarget;


    private bool isPokedexActive = false;

    [SerializeField]
    private float rotationSpeed = 1.0f;
    [SerializeField]
    private float movementSpeed = 1.0f;

    [SerializeField]
    private float activeAngle;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float fadeSpeed = 1.0f;

    struct PokedexInfoData
    {
        public string id; // name
        public bool hasBeenSeen;
        public PokedexUIEntry entry;
    }


    private List<PokedexInfoData> pokedexInfoDatas = new List<PokedexInfoData>();
    // Update is called once per frame
    private void Awake()
    {
        FillList();
    }

    void FillEntry(PokedexUIEntry entry, CreatureInfo creature)
    {
        entry.nametext.text = "Name: ?" ;
        entry.descriptiontext.text = "Description: " + creature.Description;
    }

    void CorrectlyFillEntry( string nameId)
    {
        // find entry
        var infoData = pokedexInfoDatas.FirstOrDefault(data => data.id == nameId);
        if (infoData.entry == null)
            return;

        print("We got here");
        infoData.entry.nametext.text = "Name: " + nameId;
    }

    private void Update()
    {
        isPokedexActive = ShouldBeActive();
        PokedexMovement();
    }

    bool ShouldBeActive()
    {
        // If camera is looking towards pokedex target withing a certain angle then true
        Camera cam = Camera.main;
        Vector3 direction = (pokedexTarget.transform.position - cam.transform.position).normalized;
        float angle = Vector3.Angle(cam.transform.forward, direction);
        if (angle < activeAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void PokedexMovement()
    {
        if (isPokedexActive)
        {
            Vector3 targetPos = pokedexTarget.transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * movementSpeed);

            Camera cam = Camera.main;
            Vector3 direction = (transform.position - cam.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            pokedexCanvasObj.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        }

        if (isPokedexActive && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
        }
        else if (!isPokedexActive && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
        }

        canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
    }

    void SetActiveForPokedex(bool active)
    {
        if (active)
        {
            pokedexCanvasObj.SetActive(true);
        }
        else
        {
            pokedexCanvasObj.SetActive(false);
        }
    }

    void FillList()
    {
        foreach (CreatureInfo creature in creatures)
        {
            GameObject entryObj = Instantiate(pokedexEntryPrefab, pokedexCanvasObj.transform);
            PokedexUIEntry entry = entryObj.GetComponent<PokedexUIEntry>();
            pokedexInfoDatas.Add(new PokedexInfoData
            {
                id = creature.Name,
                hasBeenSeen = false,
                entry = entry
            });
            FillEntry(entry, creature);
        }
    }

    public void PicturetakenOff(CreatureInstance creature)
    {
        for (int i = 0; i < pokedexInfoDatas.Count; i++)
        {
            PokedexInfoData data = pokedexInfoDatas[i];
            if (data.id == creature.creatureName)
            {
                CorrectlyFillEntry(creature.creatureName);
                data.hasBeenSeen = true;
                break;
            }
        }


    }


}
