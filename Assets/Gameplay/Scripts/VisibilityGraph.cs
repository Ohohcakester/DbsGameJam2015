﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VisibilityGraph
{
    private GridGraph graph;
    private List<Point> vertices;
    private List<List<Edge>> edgeList;

    private int size;

    private bool[] visited;
    private float[] distances;
    private int[] parent;

    private int startIndex; // second last index
    private int endIndex; // last index


    public VisibilityGraph(GridGraph graph)
    {
        vertices = new List<Point>();
        edgeList = new List<List<Edge>>();

        this.graph = graph;
        for (int y = 0; y <= graph.pfSizeY; ++y)
        {
            for (int x = 0; x <= graph.pfSizeX; ++x)
            {
                if (IsCorner(x, y))
                {
                    vertices.Add(new Point {x = x, y = y});
                    edgeList.Add(new List<Edge>());
                }
            }
        }
        for (int j = 0; j < vertices.Count; ++j)
        {
            for (int i = j + 1; i<vertices.Count; ++i)
            {
                if (LineOfSight(vertices[i].x, vertices[i].y, vertices[j].x, vertices[j].y))
                {
                    AddEdge(i, j);
                }
            }
        }

        // start and end points
        startIndex = vertices.Count;
        vertices.Add(Point.Null());
        edgeList.Add(new List<Edge>());
        endIndex = vertices.Count;
        vertices.Add(Point.Null());
        edgeList.Add(new List<Edge>());

        size = vertices.Count;
        distances = new float[size];
        visited = new bool[size];
        parent = new int[size];
    }

    private float Distance(int index1, int index2)
    {
        var p1 = vertices[index1];
        var p2 = vertices[index2];
        return Distance(p1.x, p1.y, p2.x, p2.y);
    }

    private float Distance(int x1, int y1, int x2, int y2)
    {
        int dx = x2 - x1;
        int dy = y2 - y1;
        return (float) Math.Sqrt(dx*dx + dy*dy);
    }
    
    public PathData Search(int sx, int sy, int ex, int ey)
    {
        RemoveInstancesOf(startIndex);
        RemoveInstancesOf(endIndex);
        InitialiseStartAndEnd(sx, sy, ex, ey);

        for (int i = 0; i < size; ++i)
        {
            distances[i] = float.PositiveInfinity;
            parent[i] = -1;
            visited[i] = false;
        }
        distances[startIndex] = 0;
        
        // Start algorithm
        var pq = new IndirectHeap(size);
        pq.decreaseKey(startIndex, 0);
        
        for (int j = 0; j < size; ++j)
        {
            int current = pq.popMinIndex();

            if (distances[current] == float.PositiveInfinity || current == endIndex)
            {
                /*Debug.Log("Explored: " + (j+1) + " out of " + size);
                Debug.Log("Success = " + distances[current]);*/
                break;
            }

            visited[current] = true;
            foreach (var edge in edgeList[current])
            {
                if (Relax(edge))
                {
                    int v = edge.v2;
                    bool noProblem = pq.decreaseKey(v, distances[v] + Distance(v, endIndex));

                    if (!noProblem)
                    {
                        // Unknown problem. Just terminate the pathfinding.
                        j = size;
                        break;
                    }
                }
            }
        }

        return GetNextDestination();
    }

    private PathData GetNextDestination()
    {
        //Debug.Log(parent[endIndex] == -1);
        if (parent[endIndex] == -1)
        {
            return PathData.Null();
        }

        float distance = 0;
        int nHops = 0;
        int current = endIndex;
        while (parent[current] != startIndex)
        {
            distance += Distance(current, parent[current]);
            nHops++;
            current = parent[current];
        }

        Point nextPoint = vertices[current];
        distance += Distance(current, parent[current]);
        nHops++;

        float realNextX, realNextY;
        graph.ToRealCoordinates(nextPoint.x, nextPoint.y, out realNextX, out realNextY);

        return new PathData
        {
            distance = distance,
            isDirect = (nHops <= 1),
            x = nextPoint.x,
            y = nextPoint.y,
            realNextX = realNextX,
            realNextY = realNextY
        };
    }

    private bool Relax(Edge edge)
    {
        int u = edge.v1;
        int v = edge.v2;

        float newDistance = distances[u] + Distance(u, v);
        if (newDistance < distances[v])
        {
            parent[v] = u;
            distances[v] = newDistance;
            return true;
        }
        return false;
    }



    public void InitialiseStartAndEnd(int sx, int sy, int ex, int ey)
    {
        vertices[startIndex] = new Point { x = sx, y = sy };
        vertices[endIndex] = new Point { x = ex, y = ey };
        
        edgeList[startIndex] = new List<Edge>();
        for (int i = 0; i < vertices.Count-2; ++i)
        {
            var start = vertices[startIndex];
            var current = vertices[i];
            if (LineOfSight(start.x, start.y, current.x,current.y))
            {
                AddEdge(startIndex, i);
            }
        }
        
        edgeList[endIndex] = new List<Edge>();
        for (int i = 0; i < vertices.Count-1; ++i)
        {
            var end = vertices[endIndex];
            var current = vertices[i];
            if (LineOfSight(end.x, end.y, current.x,current.y))
            {
                AddEdge(endIndex, i);
            }
        }
    }

    private void AddEdge(int v1, int v2)
    {
        edgeList[v1].Add(new Edge { v1 = v1, v2 = v2 });
        edgeList[v2].Add(new Edge { v1 = v2, v2 = v1 });
    }

    protected void RemoveInstancesOf(int index)
    {
        Predicate<Edge> hasIndex = (edge) => edge.v1 == index || edge.v2 == index;
        foreach (var edges in edgeList)
        {
            edges.RemoveAll(hasIndex);
        }
    }

    private bool IsCorner(int x, int y)
    {
        bool a = IsBlocked(x - 1, y - 1);
        bool b = IsBlocked(x, y - 1);
        bool c = IsBlocked(x, y);
        bool d = IsBlocked(x - 1, y);

        return ((!a && !c) || (!d && !b)) && (a || b || c || d);
    }

    private bool IsBlocked(int x, int y)
    {
        return graph.IsBlockedPfTile(x, y);
    }

    /**
     * @return true iff there is line-of-sight from (x1,y1) to (x2,y2).
     */
    public bool LineOfSight(int x1, int y1, int x2, int y2)
    {
        int dy = y2 - y1;
        int dx = x2 - x1;

        int f = 0;

        int signY = 1;
        int signX = 1;
        int offsetX = 0;
        int offsetY = 0;

        if (dy < 0)
        {
            dy *= -1;
            signY = -1;
            offsetY = -1;
        }
        if (dx < 0)
        {
            dx *= -1;
            signX = -1;
            offsetX = -1;
        }

        if (dx >= dy)
        {
            while (x1 != x2)
            {
                f += dy;
                if (f >= dx)
                {
                    if (IsBlocked(x1 + offsetX, y1 + offsetY))
                        return false;
                    y1 += signY;
                    f -= dx;
                }
                if (f != 0 && IsBlocked(x1 + offsetX, y1 + offsetY))
                    return false;
                if (dy == 0 && IsBlocked(x1 + offsetX, y1) && IsBlocked(x1 + offsetX, y1 - 1))
                    return false;

                x1 += signX;
            }
        }
        else
        {
            while (y1 != y2)
            {
                f += dx;
                if (f >= dy)
                {
                    if (IsBlocked(x1 + offsetX, y1 + offsetY))
                        return false;
                    x1 += signX;
                    f -= dy;
                }
                if (f != 0 && IsBlocked(x1 + offsetX, y1 + offsetY))
                    return false;
                if (dx == 0 && IsBlocked(x1, y1 + offsetY) && IsBlocked(x1 - 1, y1 + offsetY))
                    return false;

                y1 += signY;
            }
        }
        return true;
    }

    public List<Point> getLastPath()
    {
        if (vertices[endIndex].IsNull()) return null;
        if (parent[endIndex] == -1) return null;
        var list = new List<Point>();
        int current = endIndex;
        while (current != -1)
        {
            list.Add(vertices[current]);
            current = parent[current];
        }
        return list;
    }
}


public struct Point
{
    public int x;
    public int y;

    public static Point Null()
    {
        return new Point{x=-1,y=-1};
    }

    public bool IsNull()
    {
        return x == -1 && y == -1;
    }
}

internal struct Edge
{
    public int v1;
    public int v2;
}

public struct PathData
{
    public int x;
    public int y;
    public float realNextX;
    public float realNextY;
    public float distance;
    public bool isDirect;

    public static PathData Null()
    {
        return new PathData { x = -1, y = -1 };
    }

    public bool IsNull()
    {
        return x == -1 && y == -1;
    }
}