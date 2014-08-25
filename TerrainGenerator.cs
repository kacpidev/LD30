using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

    public GameObject ground;
    public GameObject potion;
    public GameObject sword;
    public GameObject gun;
    public GameObject hardwall;
    public GameObject turret;
    public GameObject wall;
    public GameObject hotspot;
    public GameObject entrance;
    public GameObject end;
    public GameObject player;
    public GameObject playerArea;
    public GameObject obj;
    public GameObject enemy;
    public int width;
    public int height;
    public float ammount;
    public int steps = 4;
    public int hotSpotsNumber;
    public int objectsNumber;
    public int hotSpotsDistance;
    public List<List<int>> tiles;
    public List<List<int>> areas; 
    private List<List<int>> floodTiles;
    private int zindex = 0;
    private List<Vector2> hotspots;
    private List<Vector2> objects;
    private Vector3 entrancePosition;

  
    // Use this for initialization
	void Start () {

        InitializeTileMap();
        GenerateTileMap();
        FloodfillCheck();
        FindHotspots();
        FindEntranceAndExit(); 
        FindObjects();

        CalculateAreas();
 //       InstantiateArea();

        InstantiateLayer();

    }

    // Update is called once per frame
    void Update()
    {
	
	}

    void CalculateAreas()
    {
        int type = 1;
        foreach (Vector3 h in hotspots)
        {
            areas[(int)h.x][(int)h.y] = type;
            type++;
        }

        while (!FullArea())
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3 v = FindClosestArea(i,j);
                    areas[i][j] = areas[(int)v.x][(int)v.y];
                }
            }
        }

    }

    bool FullArea()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (areas[i][j] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    Vector3 FindClosestArea(int x, int y)
    {
        int dist = 1000000;
        Vector3 vec =Vector3.zero;
        foreach (Vector2 hotspot in hotspots)
        {
            int temp = getDistance(x, y, (int)hotspot.x, (int)hotspot.y);

            if (temp < dist) 
            {
                dist = temp;
                vec = new Vector3((int)hotspot.x, (int)hotspot.y, 0);
            }
        }
        return vec;
    }

    void InstantiateArea()
    {
        string temp = "";

        for (int i = 0; i < width; i++)
        {
            temp = "";
            for (int j = 0; j < height; j++)
            {
                temp += areas[i][j];
            }
            Debug.Log(temp);
        }
    }

    void InstantiateLayer()
    {
        GameObject layer, element;
        layer = new GameObject();
        layer.name = "layer " + zindex;
        Debug.Log("wejscie " + entrancePosition.x + " " +entrancePosition.y);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                element = Instantiate(ground, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                element.transform.parent = layer.transform;
                if (tiles[i][j] == 1)
                {
                    element = Instantiate(wall, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element.transform.parent = layer.transform;
                }
                else if (tiles[i][j] == 2)
                {
                    element = Instantiate(hardwall, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element.transform.parent = layer.transform;
                }
                else if (tiles[i][j] == 10)
                {
                    element = Instantiate(hotspot, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element.transform.parent = layer.transform;
                    //element = Instantiate(enemy, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                }
                else if (tiles[i][j] == 11)
                {
                    element = Instantiate(player, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element = Instantiate(playerArea, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    Debug.Log("Player jest na " + element.transform.position.x + " " + element.transform.position.y);
                    element = Instantiate(gun, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element = Instantiate(entrance, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element.transform.parent = layer.transform;
                }
                else if (tiles[i][j] == 12)
                {
                    element = Instantiate(end, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element.transform.parent = layer.transform;
                }
                else if (tiles[i][j] == 20)
                {
                    int r = Random.Range(0, 100);
                    if (DistanceToEntrance(i, j) > 15)
                    {
                        if (r <= 40) element = Instantiate(enemy, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                        else if (r <= 70) element = Instantiate(turret, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                        else element = Instantiate(potion, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                        element.transform.parent = layer.transform;
                    }
                }
            }
        }

        zindex++;
    }

    void InstantiateLayer2(List<List<int>> t)
    {
        GameObject layer, element;
        layer = new GameObject();
        layer.name = "layer " + zindex;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (t[i][j] == 1)
                {
                    element = Instantiate(wall, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element.transform.parent = layer.transform;
                }
                else if (t[i][j] == 2)
                {
                    element = Instantiate(hardwall, new Vector3(i, j, zindex), new Quaternion()) as GameObject;
                    element.transform.parent = layer.transform;
                }
            }
        }

        zindex++;
    }

    void FloodfillCheck()
    {
        int maxId;

        floodTiles = new List<List<int>>();

        for (int i = 0; i < width; i++)
        {
            floodTiles.Add(new List<int>());
            for (int j = 0; j < height; j++)
            {
                floodTiles[i].Add(0);
                floodTiles[i][j] = tiles[i][j];
            }
        }
        
        int id = 0;
        List<int> idSize = new List<int>();
        idSize.Add(0);
        maxId = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                if (floodTiles[i][j] < 2)
                {
                    idSize[id] += flood(i, j, id);
                    if (idSize[id] > idSize[maxId])
                    {
                        maxId = id;
                    }
                    //Debug.Log("Komnata o id " + id + " ma rozmiar " + idSize[id]);
                    id++;
                    idSize.Add(0);
                }
            }
        }

        Debug.Log("maxid " + maxId);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (floodTiles[i][j] > 2 && floodTiles[i][j] != maxId + 100)
                {
                    tiles[i][j] = 2;
                }
            }
        }

    }

    private int flood(int i, int j, int id)
    {
        int size = 1;
        floodTiles[i][j] = 100 + id;
        if (i > 0 && floodTiles[i - 1][j] < 2)
        {
            size += flood(i - 1, j, id);
        }
        if (j > 0 && floodTiles[i][j-1] < 2)
        {
            size += flood(i, j - 1, id);
        }
        if (i < width-1 && floodTiles[i + 1][j] < 2)
        {
            size += flood(i + 1, j, id);
        }
        if (j<height-1 && floodTiles[i][j+1] < 2)
        {
            size += flood(i, j+1, id);
        }
        return size;
    }

    void GenerateTileMap()
    {
        List<List<int>> tempTiles = new List<List<int>>();

        for (int i = 0; i < width; i++)
        {
            tempTiles.Add(new List<int>());
            for (int j = 0; j < height; j++)
            {
                tempTiles[i].Add(0);
                tempTiles[i][j] = tiles[i][j];
            }
        }

        //cellural automata 
        for (int s = 0; s < steps; s++)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (tiles[i][j] == 2)
                    {
                        if (CountNieghbours(i, j) >= 12)
                        {
                            tempTiles[i][j] = 2;
                        }
                        else
                        {
                            tempTiles[i][j] = 1;
                        }
                    }
                    else if (tiles[i][j] == 1)
                    {
                        if (CountNieghbours(i, j) >= 8)
                        {
                            tempTiles[i][j] = 2;
                        }
                        else if (CountNieghbours(i, j) >= 4)
                        {
                            tempTiles[i][j] = 1;

                        }
                        else if (CountNieghbours(i, j) < 2)
                        {
                            tempTiles[i][j] = 0;
                        }
                    }
                    else if(tiles[i][j] ==0)
                    {
                        if (CountNieghbours(i, j) >= 5)
                        {
                            tempTiles[i][j] = 1;
                        }
                        else
                        {
                            tempTiles[i][j] = 0;
                        }

                    }
                }
            }
            //InstantiateLayer2(tempTiles);
            //InstantiateLayer();
            tiles = tempTiles;
        }
       //floodfill
    }

    public void InitializeTileMap()
    {
        tiles = new List<List<int>>();
        areas = new List<List<int>>();
        for (int i = 0; i < width; i++)
        {
            tiles.Add(new List<int>());
            areas.Add(new List<int>());
            for (int j = 0; j < height; j++)
            {
                areas[i].Add(0);
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    tiles[i].Add(2);
                else
                {
                    int random = Random.Range(0, 100);
                    if (random > ammount*100)
                    {
                        tiles[i].Add(0);
                    }
                    else
                    {
                        tiles[i].Add(1);
                    }
                }
            }
        } 
    }

    private int CountNieghbours(int x, int y)
    {
        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
        {
            return 1000;
        }
        else
            return tiles[x - 1][y - 1] + tiles[x][y - 1] + tiles[x + 1][y - 1] +
                    tiles[x - 1][y] +                       tiles[x + 1][y] +
                    tiles[x - 1][y + 1] + tiles[x][y + 1] + tiles[x + 1][y + 1];


    }

    private int getDistance(int x1, int y1, int x2, int y2)
    {
        //Debug.Log("odległość pomiędzy " + x1 + " " + y1 + " " + x2 + " " + y2 + " wynosi " + (int)Mathf.Sqrt((x1-x2)*(x1-x2) + (y1-y2)*(y1-y2)));
        return (int)Mathf.Sqrt((x1-x2)*(x1-x2) + (y1-y2)*(y1-y2));
    }

    private int DistanceToNearHotspot(int x, int y)
    {
        int dist = 1000000;
        foreach (Vector2 hotspot in hotspots)
        {
            int temp = getDistance(x, y, (int)hotspot.x, (int)hotspot.y);

            if (temp < dist) 
            {
                dist = temp;
            }
        }
        return dist;
    }

    private void FindHotspots(){

        hotspots = new List<Vector2>();
        int maxErrorsCount = width * height / 10;
        int errors = 0;

        for (int i = 0; i < hotSpotsNumber; i++) 
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while ( tiles[x][y] != 0 || DistanceToNearHotspot(x,y) < hotSpotsDistance)
            {
                x = Random.Range(0, width);
                y = Random.Range(0, height);
                errors++;
                if (errors == maxErrorsCount)
                {
                    Debug.Log("zły punkt");
                    return;
                }
            }
            hotspots.Add(new Vector2(x,y));
            tiles[x][y] = 10;
        }

    }

    private void  FindObjects(){
        objects = new List<Vector2>();
        int maxErrorsCount = width * height;
        int errors = 0;

        for (int i = 0; i < objectsNumber; i++) 
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while ( tiles[x][y] != 0 && DistanceToEntrance(x,y)>15.0f)
            {
                x = Random.Range(0, width);
                y = Random.Range(0, height);
                errors++;
                if (errors == maxErrorsCount)
                {
                    Debug.Log("zły punkt");
                    return;
                }
            }
            objects.Add(new Vector2(x,y));
            tiles[x][y] = 20;
        }

    }

    private void FindEntranceAndExit()
    {
        Vector2 entrance = Vector2.zero;
        Vector2 end = Vector2.zero;
        int longestDistance=0;

        foreach (Vector2 hotspot in hotspots)
        {
            foreach (Vector2 myHotspot in hotspots)
            {
                if (hotspot != myHotspot)
                {
                    int distance = getDistance((int)hotspot.x, (int)hotspot.y, (int)myHotspot.x, (int)myHotspot.y);
                    if (distance > longestDistance)
                    {
                        entrance = hotspot;
                        end = myHotspot;
                        longestDistance = distance;
                    }
                }
            }
        }
        entrancePosition = new Vector3(entrance.x, entrance.y);
        tiles[(int)entrance.x][(int)entrance.y] = 11;
        tiles[(int)end.x][(int)end.y] = 12;
    }

    public float DistanceToEntrance(int x, int y)
    {
        return Vector3.Distance(entrancePosition, new Vector3(x,y,0));
    }
}
