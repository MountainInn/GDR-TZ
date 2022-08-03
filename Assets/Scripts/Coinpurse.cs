using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Coinpurse : MonoBehaviour, IResetable
{
    [SerializeField] Text coinsText;
    [SerializeField] UnityEvent onAllCoinsCollected;

    uint coinsToCollect;

    uint _coins;
    uint Coins
    {
        get => _coins;
        set {
            _coins = value;
            coinsText.text = "Coins: " + _coins;
        }
    }

    void Start()
    {
        Reset();
    }

    public void AddCoin()
    {
        Coins++;

        if (Coins == coinsToCollect)
            onAllCoinsCollected.Invoke();
    }

    public void Reset()
    {
        Coins = 0;
        coinsToCollect = (uint)FindObjectsOfType<Coin>().Length;
    }
}
