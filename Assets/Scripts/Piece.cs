using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{   
    public string color;
    public string type;
    public int column;
    public int row;
    private int targetColumn;
    private int targetRow;
    private Vector2 targetPosition;
    public bool isMatched;
    private Board board;
    private GameObject secondPiece;
    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;
    public float swipeAngle;
    private GameObject otherPiece;
    // Start is called before the first frame update
    void Start()
    {
        targetColumn = (int) transform.position.x;
        targetRow = (int) transform.position.y;
        column = targetColumn;
        row = targetRow;
        isMatched = false;
        board = FindObjectOfType<Board>();
        
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
        }
        else {
            //Set the target position on Y axis
            targetPosition = new Vector2(transform.position.x, targetRow);
            transform.position = targetPosition;
            board.allPieces[column, row] = this.gameObject;
        }
        //Code for destruction of matched pieces
        findAllLegalMatches();
        if (isMatched == true) {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(1f, 1f, 1f, 0.4f);


        }

        
    }
    private void OnMouseDown() {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }
    private void OnMouseUp() {
        lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        calculateAngle();
    }
    private void calculateAngle() {
        swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x) * 180/Mathf.PI;
        movePieces();
    }
    private void movePieces() {
        //Code to the movement of one piece with the adjacent piece from a swipe
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1) {
            //Right swipe
            otherPiece = board.allPieces[column + 1, row];
            otherPiece.GetComponent<Piece>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) {
            //Up swipe
            otherPiece = board.allPieces[column, row + 1];
            otherPiece.GetComponent<Piece>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) {
            //Left swipe
            otherPiece = board.allPieces[column - 1, row];
            otherPiece.GetComponent<Piece>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) {
            //Down swipe
            otherPiece = board.allPieces[column, row - 1];
            otherPiece.GetComponent<Piece>().row += 1;
            row -= 1;
        }
        
    }
    public void findAllLegalMatches() {
        if (column > 0 && column < board.width - 1) {
            GameObject leftPiece1 = board.allPieces[column - 1, row];
            GameObject rightPiece1 = board.allPieces[column + 1, row];
            if (leftPiece1.GetComponent<Piece>().color == color && rightPiece1.GetComponent<Piece>().color == color) {
                leftPiece1.GetComponent<Piece>().isMatched = true;
                rightPiece1.GetComponent<Piece>().isMatched = true;
                isMatched = true;

            } 
        }
        if (row > 0 && row < board.height - 1) {
            GameObject downPiece1 = board.allPieces[column, row - 1];
            GameObject upPiece1 = board.allPieces[column, row + 1];
            if (downPiece1.GetComponent<Piece>().color == color && upPiece1.GetComponent<Piece>().color == color) {
                downPiece1.GetComponent<Piece>().isMatched = true;
                upPiece1.GetComponent<Piece>().isMatched = true;
                isMatched = true;

            } 
        }
    }

}
