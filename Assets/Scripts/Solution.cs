using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution
{
    private List<GameObject> solutionPieces;
    private string shape;
    private string type;
    public Solution(List<GameObject> asolutionPieces, string ashape, string atype) {
        solutionPieces = asolutionPieces;
        shape = ashape;
        type = atype;

    }
    //Setters and getters
    public void setSolutionPieces (List<GameObject> value) {
        solutionPieces = value;
    }
    public void setShape (string value) {
        shape = value;
    }
    public void setType (string value) {
        type = value;
    }
    public List<GameObject> getSolutionPieces () {
        return solutionPieces;
    }
    public string getShape () {
        return shape;
    }
    public string getType () {
        return type;
    }
    
}
