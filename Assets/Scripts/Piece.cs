using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TntPiece;

public enum piecesState {enter, wait, move, falling, destroy};

public class Piece : MonoBehaviour
{   [Header ("Board variables")]
    public piecesState currentState = piecesState.enter;
    public int column;
    public int row;
    public bool wrongPosition;
    public int previousColumn;
    public int previousRow;
    private int[][] tntTargets = new int[8][];
    public string type;
    public string color;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;
    private Vector2 tempPosition;
    private int targetColumn;
    private int targetRow;
    private float swipeAngle = 0;
    public float swipeResist = 1f;
    public bool isExplored = false;
    public GameObject destroyEffect;
    void Start() {
      targetColumn = (int) transform.position.x;
      targetRow = (int) transform.position.y;
      board = FindObjectOfType<Board>();  
      previousColumn = column;
      previousRow = row;
      wrongPosition = false;
      tntTargets[0] = new int[] {-1, -1}; tntTargets[1] = new int[] {-1, 0}; tntTargets[2] = new int[] {-1, 1};
      tntTargets[3] = new int[] {0, 1}; tntTargets[4] = new int[] {1, 1}; tntTargets[5] = new int [] {1, 0};
      tntTargets[6] = new int[] {1, -1}; tntTargets[7] = new int[] {0, -1};
    }
    public void destroyObject() {
        
        GameObject pieceToDestroy = gameObject;
        if (gameObject.GetComponent<Piece>().type == "SpecialTnt") {
            //TntPiece.specialPower();
            Instantiate(destroyEffect, gameObject.transform.position, Quaternion.identity);
            board.allPieces[gameObject.GetComponent<Piece>().column, gameObject.GetComponent<Piece>().row] = null;
            //Destroy(gameObject);
            foreach(int[] target in tntTargets) {
                if(column + target[0] >= 0 & row + target[1] >= 0 & column + target[0] < 9 & row + target[1] < 9) {
                    Instantiate(destroyEffect, gameObject.transform.position, Quaternion.identity);
                    board.allPieces[column + target[0], row + target[1]] = null;
                    Destroy(pieceToDestroy);

                }
            }
            Debug.Log("BOOM");

        }
        if (gameObject.GetComponent<Piece>().type == "Regular") { 
            Instantiate(destroyEffect, gameObject.transform.position, Quaternion.identity);
            board.allPieces[gameObject.GetComponent<Piece>().column, gameObject.GetComponent<Piece>().row] = null;
            Destroy(gameObject);
        }
        
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
                transform.position = Vector2.Lerp(transform.position, tempPosition, 22.0f*Time.deltaTime);
                if (board.allPieces[column, row] != this.gameObject) {
                    board.allPieces[column, row] = this.gameObject;
                }
            }
            if (Mathf.Abs(targetRow - transform.position.y) > 0.05f) {
                tempPosition = new Vector2(transform.position.x, targetRow);
                transform.position = Vector2.Lerp(transform.position, tempPosition, 22.0f*Time.deltaTime);
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

    private float calculateAngle() {
        if (Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > swipeResist) {
            swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x)*180/Mathf.PI;
            return swipeAngle;
        }
        else {
            return 0;
        }
    }



    





















}
