using UnityEngine;

[CreateAssetMenu(fileName = "PotionList", menuName = "Scriptable Objects/Alchemy/PotionList_SO")]
public class PotionList_SO : ScriptableObject
{
    public Potion_SO[] SimplePotions { get => simplePotions; }
    [SerializeField] private Potion_SO[] simplePotions;
    public Potion_SO[] ComplexPotions { get => complexPotions; }
    [SerializeField] private Potion_SO[] complexPotions;
}
