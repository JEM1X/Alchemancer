using UnityEngine;

public class AlchemancerMediator : MonoBehaviour
{
    public PlayerHand PlayerHand { get => playerHand; }
    [SerializeField] private PlayerHand playerHand;
    public PlayerCombat Player { get => player; }
    [SerializeField] private PlayerCombat player;
    public Horde Horde { get => horde; }
    [SerializeField] private Horde horde;
}
