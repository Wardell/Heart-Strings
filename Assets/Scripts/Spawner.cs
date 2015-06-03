using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Spawner : MonoBehaviour {
  
  
    
    public List<GameObject> objects;
    Vector3 direction;
    Transform desination;
    public BattleSheet sheet;
    float timeElaps = 0;
    int beatCount = 0;
    public float aveSignal;

    [SerializeField]
    Transform High;

    [SerializeField]
    Transform Mid;

    [SerializeField]
    Transform Low;
    void Awake()
    {
        Low = GameObject.Find("Low").GetComponent<Transform>();
        Mid = GameObject.Find("Mid").GetComponent<Transform>();
        High = GameObject.Find("High").GetComponent<Transform>();
    }
    void Spawn(Transform trans)
    {
       
            int ran = UnityEngine.Random.Range(0, 2);
            Instantiate(objects[ran], new Vector3(trans.position.x, trans.position.y,0.0f ), trans.rotation);
            //lm.enemiesOnBoard++;     
        
    }
 

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        timeElaps += Time.deltaTime;

       // Debug.Log("Spawner Beat Count: " + beatCount);
        if (Mathf.Abs(sheet.BeatTimeStamp[beatCount] - timeElaps) <= .1f)
        {
            beatCount++;
            if (Mathf.Abs(sheet.SignalChain[beatCount] - aveSignal) < 1.0f)
            {
                Spawn(Mid);
            }
            else if (sheet.SignalChain[beatCount] > aveSignal)
            {
                Spawn(High);
            }
            else
            {
                Spawn(Low);
            }
        }      
    }
}
