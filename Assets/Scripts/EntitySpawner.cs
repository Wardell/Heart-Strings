using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EntitySpawner : MonoBehaviour {

    public List<GameObject> objects;
    Vector3 direction;
    Transform desination;
    public BattleSheet sheet;
    float timeElaps = 0;
    int beatCount = 0;
    public float aveSignal;
    public List<EnemyMode> Moves;
    [SerializeField]
    Transform High;

    [SerializeField]
    Transform Mid;

    [SerializeField]
    Transform Low;
    void Awake()
    {
        Low = GameObject.Find("Item Low").GetComponent<Transform>();
        Mid = GameObject.Find("Item Mid").GetComponent<Transform>();
        High = GameObject.Find("Item High").GetComponent<Transform>();
    }
    void Spawn(Transform trans)
    {

        GameObject obj;
        Enemy e;
        int ran = UnityEngine.Random.Range(0, objects.Count);
        obj = Instantiate(objects[ran], new Vector3(trans.position.x, trans.position.y, 0.0f), trans.rotation) as GameObject;
        if (e = obj.GetComponent<Enemy>())
        {
            e.Moves = Moves;
            e.sheet = sheet;
        }
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
