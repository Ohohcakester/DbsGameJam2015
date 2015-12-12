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
    private GameObject prefab_player;

    public Player player;
    private Vector2 lastPlayerPosition;

    [SerializeField]
    private TextAsset mazeTextFile;

    private float width;
    private float height;

    private Platform[] cornerPlatforms = new Platform[4];

    public GridGraph gridGraph { get; private set; }
    private Platform[,] platforms;

	// Use this for initialization
	void Start ()
	{
	    Initialise();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    /*private void OnDrawGizmos()
    {
        if (gridGraph == null) return;
        if (gridGraph.visibilityGraph == null) return;
        var path = gridGraph.visibilityGraph.getLastPath();
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; ++i)
            {
                var s = path[i];
                var t = path[i + 1];
                float x1, x2, y1, y2;
                gridGraph.ToRealCoordinates(s.x, s.y, out x1, out y1);
                gridGraph.ToRealCoordinates(t.x, t.y, out x2, out y2);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0));
            }
        }
        Gizmos.color = Color.black;
        for (int y = 0; y < gridGraph.pfSizeY; ++y)
        {
            for (int x = 0; x < gridGraph.pfSizeX; ++x)
            {
                if (gridGraph.IsBlockedPfTile(x, y))
                {
                    float x1, x2, y1, y2;
                    gridGraph.ToRealCoordinates(x, y, out x1, out y1);
                    gridGraph.ToRealCoordinates(x + 1, y + 1, out x2, out y2);
                    Gizmos.DrawLine(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0));
                    Gizmos.DrawLine(new Vector3(x1, y2, 0), new Vector3(x2, y1, 0));

                }
            }
        }
    }*/

    private void Initialise()
    {
        if (gridGraph != null) return;
        InitialiseGridGraph(mazeTextFile.text);
        InstantiatePlayer(15,15);
    }

    private void InstantiatePlayer(float x, float y)
    {
        float actualX = minX + tileWidth * (x + 0.5f);
        float actualY = minY + tileHeight * (y + 0.5f);

        var playerObject = Instantiate(prefab_player, new Vector3(actualX, actualY, 0), prefab_player.transform.rotation) as GameObject;
        player = playerObject.GetComponent<Player>();
        Camera.main.GetComponent<CameraFollow>().setPlayer(playerObject);
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

        CreateCornerPlatforms(nRows, nCols);
        CreateSeaBed(nCols);
    }

    public Vector2 PlayerPosition()
    {
        // add iff clause
        lastPlayerPosition = player.transform.position;
        return lastPlayerPosition;
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

    void CreateCornerPlatforms(int nRows, int nCols)
    {
        cornerPlatforms[0] = CreatePlatform(-1, -1, Platform.PLATFORM_TYPE.G); // bottom left
        cornerPlatforms[1] = CreatePlatform(-1, nRows, Platform.PLATFORM_TYPE.B); // top left
        cornerPlatforms[2] = CreatePlatform(nCols, -1, Platform.PLATFORM_TYPE.G); // bottom right
        cornerPlatforms[3] = CreatePlatform(nCols, nRows, Platform.PLATFORM_TYPE.B); // top right

        cornerPlatforms[0].GetComponent<BoxCollider2D>().size = new Vector2((platforms.GetLength(0) * tileWidth + (2*tileWidth)), tileHeight);
        cornerPlatforms[0].GetComponent<BoxCollider2D>().offset = new Vector2(((platforms.GetLength(0) * tileWidth + (2 * tileWidth))) / 2.0f - (tileWidth / 2), 0);

        cornerPlatforms[3].GetComponent<BoxCollider2D>().size = new Vector2((platforms.GetLength(0) * tileWidth + (2 * tileWidth)), tileHeight);
        cornerPlatforms[3].GetComponent<BoxCollider2D>().offset = new Vector2(-((platforms.GetLength(0) * tileWidth + (0 * tileWidth))) / 2.0f - (tileWidth / 2), 0);

        cornerPlatforms[1].GetComponent<BoxCollider2D>().size = new Vector2(tileWidth, (platforms.GetLength(1) * tileHeight + (2 * tileHeight)));
        cornerPlatforms[1].GetComponent<BoxCollider2D>().offset = new Vector2(0, -((platforms.GetLength(1) * tileWidth + (0 * tileWidth))) / 2.0f - (tileWidth / 2));

        cornerPlatforms[2].GetComponent<BoxCollider2D>().size = new Vector2(tileWidth, (platforms.GetLength(1) * tileHeight + (2 * tileHeight)));
        cornerPlatforms[2].GetComponent<BoxCollider2D>().offset = new Vector2(0, ((platforms.GetLength(1) * tileWidth + (2 * tileWidth))) / 2.0f - (tileWidth / 2));
    }


    void CreateSeaBed(int nCols)
    {
        for (int i = -20; i < nCols+3; i=i+7)
        {
            CreatePlatform(i, -1, Platform.PLATFORM_TYPE.G).GetComponent<Collider2D>().enabled = false;
        }
    }
}