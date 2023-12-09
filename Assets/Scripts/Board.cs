using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum boardStates {gameInputAllowed, movingPieces, gameInputNotAllowed};

public class Board : MonoBehaviour
{
    public boardStates currentState;
    public int width;
    public int height;
    private MatchsFinder matchsFinder;
    public GameObject[] pieces;
    public GameObject[,] allPieces;
    public Tile[,] allTiles;
    public GameObject chosenPiece;
    public GameObject secondPiece;
    Vector2 positionForNewPiece;
    public float swipeResist = 1f;
    public float pieceSpeed = 17f;
    
    // Start is called before the first frame update
    void Start()
    {
        allPieces = new GameObject[width, height];
        allTiles = new Tile[width, height];
        matchsFinder = FindObjectOfType<MatchsFinder>();
        List<Solution> matchsToEliminate = new List<Solution>();
        //initial board generation with matches
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                positionForNewPiece = new Vector2(i, j);
                int pieceIndex = Random.Range(0, 4); //only are random regular pieces (0, 1, 2, 3 index in the array pieces)
                GameObject newPiece = Instantiate(pieces[pieceIndex], positionForNewPiece, Quaternion.identity);
                allPieces[i, j] = newPiece;
                newPiece.transform.parent = this.transform;
                newPiece.GetComponent<Piece>().column = i;
                newPiece.GetComponent<Piece>().row = j;
                newPiece.GetComponent<Piece>().previousColumn = i;
                newPiece.GetComponent<Piece>().previousRow = j;
                //type and color are part of the prefabs parameters
                Tile newTile = new Tile(i, j);
                allTiles[i, j] = newTile;
            }
        }
        //loop changing colors of pieces in a match until there is not matchs
        matchsToEliminate = matchsFinder.lookingForAllLegalMatches();
        while(matchsToEliminate.Count > 0) {
            foreach (Solution solution in matchsToEliminate) {
                foreach (GameObject pieceToChange in solution.getSolutionPieces()) {
                    int pieceIndex = Random.Range(0, 4);
                    positionForNewPiece = new Vector2((int)pieceToChange.transform.position.x, (int)pieceToChange.transform.position.y);
                    GameObject newPiece = Instantiate(pieces[pieceIndex], positionForNewPiece, Quaternion.identity);
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

    public void setAllPiecesUnexplored () 
    {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allPieces[i, j] != null) {
                allPieces[i, j].GetComponent<Piece>().isExplored = false;
                }
            }
        }
    }
        


    // Update is called once per frame
    void Update()
    {
        
    }

    private float calculateAngle(Vector2 touchDownPosition, Vector2 touchUpPosition) {
        if (Mathf.Abs(touchUpPosition.y - touchDownPosition.y) > swipeResist || Mathf.Abs(touchUpPosition.x - touchDownPosition.x) > swipeResist) {
            float swipeAngle = Mathf.Atan2(touchUpPosition.y - touchDownPosition.y, touchUpPosition.x - touchDownPosition.x)*180/Mathf.PI;
            return swipeAngle;
        }
        else {
            return 0;
        }
    }

    private bool areAllPiecesInRightPlace() 
    {   for (int i = 0; i < width; i++) {
            for (int j = 0; j <height; j++) {
                if (allPieces[i, j] != null && allPieces[i, j].GetComponent<Piece>().wrongPosition == true ) {
                    return false;
                }
            }
        }
        return true;
    }
    
    public void movePieces(Vector2 touchDownPosition, Vector2 touchUpPosition) {
        if(Mathf.Abs(touchUpPosition.x - touchDownPosition.x) > swipeResist || Mathf.Abs(touchUpPosition.y - touchDownPosition.y) > swipeResist ) {
            float swipeAngle = calculateAngle(touchDownPosition, touchUpPosition);
            if (swipeAngle > -45 & swipeAngle <= 45 && chosenPiece.GetComponent<Piece>().column < width - 1) {
                //Right swipe
                if (allPieces[chosenPiece.GetComponent<Piece>().column + 1, chosenPiece.GetComponent<Piece>().row] != null) {
                    secondPiece = allPieces[chosenPiece.GetComponent<Piece>().column +1, chosenPiece.GetComponent<Piece>().row];
                    chosenPiece.GetComponent<Piece>().column += 1;
                    secondPiece.GetComponent<Piece>().column -= 1;
                    allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row] = chosenPiece;
                    allPieces[secondPiece.GetComponent<Piece>().column, secondPiece.GetComponent<Piece>().row] = secondPiece;
                    chosenPiece.GetComponent<Piece>().wrongPosition = true;
                    secondPiece.GetComponent<Piece>().wrongPosition = true;
                }
            }
            else if (swipeAngle > 45 & swipeAngle <= 135 && chosenPiece.GetComponent<Piece>().row < height - 1) {
                //Up swipe
                if (allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row + 1] != null) {
                    secondPiece = allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row + 1];
                    chosenPiece.GetComponent<Piece>().row += 1;
                    secondPiece.GetComponent<Piece>().row -= 1;
                    allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row] = chosenPiece;
                    allPieces[secondPiece.GetComponent<Piece>().column, secondPiece.GetComponent<Piece>().row] = secondPiece;
                    chosenPiece.GetComponent<Piece>().wrongPosition = true;
                    secondPiece.GetComponent<Piece>().wrongPosition = true;
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
                    chosenPiece.GetComponent<Piece>().wrongPosition = true;
                    secondPiece.GetComponent<Piece>().wrongPosition = true;
                }
            }
            else if (swipeAngle >= -135 & swipeAngle < -45 && chosenPiece.GetComponent<Piece>().row > 0) {
                //Down swipe
                if (allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row - 1] != null) {
                    secondPiece = allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row - 1];
                    chosenPiece.GetComponent<Piece>().row -= 1;
                    secondPiece.GetComponent<Piece>().row += 1;
                    allPieces[chosenPiece.GetComponent<Piece>().column, chosenPiece.GetComponent<Piece>().row] = chosenPiece;
                    allPieces[secondPiece.GetComponent<Piece>().column, secondPiece.GetComponent<Piece>().row] = secondPiece;
                    chosenPiece.GetComponent<Piece>().wrongPosition = true;
                    secondPiece.GetComponent<Piece>().wrongPosition = true;
                }
            }
            if (secondPiece != null) {
                if (chosenPiece.GetComponent<Piece>().type == "Regular" && secondPiece.GetComponent<Piece>().type == "Regular") {
                    StartCoroutine(checkMoveCoroutine());
                }
                else {
                currentState = boardStates.gameInputAllowed;
                }
            }
            else {
                currentState = boardStates.gameInputAllowed;
            }
        }
        else {
            if (chosenPiece.GetComponent<Piece>().type == "SpecialTnt" || chosenPiece.GetComponent<Piece>().type == "SpecialVerticalRocket"
            || chosenPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket" || chosenPiece.GetComponent<Piece>().type == "SpecialColorBomb") {
                chosenPiece.GetComponent<Piece>().destroyObject();
                StartCoroutine(colapseAllColumns());
            }
            else if (chosenPiece.GetComponent<Piece>().type == "Regular") {
                currentState = boardStates.gameInputAllowed;
            }
        }
    }

    public IEnumerator checkMoveCoroutine() {
        
        List<Solution> allSolutions = new List<Solution>(matchsFinder.lookingForAllLegalMatches());
        
        
        yield return new WaitUntil(() => areAllPiecesInRightPlace() == true);
        
        if (allSolutions.Count == 0) {
            secondPiece.GetComponent<Piece>().column = chosenPiece.GetComponent<Piece>().column;
            secondPiece.GetComponent<Piece>().row = chosenPiece.GetComponent<Piece>().row;
            chosenPiece.GetComponent<Piece>().column = chosenPiece.GetComponent<Piece>().previousColumn;
            chosenPiece.GetComponent<Piece>().row = chosenPiece.GetComponent<Piece>().previousRow;
            chosenPiece.GetComponent<Piece>().wrongPosition = true;
            secondPiece.GetComponent<Piece>().wrongPosition = true;
            yield return new WaitUntil(() => areAllPiecesInRightPlace() == true);
            currentState = boardStates.gameInputAllowed;
            
        }
        else {
            secondPiece.GetComponent<Piece>().previousColumn = secondPiece.GetComponent<Piece>().column;
            secondPiece.GetComponent<Piece>().previousRow = secondPiece.GetComponent<Piece>().row;
            chosenPiece.GetComponent<Piece>().previousColumn = chosenPiece.GetComponent<Piece>().column;
            chosenPiece.GetComponent<Piece>().previousRow = chosenPiece.GetComponent<Piece>().row;
            StartCoroutine(destroyAllMatches(allSolutions));
            
        }
        
        chosenPiece = null;
        secondPiece = null;
        
    }

    private IEnumerator destroyAllMatches (List<Solution> allSolutions) {
        List<SpecialPieceToCreate> specialPiecesToCreate = new List<SpecialPieceToCreate>();
        int pieceIndex = 0;
        foreach (Solution solution in allSolutions) {
            if (solution.getSolutionPieces().Count > 3) {
                if (solution.getShape() == "fiveLineShape0" || solution.getShape() == "fiveLineShape1") {
                    pieceIndex = 20;}
                else if (solution.getShape() == "fiveTShape0" || solution.getShape() == "fiveTShape1" || solution.getShape() == "fiveTShape2" || solution.getShape() == "fiveTShape3") {
                    if (solution.getColor() == "Red") {pieceIndex = 6;}
                    else if (solution.getColor() == "Black") {pieceIndex = 7;}
                    else if (solution.getColor() == "Green") {pieceIndex = 5;}
                    else if (solution.getColor() == "Yellow") {pieceIndex = 4;}
                }
                else if (solution.getShape() == "fiveLShape0" || solution.getShape() == "fiveLShape1" || solution.getShape() == "fiveLShape2" || solution.getShape() == "fiveLShape3") {
                    if (solution.getColor() == "Red") {pieceIndex = 6;}
                    else if (solution.getColor() == "Black") {pieceIndex = 7;}
                    else if (solution.getColor() == "Green") {pieceIndex = 5;}
                    else if (solution.getColor() == "Yellow") {pieceIndex = 4;}
                }
                else if (solution.getShape() == "fourLineShape0") {
                    if (solution.getColor() == "Red") {pieceIndex = 14;}
                    else if (solution.getColor() == "Black") {pieceIndex = 15;}
                    else if (solution.getColor() == "Green") {pieceIndex = 13;}
                    else if (solution.getColor() == "Yellow") {pieceIndex = 12;}
                }
                else if (solution.getShape() == "fourLineShape1") {
                    if (solution.getColor() == "Red") {pieceIndex = 18;}
                    else if (solution.getColor() == "Black") {pieceIndex = 19;}
                    else if (solution.getColor() == "Green") {pieceIndex = 17;}
                    else if (solution.getColor() == "Yellow") {pieceIndex = 16;}
                }
                else if (solution.getShape() == "fourSquareShape0") {
                    if (solution.getColor() == "Red") {pieceIndex = 10;}
                    else if (solution.getColor() == "Black") {pieceIndex = 11;}
                    else if (solution.getColor() == "Green") {pieceIndex = 9;}
                    else if (solution.getColor() == "Yellow") {pieceIndex = 8;}
                }
                specialPiecesToCreate.Add(new SpecialPieceToCreate(allTiles[solution.getSolutionPieces()[0].GetComponent<Piece>().column, solution.getSolutionPieces()[0].GetComponent<Piece>().row],
                solution.getShape(), pieceIndex));   
            }
        }
        
        foreach (Solution solution in allSolutions) {
            for (int i = 0; i < solution.getSolutionPieces().Count; i++) {
                if (solution.getSolutionPieces()[i] != null) {
                    solution.getSolutionPieces()[i].GetComponent<Piece>().destroyObject();
                }
            }
        }
        foreach (SpecialPieceToCreate newSpecialPiece in specialPiecesToCreate) {
            GameObject newPiece = Instantiate(pieces[newSpecialPiece.piecesIndex], new Vector2(newSpecialPiece.tile.column, newSpecialPiece.tile.row),Quaternion.identity);
            allPieces[newSpecialPiece.tile.column, newSpecialPiece.tile.row] = newPiece; 
            newPiece.GetComponent<Piece>().column = newSpecialPiece.tile.column;
            newPiece.GetComponent<Piece>().row = newSpecialPiece.tile.row;
            newPiece.GetComponent<Piece>().previousColumn = newSpecialPiece.tile.column;
            newPiece.GetComponent<Piece>().previousRow = newSpecialPiece.tile.row;
        }
        yield return new WaitUntil(() => areAllPiecesInRightPlace() == true);
        yield return new WaitForSeconds(0.3f);
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
                else if (blankSpacesCount > 0 && allPieces[i, j].GetComponent<Piece>().row - blankSpacesCount >= 0) {
                    allPieces[i, j].GetComponent<Piece>().row -= blankSpacesCount;
                    allPieces[i, j].GetComponent<Piece>().previousRow = allPieces[i, j].GetComponent<Piece>().row; 
                    allPieces[i, j - blankSpacesCount] = allPieces[i, j];
                    allPieces[i, j - blankSpacesCount].GetComponent<Piece>().wrongPosition = true;
                    allPieces[i ,j] = null;
                }
            }
        }
        yield return new WaitUntil(() => areAllPiecesInRightPlace() == true);
        StartCoroutine(fillBoardCoroutine());       
    }

    private IEnumerator fillBoardCoroutine() {
        refillBoard();
        yield return new WaitUntil(() => areAllPiecesInRightPlace() == true);
        List<Solution> allSolutions = new List<Solution>();
        allSolutions = matchsFinder.lookingForAllLegalMatches();
        if (allSolutions.Count > 0) {       
            StartCoroutine(destroyAllMatches(allSolutions));
            yield return new WaitForSeconds(0.25f);
            
            
        }
        else {
            currentState = boardStates.gameInputAllowed;}
    }

    private void refillBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allPieces[i, j] == null) {
                    Vector2 tempPosition = new Vector2(i, 15);
                    int pieceIndex = Random.Range(0, 4);
                    GameObject newPiece = Instantiate(pieces[pieceIndex], tempPosition, Quaternion.identity);
                    allPieces[i, j] = newPiece;
                    newPiece.transform.parent = this.transform;
                    newPiece.GetComponent<Piece>().column = i;
                    newPiece.GetComponent<Piece>().row = j;
                    newPiece.GetComponent<Piece>().previousColumn = newPiece.GetComponent<Piece>().column;
                    newPiece.GetComponent<Piece>().previousRow = newPiece.GetComponent<Piece>().row;
                    newPiece.GetComponent<Piece>().wrongPosition = true;
                }
            }
        }
    }
        
    
}
