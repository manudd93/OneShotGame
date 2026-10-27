using UnityEngine;
using System;

public class Wallet : MonoBehaviour
{
    public int Coins { get; private set; } = 500;
    public event Action<int> OnChanged;

    public bool TrySpend(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        OnChanged?.Invoke(Coins);
        return true;
    }
}
