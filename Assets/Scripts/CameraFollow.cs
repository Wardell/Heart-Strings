using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraFollow : MonoBehaviour {

	// Use this for initialization
    public GameObject target;
    public float scrollSpeed = .5f;
    public List<float> tempo;
    float timeElaps = 0;
    Sweeper HorzSweeper;
    Sweeper VerticalSweeper;
    void Awake()
    {
        HorzSweeper = GameObject.Find("Horizontal Sweeper").GetComponent<Sweeper>();
        VerticalSweeper = GameObject.Find("Vertical Sweeper").GetComponent<Sweeper>();
        HorzSweeper.scrollspeed = scrollSpeed;

        GenerateSpeed(tempo[0]);
    }
	void Start () {
        Vector3 pos = target.transform.position;
        pos -= new Vector3(-1, -.5f, 30);
        this.transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {

        int temp;
        timeElaps += Time.deltaTime;
        if ((temp =Mathf.FloorToInt(timeElaps) % 60) == 0)
        {
            GenerateSpeed(tempo[temp]);
        }
        this.transform.position += new Vector3(scrollSpeed,0,0);
        //this.transform.position = new Vector3(this.transform.position.x, target.transform.position.y, this.transform.position.z);
        //target.transform.position += new Vector3(this.transform.position.x, 0, 0);
	}

    void GenerateSpeed(float tempo)
    {
        if (tempo < 70)
        {
            scrollSpeed = .01f;
        }
        else if (tempo >= 70 && tempo < 90)
        {
            scrollSpeed = .05f;
        }

        else if(tempo >= 90 && tempo < 120)
        {
            scrollSpeed = .15f;
        }
        else if(tempo >=120 && tempo < 150)
        {
            scrollSpeed = .2f;
        }
        else
        {
            scrollSpeed = .3f;
        }

       

    }
}
