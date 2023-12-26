using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{   
    public int column;
    public int row;
    private int targetColumn;
    private int targetRow;
    private Vector2 targetPosition;
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
            //Set the target position on X axis
            targetPosition = new Vector2(transform.position.x, targetRow);
            transform.position = targetPosition;
            board.allPieces[column, row] = this.gameObject;
        }

        
    }
    private void OnMouseDown() {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(firstTouchPosition);
    }
    private void OnMouseUp() {
        lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(lastTouchPosition);
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
}
