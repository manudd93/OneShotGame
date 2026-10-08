using UnityEngine;

public class ConsumableManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Player player;
    [SerializeField] CombatManager combatManager;
    [SerializeField] InventoryManager inventory;


    public void UseConsumable(ItemMaking item,int index)
    {
        if (item.itemType == ItemType.Consumabile)
        {
            switch (item.ID)
            {
                case 1:
                    UsePotion(item , index);
                    
                    Debug.Log("pozione");
                    break;
            }
        }
    }

    void UsePotion(ItemMaking item,int index)
    {
        if (gameManager.currentState == GameState.Idle)
        {
            if (!player.CheckPlayerFullLife())
            {
                player.RecoverHealt(item.RecoverHealt);
                inventory.UseItemConfirmed(index);
            }
            
        }
        if(gameManager.currentState==GameState.CombatMode || gameManager.currentState == GameState.BossMode)
        {
            if (combatManager.state == BattleState.PLAYERTURN)
            {
                if (!player.CheckPlayerFullLife())
                {
                    combatManager.PlayerPassTurn();
                    inventory.UseItemConfirmed(index);
                    player.RecoverHealt(item.RecoverHealt);
                    gameManager.WriteMessage(player.HeroName + " utilizza " + item.name);
                }
               
              
                
                
               
            }
        }
    }

    

}
