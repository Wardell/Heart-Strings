using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Player : MonoBehaviour {

    Vector3 pos = new Vector3();
    bool hasAttacked = false;
    public bool SpeedBoost = false;
    float speedUpTimer = 0;
    private int Life = 3;
    private int health = 100;
    private int score = 0;
    public Text DisplayHealth;
    public Text DisplayLives;
    public Text DisplayScore;
    Text DisplayHyper;
    const int rythmLimit = 5;
    [SerializeField]
    int rythmCount = 0;

    float hyperTime = -1;
    bool isHyper = false;
    private float speed = 10.0f;
    private float jumpSpeed = 30.0f;
    private float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController charControl;
    private Animator anim;
    private Collider hitBox;
    private PlayerHit damagerange;
    private float timeElaps = 0;
    public List<float> hypertimes;
    [SerializeField]
    private int beat = 0;
    private Color noHyperText;
    private Color showHyperText;
    private bool hasBeat= false;
    private Slider hyperGuage;
    private float sustainRythem;
    private int oldRythmCount;
    public List<float> tempo;
    void Awake()
    {
        DisplayHyper = GameObject.Find("Hyper!").GetComponent<Text>();
        damagerange = GetComponentInChildren<PlayerHit>();
        damagerange.damageFactor = 500;
        hitBox = GameObject.Find("HitRange").GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        charControl = GetComponent<CharacterController>();
        noHyperText = new Color(DisplayHyper.color.r, DisplayHyper.color.g, DisplayHyper.color.b, 0);
        showHyperText = new Color(DisplayHyper.color.r, DisplayHyper.color.g, DisplayHyper.color.b, 255);
        DisplayHyper.color = noHyperText;
        hyperGuage = GameObject.Find("Hyper Gauge").GetComponentInChildren<Slider>();
        hyperGuage.maxValue = rythmLimit;
        hyperGuage.value = rythmCount;
        damagerange.damageFactor = 1;

        GenerateSpeed(tempo[0]);
    }
    void Start()
    {
       
    }
    public void AddScore(int points)
    {
        score += points;
    }
    public void RestartHealth()
    {
        health = 100;
    }
    public void TakeLife()
    {
        Life--;
    }
    void GenerateSpeed(float tempo)
    {
        if (tempo < 70)
        {
            speed = 6f;
        }
        else if (tempo >= 70 && tempo < 90)
        {
            speed = 10f;
        }

        else if (tempo >= 90 && tempo < 120)
        {
            speed = 15f;
        }
        else if (tempo >= 120 && tempo < 150)
        {
            speed = 20f;
        }
        else
        {
            speed = 30f;
        }



    }
    
    void Update()
    {
        timeElaps += Time.deltaTime;
        DisplayHealth.text = "Health: " + health;
        DisplayLives.text = "Life X" + Life;
        DisplayScore.text = "Score: " + score;

        if (Mathf.Abs(hypertimes[beat] - timeElaps) < .1f)
        {
            beat++;
            hasBeat = true;
        }

        else
        {
            hasBeat = false;
        }

       
        
        if (hyperTime > 0)
        {
            hyperGuage.value = hyperTime;
            hyperTime -= Time.deltaTime;
        }

        if (hyperTime <= 0 && isHyper)
        {
            rythmCount = 0;
            DisplayHyper.color = noHyperText;
            damagerange.damageFactor = 5;
            speed = 6;
            jumpSpeed = 16;
            isHyper = false;
        }

        if (rythmCount > rythmLimit && !isHyper)
        {
            isHyper = true;
           
            hyperTime = rythmLimit;
            DisplayHyper.color = showHyperText;
            damagerange.damageFactor = 20;
            speed = 8;
            jumpSpeed = 20;
        }

        if (SpeedBoost)
        {
            speedUpTimer = 5.0f;
            SpeedBoost = false;
            speed *= 5;
        }
        else if (speedUpTimer > 0.0f)
        {
            speedUpTimer -= Time.deltaTime;
        }

        if (speedUpTimer < 0.0f)
        {
            speed = 6;
            speedUpTimer = 0.0f;
        }
        
        if (charControl.isGrounded)
        {
            moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, 0));
            moveDirection *= speed;
            if (Input.GetKey(KeyCode.Z))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            charControl.SimpleMove(moveDirection);
            moveDirection.y -= gravity * Time.deltaTime;
        }

       
        moveDirection.y -= gravity * Time.deltaTime;
        charControl.Move(moveDirection * Time.deltaTime);
        anim.SetFloat("Running Mode", moveDirection.x * Time.deltaTime);
        anim.SetFloat("Jump Mode", moveDirection.y);
        oldRythmCount = rythmCount;
        if (Input.GetKeyDown(KeyCode.X) && !hasAttacked)
        {
            if (hasBeat && damagerange.hasHit && !isHyper)
            {
                sustainRythem =0;
                rythmCount++;
                hyperGuage.value = rythmCount;
                Debug.Log("Rythm Count " + rythmCount);
            }
           
            hasAttacked = true;
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            hasAttacked = false;
            damagerange.hasHit = false;
        }
       
      
    }

    public bool OutOfHealth()
    {
        return health <= 0;
    }
    public bool IsDead()
    {
        return Life < 0;
    }
    public void TakeHealth(int damage)
    {
        health -= damage;
    }

}
