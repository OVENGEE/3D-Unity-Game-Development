using UnityEngine;

namespace MyGame.Character
{
    public class Hero : MonoBehaviour
    {
        public int playerHealth = 100;

        public void PrintHealth()
        {
            Debug.Log("PLAYER health" + playerHealth);
        }
    }
}