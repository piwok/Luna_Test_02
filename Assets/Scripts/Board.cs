using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum gameState {
    wait,
    move
}

public class Board : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
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
        return false;
    }
    private IEnumerator destroySolution(Solution solution) {
        isDestroySolutionDone = false;
        Solution newSolution;
        foreach(GameObject solutionPiece in solution.solutionPieces.ToList()) {
            if(solutionPiece != null) {
                //creation of special pieces when is necesary
                //TO DO?

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
                // else if(solutionPiece.GetComponent<Piece>().type == "SpecialColorBomb") {
                    //  //The piece become powerless
                    // solutionPiece.GetComponent<Piece>().type = "Powerless";
                    // solution = matchFinder.getColorBombSolution(color);
                    // newSolutions.Add(newSolution);
                //    }
                
                GameObject destroyEffectParticle = Instantiate(regularDestroyEffect, solutionPiece.transform.position, Quaternion.identity);
                Destroy(destroyEffectParticle, 1.0f);
                allPieces[solutionPiece.GetComponent<Piece>().column, solutionPiece.GetComponent<Piece>().row] = null;
                Destroy(solutionPiece);
            }
        }
        yield return new WaitForSeconds(0.15f);
        isDestroySolutionDone = true;
    }   
    public IEnumerator destroyAllSolutions() {
        isDestroyAllSolutionsDone = false;
        while(matchFinder.currentSolutions.Count > 0) {
            foreach(Solution solution in matchFinder.currentSolutions.ToList()) {
                yield return StartCoroutine(destroySolution(solution));
            }
            matchFinder.currentSolutions.Clear();
            if(matchFinder.newCurrentSolutions.Count > 0) {
                matchFinder.currentSolutions = new List<Solution>(matchFinder.newCurrentSolutions);
                matchFinder.newCurrentSolutions.Clear();
            }
        }
        //Aqui hay que crear las fichas que haya en la lista currentSpecialPiecesToCreate
        StartCoroutine(collapseColumnsCoroutine());
        isDestroyAllSolutionsDone = false;
    }
    private IEnumerator collapseColumnsCoroutine() {
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
                    newPiece.GetComponent<Piece>().column = i;
                    newPiece.GetComponent<Piece>().row = j;
                    newPiece.transform.parent = this.transform;
                }
            }
        }
    }
    private IEnumerator fillBoardCoroutine() {
        isFillBoardCoroutineDone = false;
        refillBoard();
        matchFinder.findAllLegalSolutions();
        yield return new WaitForSeconds(0.1f);
        
        while(matchFinder.currentSolutions.Count > 0) {
            yield return new WaitForSeconds(0.1f);
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
        // if(type == "SpecialColorBomb") {
        //     matchFinder.matchAllPieceOfSameColor(secondPiece.GetComponent<Piece>().color);
        //     isMatched = true;
        // }
        if(currentPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
            currentPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getColumnSolution(currentPiece);
            matchFinder.currentSolutions.Add(newSolution);
            currentPiece.GetComponent<Piece>().isMatched = true;
            
            }
        if(currentPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
            currentPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getRowSolution(currentPiece);
            matchFinder.currentSolutions.Add(newSolution);
            currentPiece.GetComponent<Piece>().isMatched = true;
        }
        if(currentPiece.GetComponent<Piece>().type == "SpecialTnt") {
            currentPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getTntSolution(currentPiece);
            matchFinder.currentSolutions.Add(newSolution);
            currentPiece.GetComponent<Piece>().isMatched = true;
        }
        StartCoroutine(destroyAllSolutions());
        matchFinder.currentSolutions.Clear();
        yield return new WaitForSeconds(0.25f);
        currentState = gameState.move;
        isCheckClickCoroutineDone = true;
    }
}
