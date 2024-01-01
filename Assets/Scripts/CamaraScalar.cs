using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraScalar : MonoBehaviour
{
    private Board board;
    public float cameraOffset;
    public float aspectRatio;
    public float padding = 2;

    // Start is called before the first frame update
    void Start()
    {
        aspectRatio = (float) Screen.width/(float) Screen.height;
        cameraOffset = -10f;
        board = FindObjectOfType<Board>();
        if(board != null) {
            repositionCamara(board.width - 1, board.height - 1);
        }
        
    }
    private void repositionCamara(float x, float y) {
        Vector3 tempPosition = new Vector3(x/2, y/2, cameraOffset);
        transform.position = tempPosition;
        if(board.width >= board.height) {
            Camera.main.orthographicSize = (board.width/2 + padding)/aspectRatio;
        }
        else {
            Camera.main.orthographicSize = board.width + 2*padding;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
