using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {

    Player player;
    LevelManager controller;
    public int damageFactor =10;
    public bool hasHit = false;
    void Awake()
    {
        player = GetComponentInParent<Player>();
        controller = GameObject.FindObjectOfType<LevelManager>();
    }
 
    void OnTriggerEnter(Collider col)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //hasAttacked = true;
            Enemy temp;
            if ((temp = col.gameObject.GetComponent<Enemy>()) != null)
            {

                Debug.Log("Hit: " + temp.name);
                hasHit = true;
                temp.LoseHealth(damageFactor);
            }
            else
            {
                hasHit = false;
            }

            Item item;
            if ((item = col.gameObject.GetComponent<Item>()) != null)
            {
                switch (item.type)
                {
                    case ItemType.SpeedUp:
                        {
                            player.SpeedBoost = true;
                            break;
                        }
                    case ItemType.SlowEnemy:
                        {
                            controller.SlowDownSpawn = true;
                            break;
                        }
                }
                item.Remove();
            }


        }
    }
    void OnTriggerStay(Collider col)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //hasAttacked = true;
            Enemy temp;
            if ((temp = col.gameObject.GetComponent<Enemy>()) != null)
            {
                Debug.Log("Hit: " + temp.name);
                hasHit = true;
                temp.LoseHealth(damageFactor);
            }

            else
            {
                hasHit = false;
            }
            Item item;
            if ((item = col.gameObject.GetComponent<Item>()) != null)
            {
                switch (item.type)
                {
                    case ItemType.SpeedUp:
                        {
                            player.SpeedBoost = true;
                            break;
                        }
                    case ItemType.SlowEnemy:
                        {
                            controller.SlowDownSpawn = true;
                            break;
                        }
                }
                item.Remove();
            }

        }
    }
}
