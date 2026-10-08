using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class EnemyManager
{
    public string EnemyName;
    public int Level;
    public int HP;
    public int maxHP;
    public int minDamage;
    public int minDamageMax;
    public int MinDamage;
    public int MaxDamagemin;
    public int MaxDamageMax;
    public int MaxDamage;
    public int idItem;
    public int MinGold;
    public int maxGold;
    public int GoldEarn;
    public string Message;
    public int mixExp;
    public int maxExp;
    public int ExpEarn;
    public bool CanPowerup;
    public int PercentagePower;
    public int minSpeed;
    public int maxSpeed;
    public int Speed;
    public int ProbailitaOggettoRilasciato;
    public GameObject vfxEnemy;
    public List<EnemySkill> AbilitaNemico = new List<EnemySkill>();
    


    public void Shuffle()
    {
        int randomHp = Random.Range(this.HP, this.maxHP);
        this.HP = +randomHp;
        maxHP = HP;
        int randoMinDamage = Random.Range(minDamage, minDamageMax);
        this.MinDamage = randoMinDamage;
        int randomMaxDamage = Random.Range(MaxDamagemin, MaxDamageMax);
        this.MaxDamage = randomMaxDamage;
        int randmGold = Random.Range(this.MinGold, this.maxGold);
        GoldEarn = randmGold;
        int expEarn = Random.Range(this.mixExp, this.maxExp);
        this.ExpEarn = expEarn;
        int randomSpeed = Random.Range(this.minSpeed, this.maxSpeed);
        Speed = randomSpeed;


    }
    public int SelectSkill()
    {
        int Selected = 0;

        for (int i = 0; i < AbilitaNemico.Count; i++)
        {
            int probailita = Random.Range(0, 100);
            if (probailita <= AbilitaNemico[0].Percentage)
            {
                Selected = i;
                break;
            }

        }
        return Selected;

    }
}
    [System.Serializable]
    public class EnemySkill
    {
        public string SkillName;
        public int minDamage;
        public int maxDamage;
        public int Percentage;

    }



