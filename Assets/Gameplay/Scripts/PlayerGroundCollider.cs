using UnityEngine;
using System.Collections;

public class PlayerGroundCollider : MonoBehaviour
{
    [SerializeField]
    private string PlatformTag = "Platform";
    public bool OnGround { get; private  set; }
    private int nPlatforms = 0;

    private void UpdateOnGround()
    {
        Debug.Log(nPlatforms);
        OnGround = nPlatforms > 0;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag != PlatformTag) return;
        nPlatforms++;
        UpdateOnGround();
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag != PlatformTag) return;
        nPlatforms--;
        UpdateOnGround();
    }
}
