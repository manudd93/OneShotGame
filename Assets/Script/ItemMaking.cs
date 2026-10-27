using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemMaking : ScriptableObject
{
 




    [Header("Informazioni")]
    public string itemName;
    
    [TextArea]
    public string description;
    public bool isStackable;
    public int maxStackSize;
    public bool consumeOnUse;
    public bool onlyOnMap;
    public Sprite icon;
    public Rarity rarità;
    [Header("Tipo")]
    public ItemType itemType;
    [Header("Impostazione Consumabile")]
    public int ID;
    public int RecoverHealt;
    public int DamageOnUse;
    public ConsumableType tipoConsumabile;
    [Header("Impostazione Abilita")]
    public SkillType tipoSkill;
   public int value;
    public int SkillID;
   
    [Header("Statistiche")]
    public int damage;
    public int defense;
    public int maxHealt;
    public int OneShot;
    public int Abilità;
    public int critico;
    public int Forza;
    public int Vitalità;
    public int Mente;
    public int Velocità;
    public int Rapidità;
    public virtual void Use(GameObject user)
    {
        Debug.Log($"Uso l'oggetto: {itemName}");
    }
    public  void Shuffle()
    {
        int percentualeRarità = Random.Range(0, 200);
        Debug.Log(percentualeRarità);
        if(percentualeRarità <= 20)
        {
            Debug.Log("Oggetto Indecente");

            rarità = Rarity.Indecente;
           

        }else if (percentualeRarità <= 45)
        {
            Debug.Log("oggetto scarso");
            rarità = Rarity.Scarso;
           
        }
        else if(percentualeRarità <= 100)
        {
            Debug.Log("oggetto normale");
            rarità = Rarity.Normale;
        }
        else if (percentualeRarità <= 150)
        {
            Debug.Log("oggetto super");
            rarità = Rarity.Super;
            

        }
        else if (percentualeRarità <= 165)
        {
            Debug.Log("oggetto Ricercato");
            rarità = Rarity.Ricercato;
            
        }
        else if (percentualeRarità <= 175)
        {
            Debug.Log("oggetto raro");
          
        }else if (percentualeRarità <= 185)
        {
            Debug.Log("oggetto epico");
            rarità = Rarity.Epico;
            
        }
        else if (percentualeRarità <= 200)
        {
            Debug.Log("oggetto Leggendario");
            rarità = Rarity.Leggendario;
            
        }



    }
}

