using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public Player player;
    public LevelManager level;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

     
        if (player.OutOfHealth())
        {
            player.TakeLife();
            player.RestartHealth();
            player.transform.position = new Vector3(-18, 4, 0);
        }
	}
}
