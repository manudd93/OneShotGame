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
        player.UpdateText();
        numeroVolte--;
        if (numeroVolte >= 0)
        {
            AumentoForza(numeroVolte);
        }
        
    }
    public void AumentoVitalita()
    {
        player.baseDefense += 1;
        player.Difesa = +1;
        player.MaxHealt += 15;
        player.baseHealt += 15;
        player.UpdateText();
       
    }
    public void CheckMacroStats()
    {

    }

    public void SetNewStats(int NewForza,int newVitalita,int newMente,int newVelocita)
    {
        AumentoForza(NewForza);
    }
}
