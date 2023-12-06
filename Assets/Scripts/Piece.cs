using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum piecesState {enter, wait, move, falling, destroy};

public class Piece : MonoBehaviour
{
    [Header ("Board variables")]
    public Board board;
    public piecesState currentState = piecesState.enter;
    public int column;
    public int row;
    public Vector2 tempPosition;
    public int previousColumn;
    public int previousRow;
    public bool wrongPosition;
    public string type;
    public string color;
    public bool isSpecialPiece = false;
    private Vector2 touchDownPosition;
    private Vector2 touchUpPosition;
    public bool isExplored = false;
    public GameObject destroyEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();  
        board = FindObjectOfType<Board>();  
        previousColumn = column;
        previousRow = row;
        wrongPosition = false;
        // tntTargets[0] = new int[] {-1, -1}; tntTargets[1] = new int[] {-1, 0}; tntTargets[2] = new int[] {-1, 1};
        // tntTargets[3] = new int[] {0, 1}; tntTargets[4] = new int[] {1, 1}; tntTargets[5] = new int [] {1, 0};
        // tntTargets[6] = new int[] {1, -1}; tntTargets[7] = new int[] {0, -1};
        
    }

    // Update is called once per frame
    void Update()
    {   if (wrongPosition == false & (Mathf.Abs(column - transform.position.x) > 0.05f || Mathf.Abs(row - transform.position.y) > 0.05f)) {
            wrongPosition = true;
        }
        else if (wrongPosition == true & (Mathf.Abs(column - transform.position.x) > 0.05f || Mathf.Abs(row - transform.position.y) > 0.05f)) {
            if (Mathf.Abs(column - transform.position.x) > 0.05f) {
                tempPosition = new Vector2(column, transform.position.y);
                transform.position = Vector2.Lerp(transform.position, tempPosition, board.pieceSpeed*Time.deltaTime);
                if (board.allPieces[column, row] != this.gameObject) {
                    board.allPieces[column, row] = this.gameObject;
                }
            }
            if (Mathf.Abs(row - transform.position.y) > 0.05f) {
                tempPosition = new Vector2(transform.position.x, row);
                transform.position = Vector2.Lerp(transform.position, tempPosition, board.pieceSpeed*Time.deltaTime);
                if (board.allPieces[column, row] != this.gameObject) {
                    board.allPieces[column, row] = this.gameObject;
                }
            }
        }
        else if (wrongPosition == true & Mathf.Abs(column - transform.position.x) < 0.05f & Mathf.Abs(row - transform.position.y) < 0.05f) {
            tempPosition = new Vector2(column, row);
            transform.position = tempPosition;
            wrongPosition = false;
        }
        
    }

    public void destroyObject() 
    {   
        if (gameObject.GetComponent<Piece>().type == "Regular") { 
            Instantiate(destroyEffect, gameObject.transform.position, Quaternion.identity);
            board.allPieces[gameObject.GetComponent<Piece>().column, gameObject.GetComponent<Piece>().row] = null;
            Destroy(gameObject);
        }
        else if (gameObject.GetComponent<Piece>().type == "SpecialTnt") { 
            Instantiate(destroyEffect, gameObject.transform.position, Quaternion.identity);
            board.allPieces[gameObject.GetComponent<Piece>().column, gameObject.GetComponent<Piece>().row] = null;
            Destroy(gameObject);
        }
        
    

    }

    private void OnMouseDown()
    {   if (board.currentState == boardStates.gameInputAllowed) {
            touchDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            board.chosenPiece = gameObject;
        }   
    }

    private void OnMouseUp()
    {   if (board.currentState == boardStates.gameInputAllowed) {
            touchUpPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            board.GetComponent<Board>().movePieces(touchDownPosition, touchUpPosition);
        }
    }
}

