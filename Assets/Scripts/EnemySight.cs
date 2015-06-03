using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour 
{
    public float feildOfViewAngle = 110f;
    public bool playerInSight;

    private SphereCollider col;
    private Animator anim;
    private GameObject player;
    private NavMeshAgent nav;


    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
       // anim = GetComponent<SphereCollider>();
        //player = GameObject.FindGameObjectWithTag();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {

        }
    }
}
