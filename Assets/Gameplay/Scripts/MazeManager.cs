using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class MazeManager : MonoBehaviour
{
    [SerializeField]
    private float minX;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float tileWidth;
    [SerializeField]
    private float tileHeight;

    [SerializeField]
    private GameObject prefab_platform;

    [SerializeField]
    private TextAsset mazeTextFile;

    private float width;
    private float height;

    private GridGraph gridGraph;
    private Platform[,] platforms;

	// Use this for initialization
	void Start ()
	{
	    Initialise();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void Initialise()
    {
        if (gridGraph != null) return;
        InitialiseGridGraph(mazeTextFile.text);
        MergeHitBoxes();
    }

    private Platform CreatePlatform(float x, float y, Platform.PLATFORM_TYPE type)
    {
        float actualX = minX + tileWidth * (x + 0.5f);
        float actualY = minY + tileHeight * (y + 0.5f);
        
        var platformObject = Instantiate(prefab_platform, new Vector3(actualX,actualY,0), prefab_platform.transform.rotation) as GameObject;
        var platform = platformObject.GetComponent<Platform>();
        platform.Initialise(type);
        return platform;
    }

    private void InitialiseGridGraph(String text)
    {
        var delim = new [] {' '};

        var lines = text.Split('\n');
        int nCols = lines[0].Trim().Split(delim, StringSplitOptions.RemoveEmptyEntries).Length;
        int nRows = lines.Length;
        var isBlocked = new bool[nCols,nRows];
        platforms = new Platform[nCols,nRows];

        for (int y = 0; y < nRows; ++y)
        {
            var chars = lines[nRows-y-1].Trim().Split(delim, StringSplitOptions.RemoveEmptyEntries);
            for (int x = 0; x < nCols; ++x)
            {
                switch (chars[x].ToLower())
                {
                    case "x":
                        isBlocked[x, y] = false;
                        platforms[x, y] = null;
                        break;
                    case "l":
                        isBlocked[x, y] = true;
                        platforms[x, y] = CreatePlatform(x, y, Platform.PLATFORM_TYPE.L);
                        break;
                    case "t":
                        isBlocked[x, y] = true;
                        platforms[x, y] = CreatePlatform(x, y, Platform.PLATFORM_TYPE.T);
                        break;
                    case "r":
                        isBlocked[x, y] = true;
                        platforms[x, y] = CreatePlatform(x, y, Platform.PLATFORM_TYPE.R);
                        break;
                    case "b":
                        isBlocked[x, y] = true;
                        platforms[x, y] = CreatePlatform(x, y, Platform.PLATFORM_TYPE.B);
                        break;
                }
            }
        }

        width = tileWidth*nCols;
        height = tileHeight*nRows;

        gridGraph = new GridGraph();
        gridGraph.Initialise(isBlocked, minX, minY, width, height);
    }


    void MergeHitBoxes()
    {
        int xhead = -1;
        int yhead = -1;
        int consecutive = 0;
        for (int y = 0; y < platforms.GetLength(1); ++y)
        {
            for (int x=0; x < platforms.GetLength(0); ++x)
            {
                if (platforms[x, y] != null)
                {
                    if (xhead >= 0 && yhead >= 0)
                    {
                        platforms[x, y].GetComponent<BoxCollider2D>().enabled = false;
                    }
                    else
                    {
                        xhead = x;
                        yhead = y;
                    }
                    consecutive++;
                }
                else
                {
                    if (xhead >= 0 && yhead >= 0)
                    {
                        platforms[xhead, yhead].GetComponent<BoxCollider2D>().size = new Vector2((consecutive*tileWidth), tileHeight);
                        platforms[xhead, yhead].GetComponent<BoxCollider2D>().offset = new Vector2(((consecutive*tileWidth))/2.0f-(tileWidth/2), 0);
                    }                    
                    xhead = -1;
                    yhead = -1;
                    consecutive = 0;
                }
            }
        }
    }
}