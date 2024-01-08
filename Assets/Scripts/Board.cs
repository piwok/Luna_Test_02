using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum gameState {
    wait,
    move
}
public class Board : MonoBehaviour {
    public gameState currentState;
    public int width;
    public int height;
    private MatchFinder matchFinder;
    public GameObject[] tilesPrefabs;
    public GameObject[] piecesPrefabs;
    private GameObject[,] allTiles;
    public GameObject[,] allPieces;
    public GameObject currentPiece;
    public GameObject secondPiece;
    public GameObject regularDestroyEffect;
    public GameObject regularCreationEffect;
    public int offsetNewPieces;
    private string[] colorBombShapes;
    private string[] tntShapes;
    private string[] verticalRocketShapes;
    private string[] horizontalRocketShapes;
    public bool isCheckMoveCoroutineDone;
    public bool isCollapseCollumnsDone;
    public bool isFillBoardCoroutineDone;
    public bool isCheckClickCoroutineDone;
    public bool isDestroyAllSolutionsDone;
    public bool isDestroySolutionDone;
    public bool isDestroyColorBombSolutionDone;
    // Start is called before the first frame update
    void Start() {
        matchFinder = FindObjectOfType<MatchFinder>();
        currentState = gameState.move;
        colorBombShapes = new string[] {"fiveLineShape0", "fiveLineShape1"};
        tntShapes = new string[] {"fiveTShape0", "fiveTShape1", "fiveTShape2", "fiveTShape3", "fiveLShape0", "fiveLShape1", "fiveLShape2", "fiveLShape3"};
        verticalRocketShapes = new string[] {"fourLineShape0"};
        horizontalRocketShapes = new string[] {"fourLineShape1"};
        isCheckMoveCoroutineDone = true;
        isCheckClickCoroutineDone = true;
        isCollapseCollumnsDone = true;
        isFillBoardCoroutineDone = true;
        isDestroyAllSolutionsDone = true;
        isDestroySolutionDone = true;
        isDestroyColorBombSolutionDone = true;
        allTiles = new GameObject[width, height];
        allPieces = new GameObject[width, height];
        setUp();
    }
    void Update() {
        if(isCheckMoveCoroutineDone == false || isCollapseCollumnsDone == false || isFillBoardCoroutineDone == false) {
            currentState = gameState.wait;
        }
        else {
            currentState = gameState.move;
        }
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
                int newRegularPieceTypeIndex = Random.Range(0, 4); //only the firts four prefabs are regular pieces
                int maxIterations = 0;
                while(isMatchAt(i, j, piecesPrefabs[newRegularPieceTypeIndex]) && maxIterations < 100) {
                    newRegularPieceTypeIndex = Random.Range(0, 4);
                    maxIterations++;
                }
                Vector2 positionForNewPiece = new Vector2(i, j + offsetNewPieces);
                GameObject newRegularPiece = Instantiate(piecesPrefabs[newRegularPieceTypeIndex], positionForNewPiece, Quaternion.identity);
                newRegularPiece.GetComponent<Piece>().column = i;
                newRegularPiece.GetComponent<Piece>().row = j;
                newRegularPiece.GetComponent<Piece>().previousColumn = i;
                newRegularPiece.GetComponent<Piece>().previousRow = j;
                newRegularPiece.transform.parent = this.transform;
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
        if(column > 0 && row > 0) {
            if(allPieces[column, row - 1].GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color &&
            exploringPiece.GetComponent<Piece>().color == allPieces[column - 1, row].GetComponent<Piece>().color &&
            exploringPiece.GetComponent<Piece>().color == allPieces[column - 1, row - 1].GetComponent<Piece>().color) {
                return true;
            }
        }
        
        return false;
    }
    private IEnumerator destroySolution(Solution solution) {
        isDestroySolutionDone = false;
        Solution newSolution;
        foreach(GameObject solutionPiece in solution.solutionPieces.ToList()) {
            //if(solutionPiece != null) {
                //new solutions for the matched special pieces
                if(solutionPiece.GetComponent<Piece>().type == "SpecialTnt") {
                    //The piece become powerless
                    solutionPiece.GetComponent<Piece>().type = "Powerless";
                    newSolution = matchFinder.getTntSolution(solutionPiece);
                    matchFinder.newCurrentSolutions.Add(newSolution);
                }
                else if(solutionPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                    //The piece become powerless
                    solutionPiece.GetComponent<Piece>().type = "Powerless";
                    newSolution = matchFinder.getColumnSolution(solutionPiece);
                    matchFinder.newCurrentSolutions.Add(newSolution);
                }
                else if(solutionPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                    //The piece become powerless
                    solutionPiece.GetComponent<Piece>().type = "Powerless";
                    newSolution = matchFinder.getRowSolution(solutionPiece);
                    matchFinder.newCurrentSolutions.Add(newSolution);
                }
                else if(solutionPiece.GetComponent<Piece>().type == "SpecialColorBomb") {
                    //The piece become powerless
                    solutionPiece.GetComponent<Piece>().type = "Powerless";
                    newSolution = matchFinder.getColorBombRandomSolution();
                    matchFinder.newCurrentSolutions.Add(newSolution);
                }
                GameObject destroyEffectParticle = Instantiate(regularDestroyEffect, solutionPiece.transform.position, Quaternion.identity);
                Destroy(destroyEffectParticle, 1.0f);
                allPieces[solutionPiece.GetComponent<Piece>().column, solutionPiece.GetComponent<Piece>().row] = null;
                Destroy(solutionPiece);
            //}
        }
        yield return new WaitForSeconds(0.15f);
        isDestroySolutionDone = true;
    }
    private IEnumerator destroyColorBombSolution(Solution solution) {
        isDestroyColorBombSolutionDone = false;
        Solution newSolution;
        foreach(GameObject solutionPiece in solution.solutionPieces.ToList()) {
            if(solutionPiece != null) {
                solutionPiece.GetComponent<SpriteRenderer>().color = Color.grey;
                yield return new WaitForSeconds(0.05f);
            }
        }
        foreach(GameObject solutionPiece in solution.solutionPieces.ToList()) {
            if(solutionPiece != null) {
                //new solutions for the matched special pieces
                if(solutionPiece.GetComponent<Piece>().type == "SpecialTnt") {
                    //The piece become powerless
                    solutionPiece.GetComponent<Piece>().type = "Powerless";
                    newSolution = matchFinder.getTntSolution(solutionPiece);
                    matchFinder.newCurrentSolutions.Add(newSolution);
                }
                else if(solutionPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                    //The piece become powerless
                    solutionPiece.GetComponent<Piece>().type = "Powerless";
                    newSolution = matchFinder.getColumnSolution(solutionPiece);
                    matchFinder.newCurrentSolutions.Add(newSolution);
                }
                else if(solutionPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                    //The piece become powerless
                    solutionPiece.GetComponent<Piece>().type = "Powerless";
                    newSolution = matchFinder.getRowSolution(solutionPiece);
                    matchFinder.newCurrentSolutions.Add(newSolution);
                }
                GameObject destroyEffectParticle = Instantiate(regularDestroyEffect, solutionPiece.transform.position, Quaternion.identity);
                Destroy(destroyEffectParticle, 1.0f);
                allPieces[solutionPiece.GetComponent<Piece>().column, solutionPiece.GetComponent<Piece>().row] = null;
                Destroy(solutionPiece);
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.15f);
        isDestroyColorBombSolutionDone = true;
    }
    public IEnumerator destroyAllSolutions() {
        isDestroyAllSolutionsDone = false;
        if(matchFinder.currentSolutions.Count > 0) {
            //createListOfSpecialPiecesToCreate();
        }
        while(matchFinder.currentSolutions.Count > 0) {
            foreach(Solution solution in matchFinder.currentSolutions.ToList()) {
                if(solution.type != "ColorBombMatches") {
                    yield return StartCoroutine(destroySolution(solution));
                } 
                else {
                    yield return StartCoroutine(destroyColorBombSolution(solution));
                }
            }
            matchFinder.currentSolutions.Clear();
            if(matchFinder.newCurrentSolutions.Count > 0) {
                matchFinder.currentSolutions = new List<Solution>(matchFinder.newCurrentSolutions);
                matchFinder.newCurrentSolutions.Clear();
            }
        }
        //createAllSpecialPieces();
        //Aqui hay que crear las fichas que haya en la lista currentSpecialPiecesToCreate
        StartCoroutine(collapseColumnsCoroutine());
        isDestroyAllSolutionsDone = true;
    }
    private IEnumerator collapseColumnsCoroutine() {
        yield return new WaitUntil(() => isDestroyAllSolutionsDone == true);
        isCollapseCollumnsDone = false;
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
        isCollapseCollumnsDone = true;
    }
    private void refillBoard() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(allPieces[i, j] == null) {
                    Vector2 newPiecePosition = new Vector2(i, j + offsetNewPieces);
                    int newPieceindex = Random.Range(0, 4);
                    GameObject newPiece = Instantiate(piecesPrefabs[newPieceindex], newPiecePosition, Quaternion.identity);
                    allPieces[i, j] = newPiece;
                    newPiece.GetComponent<Piece>().wrongPosition = true;
                    newPiece.GetComponent<Piece>().column = i;
                    newPiece.GetComponent<Piece>().row = j;
                    newPiece.GetComponent<Piece>().previousColumn = i;
                    newPiece.GetComponent<Piece>().previousRow = j;
                    newPiece.transform.parent = this.transform;
                }
            }
        }
    }
    private IEnumerator fillBoardCoroutine() {
        yield return new WaitUntil(() => isCollapseCollumnsDone == true);
        isFillBoardCoroutineDone = false;
        refillBoard();
        yield return new WaitUntil(() => areAllPiecesInPosition());
        matchFinder.findAllLegalSolutions();
        
        while(matchFinder.currentSolutions.Count > 0) {
            yield return new WaitUntil(() => isDestroyAllSolutionsDone);
            yield return StartCoroutine(destroyAllSolutions());
            matchFinder.currentSolutions.Clear();
            matchFinder.findAllLegalSolutions();
        }
        matchFinder.currentSolutions.Clear();
        currentPiece = null;
        yield return new WaitForSeconds(0.1f);
        
        //currentState = gameState.move;
        isFillBoardCoroutineDone = true;
    }
    public void checkMove() {
        StartCoroutine(checkMoveCoroutine());
    }
    public IEnumerator checkMoveCoroutine() {
        isCheckMoveCoroutineDone = false;
        if(secondPiece != null) {
            if(currentPiece.GetComponent<Piece>().type == "Regular" && secondPiece.GetComponent<Piece>().type == "Regular") {
                matchFinder.findAllLegalSolutions();
                yield return new WaitForSeconds(0.25f);
                if(!currentPiece.GetComponent<Piece>().isMatched && !secondPiece.GetComponent<Piece>().isMatched) {
                secondPiece.GetComponent<Piece>().column = currentPiece.GetComponent<Piece>().column;
                secondPiece.GetComponent<Piece>().row = currentPiece.GetComponent<Piece>().row;
                currentPiece.GetComponent<Piece>().column = currentPiece.GetComponent<Piece>().previousColumn;
                currentPiece.GetComponent<Piece>().row = currentPiece.GetComponent<Piece>().previousRow;
                yield return new WaitForSeconds(0.25f);
                currentPiece = null;
                currentState = gameState.move;
            }
            else {
                secondPiece = null;
                StartCoroutine(destroyAllSolutions());
            }
            secondPiece = null;
            }
            else if((currentPiece.GetComponent<Piece>().type != "Regular" && secondPiece.GetComponent<Piece>().type == "Regular") ||
             (currentPiece.GetComponent<Piece>().type == "Regular" && secondPiece.GetComponent<Piece>().type != "Regular")) {
                matchFinder.findAllLegalSolutions();
                yield return new WaitForSeconds(0.25f);
                if(currentPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                    currentPiece.GetComponent<Piece>().type = "Powerless";
                    Solution newSolution = matchFinder.getColumnSolution(currentPiece);
                    newSolution.addSolutionPieceToSolution(currentPiece);
                    matchFinder.currentSolutions.Add(newSolution);
                }
                else if(secondPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                    secondPiece.GetComponent<Piece>().type = "Powerless";
                    Solution newSolution = matchFinder.getColumnSolution(secondPiece);
                    newSolution.removeSolutionPieceFromSolution(currentPiece);
                    newSolution.addSolutionPieceToSolution(secondPiece);
                    matchFinder.currentSolutions.Add(newSolution);
                }
                if(currentPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                    currentPiece.GetComponent<Piece>().type = "Powerless";
                    Solution newSolution = matchFinder.getRowSolution(currentPiece);
                    newSolution.addSolutionPieceToSolution(currentPiece);
                    matchFinder.currentSolutions.Add(newSolution);
                }
                else if(secondPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                    secondPiece.GetComponent<Piece>().type = "Powerless";
                    Solution newSolution = matchFinder.getRowSolution(secondPiece);
                    newSolution.removeSolutionPieceFromSolution(currentPiece);
                    newSolution.addSolutionPieceToSolution(secondPiece);
                    matchFinder.currentSolutions.Add(newSolution);
                }
                if(currentPiece.GetComponent<Piece>().type == "SpecialTnt") {
                    currentPiece.GetComponent<Piece>().type = "Powerless";
                    Solution newSolution = matchFinder.getTntSolution(currentPiece);
                    newSolution.addSolutionPieceToSolution(currentPiece);
                    matchFinder.currentSolutions.Add(newSolution);
                }
                else if(secondPiece.GetComponent<Piece>().type == "SpecialTnt") {
                    secondPiece.GetComponent<Piece>().type = "Powerless";
                    Solution newSolution = matchFinder.getTntSolution(secondPiece);
                    newSolution.addSolutionPieceToSolution(secondPiece);
                    matchFinder.currentSolutions.Add(newSolution);
                }
                if(currentPiece.GetComponent<Piece>().type == "SpecialColorBomb") {
                    currentPiece.GetComponent<Piece>().type = "Powerless";
                    Solution newSolution = matchFinder.getColorBombSolution(secondPiece.GetComponent<Piece>().color);
                    newSolution.addSolutionPieceToSolution(currentPiece);
                    matchFinder.currentSolutions.Add(newSolution);
                }
                else if(secondPiece.GetComponent<Piece>().type == "SpecialColorBomb") {
                    secondPiece.GetComponent<Piece>().type = "Powerless";
                    Solution newSolution = matchFinder.getColorBombSolution(currentPiece.GetComponent<Piece>().color);
                    newSolution.addSolutionPieceToSolution(secondPiece);
                    matchFinder.currentSolutions.Add(newSolution);
                }
                StartCoroutine(destroyAllSolutions());
            }
            else if(currentPiece.GetComponent<Piece>().type != "Regular" && secondPiece.GetComponent<Piece>().type != "Regular") {
                matchFinder.findAllLegalSolutions();
                yield return new WaitForSeconds(0.25f);
            }
        }
        isCheckMoveCoroutineDone = true;
    }
    public void checkClick() {
        StartCoroutine(checkClickCoroutine());
    }
    public IEnumerator checkClickCoroutine() {
        isCheckClickCoroutineDone = false;
        if(currentPiece.GetComponent<Piece>().type == "SpecialColorBomb") {
            currentPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getColorBombRandomSolution();
            newSolution.addSolutionPieceToSolution(currentPiece);
            matchFinder.currentSolutions.Add(newSolution);
            //currentPiece.GetComponent<Piece>().isMatched = true;
        }
        else if(currentPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
            currentPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getColumnSolution(currentPiece);
            matchFinder.currentSolutions.Add(newSolution);
            //currentPiece.GetComponent<Piece>().isMatched = true;
            
            }
        else if(currentPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
            currentPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getRowSolution(currentPiece);
            matchFinder.currentSolutions.Add(newSolution);
            //currentPiece.GetComponent<Piece>().isMatched = true;
        }
        else if(currentPiece.GetComponent<Piece>().type == "SpecialTnt") {
            currentPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getTntSolution(currentPiece);
            matchFinder.currentSolutions.Add(newSolution);
            //currentPiece.GetComponent<Piece>().isMatched = true;
        }
        
        yield return StartCoroutine(destroyAllSolutions());
        yield return new WaitForSeconds(0.25f);
        matchFinder.currentSolutions.Clear();
        currentState = gameState.move;
        isCheckClickCoroutineDone = true;
    }
    private void createListOfSpecialPiecesToCreate() {
        SpecialPieceToCreate newSpecialPiece;
        if(matchFinder.currentSolutions.Count > 0) {
            foreach(Solution solution in matchFinder.currentSolutions.ToList()) {
                if(solution.type == "regularMatch") {
                    if(solution.shape == "fiveLineShape0" || solution.shape == "fiveLineShape1") {
                        //create a color bomb special piece
                        newSpecialPiece = new SpecialPieceToCreate(solution.newSpecialPieceColumn, solution.newSpecialPieceRow, solution.color, solution.shape);
                        matchFinder.currentSpecialPiecesToCreate.Add(newSpecialPiece);
                    }
                    else if(solution.shape == "fiveTShape0" || solution.shape == "fiveTShape1" || solution.shape == "fiveTShape2" || solution.shape == "fiveTShape3") {
                        //create a tnt special piece
                        newSpecialPiece = new SpecialPieceToCreate(solution.newSpecialPieceColumn, solution.newSpecialPieceRow, solution.color, solution.shape);
                        matchFinder.currentSpecialPiecesToCreate.Add(newSpecialPiece);
                    }
                    else if(solution.shape == "fiveLShape0" || solution.shape == "fiveLShape1" || solution.shape == "fiveLShape2" || solution.shape == "fiveLShape3") {
                        //create a tnt special piece
                        newSpecialPiece = new SpecialPieceToCreate(solution.newSpecialPieceColumn, solution.newSpecialPieceRow, solution.color, solution.shape);
                        matchFinder.currentSpecialPiecesToCreate.Add(newSpecialPiece);
                    }
                    else if(solution.shape == "fourLineShape0") {
                        //create a vertical rocket special piece
                        newSpecialPiece = new SpecialPieceToCreate(solution.newSpecialPieceColumn, solution.newSpecialPieceRow, solution.color, solution.shape);
                        matchFinder.currentSpecialPiecesToCreate.Add(newSpecialPiece);
                    }
                    else if(solution.shape == "fourLineShape1") {
                        //create a horizontal rocket special piece
                        newSpecialPiece = new SpecialPieceToCreate(solution.newSpecialPieceColumn, solution.newSpecialPieceRow, solution.color, solution.shape);
                        matchFinder.currentSpecialPiecesToCreate.Add(newSpecialPiece);
                    }
                    else if(solution.shape == "fourSquareShape0") {
                        //create a dove special piece
                        newSpecialPiece = new SpecialPieceToCreate(solution.newSpecialPieceColumn, solution.newSpecialPieceRow, solution.color, solution.shape);
                        matchFinder.currentSpecialPiecesToCreate.Add(newSpecialPiece);
                    }
                }
            }
        }
    }
    private void createAllSpecialPieces() {
        int newSpecialPieceColorIndex = -1;
        Vector2 newSpecialPiecePosition;
        GameObject newSpecialPieceCreated;
        foreach(SpecialPieceToCreate newSpecialPiece in matchFinder.currentSpecialPiecesToCreate.ToList()) {
            newSpecialPiecePosition = new Vector2(newSpecialPiece.column, newSpecialPiece.row);
            if(newSpecialPiece.matchShape == "fiveLineShape0" || newSpecialPiece.matchShape == "fiveLineShape1") {
                newSpecialPieceColorIndex = 16;
            }
            else if(newSpecialPiece.matchShape == "fourLineShape0") {
                
                if(newSpecialPiece.color == "Green") {
                    newSpecialPieceColorIndex = 8;
                }
                else if(newSpecialPiece.color == "Yellow") {
                    newSpecialPieceColorIndex = 9;
                }
                else if(newSpecialPiece.color == "Red") {
                    newSpecialPieceColorIndex = 10;
                }
                else if(newSpecialPiece.color == "Black") {
                    newSpecialPieceColorIndex = 11;
                }
            }
            else if(newSpecialPiece.matchShape == "fourLineShape1") {
                
                if(newSpecialPiece.color == "Green") {
                    newSpecialPieceColorIndex = 4;
                }
                else if(newSpecialPiece.color == "Yellow") {
                    newSpecialPieceColorIndex = 5;
                }
                else if(newSpecialPiece.color == "Red") {
                    newSpecialPieceColorIndex = 6;
                }
                else if(newSpecialPiece.color == "Black") {
                    newSpecialPieceColorIndex = 7;
                }
            }
            else if(newSpecialPiece.matchShape == "fiveTShape0" || newSpecialPiece.matchShape == "fiveTShape1" || newSpecialPiece.matchShape == "fiveTShape2" || newSpecialPiece.matchShape == "fiveTShape3") {
                
                if(newSpecialPiece.color == "Green") {
                    newSpecialPieceColorIndex = 12;
                }
                else if(newSpecialPiece.color == "Yellow") {
                    newSpecialPieceColorIndex = 13;
                }
                else if(newSpecialPiece.color == "Red") {
                    newSpecialPieceColorIndex = 14;
                }
                else if(newSpecialPiece.color == "Black") {
                    newSpecialPieceColorIndex = 15;
                }
            }
            else if(newSpecialPiece.matchShape == "fiveLShape0" || newSpecialPiece.matchShape == "fiveLShape1" || newSpecialPiece.matchShape == "fiveLShape2" || newSpecialPiece.matchShape == "fiveLShape3") {
                
                if(newSpecialPiece.color == "Green") {
                    newSpecialPieceColorIndex = 12;
                }
                else if(newSpecialPiece.color == "Yellow") {
                    newSpecialPieceColorIndex = 13;
                }
                else if(newSpecialPiece.color == "Red") {
                    newSpecialPieceColorIndex = 14;
                }
                else if(newSpecialPiece.color == "Black") {
                    newSpecialPieceColorIndex = 15;
                }
            }
            if(newSpecialPieceColorIndex != -1) {
                GameObject creationEffectParticle = Instantiate(regularCreationEffect,  newSpecialPiecePosition, Quaternion.identity);
                Destroy(creationEffectParticle, 1.0f);
                newSpecialPieceCreated = Instantiate(piecesPrefabs[newSpecialPieceColorIndex], newSpecialPiecePosition, Quaternion.identity);
                newSpecialPieceCreated.GetComponent<Piece>().column = newSpecialPiece.column;
                newSpecialPieceCreated.GetComponent<Piece>().row = newSpecialPiece.row;
                newSpecialPieceCreated.GetComponent<Piece>().previousColumn = newSpecialPiece.column;
                newSpecialPieceCreated.GetComponent<Piece>().previousRow = newSpecialPiece.row;
                newSpecialPieceCreated.transform.parent = this.transform;
                allPieces[newSpecialPiece.column, newSpecialPiece.row] = newSpecialPieceCreated;
                matchFinder.currentSpecialPiecesToCreate.Clear();
            }
        }
    }
    public bool areAllPiecesInPosition() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(allPieces[i, j] == null || allPieces[i, j].GetComponent<Piece>().wrongPosition == true) {
                    return false;
                }
            }
        }
        return true;
    }
}

