using UnityEngine;

public class AlchemancerMediator : MonoBehaviour
{
    public PlayerHand PlayerHand { get => playerHand; }
    private PlayerHand playerHand;
    public Cauldron Cauldron { get => cauldron; }
    private Cauldron cauldron;

    private void Awake()
    {
        playerHand = GetComponent<PlayerHand>();
        cauldron = GetComponent<Cauldron>();
    }
}
