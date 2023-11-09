using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchsFinder : MonoBehaviour {

    private List<int[]> pieceTypes;
    public GameObject[] pieces;
    private Board board;
    private IDictionary<string, int[][]> fiveLineShapes;
    private IDictionary<string, int[][]> fiveTShapes;
    private IDictionary<string, int[][]> fiveLShapes;
    private IDictionary<string, int[][]> fourLineShapes;
    private IDictionary<string, int[][]> squareShapes;
    private IDictionary<string, int[][]> threeLineShapes;
    private int[] tempPiece1;
    private int[] tempPiece2;
    private int[] tempPiece3;
    private int[] tempPiece4;
    private int[][] tempShape;

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
        fiveLineShapes = new Dictionary<string, int[][]>();
        fiveTShapes = new Dictionary<string, int[][]>();
        fiveLShapes = new Dictionary<string, int[][]>();
        fourLineShapes = new Dictionary<string, int[][]>();
        squareShapes = new Dictionary<string, int[][]>();
        threeLineShapes = new Dictionary<string, int[][]>();
        // Five pieces Line Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {3, 0}; tempPiece4 = new int[2] {4, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        fiveLineShapes.Add("fiveLineShape0", tempShape);
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {0, 3}; tempPiece4 = new int[2] {0, 4};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        fiveLineShapes.Add("fiveLineShape1", tempShape);
        // Five pieces T Shapes//
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {1, 1}; tempPiece4 = new int[2] {2, 1};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        fiveTShapes.Add("fiveTShape0", tempShape);
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {1, -1}; tempPiece4 = new int[2] {1, -2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        fiveLineShapes.Add("fiveTShape1", tempShape);
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {2, 1}; tempPiece4 = new int[2] {2, -1};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        fiveLineShapes.Add("fiveTShape2", tempShape);
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {1, 1}; tempPiece4 = new int[2] {1, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        fiveLineShapes.Add("fiveTShape3", tempShape);
        //Five pieces L Shapes//

        foreach (int[] piece in fiveLineShapes["fiveLineShape0"]) {
            Debug.Log(piece);
            Debug.Log(piece[1]);
        }
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
                    int k = 0;
                    //looking for 5 pieces in straight line fiveLineShape0
                    // Shape0 horizontal
                    if (exploringPiece.GetComponent<Piece>().column < 5) {
                        foreach (int[] piece in fiveLineShapes["fiveLineShape0"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 3) {
                            tempPieces.Add(exploringPiece);
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(tempPieces);
                            tempPieces.clear();
                            continue;
                        }
                    }
                    // Shape1 vertical
                    if (exploringPiece.GetComponent<Piece>().row < 5) {
                        foreach (int[] piece in fiveLineShapes["fiveLineShape1"]) {
                            probePiece = board.allPieces[i + piece[0], j + piece[1]];
                            if (exploringPiece.tag == probePiece.tag) {
                                tempPieces.Add(probePiece);
                            }
                        }
                        if (tempPieces.Count > 3) {
                            tempPieces.Add(exploringPiece);
                            foreach (GameObject piece in tempPieces) {
                                piece.GetComponent<Piece>().isExplored = true;
                            }
                            allLegalSolutions.Add(tempPieces);
                            tempPieces.clear();
                            continue;
                        }
                    }
                }
            }
        }
    }
}
