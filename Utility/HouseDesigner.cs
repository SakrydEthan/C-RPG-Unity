using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.PlayerSettings;

public class HouseDesigner : MonoBehaviour
{
    public GameObject roof;
    public GameObject roofEnd;
    public GameObject roofOverhang;
    public GameObject roofOverhangEnd;

    public GameObject wall;
    public GameObject foundation;
    public GameObject floor;
    public GameObject storeyParent;

    public int width = 4;
    public int depth = 6;
    public int floors = 3;
    public int[,] grid;

    public float scale = 3f;

    List<GameObject> storeys = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void GenerateGrid()
    {
        grid = new int[width+2,depth+2];

        //initialize interior grid with 1s
        for(int x=1; x<=width; x++)
        {
            for(int y=1; y<=depth; y++)
            {
                grid[x,y] = 1;
            }
        }

        //initialize exterior grid with 0s
        for(int y=0; y<depth+2; y++)
        {
            grid[0, y] = 0;
            grid[width+1, y] = 0;
        }
        for (int x = 0; x < width + 2; x++)
        {
            grid[x, 0] = 0;
            grid[x, depth+1] = 0;
        }

        //output grid for debugging purposes
        StringBuilder gridline = new StringBuilder("grid: ");
        for (int x = 0; x < width+2; x++)
        {
            for (int y = 0; y < depth+2; y++)
            {
                gridline.Append(grid[x, y].ToString());
            }
            gridline.Append('\n');
        }
        Debug.Log(gridline.ToString());
    }

    public void CreateHouse()
    {
        //floor: generate on 1
        //roof:
        if(grid == null)
        {
            Debug.Log("Tried to create house with blank grid!");
            return;
        }

        storeys = new List<GameObject>();
        for (int i = 0; i < floors; i++)
        {
            GameObject go = Instantiate(storeyParent, transform);
            go.name = "Floor "+(i+1).ToString();
            go.transform.localPosition = Vector3.up * i * scale;
            storeys.Add(go);
        }
        GameObject foundationParent = Instantiate(storeyParent, transform);
        foundationParent.name = "Foundation";

        //create foundation, walls, and floors
        for (int x=1; x<=width; x++)
        {
            for(int y=1; y<=depth; y++)
            {
                //check neighboring tiles to see if they're open
                int n1 = grid[x, y+1] == 0 ? 1 : 0;
                int n2 = grid[x+1, y] == 0 ? 1 : 0;
                int n3 = grid[x, y-1] == 0 ? 1 : 0;
                int n4 = grid[x-1, y] == 0 ? 1 : 0;

                //Debug.Log($"{x},{y}: n1={n1} n2={n2} n3={n3} n4={n4}");

                Vector3 pos = new Vector3(x*scale, 0f, y*scale);

                //spawn foundations
                PerimeterSpawnLogic(foundation, pos, n1, n2, n3, n4, foundationParent);
                for (int i = 0; i < floors; i++)
                {
                    //pos.y = i * scale;
                    //GameObject storeyPar = Instantiate(storeyParent, transform);
                    //storeyPar.transform.localPosition = pos + (Vector3.up*i*scale);

                    GameObject storeyPar = storeys[i];

                    PerimeterSpawnLogic(wall, pos + Vector3.up, n1, n2, n3, n4, storeyPar);
                    SpawnPrefab(floor, pos, 0f, storeyPar);
                }
            }
        }
    }

    public void CreateRoof()
    {
        GameObject roofParent = Instantiate(storeyParent, transform);
        roofParent.name = "Roof";

        for(int y=1; y<=depth; y++)
        {
            //check if roof has uneven width
            bool isEven = width % 2 == 0;

            int leftSpawned = 0;
            int rightSpawned = 0;

            int wid = (width+2)/2;
            Debug.Log($"wid:{wid}, depth:{depth}, width:{width}");

            bool hasStartedRoof = false;

            for (int l = 0; l < wid; l++)
            {
                if (grid[l, y] == 1)
                {
                    //create roof over this tile, and move it up to align with the amount of roof tiles already placed
                    Vector3 pos = new Vector3(l * scale, 0f, y * scale);
                    pos.y += (floors + leftSpawned) * scale + 1f;
                    SpawnPrefab(roof, pos, 270f, roofParent);
                    leftSpawned++;

                    if (!hasStartedRoof)
                    {
                        SpawnPrefab(roofEnd, pos, 270f, roofParent);
                        hasStartedRoof = true;
                    }
                }
            }
            hasStartedRoof = false;
            for (int r = 0; r < wid; r++)
            {
                if (grid[width-(r+1), y] == 1)
                {
                    //create roof over this tile, and move it up to align with the amount of roof tiles already placed
                    Vector3 pos = new Vector3((width - r) * scale, 0f, y * scale);
                    pos.y += (floors + rightSpawned) * scale + 1f;
                    SpawnPrefab(roof, pos, 90f, roofParent);
                    rightSpawned++;

                    if (!hasStartedRoof)
                    {
                        SpawnPrefab(roofEnd, pos, 90f, roofParent);
                        hasStartedRoof = true;
                    }
                }
            }


            /*
            bool roofComplete = false;
            int leftRoofInd = 0;
            int rightRoofInd = 0;
            int leftSpawned = 0;
            int rightSpawned = 0;

            int loopLimit = 1000;
            int loops = 0;



            while(!roofComplete && loops < loopLimit)
            {
                if (grid[leftRoofInd, y] == 1)
                {
                    //create roof over this tile, and move it up to align with the amount of roof tiles already placed
                    Vector3 pos = new Vector3(leftRoofInd * scale, 0f, y * scale);
                    pos.y += (floors + leftSpawned) * scale + 1f;
                    SpawnPrefab(roof, pos, 270f);
                    leftSpawned++;
                }
                if (grid[width-(rightRoofInd), y] == 1)
                {
                    Vector3 pos = new Vector3((width - rightRoofInd + 1) * scale, 0f, y * scale);
                    pos.y += (floors + rightSpawned) * scale + 1f;
                    SpawnPrefab(roof, pos, 90f);
                    rightSpawned++;
                }


                leftRoofInd++;
                rightRoofInd++;
                loops++;
                if (leftRoofInd > (width+1) / 2) roofComplete = true;
            }
            */
        }
    }

    public void PerimeterSpawnLogic(GameObject prefab, Vector3 pos, int n1, int n2, int n3, int n4, GameObject storey)
    {
        if (n1 == 1)
        {
            SpawnPrefab(prefab, pos, 0f, storey);
        }
        if (n2 == 1)
        {
            SpawnPrefab(prefab, pos, 90f, storey);
        }
        if (n3 == 1)
        {
            SpawnPrefab(prefab, pos, 180f, storey);
        }
        if (n4 == 1)
        {
            SpawnPrefab(prefab, pos, 270f, storey);
        }
    }
    public void PerimeterSpawnLogic(GameObject prefab, Vector3 pos, int n1, int n2, int n3, int n4)
    {
        if (n1 == 1)
        {
            SpawnPrefab(prefab, pos, 0f);
        }
        if (n2 == 1)
        {
            SpawnPrefab(prefab, pos, 90f);
        }
        if (n3 == 1)
        {
            SpawnPrefab(prefab, pos, 180f);
        }
        if (n4 == 1)
        {
            SpawnPrefab(prefab, pos, 270f);
        }
    }

    public void SpawnPrefab(GameObject prefab, Vector3 pos, float rot)
    {
        GameObject spawn = Instantiate(prefab, transform);
        spawn.transform.localPosition = pos;
        spawn.transform.Rotate(Vector3.up * rot);
    }
    public void SpawnPrefab(GameObject prefab, Vector3 pos, float rot, GameObject storey)
    {
        GameObject spawn = Instantiate(prefab, storey.transform);
        spawn.transform.localPosition = pos;
        spawn.transform.Rotate(Vector3.up * rot);
    }

    public void DeleteGeneratedHouse()
    {
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            objects.Add(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < objects.Count; i++)
        {
            DestroyImmediate(objects[i]);
        }
    }
}
