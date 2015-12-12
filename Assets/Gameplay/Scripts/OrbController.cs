using UnityEngine;
using System;

public class OrbController : MonoBehaviour
{
    private float size;
    private Transform blackCircle;
    private Transform whiteCircle;
    private Vector3 blackScale;
    private Vector3 whiteScale;
//	SpriteRenderer orbRenderer;

    void Start()
    {
        whiteCircle = transform.FindChild("whiteCircle");
        blackCircle = transform.FindChild("blackCircle");
	//	orbRenderer = transform.FindChild ("whiteCircle").GetComponent<SpriteRenderer> ();

        blackScale = blackCircle.transform.localScale;
        whiteScale = whiteCircle.transform.localScale;
        //SetSize(2);
    }

    void Update()
    {
        //blackCircle.position = new Vector3(3, 4, 5);
        blackCircle.transform.localScale = blackScale;
        whiteCircle.transform.localScale = whiteScale;
        
    }

    public void SetSize(float size)
    {
        float power = 1.1f;
        this.size = size;
        blackScale.x = size * blackScale.x;
        blackScale.y = size * blackScale.y;
        whiteScale.x = Mathf.Pow(size, power) * whiteScale.x;
        whiteScale.y = Mathf.Pow(size, power) * whiteScale.y;
    }

	public void setOriginalSize(){
		blackScale = new Vector3 (0.005f, 0.005f, 1);
		whiteScale = new Vector3 (0.004f, 0.004f, 1);
	}

	public void growAbit(){
		float currBlack = blackScale.x + 0.0005f;
		float currWhite = whiteScale.x + 0.0005f;

		currBlack = Mathf.Clamp (currBlack, 0, 0.02f);
		currWhite = Mathf.Clamp (currWhite, 0, 0.020f);

		blackScale = new Vector3 (currBlack, currBlack, 1);
		whiteScale = new Vector3 (currWhite, currWhite, 1);
	}

	/*
	public void growMedium(){
		float currBlack = blackScale.x;
		float currWhite = whiteScale.x;

		currBlack = Mathf.Clamp (currBlack * 1.2f, 0, 0.02f);
		currWhite = Mathf.Clamp (currWhite * 1.2f, 0, 0.018f);

		blackScale = new Vector3 (currBlack, currBlack, 1);
		whiteScale = new Vector3 (currWhite, currWhite, 1);
	}

	public void growAlot(){
		float currBlack = blackScale.x;
		float currWhite = whiteScale.x;

		currBlack = Mathf.Clamp (currBlack * 1.3f, 0, 0.02f);
		currWhite = Mathf.Clamp (currWhite * 1.3f, 0, 0.018f);

		blackScale = new Vector3 (currBlack, currBlack, 1);
		whiteScale = new Vector3 (currWhite, currWhite, 1);
	}
	*/
}
