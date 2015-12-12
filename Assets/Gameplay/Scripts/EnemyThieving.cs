using UnityEngine;
using System.Collections;

public class EnemyThieving : MonoBehaviour {
	public float stunTime = 2.0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        Player plyr = col.gameObject.GetComponent<Player>();
        if (plyr != null)
        {
            if (plyr.isStunned) return;
            Debug.Log("Stunning player");
            plyr.stun(2.0f);
            plyr.DestroyOrb();
        }
        else
        {
            //Debug.Log("Collided with non-player");
        }
    }
}
