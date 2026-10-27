using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int AttoNumber;
    public GameState currentState = GameState.Idle;
    public List<NpcManager> NumeroNpc = new List<NpcManager>();
    public List<EnemyManager> NumeroNemici = new List<EnemyManager>();
    public int Gold;
    public int GoldMaxEarnRandom;
    [SerializeField] TextMeshProUGUI GoldText;
    TextMessage textMessage;
    Player player;
    public float intervallo = 2f; // Secondi di attesa
    [SerializeField] int RandomEventQuest;
    [SerializeField] int RandomEventNpc;

    [SerializeField] int RandomEventGold;
    [SerializeField] int RandomEventEnemy;
    [SerializeField] int RandomEventNull;
    int SelectedNpc;
    int SelctedQuest;
    public int SelectedEnemy;
    public GameObject PanelCommand;
    public GameObject PanelCombat;
    public GameObject PanelHome;
    CombatManager combatManager;
    InventoryManager inventoryManager;
    EquipmentManager equipmentManager;
    EquipConfirmPopupUI popupUI;
    public ItemMaking itemConsumable;
    public EquipmentData itemEquip;
    public EquipmentData armorEquip;
    public EquipmentData SkillEquip;
    ConsumableManager consumableManager;
    [SerializeField] RoomManager roomManager;



    // Start is called before the first frame update
    void Start()
    {
        textMessage = GameObject.FindObjectOfType<TextMessage>();
        player = GameObject.FindObjectOfType<Player>();
        consumableManager = GetComponent<ConsumableManager>();
        combatManager = Object.FindAnyObjectByType<CombatManager>();
        inventoryManager = GetComponent<InventoryManager>();
        equipmentManager = GetComponent<EquipmentManager>();
        popupUI = GameObject.FindObjectOfType<EquipConfirmPopupUI>();

        GoldText = GameObject.FindGameObjectWithTag("GoldText").GetComponent<TextMeshProUGUI>();
        int npcSelected = UnityEngine.Random.Range(0,NumeroNpc.Capacity);
        Debug.Log(npcSelected);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            itemEquip.Shuffle();
            popupUI.Show(itemEquip, equipmentManager, inventoryManager);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            itemEquip.Shuffle();
            popupUI.Show(armorEquip, equipmentManager, inventoryManager);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            inventoryManager.AddItem(itemConsumable);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            popupUI.Show(SkillEquip, equipmentManager, inventoryManager);
            
        }
    }
    public void CheckChangeRoom(string room)
    {
        if (room == "village")
        {
            if (currentState == GameState.Idle)
            {
                roomManager.ChangeRoom(room);
            }
        }
    }

    public void ChangePanel(int panel)
    {
        switch (panel)
        {
            case 1:
                PanelCommand.gameObject.SetActive(true);
                PanelCombat.gameObject.SetActive(false);
                PanelHome.gameObject.SetActive(false);
                break;

            case 2:

                    PanelCommand.gameObject.SetActive(false);
                    PanelCombat.gameObject.SetActive(true);
                    PanelHome.gameObject.SetActive(false);
               
                break;
            case 3:
                if (currentState == GameState.Idle)
                {
                    PanelCommand.gameObject.SetActive(false);
                    PanelCombat.gameObject.SetActive(false);
                    PanelHome.gameObject.SetActive(true);
                }
                break;
        }
    }

    public void StartExplore()
    {
        if (currentState == GameState.Idle)
        {
            currentState = GameState.Searchig;
            textMessage.ClearText();
            textMessage.WriteText(player.HeroName + " si incammina per la sua strada....");
            
            
            TimerWait.EseguiDopo(2.5f, Explore);
        }
       

    }
    

    public void Explore()
    {
        if (currentState == GameState.Searchig)
        {
            textMessage.ClearText();
            //textMessage.WriteText(player.HeroName + " incontra " + Atto[AttoNumber].NumeroNpc[0].NameNpc +"...");
            //TimerWait.EseguiDopo(2.5f, NpcExecute);
            int value = 0;
            value = Randomizza();
            

                
            if (value <= RandomEventQuest)
            {
                Quest();
                return;
            }
             if(value <= RandomEventNpc)
            {
                Debug.Log("Parte npc");
                NpcExecute();
                return;
            }
             
             if (value <= RandomEventGold)
            {
                Debug.Log("Trovi gold");
                TimerWait.EseguiDopo(intervallo, EarnGoldExplore);
                return;
            }
             if (value <= RandomEventEnemy)
            {
                Debug.Log("Parte battaglia");
               TimerWait.EseguiDopo(intervallo,Battle);
                return;
            }
            if (value <= RandomEventNull)
            {
                TimerWait.EseguiDopo(intervallo, NonsuccedeNiente);
                return;
            }



        }
    }
    void EarnGoldExplore()
    {
        int gold = RandoGoldEarn();
        textMessage.ClearText();
        textMessage.WriteText(player.HeroName + " ha trovato a terra " + gold + " Gold");
        AddGold(gold);
        TimerWait.EseguiDopo(intervallo, ReturnGameStateBase);
    }
    public int RandoGoldEarn()
    {
        int gold = UnityEngine.Random.Range(1, GoldMaxEarnRandom);
        return gold;
    }
    
    public int Randomizza()
    {
        int random = UnityEngine.Random.Range(0, 100);
        Debug.Log(random);
        return random;
    }
    public int RandomizzaNpc()
    {
        int npcSelected =0;
         npcSelected=UnityEngine.Random.Range(0, NumeroNpc.Count);
        SelectedNpc = npcSelected;
        Debug.Log(npcSelected);
        return npcSelected;
    }
    public int RandomizzaNemico()
    {
        int enemy= UnityEngine.Random.Range(0, NumeroNemici.Count);
        SelectedEnemy = enemy;
        return enemy;
    }
    void Quest()
    {
        textMessage.ClearText();
        textMessage.WriteText(player.HeroName + " non ci sono quest per ora...");
        TimerWait.EseguiDopo(1.5f, ReturnGameStateBase);
    }

   public void Battle()
    {

        currentState = GameState.CombatMode;

        combatManager.BattleStart();
        ChangePanel(2);
        

    }
   public void StopBattle()
    {
    


        PanelCommand.SetActive(true);
        PanelCombat.SetActive(false);
        ReturnGameStateBase();
    }
   private void NonsuccedeNiente()
    {
        textMessage.ClearText();
        textMessage.WriteText(player.HeroName + " non trova niente di speciale...");
        TimerWait.EseguiDopo(1.5f, ReturnGameStateBase);
      
    }
    public void AddGold(int gold)
    {
        Gold += gold;
        GoldText.text = Gold.ToString();

    }
    public void RemoveGold(int gold)
    {
        Gold -= gold;
        if (Gold <= 0){
            Gold = 0;
            }
        GoldText.text = Gold.ToString();
    }
    public void NpcExecute()
    {
        int npc = RandomizzaNpc();
        textMessage.WriteText(NumeroNpc[npc].NpcMessage);
        if (NumeroNpc[SelectedNpc].takeItem)
        {
            TimerWait.EseguiDopo(2.5f, GetItemNpc);
        }
        else
        {

            TimerWait.EseguiDopo(intervallo, NpcNoItem);
        }
    }
    void NpcNoItem()
    {
        textMessage.ClearText();
        textMessage.WriteText("Solo un saluto....");
        ReturnGameStateBase();
    }
    void GetItemNpc()
    {
        textMessage.ClearText();
        textMessage.WriteText("Hai ottenuto da "+ NumeroNpc[SelectedNpc].NameNpc + " " + NumeroNpc[SelectedNpc].ItemId);
        TimerWait.EseguiDopo(intervallo, ReturnGameStateBase);
    }

    void ReturnGameStateBase()
    {
        textMessage.ClearText();
        textMessage.WriteText(player.HeroName + " si incammina....");
        
            currentState = GameState.Idle;
        
    }

    public void WriteMessage(string message)
    {
        textMessage.ClearText();
        textMessage.WriteText(message);
    }

    public void UseConsumable(ItemMaking item, int index)
    {
        consumableManager.UseConsumable(item , index);
    }
    public void SetSkill(EquipmentData skill)
    {
        combatManager.SetSkill(skill);
    }

}


public enum GameState
{
    Searchig,
    CombatMode,
    ItemShop,
    Quest,
    BossMode,
    Idle


}
