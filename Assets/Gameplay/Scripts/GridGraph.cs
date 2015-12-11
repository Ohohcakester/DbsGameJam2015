using UnityEngine;
using System.Collections.Generic;

public class GridGraph
{
    private bool[,] tileGrid;
    private bool[,] pfGrid;
    public int scale { get; private set; }
    public int sizeX { get; private set; }
    public int sizeY { get; private set; }
    public int pfSizeX { get; private set; }
    public int pfSizeY { get; private set; }
    public float realMinX;
    public float realMinY;
    public float width;
    public float height;

    private VisibilityGraph visibilityGraph;

    public void Initialise(bool[,] tiles, float realMinX, float realMinY, float width, float height)
    {
        this.realMinX = realMinX;
        this.realMinY = realMinY;
        this.width = width;
        this.height = height;
        this.sizeX = tiles.GetLength(0);
        this.sizeY = tiles.GetLength(1);
        this.pfSizeX = sizeX * scale;
        this.pfSizeY = sizeY * scale;
        tileGrid = tiles;
        pfGrid = new bool[pfSizeX,pfSizeY];
        ConfigurePfGrid();
        visibilityGraph = new VisibilityGraph(this);
    }

    private void ConfigurePfGrid()
    {
        for (int y = 0; y < pfSizeY; ++y)
        {
            for (int x = 0; x < pfSizeX; ++x)
            {
                pfGrid[x, y] = checkIsBlockedPfTile(x, y) ||
                               checkIsBlockedPfTile(x - 1, y) ||
                               checkIsBlockedPfTile(x, y - 1) ||
                               checkIsBlockedPfTile(x + 1, y) ||
                               checkIsBlockedPfTile(x, y + 1);
            }
        }
    }

    private bool checkIsBlockedPfTile(int pfX, int pfY)
    {
        if (pfX < 0) return true;
        if (pfY < 0) return true;
        if (pfX >= pfSizeX) return true;
        if (pfY >= pfSizeY) return true;
        return tileGrid[pfX/scale, pfY/scale];
    }

    public bool IsBlockedPfTile(int pfx, int pfy)
    {
        if (pfx < 0) return true;
        if (pfy < 0) return true;
        if (pfx >= pfSizeX) return true;
        if (pfy >= pfSizeY) return true;
        return pfGrid[pfx, pfy];
    }

    public bool IsBlockedCoordinate(int pfx, int pfy)
    {
        return IsBlockedPfTile(pfx, pfy) &&
               IsBlockedPfTile(pfx-1, pfy) &&
               IsBlockedPfTile(pfx, pfy-1) &&
               IsBlockedPfTile(pfx-1, pfy-1);
    }

    /// <summary>
    /// Input: Unity Coordinates. Output: Pf Grid Coordinates.
    /// </summary>
    private void ToPfGridCoordinates(float x, float y, out int gx, out int gy)
    {
        x = (x - realMinX) / width * pfSizeX;
        y = (y - realMinY) / height * pfSizeY;

        int cx = (int)(x + 0.5f);
        int cy = (int)(y + 0.5f);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (j==1 && i == 1)
                {

                }
                else {
                    int px = cx - (3 / 2) + i;
                    int py = cy - (3 / 2) + j;
                    if (IsBlockedCoordinate(px,py))
                    {
                        gx = px;
                        gy = py;
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j+=4)
            {
                int px = cx - (5 / 2) + i;
                int py = cy - (5 / 2) + j;
                if (IsBlockedCoordinate(px,py))
                {
                    gx = px;
                    gy = py;
                    return;
                }
            }
        }

        for (int i = 0; i < 5; i+=4)
        {
            for (int j = 0; j < 3; j ++)
            {
                int px = cx - (5 / 2) + i;
                int py = cy - (3 / 2) + j;
                if (IsBlockedCoordinate(px,py))
                {
                    gx = px;
                    gy = py;
                    return;
                }
            }
        }

        gx = 0;
        gy = 0;
        return;
    }

    /// <summary>
    /// Input: Pf Grid Coordinates. Output: Unity Coordinates.
    /// </summary>
    private void ToRealCoordinates(int gx, int gy, out float x, out float y)
    {
        x = width*gx/pfSizeX + realMinX;
        y = height*gy/pfSizeY + realMinY;
    }

    public Vector2 GetMoveDirection(float currX, float currY, float targetX, float targetY)
    {
        int sx, sy, ex, ey;
        ToPfGridCoordinates(currX, currY, out sx, out sy);
        ToPfGridCoordinates(targetX, targetY, out ex, out ey);

        var nextPoint = visibilityGraph.Search(sx, sy, ex, ey);
        float nextX, nextY;
        ToRealCoordinates(nextPoint.x, nextPoint.y, out nextX, out nextY);
        return new Vector2(nextX - currX, nextY - currY);
    }
}