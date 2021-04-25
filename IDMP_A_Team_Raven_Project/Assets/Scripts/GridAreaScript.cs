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
        if (Input.GetKeyDown("l")) {
            coroutine = StartCoroutine(alternateRowPattern());
        } else if (Input.GetKeyDown("k")) {
            coroutine = StartCoroutine(alternateColPattern());

        }
    }

    void generateGrid() {

        for(int row = 0; row < rows; row++) {
            for(int col = 0; col < columns; col++) {
                //instantiate tile
                GameObject currentTile = Instantiate(damagingTile,transform.parent);
                float xPos = (float)(-(gridPixelsX * 0.5f) + (tileSize * col) + (tileSize * 0.5f)) / pixelsToUnits;
                float yPos = (float)((gridPixelsY * 0.5f) - (tileSize * row) - (tileSize * 0.5f)) / pixelsToUnits;

                //float xPos = col * tileSize;
                //float yPos = row * tileSize;
                currentTile.transform.position = new Vector2(xPos, yPos);

                //deactivate the tile at first
                //currentTile.SetActive(false);
                grid[row, col] = currentTile;
            }
        }
    }

    void playRandomPattern() {
        int randomNumber;
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
            activationTimer = 0;
            
        }
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
            activationTimer = 0;

        }

    }
}
