using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] tilesPrefabs;
    public GameObject[] piecesPrefabs;
    private GameObject[,] allTiles;
    public GameObject[,] allPieces;
    public int offsetNewPieces;
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
                Vector2 positionForNewTile = new Vector2(i, j);
                GameObject newTile = Instantiate(tilesPrefabs[0], positionForNewTile, Quaternion.identity);
                newTile.transform.parent = this.transform;
                newTile.GetComponent<Tile>().column = i;
                newTile.GetComponent<Tile>().row = j;
                allTiles[i, j] = newTile;
                int newRegularPieceTypeIndex = Random.Range(0, piecesPrefabs.Length);
                int maxIterations = 0;
                while(isMatchAt(i, j, piecesPrefabs[newRegularPieceTypeIndex]) && maxIterations < 100) {
                    newRegularPieceTypeIndex = Random.Range(0, piecesPrefabs.Length);
                    maxIterations++;
                }
                maxIterations = 0;
                Vector2 positionForNewPiece = new Vector2(i, j + offsetNewPieces);
                GameObject newRegularPiece = Instantiate(piecesPrefabs[newRegularPieceTypeIndex], positionForNewPiece, Quaternion.identity);
                newRegularPiece.GetComponent<Piece>().column = i;
                newRegularPiece.GetComponent<Piece>().row = j;
                transform.parent = this.transform;
                allPieces[i, j] = newRegularPiece;
            }
        }
    }
    private bool isMatchAt(int column, int row, GameObject exploringPiece) {
        if(column > 1 && row > 1) {
            if(allPieces[column -1, row].GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color &&
            exploringPiece.GetComponent<Piece>().color == allPieces[column - 2, row].GetComponent<Piece>().color) {
                return true;
            }
            if(allPieces[column, row - 1].GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color &&
            exploringPiece.GetComponent<Piece>().color == allPieces[column, row - 2].GetComponent<Piece>().color) {
                return true;
            }
        }
        else if(column <= 1 || row <= 1) {
            if(row > 1) {
                if(allPieces[column, row - 1].GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color &&
                allPieces[column, row - 2].GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color) {
                    return true;
                }
            }
            if(column > 1) {
                if(allPieces[column - 1, row].GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color &&
                allPieces[column - 2, row].GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color) {
                    return true;
                }
            }

        }
        return false;
    }
    private void destroyMatchesAt(int column, int row) {
        if(allPieces[column, row].GetComponent<Piece>().isMatched) {
            Destroy(allPieces[column, row]);
            allPieces[column, row] = null;
        }
    }
    public void destrolAllMatches() {
        for(int i = 0; i < width;i++) {
            for(int j = 0; j < height; j++) {
                if(allPieces[i, j] != null) {
                    destroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(collapseColumnsCoroutine());
    }
    private IEnumerator collapseColumnsCoroutine() {
        int nullCount = 0;
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(allPieces[i, j] == null) {
                    nullCount++;
                }
                else if (nullCount > 0) {
                    allPieces[i, j].GetComponent<Piece>().row -= nullCount;
                    allPieces[i, j] = null;
                }
            }
        nullCount = 0;
        }
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(fillBoardCoroutine());
    }
    private void refillBoard() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(allPieces[i, j] == null) {
                    Vector2 newPiecePosition = new Vector2(i, j + offsetNewPieces);
                    int newPieceindex = Random.Range(0, 4);
                    GameObject newPiece = Instantiate(piecesPrefabs[newPieceindex], newPiecePosition, Quaternion.identity);
                    allPieces[i, j] = newPiece;
                    newPiece.GetComponent<Piece>().column = i;
                    newPiece.GetComponent<Piece>().row = j;
                }
            }
        }
    }
    private bool isMatchesOnBoard() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(allPieces[i, j] != null) {
                    if(allPieces[i, j].GetComponent<Piece>().isMatched) {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator fillBoardCoroutine() {
        refillBoard();
        yield return new WaitForSeconds(0.25f);
        while(isMatchesOnBoard()) {
            yield return new WaitForSeconds(0.25f);
            destrolAllMatches();

        }
    }
}
