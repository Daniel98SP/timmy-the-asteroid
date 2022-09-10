using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public int seed;
    public GameObject plainTile;
    public GameObject invertTile;
    public GameObject impulseTile;
    public GameObject doubleJumpTile;
    public GameObject bluePortalTile;
    public GameObject orangePortalTile;
    public GameObject spikesObstacle;
    public GameObject wallObstacle;
    public GameObject zMovementObstacle;
    public GameObject fallingObstacle;
    public GameObject blackHole;
    public int distance;

    private int rowsPerBlock = 10, currentBlockInitialRow = 0, currentRow = 0, currentColumn = 0, currentHeight = 0;
    private int[] alreadyGenerated = new int[2];
    private bool portalGenerated = false;
    private int heightFromPortal = 0;
    private int currentBlockType;
    private int wallRow, wallColumn;
    private int safeDeleteCoordinate = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (seed == 0) {
            Random.InitState((int)System.DateTime.Now.Ticks);
        } else {
            Random.InitState(seed);
        }

        alreadyGenerated[0] = 0;
        alreadyGenerated[1] = rowsPerBlock;

        InitialGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x >= alreadyGenerated[0] && portalGenerated)
        {
            currentRow = alreadyGenerated[0] + 2 * rowsPerBlock;
            portalGenerated = false;
            heightFromPortal = 0;
            safeDeleteCoordinate = currentBlockInitialRow;
        }

        if ((currentRow - 1 - gameObject.transform.position.x) < distance)
        {
            if (currentRow != alreadyGenerated[0] && currentRow != alreadyGenerated[1])
            {
                Generate();                
            }
        }        
    }

    void Generate()
    {
        if (currentRow % rowsPerBlock == 0)
        {
            int randomHeight = Random.Range(-10, 11);

            if (randomHeight == -10 && heightFromPortal != -1)
            {
                currentHeight -= 1;
                heightFromPortal -= 1;
            }
            else if (randomHeight == 10 && heightFromPortal != 1)
            {
                currentHeight += 1;
                heightFromPortal += 1;
            }

            if (!portalGenerated) 
            {
                safeDeleteCoordinate = currentBlockInitialRow;
            }

            currentBlockInitialRow = currentRow;
            currentColumn = 0;
            currentBlockType = Random.Range(0, 17);
        }        

        switch (currentBlockType)
        {
            // Full block 
            case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 10:
                for (int j = 0; j < 7; j++)
                {
                    GenerateObject(currentRow, j);
                }

                if (currentRow % rowsPerBlock == rowsPerBlock - 1)
                {                    
                    GenerateObstacle();
                }

                break;

            // Line 
            case 11: case 12: case 13: case 14: case 15:
                if (currentRow % rowsPerBlock == 0)
                {
                    currentColumn = Random.Range(0, 7);
                }

                GenerateObject(currentRow, currentColumn);
                break;

            // random 
            case 16:
                for (int j = 0; j < 7; j++)
                {
                    if (Random.Range(0,4) == 0)
                    {
                        GenerateObject(currentRow, j);
                    }
                }

                break;         
        }
        ++currentRow;
    }


    void InitialGeneration()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Instantiate(plainTile, new Vector3(i, 0, j), Quaternion.identity);
            }
        }

        currentRow = 20;
    }

    private GameObject GeneratePortalBlock(GameObject bluePortal)
    {
        int orangePortalBlockDistance = Random.Range(2, 5);
        int portalBlockRow = currentBlockInitialRow + orangePortalBlockDistance * rowsPerBlock;

        int row = Random.Range(portalBlockRow, portalBlockRow + 3);
        int column = Random.Range(0, 7);

        Instantiate(plainTile, new Vector3(row, currentHeight, column), Quaternion.identity);
        GameObject orangePortal = Instantiate(orangePortalTile, new Vector3(row, currentHeight + 0.5f, column), Quaternion.identity);
        orangePortal.GetComponent<PortalTile>().ConnectToPortal(bluePortal);

        for (int i = portalBlockRow; i < portalBlockRow + rowsPerBlock; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (i != row || j != column)
                {
                    Instantiate(plainTile, new Vector3(i, currentHeight, j), Quaternion.identity);
                }                
            }
        }

        for (int i = portalBlockRow + rowsPerBlock; i < portalBlockRow + rowsPerBlock + rowsPerBlock; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Instantiate(plainTile, new Vector3(i, currentHeight, j), Quaternion.identity);
            }
        }

        alreadyGenerated[0] = portalBlockRow;
        alreadyGenerated[1] = portalBlockRow + rowsPerBlock;
        portalGenerated = true;

        return orangePortal;
    }

    private void GenerateObject(int i, int j) {
        int randomTileOrObstacle = Random.Range(0, 500);

        if (randomTileOrObstacle < 5)
        {
            Instantiate(invertTile, new Vector3(i, currentHeight, j), Quaternion.identity);
        }
        else if (randomTileOrObstacle >= 5 && randomTileOrObstacle < 10)
        {
            Instantiate(impulseTile, new Vector3(i, currentHeight, j), Quaternion.identity);
        }
        else if (randomTileOrObstacle >= 10 && randomTileOrObstacle < 15)
        {
            Instantiate(doubleJumpTile, new Vector3(i, currentHeight, j), Quaternion.identity);
        }
        else if (randomTileOrObstacle == 15)
        {
            if (!portalGenerated) {
                Instantiate(plainTile, new Vector3(i, currentHeight, j), Quaternion.identity);
                GameObject bluePortal = Instantiate(bluePortalTile, new Vector3(i, currentHeight + 0.5f, j), Quaternion.identity);

                GameObject orangePortal = GeneratePortalBlock(bluePortal);

                bluePortal.GetComponent<PortalTile>().ConnectToPortal(orangePortal);

                safeDeleteCoordinate = currentBlockInitialRow;
            }
        }
        else if (randomTileOrObstacle >= 16 && randomTileOrObstacle < 21)
        {
            Instantiate(plainTile, new Vector3(i, currentHeight, j), Quaternion.identity);
            Instantiate(spikesObstacle, new Vector3(i, currentHeight, j), Quaternion.identity);
        }
        else if (randomTileOrObstacle == 21)
        {
            Instantiate(plainTile, new Vector3(i, currentHeight, j), Quaternion.identity);
            Instantiate(blackHole, new Vector3(i, currentHeight + 0.625f, j), Quaternion.identity);
        }
        else if (randomTileOrObstacle >= 22 && randomTileOrObstacle < 25)
        {
            Instantiate(plainTile, new Vector3(i, currentHeight, j), Quaternion.identity);
            Instantiate(fallingObstacle, new Vector3(i, currentHeight + 25.0f, j), Quaternion.identity);
        }
        else 
        {
            Instantiate(plainTile, new Vector3(i, currentHeight, j), Quaternion.identity);
        }
    }

    private void GenerateObstacle() {
        int obstacleType = Random.Range(0, 10);

        if (obstacleType < 4)
        {
            wallRow = Random.Range(currentBlockInitialRow + 2, currentBlockInitialRow + 5);
            wallColumn = Random.Range(2, 5);
            Instantiate(wallObstacle, new Vector3(wallRow, currentHeight + 1.3f, wallColumn), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        }
        else if (obstacleType == 4)
        {
            if (zMovementObstacle.tag == "fanObstacle")
            {
                Instantiate(zMovementObstacle, new Vector3(currentBlockInitialRow + 2.5f, currentHeight + 0.2f, -2), Quaternion.identity);

                GameObject tile1 = Instantiate(plainTile, new Vector3(currentBlockInitialRow + 2, currentHeight, -2), Quaternion.identity);
                tile1.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                GameObject tile2 = Instantiate(plainTile, new Vector3(currentBlockInitialRow + 2, currentHeight, -3), Quaternion.identity);
                tile2.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                GameObject tile3 = Instantiate(plainTile, new Vector3(currentBlockInitialRow + 3, currentHeight, -2), Quaternion.identity);
                tile3.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                GameObject tile4 = Instantiate(plainTile, new Vector3(currentBlockInitialRow + 3, currentHeight, -3), Quaternion.identity);
                tile4.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            }
            else 
            {
                Instantiate(zMovementObstacle, new Vector3(currentBlockInitialRow + 2.5f, currentHeight + 2, -4), Quaternion.identity);
            }
        }
    }

    public int GetCurrentHeight() {
        return currentHeight;
    }

    public int SafeObjectDeletionCoordinate()
    {
        return safeDeleteCoordinate - 30;
    }
}