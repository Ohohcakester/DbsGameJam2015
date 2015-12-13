using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    AudioClip goodEvent;
    [SerializeField]
    AudioClip badEvent;
    [SerializeField]
    AudioClip cancelledEvent;
    [SerializeField]
    AudioClip hoho;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoodEventSound()
    {
        AudioSource.PlayClipAtPoint(goodEvent, this.transform.position);   
    }

    public void BadEventSound ()
    {
        AudioSource.PlayClipAtPoint(badEvent, this.transform.position);
    }

    public void EventCancelledSound()
    {
        AudioSource.PlayClipAtPoint(cancelledEvent, this.transform.position);
    }
}
