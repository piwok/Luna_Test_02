using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPieceToCreate {
    public int column;
    public int row;
    public string color;
    public string matchShape;
    public SpecialPieceToCreate(int acolumn, int arow, string acolor, string amatchShape) {
        column = acolumn;
        row = arow;
        color = acolor;
        matchShape = amatchShape;
    }
    
}
