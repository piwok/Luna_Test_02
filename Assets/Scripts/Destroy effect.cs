using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float timeLeft = 0.10f;
    void Update() {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0.0f) {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
