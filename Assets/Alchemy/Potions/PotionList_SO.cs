using UnityEngine;

[CreateAssetMenu(fileName = "PotionList", menuName = "Scriptable Objects/Alchemy/PotionList_SO")]
public class PotionList_SO : ScriptableObject
{
    public Potion_SO[] AllPotions { get => allPotions; }
    [SerializeField] private Potion_SO[] allPotions;
}
