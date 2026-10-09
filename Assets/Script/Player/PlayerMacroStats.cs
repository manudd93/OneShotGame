using UnityEngine;

public class PlayerMacroStats : MonoBehaviour
{

    Player player;
    [SerializeField] int numberVolteForza = 0;


    private void Awake()
    {
        player = GetComponent<Player>();
    }
    public void AumentoForza(int numeroVolte)
    {
        player.baseDamageMin += 2;
        player.baseDamageMax += 2;
        player.MinDamage += 2;
        player.MaxDamage += 2;
        numberVolteForza += 1;
        
        numeroVolte--;
        player.forza++;
        
        if (numeroVolte >= 0)
        {
            AumentoForza(numeroVolte);
        }
        player.UpdateText();

    }
    public void AumentoVitalita(int numeroVolte)
    {
        player.baseDefense += 1;
        player.Difesa = +1;
        player.MaxHealt += 15;
        player.baseHealt += 15;
        
        player.vitalita++;
       
        numeroVolte--;
        if (numeroVolte >= 0)
        {
            AumentoVitalita(numeroVolte);
        }
        player.UpdateText();


    }
    public void AumentoMente(int numeroVolte)
    {
        player.Abilita += 4;
        player.baseAbilita += 4;
       
        player.mente++;
        player.UpdateText();
        numeroVolte--;
        if (numeroVolte >= 0)
        {
            AumentoMente(numeroVolte);
        }
    }

    public void AumentoVelocita(int numeroVolte)
    {
        player.baseSpeed += 1;
        player.Rapidita += 1;
        
        player.velocita++;
        player.UpdateText();
        numeroVolte--;
        if (numeroVolte >= 0)
        {
            AumentoVelocita(numeroVolte);
        }
    }

    public void CheckMacroStats()
    {

    }

    public void SetNewStats(int NewForza,int newVitalita,int newMente,int newVelocita)
    {
        AumentoForza(NewForza);
        AumentoVitalita(newVitalita);
        AumentoMente(newMente);
        AumentoVelocita(newVelocita);
    }
}
