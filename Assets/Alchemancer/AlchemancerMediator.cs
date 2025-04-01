using UnityEngine;

public class AlchemancerMediator : MonoBehaviour
{
    public PlayerHand PlayerHand { get => playerHand; }
    [SerializeField] private PlayerHand playerHand;
    public Cauldron Cauldron { get => cauldron; }
    [SerializeField] private Cauldron cauldron;
}
