using UnityEngine;

public class AlchemancerMediator : MonoBehaviour
{
    public PlayerHand PlayerHand { get => playerHand; }
    [SerializeField] private PlayerHand playerHand;
    public PlayerCombat PlayerCombat { get => playerCombat; }
    [SerializeField] private PlayerCombat playerCombat;
    public Horde Horde { get => horde; }
    [SerializeField] private Horde horde;
}
