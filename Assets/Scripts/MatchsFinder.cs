using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchsFinder : MonoBehaviour {

    private List<int[]> pieceTypes;
    public GameObject[] pieces;
    private Board board;
    private string[] shapeNames;
    private IDictionary<string, int[][]> shapes;
    private IDictionary<string, Dictionary<string, int>> shapesBoardLimits;
    private IDictionary<string, int> boardLimits;
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
    // 5 pieces match in FiveTshape0             5 pieces match in FiveTShape1             5 pieces match in FiveTShape2            5 pieces match in FiveTShape 3                                                          
    //    X                                             O X X                                       X                                       X                                       
    //    X X X                                           X                                     O X X                                       X
    //    O                                               X                                         X                                     O X X                               
    // 5 pieces match in FiveLShape 0            5 pieces match in FiveLShape1            5 pieces match in FiveLShape2            5 pieces match in FiveLShape3
    //    X                                               X X X                                   O X X                                      X
    //    X                                               X                                           X                                      X
    //    O X X                                           O                                           X                                  O X X
    // 4 pieces match in FourLineShape0          4 pieces match in FourLineShape1
    //    
    //      O X X X                                         X 
    //                                                      X
    //                                                      X
    //                                                      O
    // 4 pieces match in SquareShape0
    //    
    //      X X
    //      O X
    // 3 pieces match in ThreeLineShape0         3 pieces match in ThreeLineShape1
    //    
    //      O X X                                           X
    //                                                      X                                    
    //                                                      O
    void Start() {
        board = FindObjectOfType<Board>();
        shapes = new Dictionary<string, int[][]>();
        shapeNames = new string[15] {"fiveLineShape0", "fiveLineShape1", "fiveTShape0", "fiveTShape1", "fiveTShape2", "fiveTShape3",
        "fiveLShape0", "fiveLShape1", "fiveLShape2", "fiveLShape3", "fourLineShape0", "fourLineShape1", "fourSquareShape0", "threeLineShape0", "threeLineShape1"};
        shapesBoardLimits = new Dictionary<string, Dictionary<string, int>>();
        // Five pieces Line Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {3, 0}; tempPiece4 = new int[2] {4, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLineShape0", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 4}, {"minRow", 0}, {"maxRow", 8}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveLineShape0", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {0, 3}; tempPiece4 = new int[2] {0, 4};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLineShape1", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 8}, {"minRow", 0}, {"maxRow", 4}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveLineShape1", new Dictionary<string, int>(boardLimits));
        // Five pieces T Shapes//
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {1, 1}; tempPiece4 = new int[2] {2, 1};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveTShape0", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 0}, {"maxRow", 6}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveTShape0", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {1, -1}; tempPiece4 = new int[2] {1, -2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveTShape1", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 2}, {"maxRow", 8}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveTShape1", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {2, 1}; tempPiece4 = new int[2] {2, -1};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveTShape2", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 1}, {"maxRow", 7}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveTShape2", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {1, 1}; tempPiece4 = new int[2] {1, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveTShape3", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 0}, {"maxRow", 6}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveTShape3", new Dictionary<string, int>(boardLimits));
        //Five pieces L Shapes//
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {1, 0}; tempPiece4 = new int[2] {2, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLShape0", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 0}, {"maxRow", 6}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveLShape0", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {1, 2}; tempPiece4 = new int[2] {2, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLShape1", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 0}, {"maxRow", 6}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveLShape1", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {2, -1}; tempPiece4 = new int[2] {2, -2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLShape2", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 2}, {"maxRow", 8}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveLShape2", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {2, 1}; tempPiece4 = new int[2] {2, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3, tempPiece4};
        shapes.Add("fiveLShape3", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 0}, {"maxRow", 6}, {"matchSize", 5}};
        shapesBoardLimits.Add("fiveLShape3", new Dictionary<string, int>(boardLimits));
        // Four pieces Line Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0}; tempPiece3 = new int[2] {3, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3};
        shapes.Add("fourLineShape0", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 5}, {"minRow", 0}, {"maxRow", 8}, {"matchSize", 4}};
        shapesBoardLimits.Add("fourLineShape0", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2}; tempPiece3 = new int[2] {0, 3};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3};
        shapes.Add("fourLineShape1", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 8}, {"minRow", 0}, {"maxRow", 5}, {"matchSize", 4}};
        shapesBoardLimits.Add("fourLineShape1", new Dictionary<string, int>(boardLimits));
        // Four pieces Square Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {1, 1}; tempPiece3 = new int[2] {0, 1};
        tempShape = new int[][] {tempPiece1, tempPiece2, tempPiece3};
        shapes.Add("fourSquareShape0", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 7}, {"minRow", 0}, {"maxRow", 7}, {"matchSize", 4}};
        shapesBoardLimits.Add("fourSquareShape0", new Dictionary<string, int>(boardLimits));
        // Three pieces Line Shapes//
        tempPiece1 = new int[2] {1, 0}; tempPiece2 = new int[2] {2, 0};
        tempShape = new int[][] {tempPiece1, tempPiece2};
        shapes.Add("threeLineShape0", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 6}, {"minRow", 0}, {"maxRow", 8}, {"matchSize", 3}};
        shapesBoardLimits.Add("threeLineShape0", new Dictionary<string, int>(boardLimits));
        tempPiece1 = new int[2] {0, 1}; tempPiece2 = new int[2] {0, 2};
        tempShape = new int[][] {tempPiece1, tempPiece2};
        shapes.Add("threeLineShape1", tempShape);
        boardLimits = new Dictionary<string, int>() {{"minColumn", 0},{"maxColumn", 8}, {"minRow", 0}, {"maxRow", 6}, {"matchSize", 3}};
        shapesBoardLimits.Add("threeLineShape1", new Dictionary<string, int>(boardLimits));
    }
    
    public List<List<GameObject>> lookingForAllLegalMatches () {
        List<List<GameObject>> allLegalSolutions = new List<List<GameObject>>();
        List<GameObject> tempSolution = new List<GameObject>();
        GameObject exploringPiece;
        List<GameObject> tempPieces = new List<GameObject>();
        foreach (string currentShape in shapeNames) {
            for (int i = 0; i < board.width; i++) {
                for (int j = 0; j < board.height; j++) {
                    exploringPiece = board.allPieces[i, j];
                    if (exploringPiece.GetComponent<Piece>().isExplored == true) {
                        continue;
                    }
                    else {
                        tempSolution = checkShapeMatch (exploringPiece, i, j, currentShape, shapesBoardLimits[currentShape]["minColumn"], shapesBoardLimits[currentShape]["maxColumn"],
                            shapesBoardLimits[currentShape]["minRow"], shapesBoardLimits[currentShape]["maxRow"], shapesBoardLimits[currentShape]["matchSize"]);
                        if (tempSolution != null) {
                            allLegalSolutions.Add(new List<GameObject>(tempSolution));
                        }
                    }
                }
            }
        }
        return allLegalSolutions;
    }
    private List<GameObject> checkShapeMatch (GameObject exploringPiece, int column, int row, string shape, int minColumn, int maxColumn, int minRow, int maxRow, int matchSize) {
        List<GameObject> tempPieces = new List<GameObject>();
        int exploringColumn = column;
        int exploringRow = row;
        GameObject probePiece;
        if (exploringColumn >= minColumn & exploringColumn <= maxColumn & exploringRow >= minRow & exploringRow <= maxRow) {
            tempPieces.Add(exploringPiece);
            foreach (int[] shapePoint in shapes[shape]) {
                probePiece = board.allPieces[exploringColumn + shapePoint[0], exploringRow + shapePoint[1]];
                if (probePiece == null) {
                    
                    continue;
                }
                if (exploringPiece.tag == probePiece.tag & probePiece.GetComponent<Piece>().isExplored == false) {
                    tempPieces.Add(probePiece);
                }
                else {
                    break;
                }
            }
            if (tempPieces.Count == matchSize) {
                foreach (GameObject piece in tempPieces) {
                    piece.GetComponent<Piece>().isExplored = true;
                }
                return tempPieces;
            }
        }
        return null;
    }

    public bool isLegalMatchAt(int column, int row, GameObject newPiece) {
        
        foreach (string currentShape in shapeNames) {
            
            
            List<GameObject> tempSolution = new List<GameObject>();
            
            tempSolution = checkShapeMatch (newPiece, column, row, currentShape, shapesBoardLimits[currentShape]["minColumn"],
                shapesBoardLimits[currentShape]["maxColumn"], shapesBoardLimits[currentShape]["minRow"],
                shapesBoardLimits[currentShape]["maxRow"], shapesBoardLimits[currentShape]["matchSize"]);
            
            if (tempSolution != null) {
                
                board.setAllPiecesUnexplored();
                return true;
            }
        }
        board.setAllPiecesUnexplored();
        return false;    
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

    public bool isAMatchAt(int column, int row, GameObject new_piece) {
        List<GameObject> piecesToExplore = new List<GameObject>();
        int matchLength = 0;
        piecesToExplore.Add(new_piece);
        int exploredColumn;
        int exploredRow;
        while (piecesToExplore.Count > 0) {
            GameObject exploringPiece = piecesToExplore[0];
            exploringPiece.GetComponent<Piece>().isExplored = true;
            piecesToExplore.RemoveAt(0);
            if (matchLength == 0) {
                exploredColumn = column;
                exploredRow = row;}
            else {
                exploredColumn = exploringPiece.GetComponent<Piece>().column;
                exploredRow = exploringPiece.GetComponent<Piece>().row;}
            matchLength += 1;
            //left piece
            if (exploredColumn > 0) {
                if (board.allPieces[exploredColumn - 1, exploredRow] != null) {
                    GameObject leftPiece = board.allPieces[exploredColumn - 1, exploredRow];
                    if (leftPiece.tag == exploringPiece.tag & leftPiece.GetComponent<Piece>().isExplored == false) {
                        leftPiece.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(leftPiece);}}}
            //right piece
            if (exploredColumn < board.width - 1) {
                if (board.allPieces[exploredColumn + 1, exploredRow] != null) {
                    GameObject rightPiece = board.allPieces[exploredColumn + 1, exploredRow];
                    if (rightPiece.tag == exploringPiece.tag & rightPiece.GetComponent<Piece>().isExplored == false) {
                        rightPiece.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(rightPiece);}}}            
            //up piece
            if (exploredRow < board.height - 1) {
                if (board.allPieces[exploredColumn, exploredRow + 1] != null) {
                    GameObject upPiece = board.allPieces[exploredColumn, exploredRow + 1];
                    if (upPiece.tag == exploringPiece.tag & upPiece.GetComponent<Piece>().isExplored == false) {
                        upPiece.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(upPiece);}}}
            //down piece
            if (exploredRow > 0) {
                if (board.allPieces[exploredColumn, exploredRow - 1] != null) {
                    GameObject downPiece = board.allPieces[exploredColumn, exploredRow - 1];
                    if (downPiece.tag == exploringPiece.tag & downPiece.GetComponent<Piece>().isExplored == false) {
                        downPiece.GetComponent<Piece>().isExplored = true;
                        piecesToExplore.Add(downPiece);}}}
        }
        board.setAllPiecesUnexplored();
        if (matchLength > 2) {
            return true;}
        return false;        
    }
    
}

