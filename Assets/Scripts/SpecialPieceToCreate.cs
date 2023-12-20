using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPieceToCreate {
    public Tile tile;
    public string shape;
    public int piecesIndex;
    public SpecialPieceToCreate(Tile atile, string ashape, int apiecesIndex) {
        tile = atile;
        shape = ashape;
        piecesIndex = apiecesIndex;
    }
}
