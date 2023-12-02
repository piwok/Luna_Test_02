using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum boardStates {gameInputAllowed, movingPieces, gameInputNotAllowed};

public class Board : MonoBehaviour
{   public boardStates currentState;
    
    public int width;
    public int height;
    private MatchsFinder matchsFinder;
    private Vector2[] refillStartPoint;
    public string[] pieceTypes;
    public GameObject[] pieces;
    private GameObject[,] allTiles;
    public GameObject[,] allPieces;
    public GameObject chosenPiece;
    public GameObject secondPiece;
    public GameObject tntPiece;
    

    // Start is called before the first frame update
    void Start() {
        currentState = boardStates.gameInputAllowed;
        matchsFinder = FindObjectOfType<MatchsFinder>();
        allTiles = new GameObject[width, height];
        allPieces = new GameObject[width, height];
        SetUp();
        
    }

    private void SetUp() {
        List<Solution> matchsToEliminate = new List<Solution>();
        

        //initial board generation with matches
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector2 tempPosition = new Vector2(i, j);
                int pieceIndex = Random.Range(0, pieces.Length);
                bool isAMatch = matchsFinder.isLegalMatchInBoard();
                GameObject piece = Instantiate(pieces[pieceIndex], tempPosition, Quaternion.identity);
                allPieces[i, j] = piece;
                piece.transform.parent = this.transform;
                piece.GetComponent<Piece>().column = i;
                piece.GetComponent<Piece>().row = j;
            }
        }
        //loop changing colors of pieces in a match until there is not matches
        matchsToEliminate = matchsFinder.lookingForAllLegalMatches();
        while(matchsToEliminate.Count > 0) {
            foreach (Solution solution in matchsToEliminate) {
                foreach (GameObject pieceToChange in solution.getSolutionPieces()) {
                    int pieceIndex = Random.Range(0, pieces.Length);
                    Vector2 tempPosition = new Vector2((int)pieceToChange.transform.position.x, (int)pieceToChange.transform.position.y);
                    GameObject newPiece = Instantiate(pieces[pieceIndex], tempPosition, Quaternion.identity);
                    newPiece.transform.parent = this.transform;
                    newPiece.GetComponent<Piece>().column = pieceToChange.GetComponent<Piece>().column;
                    newPiece.GetComponent<Piece>().row = pieceToChange.GetComponent<Piece>().row;
                    allPieces[newPiece.GetComponent<Piece>().column, newPiece.GetComponent<Piece>().row] = newPiece;
                    Destroy(pieceToChange);
                }
            }
            matchsToEliminate = matchsFinder.lookingForAllLegalMatches();

        }
        setAllPiecesUnexplored();

    }
    public void setAllPiecesUnexplored () {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allPieces[i, j] != null) {
                allPieces[i, j].GetComponent<Piece>().isExplored = false;}}}
        }

    private IEnumerator destroyAllMatches (List<Solution> allSolutions) {
        List<GameObject> piecesToCreate = new List<GameObject>();
        GameObject tempPiece;
        bool flag;
        foreach (Solution solution in allSolutions) {
            
            if (solution.getSize() > 3) {
                Vector2 tempPosition = new Vector2(solution.getSolutionPieces()[0].GetComponent<Piece>().column, solution.getSolutionPieces()[0].GetComponent<Piece>().row);
                tempPiece = Instantiate(tntPiece, tempPosition, Quaternion.identity);
                if (solution.getColor() == "Red") {
                    tempPiece.GetComponent<SpriteRenderer>().color = Color.red;
                    tempPiece.GetComponent<Piece>().color = "Red";}
                if (solution.getColor() == "Blue") {tempPiece.GetComponent<SpriteRenderer>().color = Color.blue;
                    tempPiece.GetComponent<Piece>().color = "Blue";}
                if (solution.getColor() == "Yellow") {tempPiece.GetComponent<SpriteRenderer>().color = Color.yellow;
                    tempPiece.GetComponent<Piece>().color = "Yellow";}
                if (solution.getColor() == "Green") {tempPiece.GetComponent<SpriteRenderer>().color = Color.green;
                    tempPiece.GetComponent<Piece>().color = "Green";}
                tempPiece.GetComponent<Piece>().column = solution.getSolutionPieces()[0].GetComponent<Piece>().column;
                tempPiece.GetComponent<Piece>().row = solution.getSolutionPieces()[0].GetComponent<Piece>().row;
                piecesToCreate.Add(tempPiece);
            }
        }
        for (int i = 0; i < 5; i++) {
            
            flag = true;
            if (allSolutions != null){
                foreach (Solution solution in allSolutions) {
                    if ( i < solution.getSolutionPieces().Count) {
                        if (solution.getSolutionPieces()[i] != null) {
                            flag = false;
                            solution.getSolutionPieces()[i].GetComponent<Piece>().destroyObject();
                        }
                    }
                }
            }
            if (flag == true) {break;}
            yield return  new WaitForSeconds(0.05f);
        }
        foreach (GameObject newTnt in piecesToCreate) {
            allPieces[newTnt.GetComponent<Piece>().column, newTnt.GetComponent<Piece>().row] = newTnt;
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

    public bool isAMatchOnBoard(List<Solution> allSolutions) {
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
                    Vector2 tempPosition = new Vector2(i, 15);
                    int pieceIndex = Random.Range(0, pieces.Length);
                    GameObject piece = Instantiate(pieces[pieceIndex], tempPosition, Quaternion.identity);
                    allPieces[i, j] = piece;
                    piece.transform.parent = this.transform;
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
        yield return new WaitForSeconds(0.35f);
        List<Solution> allSolutions = new List<Solution>();
        allSolutions = matchsFinder.lookingForAllLegalMatches();
        if (allSolutions != null) {        
            if (isAMatchOnBoard(allSolutions)) {
            
                StartCoroutine(destroyAllMatches(allSolutions));
                yield return new WaitForSeconds(0.25f);
            
            }
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

    public IEnumerator checkMoveCoroutine() {
        yield return new WaitForSeconds(0.20f);
        
        if (!matchsFinder.isLegalMatchInBoard()) {
            secondPiece.GetComponent<Piece>().column = chosenPiece.GetComponent<Piece>().column;
            secondPiece.GetComponent<Piece>().row = chosenPiece.GetComponent<Piece>().row;
            chosenPiece.GetComponent<Piece>().column = chosenPiece.GetComponent<Piece>().previousColumn;
            chosenPiece.GetComponent<Piece>().row = chosenPiece.GetComponent<Piece>().previousRow;
        }
        else {
            secondPiece.GetComponent<Piece>().previousColumn = secondPiece.GetComponent<Piece>().column;
            secondPiece.GetComponent<Piece>().previousRow = secondPiece.GetComponent<Piece>().row;
            chosenPiece.GetComponent<Piece>().previousColumn = chosenPiece.GetComponent<Piece>().column;
            chosenPiece.GetComponent<Piece>().previousRow = chosenPiece.GetComponent<Piece>().row;
            StartCoroutine(destroyAllMatches(matchsFinder.lookingForAllLegalMatches()));
            
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



    




  
     
  
    

