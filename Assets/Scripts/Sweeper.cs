using UnityEngine;
using System.Collections;

public class Sweeper : MonoBehaviour {
    public float scrollspeed;
	// Use this for initialization
	void Start () {

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(scrollspeed, 0,0);
	}

    void OnTriggerEnter(Collider col)
    {
        Player p;
        Debug.Log(col.name);
        if (p=col.GetComponentInParent<Player>())
        {
           Camera c = GameObject.FindObjectOfType<Camera>();
           p.transform.position = new Vector3(c.transform.position.x, 0, 0);
            //Debug.Break();
        }
        else
        {
            Debug.Log("Destroyed Object" + col.name);
            if (col.gameObject.transform.parent != null)
                Destroy(col.gameObject.transform.parent.gameObject);

            else
            {
                Destroy(col.gameObject);
            }
        }
       
    }
}
