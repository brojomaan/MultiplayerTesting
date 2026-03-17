using Data;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "CardData/Card")]
public class CardData : ScriptableObject
{
    public int id;
    public string displayName;
    public Sprite icon;

    public CardEffect effect;

}
