using UnityEngine;
using MyGame.Character;

public class Testing : MonoBehaviour
{
    public Hero myHero;
    public Enemy myEnemy;
    

    void Start()
    {
        myEnemy.PrintDamage();
        myHero.PrintHealth();  
    }
}
