using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution
{
    public List<GameObject> solutionPieces;
    private string shape;
    private string type;
    private string color;
    public int numStepsTodestruccion;
    private int size;
    public Solution(List<GameObject> asolutionPieces, string ashape, string atype, string acolor, string adestructionSpeedType) {
        solutionPieces = asolutionPieces;
        shape = ashape;
        type = atype;
        color = acolor;
        destructionSpeedType = adestructionSpeedType;
        size = asolutionPieces.Count;

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
    public void setColor (string value) {
        color = value;
    }
    public void setNumStepsToDestruccion (int value) {
        numStepsTodestruccion = value;
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
    public string getColor () {
        return color;
    }
    public int getSize () {
        return size;
    }
    
}
