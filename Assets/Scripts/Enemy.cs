using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

    public string Name;
    private EnemyMode attack;
    public int tempo;
    public List<EnemyMode> Moves = new List<EnemyMode>();
    public BattleSheet sheet;
    public int health = 20;
    private Vector3 playerPosition;
    public Player playerInContact;
    private bool hasAttacked;
    private float time = 0;
    private Animator anim;
    public int ScoreAmount = 100;
    private bool dyingProcess = false;
    private float bioclock = 0;
    private EnemyMode currentstate;
    public float speed = 2;
    private float SightThreshold = 50;
    private CharacterController controller;
    public float Damping = 6;
    public float AttackDistance = 1.5f;
    public float attackTimer = 0;
    public float coolDown = 1.2f;
    private float DamageFactor = 1;
    int beatCount = 0;
    void Awake()
    {
        playerInContact = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        anim.speed = 1;
    }
  

    public void Death()
    {

           LevelManager mag = GameObject.FindObjectOfType<LevelManager>();
           mag.enemiesOnBoard--;
            playerInContact.AddScore(ScoreAmount);
            anim.SetBool("IsDead", true);

            dyingProcess = true;

            Destroy(GetComponent<CharacterController>());
            Destroy(this.gameObject, 2f);
            
    }


    
    // Use this for initialization
    void Start()
    {

	}
    
    public void LoseHealth(int damageTaken)
    {
        health -= damageTaken;
    }

    public float lookAt()
    {
        float distance =playerPosition.x - transform.position.x;
        if( distance > 0)
        {
            transform.Rotate(new Vector3(0,1,0),180);
            return 1;
        }
        else
        {
            transform.Rotate(new Vector3(0,1,0),180);
            return -1;
        }
    }

    void FixedUpdate()
    {
        bioclock += Time.deltaTime;
    }
	void Update () {
        playerPosition = playerInContact.transform.position;
        float distance = Vector3.Distance(playerPosition, transform.position);
        if ((int)bioclock > Moves.Count-1)
        {
            bioclock = 0;
        }
       
     
        //Debug.Log("Bio Clock: " + bioclock);
        //currentstate = Moves[(int)bioclock];
        if (Mathf.Abs(sheet.BeatTimeStamp[beatCount] - bioclock) <= .1)
        {
            beatCount++;
        }
        if(distance  < SightThreshold)
        {
            currentstate = Moves[beatCount];
            if (health > 0 )
            {
                Debug.Log("Current state :" + currentstate);
                switch (currentstate)
                {
                    case EnemyMode.Idle:
                        Debug.Log("Idle");
                        
                        break;
                    case EnemyMode.Minor:
                        anim.SetTrigger("Minor");
                        DamageFactor = 1;
                        
                        break;
                    case EnemyMode.Moderate:
                        anim.SetTrigger("Moderate");
                        DamageFactor = 1.3f;
                       
                        break;
                    case EnemyMode.Drastic:
                        anim.SetTrigger("Drastic");
                        DamageFactor = 1.5f;
                        
                        break;
                    case EnemyMode.Repeat:
                        break;
                }
            lookAt();
            controller.Move(new Vector3(speed * lookAt(), -10, 0) * Time.deltaTime);
            if (distance < AttackDistance)
            {         
               
                if(attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                if (attackTimer <= 0 && !dyingProcess)
                {
                    beatCount++;
                    playerInContact.TakeHealth((int)(5 * DamageFactor));
                    attackTimer = coolDown;
                }
               
              
            }
        }

       
        }
        if (health < 0 && !dyingProcess)
        {
            dyingProcess = true;
            Death();
        }

        if (hasAttacked && time < 1.0f)
        {
            time += Time.deltaTime;
        }
        else
        {
            hasAttacked = false;
        }
        
	}


}
