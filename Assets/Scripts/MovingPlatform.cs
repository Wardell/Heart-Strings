using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    Transform platform;
    [SerializeField]
    Transform startTransform;
    [SerializeField]
    Transform endTransform;
    private float TimeElaps = 0;
    public float platformSpeed;
    Vector3 direction;
    Transform desination;
    public BattleSheet sheet;
    public float aveSignal;
    [SerializeField]
    private int beatcounter;
    void Awake()
    {

    }

    void SetDestination(Transform dist)
    {
        desination = dist;
        direction = (desination.position - platform.position).normalized;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(startTransform.position, platform.localScale);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(endTransform.position, platform.localScale);
    }
    // Use this for initialization
    void Start()
    {
        SetDestination(startTransform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(platform.position, desination.position) < platformSpeed * Time.fixedDeltaTime)
        {
            SetDestination(desination == startTransform ? endTransform : startTransform);
        }
        platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction * (platformSpeed * Time.deltaTime));
       
    }

    void FixedUpdate()
    {  
      
    }
}
