using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum boardState {gameInputAllowed, gameInputNotAllowed};

public class Board : MonoBehaviour
{   Debug.Log("wwwwwwwwwww");
    public boardState currentState;
    public int width;
    public int height;
    public GameObject backgroundTilePrefab;
    private Vector2[] refillStartPoint;
    public string[] dotTypes;
    public GameObject[] dots;
    public GameObject destroyEffect;
    private GameObject[,] allTiles;
    public GameObject[,] allDots;
    public GameObject chosenDot;
    public GameObject secondDot;

    // Start is called before the first frame update
    void Start() {
        
        currentState = boardState.gameInputAllowed;  
        allTiles = new GameObject[width, height];
        allDots = new GameObject[width, height];
        refillStartPoint = new Vector2[4];
        refillStartPoint[0] = new Vector2(5, -5);
        refillStartPoint[1] = new Vector2(-5, 5);
        refillStartPoint[2] = new Vector2(5, 15);
        refillStartPoint[3] = new Vector2(15, 5);
        SetUp();}

    private void SetUp() {
        
        
        for (int i = 0; i < width; i++) {
            
            
            for (int j = 0; j < height; j++) {
                Vector2 tempPosition = new Vector2(i, j);
                // GameObject backgroundTile = Instantiate(backgroundTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                // backgroundTile.transform.parent = this.transform;
                // backgroundTile.name = "(" + i + "," + j + ")";
                int dotIndex = Random.Range(0, dots.Length);
                while (isAMatchAt(i, j, dots[dotIndex])) {
                    dotIndex = Random.Range(0, dots.Length);}
                GameObject dot = Instantiate(dots[dotIndex], tempPosition, Quaternion.identity);
                allDots[i, j] = dot;
                dot.transform.parent = this.transform;
                dot.name = "(" + i + "," + j + ")";
                dot.GetComponent<Piece>().type = dotTypes[dotIndex];
                dot.GetComponent<Piece>().column = i;
                dot.GetComponent<Piece>().row = j;
            }
        }
    }

    public void lookingForAllMatches() {
        List<GameObject> piecesToExplore = new List<GameObject>();
        List<GameObject> piecesMatched = new List<GameObject>();
        List<List<GameObject>> allSolutions = new List<List<GameObject>>();
        GameObject lookingPiece;
        GameObject leftDot;
        GameObject rightDot;
        GameObject upDot;
        GameObject downDot;
        int exploredColumn;
        int exploredRow;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if (allDots[i,j].GetComponent<Piece>().isExplored == true) {}
                    else {
                        piecesToExplore.Add(allDots[i,j]);
                        while (piecesToExplore.Count > 0) {
                            lookingPiece = piecesToExplore[0];
                            exploredColumn = lookingPiece.GetComponent<Piece>().column;
                            exploredRow = lookingPiece.GetComponent<Piece>().row;
                            piecesToExplore.RemoveAt(0);
                            //left dot
                            if (exploredColumn > 0) {
                                if (allDots[exploredColumn - 1, exploredRow] != null) {
                                    leftDot = allDots[exploredColumn - 1, exploredRow];
                                    if (leftDot.GetComponent<Piece>().isExplored == false && lookingPiece.tag == leftDot.tag) {
                                        piecesToExplore.Add(leftDot);
                                        piecesMatched.Add(leftDot);
                                        leftDot.GetComponent<Piece>().isExplored = true;}}}
                            //right dot
                            if (exploredColumn < width - 1) {
                                if (allDots[exploredColumn + 1, exploredRow] != null) {
                                    rightDot = allDots[exploredColumn + 1, exploredRow];
                                    if (rightDot.GetComponent<Piece>().isExplored == false && lookingPiece.tag == rightDot.tag) {
                                        piecesToExplore.Add(rightDot);
                                        piecesMatched.Add(rightDot);
                                        rightDot.GetComponent<Piece>().isExplored = true;}}}
                            //up dot
                            if (exploredRow < height - 1) {
                                if (allDots[exploredColumn, exploredRow + 1] != null) {
                                    upDot = allDots[exploredColumn, exploredRow + 1];
                                    if (upDot.GetComponent<Piece>().isExplored == false && lookingPiece.tag == upDot.tag) {
                                        piecesToExplore.Add(upDot);
                                        piecesMatched.Add(upDot);
                                        upDot.GetComponent<Piece>().isExplored = true;}}}
                            //down_dot
                            if (exploredRow > 0) {
                                if (allDots[exploredColumn, exploredRow - 1] != null) {
                                    downDot = allDots[exploredColumn, exploredRow - 1];
                                    if (downDot.GetComponent<Piece>().isExplored == false && lookingPiece.tag == downDot.tag) {
                                        piecesToExplore.Add(downDot);
                                        piecesMatched.Add(downDot);
                                        downDot.GetComponent<Piece>().isExplored = true;}}}
                        }
                        if (piecesMatched.Count >= 3) {
                            for (int k = 0; k < piecesMatched.Count; k++) {
                                piecesMatched[k].GetComponent<SpriteRenderer>().color = Color.gray;
                                //Instantiate(destroyEffect, piecesMatched[k].transform.position, Quaternion.identity);
                                piecesMatched[k].GetComponent<Piece>().isMatched = true;}
                            allSolutions.Add(piecesMatched);}
                        piecesMatched.Clear();
                    }
                }
            }
        }
        // if (allSolutions.Count > 0) {
        //     for (int i = 0; i < allSolutions.Count; i++) {
        //         for (int j = 0; j < allSolutions[i].Count; j++) {
        //             allSolutions[i][j].GetComponent<SpriteRenderer>().color = Color.gray;
        //             }
        //     }
        // }
    setAllDotsUnexplored();
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
            //left dot
            if (exploredColumn > 0) {
                if (allDots[exploredColumn - 1, exploredRow] != null) {
                    GameObject leftDot = allDots[exploredColumn - 1, exploredRow];
                    if (leftDot.tag == exploringPiece.tag & leftDot.GetComponent<Piece>().isExplored == false) {
                        leftDot.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(leftDot);}}}
            //right dot
            if (exploredColumn < width - 1) {
                if (allDots[exploredColumn + 1, exploredRow] != null) {
                    GameObject rightDot = allDots[exploredColumn + 1, exploredRow];
                    if (rightDot.tag == exploringPiece.tag & rightDot.GetComponent<Piece>().isExplored == false) {
                        rightDot.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(rightDot);}}}            
            //up dot
            if (exploredRow < height - 1) {
                if (allDots[exploredColumn, exploredRow + 1] != null) {
                    GameObject upDot = allDots[exploredColumn, exploredRow + 1];
                    if (upDot.tag == exploringPiece.tag & upDot.GetComponent<Piece>().isExplored == false) {
                        upDot.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(upDot);}}}
            //down dot
            if (exploredRow > 0) {
                if (allDots[exploredColumn, exploredRow - 1] != null) {
                    GameObject downDot = allDots[exploredColumn, exploredRow - 1];
                    if (downDot.tag == exploringPiece.tag & downDot.GetComponent<Piece>().isExplored == false) {
                        downDot.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(downDot);}}}
        }
        setAllDotsUnexplored();
        if (matchLength > 2) {
            //setAllDotsUnexplored();
            return true;}
        //setAllDotsUnexplored();
        return false;        
    }

    public void setAllDotsUnexplored () {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                allDots[i, j].GetComponent<Piece>().isExplored = false;}}}
        for (int j = 0; j < dots.Length; j++) {
            dots[j].GetComponent<Piece>().isExplored = false;}}

    public void setAllDotsUnmatched () {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    allDots[i, j].GetComponent<Piece>().isMatched = false;}}}}

    public void destroyAllMatches () {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if (allDots[i, j].GetComponent<Piece>().isMatched == true) {
                        Instantiate(destroyEffect, allDots[i, j].transform.position, Quaternion.identity);
                        Destroy(allDots[i, j]);
                        allDots[i, j] = null;}}}}
    StartCoroutine(colapseAllColumns());
    }

    public void destroyAt (int column, int row) {
        List<GameObject> piecesMachted = new List<GameObject>();
        List<GameObject> piecesToExplore = new List<GameObject>();
        GameObject exploringPiece;
        int exploredColumn;
        int exploredRow;
        piecesToExplore.Add(allDots[column, row]);
        while(piecesToExplore.Count > 0) {
            exploringPiece = piecesToExplore[0];
            exploredColumn = exploringPiece.GetComponent<Piece>().column;
            exploredRow = exploringPiece.GetComponent<Piece>().row;
            piecesMachted.Add(exploringPiece);
            exploringPiece.GetComponent<Piece>().isMatched = true;
            piecesToExplore.RemoveAt(0);
            //Left dot
            if(exploredColumn > 0) {
                GameObject leftDot = allDots[exploredColumn - 1, row];
                if(leftDot.tag == exploringPiece.tag & leftDot.GetComponent<Piece>().isMatched == false) {
                    piecesToExplore.Add(leftDot);
                    piecesMachted.Add(leftDot);}}
            //Right dot
            if(exploredColumn < width - 1) {
                GameObject rightDot = allDots[exploredColumn + 1, row];
                if(rightDot.tag == exploringPiece.tag & rightDot.GetComponent<Piece>().isMatched == false) {
                    piecesToExplore.Add(rightDot);
                    piecesMachted.Add(rightDot);}}
            //Up dot
            if(exploredRow < height - 1) {
                GameObject upDot = allDots[exploredColumn, row + 1];
                if(upDot.tag == exploringPiece.tag & upDot.GetComponent<Piece>().isMatched == false) {
                    piecesToExplore.Add(upDot);
                    piecesMachted.Add(upDot);}}
            //Down dot
            if(exploredColumn > 0) {
                GameObject downDot = allDots[exploredColumn, row - 1];
                if(downDot.tag == exploringPiece.tag & downDot.GetComponent<Piece>().isMatched == false) {
                    piecesToExplore.Add(downDot);
                    piecesMachted.Add(downDot);}}
                
        }
        if(piecesMachted.Count > 2) {
            while(piecesMachted[0] != null) {
                Destroy(piecesMachted[0]);}}
        }
    
    private IEnumerator colapseAllColumns() {
        int blankSpacesCount;
        for (int i = 0; i < width; i++) {
            blankSpacesCount = 0;
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] == null) {
                    blankSpacesCount += 1;
                }
                else if (blankSpacesCount > 0) {
                    if ((allDots[i, j].GetComponent<Piece>().row - blankSpacesCount) >= 0) {
                    allDots[i, j].GetComponent<Piece>().row -= blankSpacesCount;
                    allDots[i, j].GetComponent<Piece>().previousRow = allDots[i, j].GetComponent<Piece>().row; 
                    allDots[i, j - blankSpacesCount] = allDots[i, j];
                    allDots[i ,j] = null;
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(fillBoardCoroutine());       
    }

    public bool isAMatchOnBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if (allDots[i, j].GetComponent<Piece>().isMatched == true) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void refillBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] == null) {
                    Vector2 tempPosition = refillStartPoint[Random.Range(0, 4)];
                    int dotIndex = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotIndex], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.transform.parent = this.transform;
                    piece.GetComponent<Piece>().type = dotTypes[dotIndex];
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
        yield return new WaitForSeconds(0.5f);
        lookingForAllMatches();
        while(isAMatchOnBoard()) {
            yield return new WaitForSeconds(0.5f);
            destroyAllMatches();
        }
    }


  // Update is called once per frame
    void Update()
    {
      
    }
    
}



    




  
     
  
    

