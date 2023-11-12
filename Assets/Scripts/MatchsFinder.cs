using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchsFinder : MonoBehaviour {

    private List<int[]> pieceTypes;
    public GameObject[] pieces;
    private Board board;
    private IDictionary<string, int[][]> shapes;
    private int[] tempPiece1;
    private int[] tempPiece2;
    private int[] tempPiece3;
    private int[] tempPiece4;
    private int[][] tempShape;
    //variables for lookingForAllMatches method//
    public List<GameObject> piecesMatched;
    
    // --------------------------------------- //

    // 5 pieces math in FiveLineShape0          5 pieces math in FiveLineShape1
    //    
    //      O X X X X                                       X 
    //                                                      X
    //                                                      X
    //                                                      X
    //                                                      O

    // 5 pieces math in FiveTshape0             5 pieces math in FiveTShape1             5 pieces math in FiveTShape2            5 pieces math in FiveTShape 3                                                          
    //    X                                             O X X                                       X                                       X                                       
    //    X X X                                           X                                     O X X                                       X
    //    O                                               X                                         X                                     O X X                               



    // 5 pieces math in FiveLShape 0            5 pieces math in FiveLShape1            5 pieces math in FiveLShape2            5 pieces math in FiveLShape3
    //    X                                               X X X                                   O X X                                      X
    //    X                                               X                                           X                                      X
    //    O X X                                           O                                           X                                  O X X



    // 4 pieces math in FourLineShape0          4 pieces math in FourLineShape1
    //    
    //      O X X X                                         X 
    //                                                      X
    //                                                      X
    //                                                      O


    
    // 4 pieces math in SquareShape0
    //    
    //      X X
    //      O X



    // 3 pieces math in ThreeLineShape0         3 pieces math in ThreeLineShape1
    //    
    //      O X X                                           X
    //                                                      X                                    
    //                                                      O
    void Start() {
        board = FindObjectOfType<Board>();
        shapes = new Dictionary<string, int[][]>();
        // Five pieces Line Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {3, 0}; tempPiece4 = new int[2] {4, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLineShape0", tempShape);
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {0, 3}; tempPiece4 = new int[2] {0, 4};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLineShape1", tempShape);
        // Five pieces T Shapes//
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {1, 1}; tempPiece4 = new int[2] {2, 1};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveTShape0", tempShape);
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {1, -1}; tempPiece4 = new int[2] {1, -2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveTShape1", tempShape);
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {2, 1}; tempPiece4 = new int[2] {2, -1};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveTShape2", tempShape);
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {1, 1}; tempPiece4 = new int[2] {1, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveTShape3", tempShape);
        //Five pieces L Shapes//
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {1, 0}; tempPiece4 = new int[2] {2, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLShape0", tempShape);
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {1, 2}; tempPiece4 = new int[2] {2, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLShape1", tempShape);
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {2, -1}; tempPiece4 = new int[2] {2, -2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLShape2", tempShape);
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {2, 1}; tempPiece4 = new int[2] {2, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLShape3", tempShape);
        // Four pieces Line Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {3, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3};
        shapes.Add("fourLineShape0", tempShape);
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {0, 3};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3};
        shapes.Add("fourLineShape1", tempShape);
        // Four pieces Square Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {1, 1}; tempPiece3 = new int[2] {0, 1};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3};
        shapes.Add("fourSquareShape0", tempShape);
        // Three pieces Line Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2};
        shapes.Add("threeLineShape0", tempShape);
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2};
        shapes.Add("threeLineShape1", tempShape);
    }
    
    public List<List<GameObject>> lookingForAllLegalMatches () {
        List<List<GameObject>> allLegalSolutions = new List<List<GameObject>>();
        GameObject exploringPiece;
        List<GameObject> tempPieces = new List<GameObject>();
        GameObject probePiece;
        for (int i = 0; i < board.width; i++) {
            for (int j = 0; j < board.height; j++) {
                exploringPiece = board.allPieces[i, j];
                if (exploringPiece.GetComponent<Piece>().isExplored == true) {}
                else {
                    //looking for 5 pieces in straight line fiveLineShape0
                    // Shape0 horizontal
                    if (exploringPiece.GetComponent<Piece>().column < 5) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveLineShape0"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape1 vertical
                    if (exploringPiece.GetComponent<Piece>().row < 5) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveLineShape1"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    //looking for 5 pieces in fiveTShapes
                    // Shape0
                    if (exploringPiece.GetComponent<Piece>().column < 7 & exploringPiece.GetComponent<Piece>().row < 7 ) {
                         tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveTShape0"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                           
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape1
                    if (exploringPiece.GetComponent<Piece>().column < 7 & exploringPiece.GetComponent<Piece>().row > 1) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveTShape1"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape 2
                    if (exploringPiece.GetComponent<Piece>().column < 7 & exploringPiece.GetComponent<Piece>().row < 8 & exploringPiece.GetComponent<Piece>().row > 0) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveTShape2"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape 3
                    if (exploringPiece.GetComponent<Piece>().column < 7 & exploringPiece.GetComponent<Piece>().row < 7) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveTShape3"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    //looking for 5 pieces in fiveLShapes
                    // Shape 0
                    if (exploringPiece.GetComponent<Piece>().column < 7 & exploringPiece.GetComponent<Piece>().row < 7) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveLShape0"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                           
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape 1
                    if (exploringPiece.GetComponent<Piece>().column < 7 & exploringPiece.GetComponent<Piece>().row < 7) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveLShape1"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape 2
                    if (exploringPiece.GetComponent<Piece>().column < 7 & exploringPiece.GetComponent<Piece>().row > 1) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveLShape2"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape 3
                    if (exploringPiece.GetComponent<Piece>().column < 7 & exploringPiece.GetComponent<Piece>().row < 7) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fiveLShape3"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 4) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    //looking for 4 pieces in fourLineShapes
                    // Shape 0
                    if (exploringPiece.GetComponent<Piece>().column < 6) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fourLineShape0"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 3) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape 1
                    if (exploringPiece.GetComponent<Piece>().row < 6) {
                         tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fourLineShape1"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 3) {
                           
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    //looking for 4 pieces in fourSquareShapes
                    // Shape 0
                    if (exploringPiece.GetComponent<Piece>().column < 8 & exploringPiece.GetComponent<Piece>().row < 8) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["fourSquareShape0"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count >3) {
                        
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    //looking for 3 pieces in threeLineShapes
                    // Shape 0
                    if (exploringPiece.GetComponent<Piece>().column < 7) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["threeLineShape0"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 2) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    // Shape 1
                    if (exploringPiece.GetComponent<Piece>().row < 7) {
                        tempPieces.Add(exploringPiece);
                        foreach (int[] piece in shapes["threeLineShape1"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 2) {
                            
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(new List<GameObject>(tempPieces));
                            tempPieces.Clear();
                            continue;
                        }
                    tempPieces.Clear();
                    }
                    
                }
            }
        }
    
    foreach (List<GameObject> solution in allLegalSolutions) {
        
        foreach (GameObject pieceGO in solution) {
            
        }
    }
    return allLegalSolutions;
    }



    public List<List<GameObject>> lookingForAllMatches() {
        List<GameObject> piecesToExplore = new List<GameObject>();
        //List<GameObject> piecesMatched = new List<GameObject>();
        List<List<GameObject>> allSolutions = new List<List<GameObject>>();
        GameObject lookingPiece;
        GameObject leftPiece;
        GameObject rightPiece;
        GameObject upPiece;
        GameObject downPiece;
        int exploredColumn;
        int exploredRow;
        for (int i = 0; i < board.width; i++) {
            for (int j = 0; j < board.height; j++) {
                if (board.allPieces[i, j] != null) {
                    if (board.allPieces[i,j].GetComponent<Piece>().isExplored == true) {}
                    else {
                        piecesToExplore.Add(board.allPieces[i,j]);
                        while (piecesToExplore.Count > 0) {
                            lookingPiece = piecesToExplore[0];
                            exploredColumn = lookingPiece.GetComponent<Piece>().column;
                            exploredRow = lookingPiece.GetComponent<Piece>().row;
                            piecesToExplore.RemoveAt(0);
                            //left piece
                            if (exploredColumn > 0) {
                                if (board.allPieces[exploredColumn - 1, exploredRow] != null) {
                                    leftPiece = board.allPieces[exploredColumn - 1, exploredRow];
                                    if (leftPiece.GetComponent<Piece>().isExplored == false && lookingPiece.tag == leftPiece.tag) {
                                        piecesToExplore.Add(leftPiece);
                                        piecesMatched.Add(leftPiece);
                                        leftPiece.GetComponent<Piece>().isExplored = true;}}}
                            //right piece
                            if (exploredColumn < board.width - 1) {
                                if (board.allPieces[exploredColumn + 1, exploredRow] != null) {
                                    rightPiece = board.allPieces[exploredColumn + 1, exploredRow];
                                    if (rightPiece.GetComponent<Piece>().isExplored == false && lookingPiece.tag == rightPiece.tag) {
                                        piecesToExplore.Add(rightPiece);
                                        piecesMatched.Add(rightPiece);
                                        rightPiece.GetComponent<Piece>().isExplored = true;}}}
                            //up piece
                            if (exploredRow < board.height - 1) {
                                if (board.allPieces[exploredColumn, exploredRow + 1] != null) {
                                    upPiece = board.allPieces[exploredColumn, exploredRow + 1];
                                    if (upPiece.GetComponent<Piece>().isExplored == false && lookingPiece.tag == upPiece.tag) {
                                        piecesToExplore.Add(upPiece);
                                        piecesMatched.Add(upPiece);
                                        upPiece.GetComponent<Piece>().isExplored = true;}}}
                            //down_piece
                            if (exploredRow > 0) {
                                if (board.allPieces[exploredColumn, exploredRow - 1] != null) {
                                    downPiece = board.allPieces[exploredColumn, exploredRow - 1];
                                    if (downPiece.GetComponent<Piece>().isExplored == false && lookingPiece.tag == downPiece.tag) {
                                        piecesToExplore.Add(downPiece);
                                        piecesMatched.Add(downPiece);
                                        downPiece.GetComponent<Piece>().isExplored = true;}}}
                        }
                        if (piecesMatched.Count >= 3) {
                            
                            allSolutions.Add(new List<GameObject>(piecesMatched));

                            
                        }
                        piecesMatched.Clear();
                    }
                }
            }
        }
        board.setAllPiecesUnexplored();
        
        return allSolutions;
    }


    private List<GameObject> checkShapeMatch (GameObject exploringPiece, string shape, int minColumn, int maxColumn, int minRow, int maxRow, int matchSize) {
        List<GameObject> tempPieces = new List<GameObject>();
        int exploringColumn = exploringPiece.GetComponent<Piece>().column;
        int exploringRow = exploringPiece.GetComponent<Piece>().row;

        if (exploringColumn > minColumn & exploringColumn < maxColumn & exploringRow > minRow & exploringRow < maxRow) {
            tempPieces.Add(exploringPiece);
            foreach (int[] shape in shapes[shape]) {
                probePiece = board.allPieces[i + shape[0], j + shape[1]];
                if (exploringPiece.tag == probePiece.tag) {
                    tempPieces.Add(probePiece);
                }
                else {
                    break;
                }
            }
            if (tempPieces.Count == matchSize) {
                return tempPieces;
            }
        }
        return null;
    }
}

