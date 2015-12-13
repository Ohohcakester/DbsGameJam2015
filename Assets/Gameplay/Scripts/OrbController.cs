using UnityEngine;
using System;

public class OrbController : MonoBehaviour
{
    private Transform blackCircle;
    private Transform whiteCircle;
    private Vector3 blackScale;
    private Vector3 whiteScale;
//	SpriteRenderer orbRenderer;

    private float baseWhiteScale = 0.005f;
    private float whiteScaleIncrement = 0.0005f;
    private float maxBlackScale = 0.02f;
    private float baseBlackScale = 0.004f;
    private float maxWhiteScale = 0.02f;
    private float blackScaleIncrement = 0.0005f;

    void Start()
    {
        whiteCircle = transform.FindChild("whiteCircle");
        blackCircle = transform.FindChild("blackCircle");
	//	orbRenderer = transform.FindChild ("whiteCircle").GetComponent<SpriteRenderer> ();

        blackScale = blackCircle.transform.localScale;
        whiteScale = whiteCircle.transform.localScale;
        //SetSize(2);
    }

    void LateUpdate()
    {
        //blackCircle.position = new Vector3(3, 4, 5);
        blackCircle.transform.localScale = blackScale;
        whiteCircle.transform.localScale = whiteScale;
    }

	/*public void setOriginalSize(){
		blackScale = new Vector3 (0.005f, 0.005f, 1);
		whiteScale = new Vector3 (0.004f, 0.004f, 1);
	}

	public void growAbit(){
		float currBlack = blackScale.x + 0.0005f;
		float currWhite = whiteScale.x + 0.0005f;

		currBlack = Mathf.Clamp (currBlack, 0, maxBlackSc);
		currWhite = Mathf.Clamp (currWhite, 0, 0.020f);

		blackScale = new Vector3 (currBlack, currBlack, 1);
		whiteScale = new Vector3 (currWhite, currWhite, 1);
	}*/

   
    /// <summary>
    /// Default: 0
    /// </summary>
    public void setSize(float size)
    {
        float currBlack = baseBlackScale + size * blackScaleIncrement;
        float currWhite = baseWhiteScale + size * whiteScaleIncrement;

        currBlack = Mathf.Clamp(currBlack, 0, maxBlackScale);
        currWhite = Mathf.Clamp(currWhite, 0, maxWhiteScale);

        blackScale = new Vector3(currBlack, currBlack, 1);
        whiteScale = new Vector3(currWhite, currWhite, 1);
    }

    public void setOriginalSize()
    {
        setSize(0);
    }

    public void Gone()
    {
        setSize(-9.99f);
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
