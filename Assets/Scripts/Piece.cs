using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum pieceState {enter, wait, move, falling, destroy};

public class Piece : MonoBehaviour
{   [Header ("Board variables")]
    public pieceState currentState = pieceState.enter;
    public int column;
    public int row;
    public bool wrongPosition = false;
    public int previousColumn;
    public int previousRow;
    public GameObject otherDot;
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
    public bool isMatched = false;
    //-----PRUEBA BEL
    // private float degreesPerSecond = 20f;
    // private float scalingFactor = 1.1f;

    // private Vector3 scale;
    //-- FIN PRUEBA BEL

    // Start is called before the first frame update
    void Start() {
      targetColumn = (int) transform.position.x;
      targetRow = (int) transform.position.y;
      board = FindObjectOfType<Board>();  
      previousColumn = column;
      previousRow = row;
      //scale = transform.localScale;
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
                if (board.allDots[column, row] != this.gameObject) {
                    board.allDots[column, row] = this.gameObject;
                }
            }
            if (Mathf.Abs(targetRow - transform.position.y) > 0.05f) {
                tempPosition = new Vector2(transform.position.x, targetRow);
                transform.position = Vector2.Lerp(transform.position, tempPosition, 0.05f);
                if (board.allDots[column, row] != this.gameObject) {
                    board.allDots[column, row] = this.gameObject;
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
    {   
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        board.chosenDot = this.gameObject;        
    }

    private void OnMouseUp()
    {
        lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        calculateAngle();
        
        
        
    }

    void calculateAngle()

    {   
        if (Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > swipeResist) 
    {
        swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x)*180/Mathf.PI;
        movePieces();
    }
    }

    void movePieces() {   
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width) {
            //Right swipe
            if (board.allDots[column + 1, row] != null) {
                otherDot = board.allDots[column +1, row];
                otherDot.GetComponent<Piece>().column -= 1;
                board.allDots[otherDot.GetComponent<Piece>().column, otherDot.GetComponent<Piece>().row] = otherDot;
                column += 1;
                board.allDots[column, row] = this.gameObject;}}
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height) {
            //Up swipe
            if (board.allDots[column, row + 1] != null) {
                otherDot = board.allDots[column, row + 1];
                otherDot.GetComponent<Piece>().row -= 1;
                board.allDots[otherDot.GetComponent<Piece>().column, otherDot.GetComponent<Piece>().row] = otherDot;
                row += 1;
                board.allDots[column, row] = this.gameObject;}}
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) {
            //Left swipe
            if (board.allDots[column - 1, row] != null) {
                otherDot = board.allDots[column - 1, row];
                otherDot.GetComponent<Piece>().column += 1;
                board.allDots[otherDot.GetComponent<Piece>().column, otherDot.GetComponent<Piece>().row] = otherDot;
                column -= 1;
                board.allDots[column, row] = this.gameObject;}}
        else if (swipeAngle >= -135 && swipeAngle < -45 && row > 0) {
            //Down swipe
            if (board.allDots[column, row - 1] != null) {
                otherDot = board.allDots[column, row - 1];
                otherDot.GetComponent<Piece>().row += 1;
                board.allDots[otherDot.GetComponent<Piece>().column, otherDot.GetComponent<Piece>().row] = otherDot;
                row -= 1;
                board.allDots[column, row] = this.gameObject;}}
        StartCoroutine(checkMoveCoroutine());
    }

    public IEnumerator checkMoveCoroutine() {
        yield return new WaitForSeconds(0.35f);
        if (!board.isAMatchAt(column, row, this.gameObject) & !board.isAMatchAt(otherDot.GetComponent<Piece>().column, otherDot.GetComponent<Piece>().row, otherDot)) {
            otherDot.GetComponent<Piece>().column = column;
            otherDot.GetComponent<Piece>().row = row;
            column = previousColumn;
            row = previousRow;
        }
        else {
            board.lookingForAllMatches();
            board.destroyAllMatches();
            
        }
        otherDot = null;
    }

    





















}
