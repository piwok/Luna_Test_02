using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] tilesPrefabs;
    public GameObject[] piecesPrefabs;
    private GameObject[,] allTiles;
    public GameObject[,] allPieces;
    // Start is called before the first frame update
    void Start()
    {
        allTiles = new GameObject[width, height];
        allPieces = new GameObject[width, height];
        setUp();
        
    }
    private void setUp() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector2 positionForNewPiece = new Vector2(i, j);
                GameObject newTile = Instantiate(tilesPrefabs[0], positionForNewPiece, Quaternion.identity);
                newTile.transform.parent = this.transform;
                newTile.GetComponent<Tile>().column = i;
                newTile.GetComponent<Tile>().row = j;
                allTiles[i, j] = newTile;
                int newRegularPieceTypeIndex = Random.Range(0, piecesPrefabs.Length);
                GameObject newRegularPiece = Instantiate(piecesPrefabs[newRegularPieceTypeIndex], positionForNewPiece, Quaternion.identity);
                allPieces[i, j] = newRegularPiece;

            }
        }
    }
}
