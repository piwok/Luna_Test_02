using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{   
    [Header("Board variables")]
    public string color;
    public string type;
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    private int targetColumn;
    private int targetRow;
    private Vector2 targetPosition;
    public bool isMatched;
    public bool isSeedOfSpecialPiece;
    private Board board;
    private MatchFinder matchFinder;
    public GameObject secondPiece;

    [Header("Swipe variables")]
    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;
    public float swipeAngle;
    public float swipeThreshold;
    // Start is called before the first frame update
    void Start() {
        swipeThreshold = 1f;
        isMatched = false;
        isSeedOfSpecialPiece = false;
        board = FindObjectOfType<Board>();
        matchFinder = FindObjectOfType<MatchFinder>();
    }
    //this is for testing and debugging
    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(1)) {
            int pieceToDestroyIndex = -1;
            if(color == "Green") {
                pieceToDestroyIndex = 8;
            }
            else if(color == "Yellow") {
                pieceToDestroyIndex = 9;
            }
            else if(color == "Red") {
                pieceToDestroyIndex = 10;
            }
            else if(color == "Black") {
                pieceToDestroyIndex = 11;
            }
            Vector2 pieceToDestroyPosition = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            board.allPieces[column, row] = Instantiate(board.piecesPrefabs[pieceToDestroyIndex], pieceToDestroyPosition, Quaternion.identity);
            board.allPieces[column, row].GetComponent<Piece>().column = column;
            board.allPieces[column, row].GetComponent<Piece>().row = row;
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {   //Code for the movement of the piece, the piece always goes to the position of the column and row variables
        targetColumn = column;
        targetRow = row;
        if (Mathf.Abs(targetColumn - transform.position.x) > 0.1f) {
            //Move towards the target
            targetPosition = new Vector2(targetColumn, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, targetPosition, 17f*Time.deltaTime);
            if(board.allPieces[column, row] != this.gameObject) {
                board.allPieces[column, row] = this.gameObject;
            }
        }
        else {
            //Set the target position on X axis
            targetPosition = new Vector2(targetColumn, transform.position.y);
            transform.position = targetPosition;
            board.allPieces[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targetRow - transform.position.y) > 0.1f) {
            //Move towards the target
            targetPosition = new Vector2(transform.position.x, targetRow);
            transform.position = Vector2.Lerp(transform.position, targetPosition,17f*Time.deltaTime);
            if(board.allPieces[column, row] != this.gameObject) {
                board.allPieces[column, row] = this.gameObject;
            }
        }
        else {
            //Set the target position on Y axis
            targetPosition = new Vector2(transform.position.x, targetRow);
            transform.position = targetPosition;
            board.allPieces[column, row] = this.gameObject;
        }
    }
    private void OnMouseDown() {
        if(board.currentState == gameState.move) {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private void OnMouseUp() {
        if(board.currentState == gameState.move) {
            lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            calculateAngle();
        }
    }
    private void calculateAngle() {
        if(Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > swipeThreshold ||
        Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > swipeThreshold) {
            swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x) * 180/Mathf.PI;
            
            board.currentState = gameState.wait;
            board.currentPiece = this.gameObject;
            movePieces();
        }
        else {
            board.currentState = gameState.wait;
            board.currentPiece = this.gameObject;
            StartCoroutine(checkClickCoroutine());
            
            
        }
    }
    private void movePieces() {
        //Code to the movement of one piece with the adjacent piece from a swipe
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1) {
            //Right swipe
            secondPiece = board.allPieces[column + 1, row];
            previousColumn = column;
            previousRow = row;
            secondPiece.GetComponent<Piece>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) {
            //Up swipe
            secondPiece = board.allPieces[column, row + 1];
            previousColumn = column;
            previousRow = row;
            secondPiece.GetComponent<Piece>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) {
            //Left swipe
            secondPiece = board.allPieces[column - 1, row];
            previousColumn = column;
            previousRow = row;
            secondPiece.GetComponent<Piece>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) {
            //Down swipe
            secondPiece = board.allPieces[column, row - 1];
            previousColumn = column;
            previousRow = row;
            secondPiece.GetComponent<Piece>().row += 1;
            row -= 1;
        }
        StartCoroutine(checkMoveCoroutine());
    }
    public IEnumerator checkMoveCoroutine() {
        board.isCheckMoveCoroutineDone = false;
        // if(type == "SpecialColorBomb") {
        //     Solution newSolution = new Solution(matchFinder.getColorBombSolution(color)); //TENGO QUE HACER LAS SOLUCIONES ESPECIALES DEL MOVIMIENTO
        //     matchFinder.matchAllPieceOfSameColor(secondPiece.GetComponent<Piece>().color);
        //     isMatched = true;
            
        //     }
        // else if(secondPiece.GetComponent<Piece>().type == "SpecialColorBomb") {
        //     secondPiece.GetComponent<Piece>().type = "Powerless";
        //     matchFinder.matchAllPieceOfSameColor(color);
        //     

        // }
        if(type == "SpecialVerticalRocket") {
            type = "Powerless";
            Solution newSolution = matchFinder.getColumnSolution(this.gameObject);
            newSolution.addSolutionPieceToSolution(this.gameObject);
            matchFinder.currentSolutions.Add(newSolution);
            }
        else if(secondPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
            secondPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getColumnSolution(secondPiece);
            newSolution.addSolutionPieceToSolution(secondPiece);
            matchFinder.currentSolutions.Add(newSolution);
            

        }
        if(type == "SpecialHorizontalRocket") {
            type = "Powerless";
            Solution newSolution = matchFinder.getRowSolution(this.gameObject);
            newSolution.addSolutionPieceToSolution(this.gameObject);
            matchFinder.currentSolutions.Add(newSolution);
            
            
            }
        else if(secondPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
            secondPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getRowSolution(secondPiece);
            newSolution.addSolutionPieceToSolution(secondPiece);
            matchFinder.currentSolutions.Add(newSolution);
            

        }
        if(type == "SpecialTnt") {
            type = "Powerless";
            Solution newSolution = matchFinder.getTntSolution(this.gameObject);
            newSolution.addSolutionPieceToSolution(this.gameObject);
            matchFinder.currentSolutions.Add(newSolution);
            
            
        }
        else if(secondPiece.GetComponent<Piece>().type == "SpecialTnt") {
            secondPiece.GetComponent<Piece>().type = "Powerless";
            Solution newSolution = matchFinder.getTntSolution(secondPiece);
            newSolution.addSolutionPieceToSolution(secondPiece);
            matchFinder.currentSolutions.Add(newSolution);
            

        }
        
        matchFinder.findAllLegalSolutions();
        yield return new WaitForSeconds(0.25f);
        if (secondPiece != null) {
            if(!isMatched && !secondPiece.GetComponent<Piece>().isMatched && type == "Regular" && secondPiece.GetComponent<Piece>().type == "Regular") {
                secondPiece.GetComponent<Piece>().column = column;
                secondPiece.GetComponent<Piece>().row = row;
                column = previousColumn;
                row = previousRow;
                yield return new WaitForSeconds(0.25f);
                board.currentPiece = null;
                board.currentState = gameState.move;
            }
            else {
                board.destroyAllSolutions();
            }
            secondPiece = null;
        }
        board.isCheckMoveCoroutineDone = true;
    }
    public IEnumerator checkClickCoroutine() {
        
        
        board.isCheckClickCoroutineDone = false;
        // if(type == "SpecialColorBomb") {
        //     matchFinder.matchAllPieceOfSameColor(secondPiece.GetComponent<Piece>().color);
        //     isMatched = true;
        // }
        if(type == "SpecialVerticalRocket") {
            Solution newSolution = matchFinder.getColumnSolution(this.gameObject);
            matchFinder.currentSolutions.Add(newSolution);
            isMatched = true;
            
            }
        if(type == "SpecialHorizontalRocket") {
            Solution newSolution = matchFinder.getRowSolution(this.gameObject);
            matchFinder.currentSolutions.Add(newSolution);
            isMatched = true;
        }
        if(type == "SpecialTnt") {
            Solution newSolution = matchFinder.getTntSolution(this.gameObject);
            matchFinder.currentSolutions.Add(newSolution);
            isMatched = true;
        }
        board.destroyAllSolutions();
        matchFinder.currentSolutions.Clear();
        yield return new WaitForSeconds(0.25f);
        board.currentState = gameState.move;
        board.isCheckClickCoroutineDone = true;
    }
    public void makeSpecialHorizontalRocket() {
        int pieceToDestroyIndex = -1;
        if(color == "Green") {
            pieceToDestroyIndex = 4;
        }
        else if(color == "Yellow") {
            pieceToDestroyIndex = 5;
        }
        else if(color == "Red") {
            pieceToDestroyIndex = 6;
        }
        else if(color == "Black") {
            pieceToDestroyIndex = 7;
        }
        Vector2 pieceToDestroyPosition = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        board.allPieces[column, row] = Instantiate(board.piecesPrefabs[pieceToDestroyIndex], pieceToDestroyPosition, Quaternion.identity);
        board.allPieces[column, row].GetComponent<Piece>().column = column;
        board.allPieces[column, row].GetComponent<Piece>().row = row;
        Destroy(this.gameObject);
    }
    public void makeSpecialVerticalRocket() {
        int pieceToDestroyIndex = -1;
        if(color == "Green") {
            pieceToDestroyIndex = 8;
        }
        else if(color == "Yellow") {
            pieceToDestroyIndex = 9;
        }
        else if(color == "Red") {
            pieceToDestroyIndex = 10;
        }
        else if(color == "Black") {
            pieceToDestroyIndex = 11;
        }
        Vector2 pieceToDestroyPosition = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        board.allPieces[column, row] = Instantiate(board.piecesPrefabs[pieceToDestroyIndex], pieceToDestroyPosition, Quaternion.identity);
        board.allPieces[column, row].GetComponent<Piece>().column = column;
        board.allPieces[column, row].GetComponent<Piece>().row = row;
        Destroy(this.gameObject);
    }
    public void createSpecialPiece(string typeOfSpecialPiece, int column, int row, string color) {
        int pieceIndex = -1;
        Vector2 positionNewPiece = new Vector2(column, row);
        if(typeOfSpecialPiece == "colorBombPiece") {
            pieceIndex = 16;
            board.allPieces[column, row] = Instantiate(board.piecesPrefabs[pieceIndex], positionNewPiece, Quaternion.identity);
            board.allPieces[column, row].GetComponent<Piece>().column = column;
            board.allPieces[column, row].GetComponent<Piece>().row = row;
            board.allPieces[column, row].GetComponent<Piece>().previousColumn = column;
            board.allPieces[column, row].GetComponent<Piece>().previousRow = row;
        }
        else if(typeOfSpecialPiece == "tntPiece") {
            if(color == "Green") {
                pieceIndex = 12;
            }
            else if(color == "Yellow") {
                pieceIndex = 13;
            }
            else if(color == "Red") {
                pieceIndex = 14;
            }
            else if(color == "Black") {
                pieceIndex = 15;
            }
            board.allPieces[column, row] = Instantiate(board.piecesPrefabs[pieceIndex], positionNewPiece, Quaternion.identity);
            board.allPieces[column, row].GetComponent<Piece>().column = column;
            board.allPieces[column, row].GetComponent<Piece>().row = row;
            board.allPieces[column, row].GetComponent<Piece>().previousColumn = column;
            board.allPieces[column, row].GetComponent<Piece>().previousRow = row;
        }
        else if(typeOfSpecialPiece == "verticalRocketPiece") {
            if(color == "Green") {
                pieceIndex = 8;
            }
            else if(color == "Yellow") {
                pieceIndex = 9;
            }
            else if(color == "Red") {
                pieceIndex = 10;
            }
            else if(color == "Black") {
                pieceIndex = 11;
            }
            board.allPieces[column, row] = Instantiate(board.piecesPrefabs[pieceIndex], positionNewPiece, Quaternion.identity);
            board.allPieces[column, row].GetComponent<Piece>().column = column;
            board.allPieces[column, row].GetComponent<Piece>().row = row;
            board.allPieces[column, row].GetComponent<Piece>().previousColumn = column;
            board.allPieces[column, row].GetComponent<Piece>().previousRow = row;
        }
        else if(typeOfSpecialPiece == "horizontalRocketPiece") {
            if(color == "Green") {
                pieceIndex = 12;
            }
            else if(color == "Yellow") {
                pieceIndex = 13;
            }
            else if(color == "Red") {
                pieceIndex = 14;
            }
            else if(color == "Black") {
                pieceIndex = 15;
            }
            board.allPieces[column, row] = Instantiate(board.piecesPrefabs[pieceIndex], positionNewPiece, Quaternion.identity);
            board.allPieces[column, row].GetComponent<Piece>().column = column;
            board.allPieces[column, row].GetComponent<Piece>().row = row;
            board.allPieces[column, row].GetComponent<Piece>().previousColumn = column;
            board.allPieces[column, row].GetComponent<Piece>().previousRow = row;
        }
    }
}
