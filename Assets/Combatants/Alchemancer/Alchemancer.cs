using UnityEngine;

public class Alchemancer : MonoBehaviour
{
    public PlayerHand PlayerHand { get => playerHand; }
    [SerializeField] private PlayerHand playerHand;
    public PlayerCombat PlayerCombat { get => playerCombat; }
    [SerializeField] private PlayerCombat playerCombat;
}
