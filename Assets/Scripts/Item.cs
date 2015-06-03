using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	// Use this for initialization
    public ItemType type;

    public void Remove()
    {
        Debug.Log("Got Speed");
        Destroy(this.gameObject);
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
