using UnityEngine;

public class AlchemancerMediator : MonoBehaviour
{
    public PlayerHand PlayerHand { get => playerHand; }
    [SerializeField] private PlayerHand playerHand;
    public Player Player { get => player; }
    [SerializeField] private Player player;
    public Horde Horde { get => horde; }
    [SerializeField] private Horde horde;
}
