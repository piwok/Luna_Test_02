using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum piecesState {enter, wait, move, falling, destroy};

public class Piece : MonoBehaviour
{   [Header ("Board variables")]
    public piecesState currentState = piecesState.enter;
    public int column;
    public int row;
    public bool wrongPosition;
    public int previousColumn;
    public int previousRow;
    public string type;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;
    private Vector2 tempPosition;
    private int targetColumn;
    private int targetRow;
    private float swipeAngle = 0;
    public float swipeResist = 1f;
    public bool isExplored = false;
    void Start() {
      targetColumn = (int) transform.position.x;
      targetRow = (int) transform.position.y;
      board = FindObjectOfType<Board>();  
      previousColumn = column;
      previousRow = row;
      wrongPosition = false;
    }

    // Update is called once per frame
    void Update() {
        targetColumn = column;
        targetRow = row;
        if (wrongPosition == false & (Mathf.Abs(targetColumn - transform.position.x) > 0.05f || Mathf.Abs(targetRow - transform.position.y) > 0.05f)) {
            wrongPosition = true;
        }
        else if (wrongPosition == true & (Mathf.Abs(targetColumn - transform.position.x) > 0.05f || Mathf.Abs(targetRow - transform.position.y) > 0.05f)) {
            if (Mathf.Abs(targetColumn - transform.position.x) > 0.05f) {
                tempPosition = new Vector2(targetColumn, transform.position.y);
                transform.position = Vector2.Lerp(transform.position, tempPosition, 0.05f);
                if (board.allPieces[column, row] != this.gameObject) {
                    board.allPieces[column, row] = this.gameObject;
                }
            }
            if (Mathf.Abs(targetRow - transform.position.y) > 0.05f) {
                tempPosition = new Vector2(transform.position.x, targetRow);
                transform.position = Vector2.Lerp(transform.position, tempPosition, 0.05f);
                if (board.allPieces[column, row] != this.gameObject) {
                    board.allPieces[column, row] = this.gameObject;
                }
            }
        }
        else if (wrongPosition == true & Mathf.Abs(targetColumn - transform.position.x) < 0.05f & Mathf.Abs(targetRow - transform.position.y) < 0.05f) {
            tempPosition = new Vector2(targetColumn, targetRow);
            transform.position = tempPosition;
            
            wrongPosition = false;
            
        }
    }

    private void OnMouseDown()
    {   if (board.currentState == boardStates.gameInputAllowed) {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            board.chosenPiece = this.gameObject;
        }   
    }

    private void OnMouseUp()
    {   if (board.currentState == boardStates.gameInputAllowed) {
            lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            board.movePieces(calculateAngle());
        }
    }

    float calculateAngle() {
        if (Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > swipeResist) {
            swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x)*180/Mathf.PI;
            return swipeAngle;
        }
        else {
            return 0;
        }
    }



    





















}
