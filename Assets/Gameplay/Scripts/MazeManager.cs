using System;
using UnityEngine;
using System.Collections;

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

    private float width;
    private float height;

    private GridGraph gridGraph;
    private bool[,] platforms;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void InitialiseGridGraph(String text)
    {
        var lines = text.Split('\n');
        int nCols = lines[0].Split(' ').Length;
        int nRows = lines.Length;
        var isBlocked = new bool[nCols,nRows];
        platforms = new bool[nCols,nRows];

        for (int y = 0; y < nRows; ++y)
        {
            var chars = lines[y].Split(' ');
            for (int x = 0; x < nCols; ++x)
            {
                switch (chars[x].ToLower())
                {
                    case "x":
                        isBlocked[x, y] = false;
                        platforms[x, y] = false;
                        break;
                    case "l":
                        isBlocked[x, y] = true;
                        platforms[x, y] = false;
                        break;
                    case "t":
                        isBlocked[x, y] = true;
                        platforms[x, y] = false;
                        break;
                    case "r":
                        isBlocked[x, y] = true;
                        platforms[x, y] = false;
                        break;
                    case "b":
                        isBlocked[x, y] = true;
                        platforms[x, y] = false;
                        break;
                }
            }
        }

        width = tileWidth*nCols;
        height = tileHeight*nRows;

        gridGraph = new GridGraph();
        gridGraph.Initialise(isBlocked, minX, minY, width, height);
    }
}
