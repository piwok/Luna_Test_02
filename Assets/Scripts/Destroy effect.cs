using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SelfDestruct : MonoBehaviour {
     public float timeLeft = 0.5f;

     void Update() {
          timeLeft -= Time.deltaTime;
          if (timeLeft <= 0.0f) {
            
              Destroy(this.gameObject);
          }
     }
}

