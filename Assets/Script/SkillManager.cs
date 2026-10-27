using UnityEngine;

public class SkillManager : MonoBehaviour
{


    [SerializeField] CombatManager combatManager;
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
     
       
    }
    public void UseSkill(EquipmentData Skillequip)
    {
        switch (Skillequip.SkillID)
        {

            case 1:
                UseFireBall(Skillequip);
                    break;
        }
    }



    void UseFireBall(EquipmentData equip)
    {

        if (combatManager.state == BattleState.PLAYERTURN && gameManager.currentState == GameState.CombatMode)
        {
            combatManager.UseSkill();
        }
        {

        }

    }
}
