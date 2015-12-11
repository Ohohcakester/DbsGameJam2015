using UnityEngine;
using System.Collections.Generic;
using System.Net.Configuration;

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

    private bool IsBlockedPfTile(int pfx, int pfy)
    {
        if (pfx < 0) return true;
        if (pfy < 0) return true;
        if (pfx >= pfSizeX) return true;
        if (pfy >= pfSizeY) return true;
        return pfGrid[pfx, pfy];
    }

    private bool IsBlockedCoordinate(int pfx, int pfy)
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

        gx = 0;
        gy = 0;
    }

    /// <summary>
    /// Input: Pf Grid Coordinates. Output: Unity Coordinates.
    /// </summary>
    private void ToRealCoordinates(int gx, int gy, out float x, out float y)
    {
        x = width*gx/pfSizeX + realMinX;
        y = height*gy/pfSizeY + realMinY;
    }

    public Vector3 GetMoveDirection(float currX, float currY, float targetX, float targetY)
    {
        return Vector3.zero;
    }
}