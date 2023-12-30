using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        currentMatches = new List<GameObject>();
        
    }
    void Update() {
        //Debug.Log(currentMatches.Count);
    }
    public void findAllLegalMatches() {
        StartCoroutine(findAllLegalMatchesCoroutine());
    }
    //the three pieces are left, current, right or down, current, up
    private void formatThreeMatchedPieces(GameObject piece1, GameObject piece2, GameObject piece3) {
        if(piece2.GetComponent<Piece>().type == "SpecialVerticalRocket") {
            currentMatches.Union(getColumnPieces(piece2.GetComponent<Piece>().column));
        }
        if(piece1.GetComponent<Piece>().type == "SpecialVerticalRocket") {
            currentMatches.Union(getColumnPieces(piece1.GetComponent<Piece>().column));
        }
        if(piece3.GetComponent<Piece>().type == "SpecialVerticalRocket") {
            currentMatches.Union(getColumnPieces(piece3.GetComponent<Piece>().column));
        }
        if(piece2.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
            currentMatches.Union(getRowPieces(piece2.GetComponent<Piece>().row));
        }
        if(piece1.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
            currentMatches.Union(getRowPieces(piece1.GetComponent<Piece>().row));
        }
        if(piece3.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
            currentMatches.Union(getRowPieces(piece3.GetComponent<Piece>().row));
        }
        if(!currentMatches.Contains(piece2)) {
            currentMatches.Add(piece2);
        }
        piece2.GetComponent<Piece>().isMatched = true;
        if(!currentMatches.Contains(piece1)) {
            currentMatches.Add(piece1);
        }
        piece1.GetComponent<Piece>().isMatched = true;
        if(!currentMatches.Contains(piece3)) {
            currentMatches.Add(piece3);
        }
        piece3.GetComponent<Piece>().isMatched = true;

    }
    private IEnumerator findAllLegalMatchesCoroutine() {
        yield return new WaitForSeconds(0.15f);
        for(int i = 0; i < board.width; i++) {
            for(int j = 0; j < board.height; j++) {
                GameObject exploringPiece = board.allPieces[i, j];
                if(exploringPiece != null) {
                    if(i > 0 && i < board.width - 1) {
                        GameObject leftPiece = board.allPieces[i - 1, j];
                        GameObject rightPiece = board.allPieces[i + 1, j];
                        if(leftPiece != null && rightPiece != null) {
                            if(leftPiece.GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color &&
                            rightPiece.GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color) {
                                formatThreeMatchedPieces(leftPiece, exploringPiece, rightPiece);
                            }
                        }
                    }
                    if(j > 0 && j < board.height - 1) {
                        GameObject downPiece = board.allPieces[i, j - 1];
                        GameObject upPiece = board.allPieces[i, j + 1];
                        if(downPiece != null && upPiece != null) {
                            if(downPiece.GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color &&
                            upPiece.GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color) {
                                formatThreeMatchedPieces(downPiece, exploringPiece, upPiece);
                            }
                        }
                    }
                }
            }
        }
    }
    private List<GameObject> getColumnPieces(int column) {
        List<GameObject> columnPieces = new List<GameObject>();
        for(int i = 0; i < board.height; i++) {
            if(board.allPieces[column, i] != null) {
                columnPieces.Add(board.allPieces[column, i]);
                board.allPieces[column, i].GetComponent<Piece>().isMatched = true;
            }
        }
        return columnPieces; 
    }
    private List<GameObject> getRowPieces(int row) {
        List<GameObject> rowPieces = new List<GameObject>();
        for(int i = 0; i < board.width; i++) {
            if(board.allPieces[i, row] != null) {
                rowPieces.Add(board.allPieces[i, row]);
                board.allPieces[i, row].GetComponent<Piece>().isMatched = true;
            }
        }
        return rowPieces; 
    }
    public void matchAllPieceOfSameColor(string color) {
        for(int i = 0; i < board.width; i++) {
            for(int j = 0; j < board.height; j++) {
                if(board.allPieces[i, j] != null && board.allPieces[i, j].GetComponent<Piece>().color == color) {
                    board.allPieces[i, j].GetComponent<Piece>().isMatched = true;
                }
            }
        }
    }
    public void checkSpecialPiecesToCreate() {
        //Did the player move something?
        if(board.currentPiece != null) {
            //If the piece moved matched?
            if(board.currentPiece.GetComponent<Piece>().isMatched) {
                //make it unmatched
                board.currentPiece.GetComponent<Piece>().isMatched = false;
                //Special rocket create with a horizontal swipe
                if((board.currentPiece.GetComponent<Piece>().swipeAngle > -45 && board.currentPiece.GetComponent<Piece>().swipeAngle <= 45) ||
                (board.currentPiece.GetComponent<Piece>().swipeAngle < -135 || board.currentPiece.GetComponent<Piece>().swipeAngle >= 135)) {
                    board.currentPiece.GetComponent<Piece>().makeSpecialHorizontalRocket();
                //Special rocket create with a vertical swipe
                } 
                else {
                    board.currentPiece.GetComponent<Piece>().makeSpecialVerticalRocket();
                }
            }
            //is the second piece matched?
            else if(board.currentPiece.GetComponent<Piece>().secondPiece != null) {
                if(board.currentPiece.GetComponent<Piece>().secondPiece.GetComponent<Piece>().isMatched) {
                    board.currentPiece.GetComponent<Piece>().secondPiece.GetComponent<Piece>().isMatched = false;
                    if((board.currentPiece.GetComponent<Piece>().swipeAngle > -45 && board.currentPiece.GetComponent<Piece>().swipeAngle <= 45) ||
                    (board.currentPiece.GetComponent<Piece>().swipeAngle < -135 || board.currentPiece.GetComponent<Piece>().swipeAngle >= 135)) {
                        board.currentPiece.GetComponent<Piece>().secondPiece.GetComponent<Piece>().makeSpecialHorizontalRocket();
                    //Special rocket create with a vertical swipe
                    } 
                    else {
                        board.currentPiece.GetComponent<Piece>().secondPiece.GetComponent<Piece>().makeSpecialVerticalRocket();
                    }
                }
            }
        }
    }
}
