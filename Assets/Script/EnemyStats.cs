using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public int CurrentHealt;
    public int MaxHealt;
    public int minDamage;
    public int MaxDamage;
    public int Gold;
    public bool FoundItem;
    public int Speed;
    public int difesa;
   public bool isDie;
   [SerializeField] float maxBar = 100f;
    GameManager gameManager;
    public Image EnemyHealtBar;
    Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        player = GameObject.FindAnyObjectByType<Player>();
        
       
    }

    // Update is called once per frame
    public void EnemyDie()
    {
        Destroy(gameObject);
    }

    int CalculateDamage(int damage)
    {

        float dannoFinale = damage * (100f / (100f + this.difesa * 2f));
        dannoFinale -= this.difesa * 0.25f;

        return Mathf.Max(1, Mathf.RoundToInt(dannoFinale));
    }
    public void TakeDamage(int damage)
    {
        int calculateDamage = CalculateDamage(damage);
        CurrentHealt -= calculateDamage;
        EnemyHealtBar.fillAmount = CurrentHealt / maxBar;
        gameManager.WriteMessage(player.HeroName + " infligge " + calculateDamage);
       
        if (CurrentHealt <= 0)
        {
            isDie = true;
        }
        else
            isDie = false;
    }

    public void LoadStats(int maxHealt,int minDamage,int maxDamage,int Gold,bool isItem,int speed)
    {
        
        MaxHealt = maxHealt;
        CurrentHealt = maxHealt;
        this.minDamage = minDamage;
        this.MaxDamage = maxDamage;
        this.Gold = Gold;
        FoundItem = isItem;
        this.Speed = speed;
        maxBar = MaxHealt;

    }
    }
   
