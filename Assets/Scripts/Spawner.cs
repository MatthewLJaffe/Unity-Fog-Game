using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] weakEnemys;
    [SerializeField] private GameObject[] strongEnemys;
    [SerializeField] private GameObject[] chests;
    [SerializeField] private int numChests;
    [SerializeField] private int numEnemys;
    [SerializeField] private int rows;
    [SerializeField] private int collumns;
    [SerializeField] private int clusterSize = 3;

    private float xEdgeSpace = 1.5f;
    private float yEdgeSpace = 5.5f;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    // Start is called before the first frame update
    private void Awake() 
    {
        // i hate myself
        if(numChests % 2 == 1) {
            numChests += 1;
        }
        if (numEnemys % 4 != 0) {
            numEnemys += 4 - numEnemys % 4;
        }
        xMin = (int)(ScreenBounds.MIN_X + xEdgeSpace);
        xMax = (int)(ScreenBounds.MAX_X - xEdgeSpace);
        yMin = (int)(ScreenBounds.MIN_Y + yEdgeSpace);
        yMax = (int)(ScreenBounds.MAX_Y - yEdgeSpace);
    }

    void Start()
    {

        for(int c = 0; c < collumns; c++) 
        {
            for(int r = 0; r < rows; r++) 
            {

                float locMinX = xMin + (c * ((xMax - xMin) / collumns));
                float locMaxX = locMinX + (xMax - xMin) / collumns;
                float locMinY = yMin + (r * ((yMax - yMin) / rows));
                float locMaxY = locMinY + (yMax - yMin) / rows;
                int numberOfChestSpawns = numChests / (rows * collumns);
                int numberOfEnemySpawns = numEnemys / (rows * collumns);
                spawn(distributeSpawnCoordinates(locMinX, locMaxX, locMinY, locMaxY, numberOfChestSpawns), chests);
                if (middleRow(r)) {
                    spawn(distributeSpawnCoordinates(locMinX, locMaxX, locMinY, locMaxY, numberOfEnemySpawns), strongEnemys);
                }
                else {
                    spawn(clusterSpawnCoordinates(locMinX, locMaxX, locMinY, locMaxY, numberOfEnemySpawns), weakEnemys);
                }
            }
        }      
    }

    private Vector3[] distributeSpawnCoordinates(float minX, float maxX, float minY, float maxY, int numSpawns) 
    {
        Vector3[] spawnCoordinates = new Vector3[numSpawns];
        for(int i = 0; i < spawnCoordinates.Length; i++) 
        {
            spawnCoordinates[i] = new Vector3(Mathf.Round(Random.Range(minX, maxX)), Mathf.Round(Random.Range(minY, maxY)), 0);
            //put it on far left or right if copy of other coordinates
            if (SpaceCheck.Instance.getOccupied(spawnCoordinates[i]) != null) {
                if (i % 2 == 0)
                    spawnCoordinates[i] = new Vector3(xMin - 1, Mathf.Round(Random.Range(minY, maxY)));
                else
                    spawnCoordinates[i] = new Vector3(xMax + 1, Mathf.Round(Random.Range(minY, maxY)));
            }
        }
        return spawnCoordinates;
    }

    private Vector3[] clusterSpawnCoordinates(float minX, float maxX, float minY, float maxY, int numSpawns) {
        Vector3[] spawnCoordinates = new Vector3[numSpawns];
        int coordinateIndex = spawnCoordinates.Length-1;
        while(coordinateIndex >= 0) 
        {
            if(coordinateIndex >= clusterSize - 1) {
                int xBound = (int)Mathf.Round(Random.Range(minX, maxX - clusterSize));
                int yBound = (int)Mathf.Round(Random.Range(minY, maxY - clusterSize));
                Vector3[] cluster = getCluster(xBound, yBound, clusterSize, new Vector3[clusterSize]);
                for(int i = cluster.Length -1; i >= 0; i--) {
                    spawnCoordinates[coordinateIndex] = cluster[i];
                    coordinateIndex--;
                }
            }
            else {
                Vector3[] remainingCoordinates = distributeSpawnCoordinates(minX, maxX, minY, maxY, coordinateIndex + 1);
                for(int i = coordinateIndex; i >= 0; i--) {
                    spawnCoordinates[coordinateIndex] = remainingCoordinates[i];
                    coordinateIndex--;
                }
            }
            
        }
        return spawnCoordinates;
    }

    private Vector3[] getCluster(int xBound, int yBound, int size, Vector3[] cluster) {
        if(size == 0) {
            return cluster;
        }
        int randX = (int)Mathf.Round(Random.Range(xBound, xBound + clusterSize));
        int randY = (int)Mathf.Round(Random.Range(yBound, yBound + clusterSize));
        Vector3 randVector = new Vector3(randX, randY, 0);
        if (SpaceCheck.Instance.getOccupied(randVector) != null || contains(cluster, randVector)) {
            return getCluster(xBound, yBound, size, cluster);
        } 
        else {
            cluster[size - 1] = randVector;
            return getCluster(xBound, yBound, size - 1, cluster);
        }
    }

    private bool contains(Vector3[] vectors,Vector3 v) {
        for(int i = 0; i < vectors.Length; i++) {
            if(v.Equals(vectors[i])) {
                return true;
            }
        }
        return false;
    }

    private void spawn(Vector3[] coordinates, GameObject[] spawns) {
        for(int i = 0; i < coordinates.Length; i++) {
            Instantiate(spawns[(int)Mathf.Round(Random.Range(0,spawns.Length))], coordinates[i], transform.rotation);
        }
    }
    private bool middleRow(int rowNum) {
        return rowNum != 0 && rowNum != rows - 1;
    }
}
