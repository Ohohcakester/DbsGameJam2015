using UnityEngine;
using System;

public class OrbController : MonoBehaviour
{
    private float size;
    private Transform blackCircle;
    private Transform whiteCircle;
    private Vector3 blackScale;
    private Vector3 whiteScale;

    void Start()
    {
        whiteCircle = transform.FindChild("whiteCircle");
        blackCircle = transform.FindChild("blackCircle");

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
}
