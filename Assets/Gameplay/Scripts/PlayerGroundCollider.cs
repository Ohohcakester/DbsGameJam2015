using UnityEngine;
using System.Collections;

public class PlayerGroundCollider : MonoBehaviour
{
    [SerializeField]
    private int PlatformLayer;
    public bool OnGround { get; private  set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer != PlatformLayer) return;
        OnGround = true;
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer != PlatformLayer) return;
        OnGround = false;
    }
}
