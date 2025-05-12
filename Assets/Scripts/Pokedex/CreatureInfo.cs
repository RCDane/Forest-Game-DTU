using UnityEngine;

[CreateAssetMenu(fileName = "CreatureInfo", menuName = "Pokedex/CreatureInfo", order = 1)]
public class CreatureInfo : ScriptableObject
{

    public string Name;
    public string Description;
    public string Id;
}
