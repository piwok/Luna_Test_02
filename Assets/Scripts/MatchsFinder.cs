using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchsFinder : MonoBehaviour
{   
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
    // Start is called before the first frame update
    void Start()
    {   board = FindObjectOfType<Board>();
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Solution> lookingForAllLegalMatches () {
    List<Solution> allLegalSolutions = new List<Solution>();
    List<GameObject> allThreeSizeInitialSolutions = new List<GameObject>();
    Solution tempSolution;
    GameObject exploringPiece;
    string exploringShape;
    int exploringColumn;
    int exploringRow;
    List<GameObject> horizontalThreeInitial = new List<GameObject>();
    List<GameObject> verticalThreeInitial = new List<GameObject>();
    List<GameObject> tempPieces = new List<GameObject>();
    //looking for three size horizontal line 
    for (int i = 0; i < board.width - 2; i++) {
        for (int j = 0; j < board.height; j++) {
            exploringPiece = board.allPieces[i, j];
            if (exploringPiece != null & board.allPieces[i + 1, j] != null & board.allPieces[i + 2, j] != null) {
                if (board.allPieces[i, j].GetComponent<Piece>().color == board.allPieces[i + 1, j].GetComponent<Piece>().color
                && board.allPieces[i + 1, j].GetComponent<Piece>().color == board.allPieces[i + 2, j].GetComponent<Piece>().color) {
                    allThreeSizeInitialSolutions.Add(board.allPieces[i, j]);
                    allThreeSizeInitialSolutions.Add(board.allPieces[i + 1, j]);
                    allThreeSizeInitialSolutions.Add(board.allPieces[i + 2, j]);
                }
            }
            else {
                continue;
            }
        }
    }
    //looking for three size vertical line
    for (int i = 0; i < board.width; i++) {
        for (int j = 0; j < board.height - 2; j++) {
            exploringPiece = board.allPieces[i, j];
            if (exploringPiece != null & board.allPieces[i, j + 1] != null & board.allPieces[i, j + 2] != null) {
                if (board.allPieces[i, j].GetComponent<Piece>().color == board.allPieces[i, j + 1].GetComponent<Piece>().color
                & board.allPieces[i, j + 1].GetComponent<Piece>().color == board.allPieces[i, j + 2].GetComponent<Piece>().color) {
                    allThreeSizeInitialSolutions.Add(board.allPieces[i, j]);
                    allThreeSizeInitialSolutions.Add(board.allPieces[i, j + 1]);
                    allThreeSizeInitialSolutions.Add(board.allPieces[i, j + 2]);
                }
                else {
                    continue;
                }
            }
        }
    }
    //looking for 5 size shape matchs in the points of allThreeInitialSolutios
    foreach(GameObject pieceToExplore in allThreeSizeInitialSolutions) {
        exploringPiece = pieceToExplore;
        exploringColumn = exploringPiece.GetComponent<Piece>().column;
        exploringRow = exploringPiece.GetComponent<Piece>().row;
        for (int i = 0; i < 12; i++) {
            exploringShape = shapeNames[i];
            tempPieces = checkShapeMatch(exploringPiece, exploringColumn, exploringRow, exploringShape);
            if (tempPieces != null) {
                tempSolution = new Solution(tempPieces, exploringShape,
                exploringPiece.GetComponent<Piece>().type, exploringPiece.GetComponent<Piece>().color);
                allLegalSolutions.Add(tempSolution);
            }
        }
    }
    //looking for 4 size squares shape matchs in all points
    for (int i = 0; i < board.width; i++) {
        for (int j = 0; j < board.height; j++) {
            exploringPiece = board.allPieces[i, j];
            if (exploringPiece != null) { 
                exploringColumn = exploringPiece.GetComponent<Piece>().column;
                exploringRow = exploringPiece.GetComponent<Piece>().row;
                exploringShape = "fourSquareShape0";
                tempPieces = checkShapeMatch(exploringPiece, exploringColumn, exploringRow, exploringShape);
                if (tempPieces != null) {
                    tempSolution = new Solution(tempPieces, exploringShape,
                    exploringPiece.GetComponent<Piece>().type, exploringPiece.GetComponent<Piece>().color);
                    allLegalSolutions.Add(tempSolution);
                }
            }
            else {
                continue;
            } 
        }
    }
    //looking for 3 size shape matchs in the points of allThreeInitialSolutios
    foreach(GameObject pointToExplore in allThreeSizeInitialSolutions) {
        exploringPiece = pointToExplore;
        exploringColumn = exploringPiece.GetComponent<Piece>().column;
        exploringRow = exploringPiece.GetComponent<Piece>().row;
        for (int i = 13; i < 15; i++) {
            exploringShape = shapeNames[i];
            tempPieces = checkShapeMatch(exploringPiece, exploringColumn, exploringRow, exploringShape);
            if (tempPieces != null) {
                tempSolution = new Solution(tempPieces, exploringShape,
                exploringPiece.GetComponent<Piece>().type, exploringPiece.GetComponent<Piece>().color);
                allLegalSolutions.Add(tempSolution);
            }
        }
    }
    board.setAllPiecesUnexplored();
    return allLegalSolutions;
}
private List<GameObject> checkShapeMatch (GameObject exploringPiece, int column, int row, string exploringShape) {
    List<GameObject> tempPieces = new List<GameObject>();
    int exploringColumn = column;
    int exploringRow = row;
    int minColumn = shapesBoardLimits[exploringShape]["minColumn"];
    int maxColumn = shapesBoardLimits[exploringShape]["maxColumn"];
    int minRow = shapesBoardLimits[exploringShape]["minRow"];
    int maxRow = shapesBoardLimits[exploringShape]["maxRow"];
    int matchSize = shapesBoardLimits[exploringShape]["matchSize"];
    GameObject probePiece;
    if (exploringColumn >= minColumn && exploringColumn <= maxColumn && exploringRow >= minRow && exploringRow <= maxRow) {
        tempPieces.Add(exploringPiece);
        foreach (int[] shapePoint in shapes[exploringShape]) {
            probePiece = board.allPieces[exploringColumn + shapePoint[0], exploringRow + shapePoint[1]];
            if (probePiece == null) {
                continue;
            }
            if (exploringPiece.GetComponent<Piece>().color == probePiece.GetComponent<Piece>().color & probePiece.GetComponent<Piece>().isExplored == false) {
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



}
