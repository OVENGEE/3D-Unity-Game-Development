using UnityEngine;

namespace MyGame.Character
{
    public class Enemy : MonoBehaviour
    {
        public int damage = 25;

        public void PrintDamage()
        {
            Debug.Log("Damage" + damage);
        }
    }

}
