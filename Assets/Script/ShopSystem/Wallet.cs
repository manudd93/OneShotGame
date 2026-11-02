using UnityEngine;
using System;

public class Wallet : MonoBehaviour
{
    public int Gold = 500;
    [SerializeField] GameManager gameManager;
    public event Action<int> OnChanged;

    public bool TrySpend(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        OnChanged?.Invoke(Gold);
        gameManager.RefreshGold();
        return true;
       
    }

    public int GetGold()
    {
        return Gold;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        gameManager.RefreshGold();
    }

    public void RemoveGold(int amount)
    {
        Gold -= Gold;
        if (Gold <= 0)
        {
            Gold = 0;
        }
        gameManager.RefreshGold();
    }
}
