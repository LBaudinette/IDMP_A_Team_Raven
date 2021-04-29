using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GridAreaScript : MonoBehaviour
{
    public float activationDelay;
    private int rows, columns;
    private float tileSize, pixelsToUnits, gridPixelsX, gridPixelsY, 
        activationTimer = 0;
    public bool isCasting = false;
    public GameObject damagingTile;

    private GameObject[,] grid;

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        

        pixelsToUnits = damagingTile.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        tileSize = damagingTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x * pixelsToUnits;

        gridPixelsX = gameObject.GetComponent<BoxCollider2D>().bounds.size.x * pixelsToUnits;
        gridPixelsY = gameObject.GetComponent<BoxCollider2D>().bounds.size.y * pixelsToUnits;

        columns = (int)( gridPixelsX/ tileSize);
        rows = (int)(gridPixelsY / tileSize);

        grid = new GameObject[rows, columns];


        Debug.Log("TILE SIZE: " + tileSize);
        Debug.Log("Columns: " + columns);
        Debug.Log("ROWS: " + rows);

        generateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        var input = Input.inputString;
        
        switch (input) {
            case "z":
                coroutine = StartCoroutine(alternateRowPattern());
                break;
            case "x":
                coroutine = StartCoroutine(alternateColPattern());
                break;
            case "c":
                coroutine = StartCoroutine(alternateCheckerPattern());
                break;
            case "v":
                checkerFlash();
                break;
            case "b":
                crossFlash(GameObject.FindWithTag("Player").transform.position);
                break;
            case "n":
                flashClosestCells(GameObject.FindWithTag("Player").transform.position);
                break;
        }
    }

    void generateGrid() {

        for(int row = 0; row < rows; row++) {
            for(int col = 0; col < columns; col++) {
                //instantiate tile
                GameObject currentTile = Instantiate(damagingTile,transform.parent);
                float xPos = (float)(-(gridPixelsX * 0.5f) + (tileSize * col) + (tileSize * 0.5f)) / pixelsToUnits;
                float yPos = (float)((gridPixelsY * 0.5f) - (tileSize * row) - (tileSize * 0.5f)) / pixelsToUnits;


                currentTile.transform.position = new Vector2(xPos, yPos);

                //deactivate the tile at first
                currentTile.SetActive(false);
                grid[row, col] = currentTile;
            }
        }
    }

   public void playRandomPattern() {
        //Generate number between 0 and the number of patterns 
        int randomNumber = Random.Range(0,4);
        isCasting = true;
        switch (randomNumber) {
            case 0:
                coroutine = StartCoroutine(alternateRowPattern());
                break;
            case 1:
                coroutine = StartCoroutine(alternateColPattern());
                break;
            case 2:
                coroutine = StartCoroutine(alternateCheckerPattern());
                break;
            case 3:
                coroutine = StartCoroutine(checkerFlash());
                break;
        }

    }

    //Starting from the top, activate every second row
    IEnumerator alternateRowPattern() {
        for(int row = 0; row < rows; row += 2) {
            for(int col = 0; col < columns; col++) {
                grid[row, col].GetComponent<DamagingTileScript>().activateTile();
            }
            while(activationTimer < activationDelay) {
                activationTimer += Time.deltaTime;
                yield return null;
            }
            activationTimer = 0f;
            
        }
        isCasting = false;

    }

    IEnumerator alternateColPattern() {
        for (int col = 0; col < columns; col += 2) {
            for (int row = 0; row < rows; row++) {
                grid[row, col].GetComponent<DamagingTileScript>().activateTile();
            }
            while (activationTimer < activationDelay) {
                activationTimer += Time.deltaTime;
                yield return null;
            }
            activationTimer = 0f;

        }
        isCasting = false;

    }

    //Alternate tiles through a checker pattern
    IEnumerator alternateCheckerPattern() {
        for (int row = 0; row < rows; row++) {
            //for every odd row, do every second column
            if (row % 2 == 1) {
                for (int col = 1; col < columns; col += 2) {

                    grid[row, col].GetComponent<DamagingTileScript>().activateTile();
                }
            }
            else {
                for (int col = 0; col < columns; col += 2) {

                    grid[row, col].GetComponent<DamagingTileScript>().activateTile();
                }
            }
             
            while (activationTimer < activationDelay) {
                activationTimer += Time.deltaTime;
                yield return null;
            }
            activationTimer = 0f;

        }
        isCasting = false;

    }

    //Flash multiple tiles at once though a checker pattern
    IEnumerator checkerFlash() {
        //store all tiles to activate in "tiles"
        var tiles = new List<GameObject>();

        for (int row = 0; row < rows; row++) {
            //for every odd row, do every second column
            if (row % 2 == 1) {
                for (int col = 1; col < columns; col += 2) {

                    tiles.Add(grid[row, col]);
                }
            }
            else {
                for (int col = 0; col < columns; col += 2) {

                    tiles.Add(grid[row, col]);
                }
            }
        }

        foreach(GameObject tile in tiles) 
            tile.GetComponent<DamagingTileScript>().activateTile();

        isCasting = false;

        return null;
    }

    void crossFlash(Vector2 origin) {
       //A vector storing the row(x) and column(y) in the grid array of the closest cell
        Vector2 closestCellPos = findClosestCell(origin);


        try {
            grid[(int)closestCellPos.x + 1, (int)closestCellPos.y].GetComponent<DamagingTileScript>().activateTile();
        }
        catch {

        }
        try {
            grid[(int)closestCellPos.x, (int)closestCellPos.y + 1].GetComponent<DamagingTileScript>().activateTile();
        }
        catch {

        }
        try {
            grid[(int)closestCellPos.x - 1, (int)closestCellPos.y].GetComponent<DamagingTileScript>().activateTile();
        }
        catch {

        }
        try {
            grid[(int)closestCellPos.x, (int)closestCellPos.y - 1].GetComponent<DamagingTileScript>().activateTile();

        }
        catch{
        }
        isCasting = false;
    }

    //Flash all cells in a 3x3 area around the origin
    void flashClosestCells(Vector2 origin) {
        Vector2 closestCellPos = findClosestCell(origin);
        for(int row = (int)closestCellPos.x - 1; row <= closestCellPos.x + 1; row++) {
            for(int col = (int)closestCellPos.y - 1; col <= closestCellPos.y + 1; col++) {
                try {
                    grid[row, col].GetComponent<DamagingTileScript>().activateTile();
                }
                catch {

                }
            }
        }
        isCasting = false;

    }


    //returns the row and column from the grid 2d array
    private Vector2 findClosestCell(Vector2 origin) {
        //Set the closest cell to the first cell so there is something to compare at first
        Transform closestCell = grid[0, 0].transform;
        int closestRow = 0, closestCol = 0;

        //Find cell closest to origin
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                if (Vector2.Distance((Vector2)grid[row, col].transform.position, origin)
                    < Vector2.Distance(closestCell.position, origin)) {
                    closestCell = grid[row, col].transform;
                    closestRow = row;
                    closestCol = col;
                }
            }
        }
        return new Vector2(closestRow, closestCol);
    }

}
