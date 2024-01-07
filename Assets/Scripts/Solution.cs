using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution
{
    //There is threee types of solution:
    // - "swipeMatches": the matches are the product of a swipe movement: matches in current and second piece
    // - "regularMatches": the matches are product of collapse columns and refill board pieces, with the new positions and pieces matches are generated
    // - "specialPowerMatches": the matches are product of a tnt explosion, a vertical or horizontal rocket, a color bomb, a dove or double special power
    // the color parameter is the color of the matched pieces in "movementMatches" and "regularMatches". "specialPowerMatches" color is null
    // the shape parameter is the shape of the regular match, "specialPowerMatch" shape is null
    public List<GameObject> solutionPieces;
    public string shape;
    public string type;
    public string color;
    public int newSpecialPieceColumn;
    public int newSpecialPieceRow;
    public int size;
    public Solution(List<GameObject> asolutionPieces, string ashape, string atype, string acolor, int anewSpecialPieceColumn, int anewSpecialPieceRow) {
        solutionPieces = asolutionPieces;
        shape = ashape;
        type = atype;
        color = acolor;
        newSpecialPieceColumn = anewSpecialPieceColumn;
        newSpecialPieceRow = anewSpecialPieceRow;
        size = asolutionPieces.Count;
    }
    public void addSolutionPieceToSolution(GameObject newSolutionPiece) {
        solutionPieces.Add(newSolutionPiece);
    }
    public void removeSolutionPieceFromSolution(GameObject SolutionPiece) {
        solutionPieces.Remove(SolutionPiece);
    }
}