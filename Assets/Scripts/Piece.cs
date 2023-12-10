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
    public List<int[]> allTntTargets;
    public List<int[]> halfRightSideTntTargets;
    public List<int[]> halfLeftSideTntTargets;
    
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        previousColumn = column;
        previousRow = row;
        wrongPosition = false;
        allTntTargets = new List<int[]>();
        halfRightSideTntTargets = new List<int[]>();
        halfLeftSideTntTargets = new List<int[]>();
        //one square distance all points
        allTntTargets.Add(new int[] {-1, -1}); allTntTargets.Add(new int[] {-1, 0}); allTntTargets.Add(new int[] {-1, 1}); allTntTargets.Add(new int[] {0, 1}); allTntTargets.Add(new int[] {1, 1});
        allTntTargets.Add(new int [] {1, 0}); allTntTargets.Add(new int[] {1, -1}); allTntTargets.Add(new int[] {0, -1});
        //two square distance all points
        allTntTargets.Add(new int[] {-2, -2});allTntTargets.Add(new int[] {-2, -1});allTntTargets.Add(new int[] {-2, 0});allTntTargets.Add(new int[] {-2, 1});allTntTargets.Add(new int[] {-2, 2});
        allTntTargets.Add(new int[] {-1, 2});allTntTargets.Add(new int[] {0, 2});allTntTargets.Add(new int[] {1, 2});allTntTargets.Add(new int[] {2, 2});allTntTargets.Add(new int[] {2, 1});
        allTntTargets.Add(new int[] {2, 0});allTntTargets.Add(new int[] {2, -1});allTntTargets.Add(new int[] {2, -2});allTntTargets.Add(new int[] {1, -2});allTntTargets.Add(new int[] {0, -2});
        allTntTargets.Add(new int[] {-1, -2});
        //one square distance half right side points
        halfRightSideTntTargets.Add(new int[] {0, 1}); halfRightSideTntTargets.Add(new int[] {1, 1}); halfRightSideTntTargets.Add(new int [] {1, 0});
        halfRightSideTntTargets.Add(new int[] {1, -1}); halfRightSideTntTargets.Add(new int[] {0, -1});
        //one square distance half left side points
        halfLeftSideTntTargets.Add(new int[] {-1, -1}); halfLeftSideTntTargets.Add(new int[] {-1, 0}); halfLeftSideTntTargets.Add(new int[] {-1, 1}); 
        halfLeftSideTntTargets.Add(new int[] {0, 1}); halfLeftSideTntTargets.Add(new int[] {0, -1});
        //two square distance half right side points
        halfRightSideTntTargets.Add(new int[] {0, 2}); halfRightSideTntTargets.Add(new int[] {1, 2}); halfRightSideTntTargets.Add(new int[] {2, 2});
        halfRightSideTntTargets.Add(new int[] {2, 1}); halfRightSideTntTargets.Add(new int[] {2, 0}); halfRightSideTntTargets.Add(new int[] {2, -1});
        halfRightSideTntTargets.Add(new int[] {2, -2}); halfRightSideTntTargets.Add(new int[] {1, -2}); halfRightSideTntTargets.Add(new int[] {0, -2});
        //two square distance half left side points
        halfLeftSideTntTargets.Add(new int[] {-2, -2});halfLeftSideTntTargets.Add(new int[] {-2, -1});halfLeftSideTntTargets.Add(new int[] {-2, 0});
        halfLeftSideTntTargets.Add(new int[] {-2, 1});halfLeftSideTntTargets.Add(new int[] {-2, 2});halfLeftSideTntTargets.Add(new int[] {-1, 2});
        halfLeftSideTntTargets.Add(new int[] {0, 2}); halfLeftSideTntTargets.Add(new int[] {0, -2});halfLeftSideTntTargets.Add(new int[] {-1, -2});
        //three square distance half right side points
        halfRightSideTntTargets.Add(new int[] {0, 3}); halfRightSideTntTargets.Add(new int[] {1, 3}); halfRightSideTntTargets.Add(new int[] {2, 3});
        halfRightSideTntTargets.Add(new int[] {3, 3}); halfRightSideTntTargets.Add(new int[] {3, 2}); halfRightSideTntTargets.Add(new int[] {3, 1});
        halfRightSideTntTargets.Add(new int[] {3, 0}); halfRightSideTntTargets.Add(new int[] {3, -1}); halfRightSideTntTargets.Add(new int[] {3, -2});
        halfRightSideTntTargets.Add(new int[] {3, -3}); halfRightSideTntTargets.Add(new int[] {2, -3}); halfRightSideTntTargets.Add(new int[] {1, -3});
        halfRightSideTntTargets.Add(new int[] {0, -3});
        //three square distance half left side points
        halfLeftSideTntTargets.Add(new int[] {0, 3}); halfLeftSideTntTargets.Add(new int[] {-1, 3}); halfLeftSideTntTargets.Add(new int[] {-2, 3});
        halfLeftSideTntTargets.Add(new int[] {-3, 3}); halfLeftSideTntTargets.Add(new int[] {-3, 2}); halfLeftSideTntTargets.Add(new int[] {-3, 1});
        halfLeftSideTntTargets.Add(new int[] {-3, 0}); halfLeftSideTntTargets.Add(new int[] {-3, -1}); halfLeftSideTntTargets.Add(new int[] {-3, -2});
        halfLeftSideTntTargets.Add(new int[] {-3, -3}); halfLeftSideTntTargets.Add(new int[] {-2, -3}); halfLeftSideTntTargets.Add(new int[] {-1, -3});
        halfLeftSideTntTargets.Add(new int[] {0, -3});
        //four square distance half right side points
        halfRightSideTntTargets.Add(new int[] {0, 4}); halfRightSideTntTargets.Add(new int[] {1, 4}); halfRightSideTntTargets.Add(new int[] {2, 4});
        halfRightSideTntTargets.Add(new int[] {3, 4}); halfRightSideTntTargets.Add(new int[] {4, 4}); halfRightSideTntTargets.Add(new int[] {4, 3});
        halfRightSideTntTargets.Add(new int[] {4, 2}); halfRightSideTntTargets.Add(new int[] {4, 1}); halfRightSideTntTargets.Add(new int[] {4, 0});
        halfRightSideTntTargets.Add(new int[] {4, -1}); halfRightSideTntTargets.Add(new int[] {4, -2}); halfRightSideTntTargets.Add(new int[] {4, -3});
        halfRightSideTntTargets.Add(new int[] {4, -4}); halfRightSideTntTargets.Add(new int[] {3, -4}); halfRightSideTntTargets.Add(new int[] {2, -4});
        halfRightSideTntTargets.Add(new int[] {1, -4}); halfRightSideTntTargets.Add(new int[] {0, -4});
        // four square distance half left side points
        halfLeftSideTntTargets.Add(new int[] {0, 4}); halfLeftSideTntTargets.Add(new int[] {-1, 4}); halfLeftSideTntTargets.Add(new int[] {-2, 4});
        halfLeftSideTntTargets.Add(new int[] {-3, 4}); halfLeftSideTntTargets.Add(new int[] {-4, 4}); halfLeftSideTntTargets.Add(new int[] {-4, 3});
        halfLeftSideTntTargets.Add(new int[] {-4, 2}); halfLeftSideTntTargets.Add(new int[] {-4, 1}); halfLeftSideTntTargets.Add(new int[] {-4, 0});
        halfLeftSideTntTargets.Add(new int[] {-4, -1}); halfLeftSideTntTargets.Add(new int[] {-4, -2}); halfLeftSideTntTargets.Add(new int[] {-4, -3});
        halfLeftSideTntTargets.Add(new int[] {-4, -4});halfLeftSideTntTargets.Add(new int[] {-3, -4}); halfLeftSideTntTargets.Add(new int[] {-2, -4});
        halfLeftSideTntTargets.Add(new int[] {-1, -4}); halfLeftSideTntTargets.Add(new int[] {0, -4});

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

    public Solution getPiecesToDestroy() 
    {   
        List<GameObject> newSolution = new List<GameObject>();
        if (type == "SpecialTnt") { 
            foreach (int[] tntTarget in allTntTargets) {
                if (column + tntTarget[0] < board.width && column + tntTarget[0] >= 0 && row + tntTarget[1] < board.height && row + tntTarget[1] >= 0 &&
                board.allPieces[column + tntTarget[0], row + tntTarget[1]] != null) {
                    newSolution.Add(board.allPieces[column + tntTarget[0], row + tntTarget[1]]);
                }
            }
            newSolution.Add(gameObject);
            return new Solution(newSolution, null, null, null);
        }
        else if (type == "SpecialVerticalRocket") { 
            for (int i = 0; i < board.height; i++) {
                if (board.allPieces[column, i] != null) {
                    newSolution.Add(board.allPieces[column, i]);
                }
            }
            return new Solution(newSolution, null, null, null);
        }
        else if (type == "SpecialHorizontalRocket") { 
            for (int i = 0; i < board.width; i++) {
                if (board.allPieces[i, row] != null) {
                    newSolution.Add(board.allPieces[i, row]);
                }
            }
            return new Solution(newSolution, null, null, null);
        }
        else if (type == "SpecialColorBomb") { 
            
            string[] colors = new string[4] {"Red", "Yellow", "Green", "Black"};
            string colorToDestroy = colors[Random.Range(0, 4)];
            for (int i = 0; i < board.width; i++) {
                for (int j = 0; j < board.height; j++) {
                    if (board.allPieces[i, j] != null && board.allPieces[i, j].GetComponent<Piece>().color == colorToDestroy) {
                        newSolution.Add(board.allPieces[i, j]);
                    }
                }
            }
            
            
            return new Solution(newSolution, null, null, null);
        }
        return null;
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
    public List<Solution> GetPiecesToDestroySpecialPower (string type) {
        List<Solution> piecesToDestroy = new List<Solution>();
        if (type == "SpecialTnt") {

        }
        else if (type == "SpecialVerticalRocket") {

        }
        else if (type == "SpecialHorizontalRocket") {

        }
        else if (type == "SpecialDove") {

        }
        else if (type == "SpecialColorBomb") {

        }
        return piecesToDestroy;

    }
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
}

