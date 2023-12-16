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
    public bool isSpecialPiece;
    public bool isExplored;
    public bool isMatchToDestroy;
    public bool StartDestructionFlag;
    public float startingTimeToDestroy;
    public float destructionTimeStep;
    public int destructionSteps;
    public float timeLeftToDestruction;
    private Vector2 touchDownPosition;
    private Vector2 touchUpPosition;
    public GameObject destroyEffect;
    public List<int[]> allTntTargets;
    public List<int[]> halfRightSideTntTargets;
    public List<int[]> halfLeftSideTntTargets;
    
    // Start is called before the first frame update
    void Start()
    {   isSpecialPiece = false;
        isExplored = false;
        isMatchToDestroy = false;
        StartDestructionFlag = false;
        startingTimeToDestroy = 0;
        destructionTimeStep = 0.15f;
        destructionSteps = -1;
        board = FindObjectOfType<Board>();
        previousColumn = column;
        previousRow = row;
        wrongPosition = false;
    }

    // Update is called once per frame
    void Update()
    {   
        ///////////////////////////////////////////////////////////////////////////////////////
        //Code to move the piece to a the new position
        if (wrongPosition == false & (Mathf.Abs(column - transform.position.x) > 0.05f || Mathf.Abs(row - transform.position.y) > 0.05f)) {
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

        //////////////////////////////////////////////////////////////////////////////////////////
        //Code to destroy the piece
        if (isMatchToDestroy == true) {
            if (type == "Regular") {
                if (StartDestructionFlag == false) {
                    timeLeftToDestruction = destructionSteps * destructionTimeStep;
                    StartDestructionFlag = true;
                }
                else {
                    timeLeftToDestruction -= Time.deltaTime;
                    }
                if (timeLeftToDestruction < 0) {
                    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    board.allPieces[column, row] = null;
                    Destroy(gameObject);
                }
            } 
            else if (type == "SpecialTnt") {
                if (StartDestructionFlag == false) {
                    timeLeftToDestruction = destructionSteps * destructionTimeStep;
                    StartDestructionFlag = true;
                }
                else {
                    timeLeftToDestruction -= Time.deltaTime;
                    }
                if (timeLeftToDestruction < 0) {
                    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    board.allPieces[column, row] = null;
                    Destroy(gameObject);
                }
            }
            else if (type == "SpecialVerticalRocket") {
                if (StartDestructionFlag == false) {
                    timeLeftToDestruction = destructionSteps * destructionTimeStep;
                    StartDestructionFlag = true;
                }
                else {
                    timeLeftToDestruction -= Time.deltaTime;
                    }
                if (timeLeftToDestruction < 0) {
                    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    board.allPieces[column, row] = null;
                    Destroy(gameObject);
                }
            }
            else if (type == "SpecialHorizontalRocket") {
                if (StartDestructionFlag == false) {
                    timeLeftToDestruction = destructionSteps * destructionTimeStep;
                    StartDestructionFlag = true;
                }
                else {
                    timeLeftToDestruction -= Time.deltaTime;
                    }
                if (timeLeftToDestruction < 0) {
                    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    board.allPieces[column, row] = null;
                    Destroy(gameObject);
                }
            }
            else if (type == "SpecialColorBomb") {
                if (StartDestructionFlag == false) {
                    timeLeftToDestruction = destructionSteps * destructionTimeStep;
                    StartDestructionFlag = true;
                }
                else {
                    timeLeftToDestruction -= Time.deltaTime;
                    }
                if (timeLeftToDestruction < 0) {
                    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    board.allPieces[column, row] = null;
                    Destroy(gameObject);
                }
            }
        }
    }
       
    

    public Solution getPiecesToDestroy() 
    {   
        List<GameObject> newSolution = new List<GameObject>();
        if (type == "SpecialTnt") {
            int minLeftColumn;
            int minLeftRow;
            int maxRightColumn;
            int maxRightRow;
            if (column - 3 < 0) {
                minLeftColumn = 0;
            } else {
                minLeftColumn = column - 2;
            }
            if (row - 3 < 0) {
                minLeftRow = 0;
            } else {
                minLeftRow = row - 2;
            }
            if (column + 2 > board.width - 1) {
                maxRightColumn = board.width - 1;
            } else {
                maxRightColumn = column + 3;
            }
            if (row + 2 > board.height - 1) {
                maxRightRow = board.height - 1;
            } else {
                maxRightRow = row + 3;
            }
            // Debug.Log(minLeftRow);
            // Debug.Log(minLeftRow);
            // Debug.Log(maxRightColumn);
            // Debug.Log(maxRightRow);
            
            for (int i = minLeftColumn; i < maxRightColumn; i++) {
                Debug.Log(i);
                for (int j = minLeftRow; j < maxRightRow; j++) {
                    Debug.Log(j);
                    if (board.allPieces[i, j] != gameObject && board.allPieces[i, j] != null && 
                    board.allPieces[i, j].GetComponent<Piece>().isMatchToDestroy == false) { //dont include itself in the explosion
                    newSolution.Add(board. allPieces[i, j]);

                    }
                }
            }
            // foreach (int[] tntTarget in allTntTargets) {
            //     if (column + tntTarget[0] < board.width && column + tntTarget[0] >= 0 && row + tntTarget[1] < board.height && row + tntTarget[1] >= 0 &&
            //     board.allPieces[column + tntTarget[0], row + tntTarget[1]] != null && board.allPieces[column + tntTarget[0], row + tntTarget[1]].GetComponent<Piece>().isMatchToDestroy == false) {
            //         newSolution.Add(board.allPieces[column + tntTarget[0], row + tntTarget[1]]);
            //     }
            // }
            Debug.Log(newSolution.Count);
            return new Solution(newSolution, null, null, null);
        }
        else if (type == "SpecialVerticalRocket") { 
            for (int i = 1; i < board.height; i++) {
                if (row + i < board.height && board.allPieces[column, row + i] != null && board.allPieces[column, row + i].GetComponent<Piece>().isMatchToDestroy == false) {
                    newSolution.Add(board.allPieces[column, row + i]);
                }
                if (row - i >= 0 && board.allPieces[column, row - i] != null && board.allPieces[column, row - i].GetComponent<Piece>().isMatchToDestroy == false) {
                    newSolution.Add(board.allPieces[column, row - i]);
                }
            }
            return new Solution(newSolution, null, null, null); 
        }
        else if (type == "SpecialHorizontalRocket") { 
            for (int i = 1; i < board.width; i++) {
                if (column + i < board.width && board.allPieces[column + i, row] != null && board.allPieces[column + i, row].GetComponent<Piece>().isMatchToDestroy == false) {
                    newSolution.Add(board.allPieces[column + i, row]);
                }
                if (column - i >= 0 && board.allPieces[column - i, row] != null && board.allPieces[column - i, row].GetComponent<Piece>().isMatchToDestroy == false) {
                    newSolution.Add(board.allPieces[column - i, row]);
                }
            }
            return new Solution(newSolution, null, null, null);
        }
        else if (type == "SpecialColorBomb") { 
            
            string[] colors = new string[4] {"Red", "Yellow", "Green", "Black"};
            string colorToDestroy = colors[Random.Range(0, 4)];
            for (int i = 0; i < board.width; i++) {
                for (int j = 0; j < board.height; j++) {
                    if (board.allPieces[i, j] != null && board.allPieces[i, j].GetComponent<Piece>().color == colorToDestroy && board.allPieces[i, j].GetComponent<Piece>().isMatchToDestroy == false ) {
                        newSolution.Add(board.allPieces[i, j]);
                    }
                }
            }
            
            
            return new Solution(newSolution, null, null, null);
        }
        return null;
    }
    //TO DO
    public List<Solution> GetPiecesToDestroySpecialDoublePower (string type1, string type2) {
        List<Solution> piecesToDestroy = new List<Solution>();
        // big tnt explosion 4 pieces of radius
        if (type1 == "SpecialTnt" && type2 == "SpecialTnt") {

        }
        //double rocket vertical and horizontal
        else if ((type1 == "SpecialVerticalRocket" && type2 == "SpecialVerticalRocket") ||
                (type1 == "SpecialHorizontalRocket" && type2 == "SpecialHorizontalRocket") || 
                (type1 == "SpecialVerticalRocket" && type2 == "SpecialHorizontalRocket") || 
                (type1 == "SpecialHorizontalRocket" && type2 == "SpecialVerticalRocket")) {

        }
        //triple rocket vertical and horizontal
        else if ((type1 == "SpecialHorizontalRocket" || type1 == "SpecialVerticalRocket") && type2 == "SpecialTnt") {

        }
        //triple rocket vertical and horizontal
        else if (type1 == "SpecialTnt" && (type2 == "SpecialHorizontalRocket" || type2 == "SpecialVerticalRocket")) {

        }
        //vertical rocket in best place to destroy blockers
        else if ((type1 == "SpecialDove" && type2 == "SpecialVerticalRocket") ||
                 (type1 == "SpecialVerticalRocket" && type2 == "SpecialDove")) {

        }
        //horizontal rocket in best place to destroy blockers
        else if ((type1 == "SpecialDove" && type2 == "SpecialHorizontalRocket") ||
                 (type1 == "SpecialHorizontalRocket" && type2 == "SpecialDove")) {

        }
        //Tnt in best place to destroy blockers
        else if ((type1 == "SpecialDove" && type2 == "SpecialTnt") ||
                 (type1 == "SpecialTnt" && type2 == "SpecialDove")) {

        }
        //Big explosion radius all board
        else if (type1 == "SpecialColorBomb" && type2 == "SpecialColorBomb") {

        }
        //Random Tnt kegs in all board
        else if ((type1 == "SpecialColorBomb" && type2 == "SpecialTnt") ||
                (type1 == "SpecialTnt" && type2 == "SpecialColorBomb")) {

        }
        //Random vertical and horizontal rockets in all board
        else if ((type1 == "SpecialColorBomb" && (type2 == "SpecialVerticalRocket" || type2 == "SpecialHorizontalRocket")) ||
                ((type1 == "SpecialVertcialRocket" || type1 == "SpecialHorizontalRocket") && type2 == "SpecialColorBomb")) {

        }
        //Random doves in all board
        else if ((type1 == "SpecialColorBomb" && type2 == "SpecialDove") ||
                (type1 == "SpecialDove" && type2 == "SpecialColorBomb")) {

        }
        
    return piecesToDestroy;

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
            board.currentState = boardStates.gameInputNotAllowed;
            board.GetComponent<Board>().movePieces(touchDownPosition, touchUpPosition);
            
        }
    }
        
}

