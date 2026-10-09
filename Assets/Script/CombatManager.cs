using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOSE,WAITING };
public class CombatManager : MonoBehaviour
{
    [SerializeField] int TurnCounter = 1;
    public BattleState state;
    GameManager gameManager;
    Player player;
    LevelManager levelManager;
    int EnemySelected;
    bool isDeadPlayer;
   public GameObject EnemyField;
  [SerializeField] EnemyStats enemyStats;
    //public InventorySlotUI SlotSkill;
    [SerializeField] EquipmentManager equipmentManager;
    public EquipmentData SkillEquip;


    // Start is called before the first frame update
    void Start()
    {
       gameManager= GameObject.FindObjectOfType<GameManager>();
        player = GameObject.FindAnyObjectByType<Player>();
        levelManager = GameObject.FindAnyObjectByType<LevelManager>();

     //  gameManager.Battle();

        // BattleStart();
    }

    // Update is called once per frame

   public void BattleStart()
    {
        state = BattleState.START;
        
        EnemySelected = gameManager.RandomizzaNemico();
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
         Instantiate(gameManager.NumeroNemici[EnemySelected].vfxEnemy,EnemyField.transform.position,Quaternion.identity);

        //gameManager.WriteMessage("Inizia il combattimento con " + gameManager.NumeroNemici[EnemySelected].EnemyName);

        enemyStats = Object.FindAnyObjectByType<EnemyStats>();
        yield return new WaitForSeconds(2f);

        //gameManager.WriteMessage("Nemico: " + gameManager.NumeroNemici[EnemySelected].Message);
        gameManager.WriteMessage("Appare " + gameManager.NumeroNemici[EnemySelected].EnemyName+"....");
        TimerWait.EseguiDopo(gameManager.intervallo, EnemyMessage);
     
        
    }
    public void UseSkill()
    {
        
    }
    void EnemyMessage()
    {
        
        gameManager.WriteMessage("Nemico " + gameManager.NumeroNemici[EnemySelected].Message);
        gameManager.NumeroNemici[EnemySelected].Shuffle();
        LoadEnemyStats();
       
    }


    void LoadEnemyStats()
    {
        enemyStats.LoadStats(gameManager.NumeroNemici[EnemySelected].maxHP, gameManager.NumeroNemici[EnemySelected].minDamage, gameManager.NumeroNemici[EnemySelected].MaxDamage, gameManager.NumeroNemici[EnemySelected].GoldEarn, false, gameManager.NumeroNemici[EnemySelected].Speed);
        TimerWait.EseguiDopo(1f, CheckBattleTurn);
    }

    void CheckBattleTurn()
    {
        if (player.Rapidita >= enemyStats.Speed)
        {
            TimerWait.EseguiDopo(2f, StartPlayerTurn);

        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void StartPlayerTurn()
    {
        CheckWinLose();
        if (state == BattleState.WON || state == BattleState.LOSE)
        {
            return;
        }
        gameManager.WriteMessage("Cosa vuoi fare...!");
        TimerWait.EseguiDopo(2f, waitPlayer);
    }


    void waitPlayer()
    {
        state = BattleState.PLAYERTURN;
    }
    public void PlayerBasicAttack()
    {
        
        if (state != BattleState.PLAYERTURN)
            return;
        state = BattleState.WAITING;

       StartCoroutine(PlayerNormalAttack());




    }

    int PlayerCalculateBasicAttack()
    {
        int damage = Random.Range(player.MinDamage,player.MaxDamage);
        return damage;
    }
    IEnumerator PlayerNormalAttack()
    {
        gameManager.WriteMessage(player.HeroName + " attacca " + gameManager.NumeroNemici[EnemySelected].EnemyName + "..."); ;

        yield return new WaitForSeconds(2.5f);

        enemyStats.TakeDamage(PlayerCalculateBasicAttack());
       
        TimerWait.EseguiDopo(2f, StartEnemyTurn);

    }

    public void PlayerPassTurn()
    {
        state = BattleState.ENEMYTURN;
        TimerWait.EseguiDopo(1f, StartEnemyTurn);
    }
  public void StartEnemyTurn()
    {
        CheckWinLose();
        if (state == BattleState.WON || state==BattleState.LOSE)
        {
            return;
        }
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    int EnemyBasicAttack()
    {

        int damage = Random.Range(gameManager.NumeroNemici[EnemySelected].minDamage, gameManager.NumeroNemici[EnemySelected].MaxDamage);
        return damage;
    }
    IEnumerator EnemyTurn()
    {
        
        gameManager.WriteMessage(gameManager.NumeroNemici[EnemySelected].EnemyName + " attacca " + player.HeroName);

        int basicAttack = EnemyBasicAttack();
       
        yield return new WaitForSeconds(2f);
        if (!isDeadPlayer)
        {
            player.TakeDamage(basicAttack);
           
            TimerWait.EseguiDopo(2f,StartPlayerTurn);

        }
       
    }
    public void Win()
    {
        enemyStats.EnemyDie();
        gameManager.WriteMessage("Ottieni " + gameManager.NumeroNemici[EnemySelected].ExpEarn + "esperienza..");
        levelManager.AddExperience(gameManager.NumeroNemici[EnemySelected].ExpEarn);
        
        TimerWait.EseguiDopo(2f, exitBattle);
    }

    void exitBattle()
    {
        gameManager.StopBattle();
    }
    
   public void CheckWinLose()
    {
        if (enemyStats.isDie)
        {
            state = BattleState.WON;
            gameManager.WriteMessage("Hai vinto la battaglia...");
            TimerWait.EseguiDopo(2f, Win);
           
        }
        if (player.isDie)
        {
            state = BattleState.LOSE;
            gameManager.WriteMessage("Hai perso...");
        }
        
    }


}
