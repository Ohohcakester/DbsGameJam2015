using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    [SerializeField]
    private Sprite l;
    [SerializeField]
    private Sprite t0;
    [SerializeField]
    private Sprite t1;
    [SerializeField]
    private Sprite r;
    [SerializeField]
    private Sprite b;
    [SerializeField]
    private Sprite g;

    public enum PLATFORM_TYPE
    {
        L,T,R,B,G
    }

    public void Initialise(PLATFORM_TYPE type)
    {
        Random.seed = 42;
        switch(type)
        {
            case PLATFORM_TYPE.L:
                this.GetComponent<SpriteRenderer>().sprite = l;
                break;
            case PLATFORM_TYPE.T:
                float ran = Random.value;
                if (ran <0.5)
                {
                    this.GetComponent<SpriteRenderer>().sprite = t0;
                } else
                {
                    this.GetComponent<SpriteRenderer>().sprite = t1;
                }               
                break;
            case PLATFORM_TYPE.R:
                this.GetComponent<SpriteRenderer>().sprite = r;
                break;
            case PLATFORM_TYPE.B:
                this.GetComponent<SpriteRenderer>().sprite = b;
                break;
            case PLATFORM_TYPE.G:
                this.GetComponent<SpriteRenderer>().sprite = g;
                break;
            default:
                this.GetComponent<SpriteRenderer>().sprite = g;
                break;
        }
    }
}