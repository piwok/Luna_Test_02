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
        
    }
    //this is for testing and debugging
    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(1)) {
            int pieceToDestroyIndex = -1;
            if(color == "Green") {
                pieceToDestroyIndex = 12;
            }
            else if(color == "Yellow") {
                pieceToDestroyIndex = 13;
            }
            else if(color == "Red") {
                pieceToDestroyIndex = 14;
            }
            else if(color == "Black") {
                pieceToDestroyIndex = 15;
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
            StartCoroutine(board.checkClickCoroutine());
            
            
        }
    }
    private void movePieces() {
        
        //Code to the movement of one piece with the adjacent piece from a swipe
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1) {
            //Right swipe
            board.secondPiece = board.allPieces[column + 1, row];
            previousColumn = column;
            previousRow = row;
            board.secondPiece.GetComponent<Piece>().column -= 1;
            column += 1;
            board.allPieces[board.secondPiece.GetComponent<Piece>().column, board.secondPiece.GetComponent<Piece>().row] = board.secondPiece;
            board.allPieces[column, row] = this.gameObject;

            
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) {
            //Up swipe
            board.secondPiece = board.allPieces[column, row + 1];
            previousColumn = column;
            previousRow = row;
            board.secondPiece.GetComponent<Piece>().row -= 1;
            row += 1;
            board.allPieces[board.secondPiece.GetComponent<Piece>().column, board.secondPiece.GetComponent<Piece>().row] = board.secondPiece;
            board.allPieces[column, row] = this.gameObject;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) {
            //Left swipe
            board.secondPiece = board.allPieces[column - 1, row];
            previousColumn = column;
            previousRow = row;
            board.secondPiece.GetComponent<Piece>().column += 1;
            column -= 1;
            board.allPieces[board.secondPiece.GetComponent<Piece>().column, board.secondPiece.GetComponent<Piece>().row] = board.secondPiece;
            board.allPieces[column, row] = this.gameObject;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) {
            //Down swipe
            board.secondPiece = board.allPieces[column, row - 1];
            previousColumn = column;
            previousRow = row;
            board.secondPiece.GetComponent<Piece>().row += 1;
            row -= 1;
            board.allPieces[board.secondPiece.GetComponent<Piece>().column, board.secondPiece.GetComponent<Piece>().row] = board.secondPiece;
            board.allPieces[column, row] = this.gameObject;
        }
        board.checkMove();
    }
    
}
