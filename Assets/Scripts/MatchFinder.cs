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
        Debug.Log(currentMatches.Count);
    }
    public void findAllLegalMatches() {
        StartCoroutine(findAllLegalMatchesCoroutine());
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
                                if(exploringPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                                    currentMatches.Union(getColumnPieces(i));
                                }
                                if(leftPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                                    currentMatches.Union(getColumnPieces(i - 1));
                                }
                                if(rightPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                                    currentMatches.Union(getColumnPieces(i + 1));
                                }
                                if(exploringPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                                    currentMatches.Union(getRowPieces(j));
                                }
                                if(leftPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                                    currentMatches.Union(getRowPieces(j));
                                }
                                if(rightPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                                    currentMatches.Union(getRowPieces(j));
                                }
                                if(!currentMatches.Contains(leftPiece)) {
                                    currentMatches.Add(leftPiece);
                                }
                                leftPiece.GetComponent<Piece>().isMatched = true;
                                if(!currentMatches.Contains(rightPiece)) {
                                    currentMatches.Add(rightPiece);
                                }
                                rightPiece.GetComponent<Piece>().isMatched = true;
                                 if(!currentMatches.Contains(exploringPiece)) {
                                    currentMatches.Add(exploringPiece);
                                }
                                exploringPiece.GetComponent<Piece>().isMatched = true;
                            }
                        }
                    }
                    if(j > 0 && j < board.height - 1) {
                        GameObject downPiece = board.allPieces[i, j - 1];
                        GameObject upPiece = board.allPieces[i, j + 1];
                        if(downPiece != null && upPiece != null) {
                            if(downPiece.GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color &&
                            upPiece.GetComponent<Piece>().color == exploringPiece.GetComponent<Piece>().color) {
                                if(exploringPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                                    currentMatches.Union(getColumnPieces(i));
                                }
                                if(downPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                                    currentMatches.Union(getColumnPieces(i));
                                }
                                if(upPiece.GetComponent<Piece>().type == "SpecialVerticalRocket") {
                                    currentMatches.Union(getColumnPieces(i));
                                }
                                if(exploringPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                                    currentMatches.Union(getRowPieces(j));
                                }
                                if(downPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                                    currentMatches.Union(getRowPieces(j - 1));
                                }
                                if(upPiece.GetComponent<Piece>().type == "SpecialHorizontalRocket") {
                                    currentMatches.Union(getRowPieces(j + 1));
                                }
                                if(!currentMatches.Contains(downPiece)) {
                                    currentMatches.Add(downPiece);
                                }
                                downPiece.GetComponent<Piece>().isMatched = true;
                                if(!currentMatches.Contains(upPiece)) {
                                    currentMatches.Add(upPiece);
                                }
                                upPiece.GetComponent<Piece>().isMatched = true;
                                 if(!currentMatches.Contains(exploringPiece)) {
                                    currentMatches.Add(exploringPiece);
                                }
                                exploringPiece.GetComponent<Piece>().isMatched = true;
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
    public void checkSpecialPiecesToCreate() {
        //Did the player move something?
        if(board.currentPiece != null) {
            //If the piece moved matched?
            if(board.currentPiece.GetComponent<Piece>().isMatched) {
                //make it unmatched
                board.currentPiece.GetComponent<Piece>().isMatched = false;
                //currentMatches.Remove(board.currentPiece);
                //Decide the type of special piece
                int typeSpecialPiece = Random.Range(0, 100);
                if(typeSpecialPiece < 50) {
                    //Make a horizontal rocket
                    board.currentPiece.GetComponent<Piece>().makeSpecialHorizontalRocket();

                }
                else if(typeSpecialPiece >= 50) {
                    //make a vertical rocket
                    board.currentPiece.GetComponent<Piece>().makeSpecialVerticalRocket();

                }
            }
            //is the second piece matched?
            else if(board.currentPiece.GetComponent<Piece>().secondPiece != null) {
                if(board.currentPiece.GetComponent<Piece>().secondPiece.GetComponent<Piece>().isMatched) {
                    board.currentPiece.GetComponent<Piece>().secondPiece.GetComponent<Piece>().isMatched = false;
                    int typeSpecialPiece = Random.Range(0, 100);
                    if(typeSpecialPiece < 50) {
                    //Make a horizontal rocket
                    board.currentPiece.GetComponent<Piece>().secondPiece.GetComponent<Piece>().makeSpecialHorizontalRocket();
                    }
                    else if(typeSpecialPiece >= 50) {
                    //make a vertical rocket
                    board.currentPiece.GetComponent<Piece>().secondPiece.GetComponent<Piece>().makeSpecialVerticalRocket();
                    }

                }

            
            }

        }
    }
}
