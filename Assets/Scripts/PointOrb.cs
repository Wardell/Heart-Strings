using UnityEngine;
using System.Collections;

public class PointOrb : MonoBehaviour {

    void OnTriggerEnter(Collider obj)
    {
        Player p;
        if (p = obj.GetComponent<Player>())
        {
            p.AddScore(100);
            Destroy(this.gameObject);
        }
    }
}
