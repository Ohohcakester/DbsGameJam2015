using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    [SerializeField]
    private Sprite l;
    [SerializeField]
    private Sprite t;
    [SerializeField]
    private Sprite r;
    [SerializeField]
    private Sprite b;

    public enum PLATFORM_TYPE
    {
        L,T,R,B
    }

    public void Initialise(PLATFORM_TYPE type)
    {
        switch(type)
        {
            case PLATFORM_TYPE.L:
                this.GetComponent<SpriteRenderer>().sprite = l;
                break;
            case PLATFORM_TYPE.T:
                this.GetComponent<SpriteRenderer>().sprite = t;
                break;
            case PLATFORM_TYPE.R:
                this.GetComponent<SpriteRenderer>().sprite = r;
                break;
            case PLATFORM_TYPE.B:
                this.GetComponent<SpriteRenderer>().sprite = b;
                break;
        }
    }
}