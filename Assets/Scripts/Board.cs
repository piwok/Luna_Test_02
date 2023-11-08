using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum boardStates {gameInputAllowed, movingPieces, gameInputNotAllowed};

public class Board : MonoBehaviour
{   public boardStates currentState;
    
    public int width;
    public int height;
    public GameObject backgroundTilePrefab;
    private Vector2[] refillStartPoint;
    public string[] pieceTypes;
    public GameObject[] pieces;
    public GameObject destroyEffect;
    private GameObject[,] allTiles;
    public GameObject[,] allPieces;
    public GameObject chosenPiece;
    public GameObject secondPiece;
    public List<GameObject> piecesMatched;

    // Start is called before the first frame update
    void Start() {
        currentState = boardStates.gameInputAllowed;
        allTiles = new GameObject[width, height];
        allPieces = new GameObject[width, height];
        refillStartPoint = new Vector2[4];
        refillStartPoint[0] = new Vector2(5, -5);
        refillStartPoint[1] = new Vector2(-5, 5);
        refillStartPoint[2] = new Vector2(5, 15);
        refillStartPoint[3] = new Vector2(15, 5);
        piecesMatched = new List<GameObject>();
        SetUp();}

    private void SetUp() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector2 tempPosition = new Vector2(i, j);
                // GameObject backgroundTile = Instantiate(backgroundTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                // backgroundTile.transform.parent = this.transform;
                // backgroundTile.name = "(" + i + "," + j + ")";
                int pieceIndex = Random.Range(0, pieces.Length);
                while (isAMatchAt(i, j, pieces[pieceIndex])) {
                    pieceIndex = Random.Range(0, pieces.Length);}
                GameObject piece = Instantiate(pieces[pieceIndex], tempPosition, Quaternion.identity);
                allPieces[i, j] = piece;
                piece.transform.parent = this.transform;
                piece.GetComponent<Piece>().type = pieceTypes[pieceIndex];
                piece.GetComponent<Piece>().column = i;
                piece.GetComponent<Piece>().row = j;}}
    }

    public List<List<GameObject>> lookingForAllMatches() {
        List<GameObject> piecesToExplore = new List<GameObject>();
        //List<GameObject> piecesMatched = new List<GameObject>();
        List<List<GameObject>> allSolutions = new List<List<GameObject>>();
        GameObject lookingPiece;
        GameObject leftPiece;
        GameObject rightPiece;
        GameObject upPiece;
        GameObject downPiece;
        int exploredColumn;
        int exploredRow;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allPieces[i, j] != null) {
                    if (allPieces[i,j].GetComponent<Piece>().isExplored == true) {}
                    else {
                        piecesToExplore.Add(allPieces[i,j]);
                        while (piecesToExplore.Count > 0) {
                            lookingPiece = piecesToExplore[0];
                            exploredColumn = lookingPiece.GetComponent<Piece>().column;
                            exploredRow = lookingPiece.GetComponent<Piece>().row;
                            piecesToExplore.RemoveAt(0);
                            //left piece
                            if (exploredColumn > 0) {
                                if (allPieces[exploredColumn - 1, exploredRow] != null) {
                                    leftPiece = allPieces[exploredColumn - 1, exploredRow];
                                    if (leftPiece.GetComponent<Piece>().isExplored == false && lookingPiece.tag == leftPiece.tag) {
                                        piecesToExplore.Add(leftPiece);
                                        piecesMatched.Add(leftPiece);
                                        leftPiece.GetComponent<Piece>().isExplored = true;}}}
                            //right piece
                            if (exploredColumn < width - 1) {
                                if (allPieces[exploredColumn + 1, exploredRow] != null) {
                                    rightPiece = allPieces[exploredColumn + 1, exploredRow];
                                    if (rightPiece.GetComponent<Piece>().isExplored == false && lookingPiece.tag == rightPiece.tag) {
                                        piecesToExplore.Add(rightPiece);
                                        piecesMatched.Add(rightPiece);
                                        rightPiece.GetComponent<Piece>().isExplored = true;}}}
                            //up piece
                            if (exploredRow < height - 1) {
                                if (allPieces[exploredColumn, exploredRow + 1] != null) {
                                    upPiece = allPieces[exploredColumn, exploredRow + 1];
                                    if (upPiece.GetComponent<Piece>().isExplored == false && lookingPiece.tag == upPiece.tag) {
                                        piecesToExplore.Add(upPiece);
                                        piecesMatched.Add(upPiece);
                                        upPiece.GetComponent<Piece>().isExplored = true;}}}
                            //down_piece
                            if (exploredRow > 0) {
                                if (allPieces[exploredColumn, exploredRow - 1] != null) {
                                    downPiece = allPieces[exploredColumn, exploredRow - 1];
                                    if (downPiece.GetComponent<Piece>().isExplored == false && lookingPiece.tag == downPiece.tag) {
                                        piecesToExplore.Add(downPiece);
                                        piecesMatched.Add(downPiece);
                                        downPiece.GetComponent<Piece>().isExplored = true;}}}
                        }
                        if (piecesMatched.Count >= 3) {
                            
                            allSolutions.Add(new List<GameObject>(piecesMatched));

                            
                        }
                        piecesMatched.Clear();
                    }
                }
            }
        }
        setAllPiecesUnexplored();
        Debug.Log(allSolutions.Count);
        return allSolutions;
    }
    public bool isAMatchAt(int column, int row, GameObject new_piece) {
        List<GameObject> piecesToExplore = new List<GameObject>();
        int matchLength = 0;
        piecesToExplore.Add(new_piece);
        int exploredColumn;
        int exploredRow;
        while (piecesToExplore.Count > 0) {
            GameObject exploringPiece = piecesToExplore[0];
            exploringPiece.GetComponent<Piece>().isExplored = true;
            piecesToExplore.RemoveAt(0);
            if (matchLength == 0) {
                exploredColumn = column;
                exploredRow = row;}
            else {
                exploredColumn = exploringPiece.GetComponent<Piece>().column;
                exploredRow = exploringPiece.GetComponent<Piece>().row;}
            matchLength += 1;
            //left piece
            if (exploredColumn > 0) {
                if (allPieces[exploredColumn - 1, exploredRow] != null) {
                    GameObject leftPiece = allPieces[exploredColumn - 1, exploredRow];
                    if (leftPiece.tag == exploringPiece.tag & leftPiece.GetComponent<Piece>().isExplored == false) {
                        leftPiece.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(leftPiece);}}}
            //right piece
            if (exploredColumn < width - 1) {
                if (allPieces[exploredColumn + 1, exploredRow] != null) {
                    GameObject rightPiece = allPieces[exploredColumn + 1, exploredRow];
                    if (rightPiece.tag == exploringPiece.tag & rightPiece.GetComponent<Piece>().isExplored == false) {
                        rightPiece.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(rightPiece);}}}            
            //up piece
            if (exploredRow < height - 1) {
                if (allPieces[exploredColumn, exploredRow + 1] != null) {
                    GameObject upPiece = allPieces[exploredColumn, exploredRow + 1];
                    if (upPiece.tag == exploringPiece.tag & upPiece.GetComponent<Piece>().isExplored == false) {
                        upPiece.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(upPiece);}}}
            //down piece
            if (exploredRow > 0) {
                if (allPieces[exploredColumn, exploredRow - 1] != null) {
                    GameObject downPiece = allPieces[exploredColumn, exploredRow - 1];
                    if (downPiece.tag == exploringPiece.tag & downPiece.GetComponent<Piece>().isExplored == false) {
                        downPiece.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(downPiece);}}}
        }
        setAllPiecesUnexplored();
        if (matchLength > 2) {
            return true;}
        return false;        
    }

    public void setAllPiecesUnexplored () {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allPieces[i, j] != null) {
                allPieces[i, j].GetComponent<Piece>().isExplored = false;}}}
        for (int j = 0; j < pieces.Length; j++) {
            pieces[j].GetComponent<Piece>().isExplored = false;}}

    private IEnumerator destroyAllMatches (List<List<GameObject>> allSolutions) {
            
        bool flag;
        for (int i = 0; i < 100; i++) {
            flag = true;
            foreach (List<GameObject> solution in allSolutions) {
                if ( i < solution.Count) {
                    flag = false;
                    Instantiate(destroyEffect, solution[i].transform.position, Quaternion.identity);
                    allPieces[(int)solution[i].transform.position.x, (int)solution[i].transform.position.y] = null;
                    Destroy(solution[i]);
                    
                }
            }
            if (flag == true) {break;}
            yield return  new WaitForSeconds(0.1f);
        }
        StartCoroutine(colapseAllColumns());
    }
    
    private IEnumerator colapseAllColumns() {
        int blankSpacesCount;
        for (int i = 0; i < width; i++) {
            blankSpacesCount = 0;
            for (int j = 0; j < height; j++) {
                if (allPieces[i, j] == null) {
                    blankSpacesCount += 1;
                }
                else if (blankSpacesCount > 0) {
                    if ((allPieces[i, j].GetComponent<Piece>().row - blankSpacesCount) >= 0) {
                    allPieces[i, j].GetComponent<Piece>().row -= blankSpacesCount;
                    allPieces[i, j].GetComponent<Piece>().previousRow = allPieces[i, j].GetComponent<Piece>().row; 
                    allPieces[i, j - blankSpacesCount] = allPieces[i, j];
                    allPieces[i ,j] = null;
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(fillBoardCoroutine());       
    }

    public bool isAMatchOnBoard(List<List<GameObject>> allSolutions) {
        if (allSolutions.Count > 0) {
            return true;
        } else {
            return false;
        }
    }

    private void refillBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allPieces[i, j] == null) {
                    Vector2 tempPosition = refillStartPoint[Random.Range(0, 4)];
                    int pieceIndex = Random.Range(0, pieces.Length);
                    GameObject piece = Instantiate(pieces[pieceIndex], tempPosition, Quaternion.identity);
                    allPieces[i, j] = piece;
                    piece.transform.parent = this.transform;
                    piece.GetComponent<Piece>().type = pieceTypes[pieceIndex];
                    piece.GetComponent<Piece>().column = i;
                    piece.GetComponent<Piece>().row = j;
                    piece.GetComponent<Piece>().previousColumn = piece.GetComponent<Piece>().column;
                    piece.GetComponent<Piece>().previousRow = piece.GetComponent<Piece>().row;
                }
            }
        }
    }

    private IEnumerator fillBoardCoroutine() {
        refillBoard();
        yield return new WaitForSeconds(0.4f);
        List<List<GameObject>> allSolutions = new List<List<GameObject>>();
        allSolutions = lookingForAllMatches();        
        if (isAMatchOnBoard(allSolutions)) {
            
            StartCoroutine(destroyAllMatches(allSolutions));
            yield return new WaitForSeconds(0.25f);
            
        }
    }

    public void movePieces(float swipeAngle) {
        if (swipeAngle > -45 && swipeAngle <= 45 && chosenPiece.GetComponent<Piece>().column < width) {
            //Right swipe
            if (allPieces[chosenPiece.GetComponent<Piece>().column + 1, chosenPiece.GetComponent<Piece>().row] != null) {
                secondPiece = allPieces[chosenPiece.GetComponent<Piece>().column +1, chosenPiece.GetComponent<Piece>().row];
                chosenPiece.GetComponent<Piece>().column += 1;
                secondPiece.GetComponent<Piece>().column -= 1;
                allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row] = chosenPiece;
                allPieces[secondPiece.GetComponent<Piece>().column, secondPiece.GetComponent<Piece>().row] = secondPiece;
            }
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && chosenPiece.GetComponent<Piece>().row < height) {
            //Up swipe
            if (allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row + 1] != null) {
                secondPiece = allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row + 1];
                chosenPiece.GetComponent<Piece>().row += 1;
                secondPiece.GetComponent<Piece>().row -= 1;
                allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row] = chosenPiece;
                allPieces[secondPiece.GetComponent<Piece>().column, secondPiece.GetComponent<Piece>().row] = secondPiece;
            }
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && chosenPiece.GetComponent<Piece>().column > 0) {
            //Left swipe
            if (allPieces[chosenPiece.GetComponent<Piece>().column - 1, chosenPiece.GetComponent<Piece>().row] != null) {
                secondPiece = allPieces[chosenPiece.GetComponent<Piece>().column - 1, chosenPiece.GetComponent<Piece>().row];
                chosenPiece.GetComponent<Piece>().column -= 1;
                secondPiece.GetComponent<Piece>().column += 1;
                allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row] = chosenPiece;
                allPieces[secondPiece.GetComponent<Piece>().column, secondPiece.GetComponent<Piece>().row] = secondPiece;
            }
        }
        else if (swipeAngle >= -135 && swipeAngle < -45 && chosenPiece.GetComponent<Piece>().row > 0) {
            //Down swipe
            if (allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row - 1] != null) {
                secondPiece = allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row - 1];
                chosenPiece.GetComponent<Piece>().row -= 1;
                secondPiece.GetComponent<Piece>().row += 1;
                allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row] = chosenPiece;
                allPieces[secondPiece.GetComponent<Piece>().column, secondPiece.GetComponent<Piece>().row] = secondPiece;
            }
        }
        //currentState = boardStates.movingPieces;
        StartCoroutine(checkMoveCoroutine());
    }

    public IEnumerator checkMoveCoroutine() { //Revisar esto esta mal
        yield return new WaitForSeconds(0.35f);
        if (!isAMatchAt(chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row, chosenPiece) & !isAMatchAt(secondPiece.GetComponent<Piece>().column, secondPiece.GetComponent<Piece>().row, secondPiece)) {
            secondPiece.GetComponent<Piece>().column = chosenPiece.GetComponent<Piece>().column;
            secondPiece.GetComponent<Piece>().row = chosenPiece.GetComponent<Piece>().row;
            chosenPiece.GetComponent<Piece>().column = chosenPiece.GetComponent<Piece>().previousColumn;
            chosenPiece.GetComponent<Piece>().row = chosenPiece.GetComponent<Piece>().previousRow;
        }
        else {
            
            
            StartCoroutine(destroyAllMatches(lookingForAllMatches()));
            
        }
        secondPiece = null;
    }


  // Update is called once per frame
    void Update()
    {
        if (currentState == boardStates.movingPieces) {

        }
      
    }
    
}



    




  
     
  
    

