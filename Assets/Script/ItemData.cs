using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ItemData 
{
    public  int ID;
    public string NameItem;
    public ItemType tipoOggetto;
    public bool OnlyOnMap;
    public string Descrizione;
    public Image VFX;
    public Rarity rarity;

}


public enum ItemType
{
    Consumabile,
    Arma,
    Armatura,
    Abilita,
    Passiva,
    Accessorio
}
public enum ConsumableType
{
    LoseRecover,
    Buff,
    Nerf,
    Damage,
    QuestItem,
    Passive
    

}
public enum SkillType
{
    Damage,
    Heal,
    Passive

}

public enum Rarity
{
    Indecente,
    Scarso,
    Normale,
    Super,
    Ricercato,
    Raro,
    Epico,
    Leggendario,
    
}


