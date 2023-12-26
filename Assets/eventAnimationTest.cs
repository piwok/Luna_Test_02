using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventAnimationTest : MonoBehaviour
{
    public Animator animator;
    public void eventAnimationTEST() {
        GetComponent<Piece>().isDestroyAnimationEnd = true;
    }
}
