using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    ParticleSystem ps;
    void Start() {
        ps = GetComponent<ParticleSystem>();

        if (!ps.isPlaying) {
            print("particles playing");
            ps.Play();
        }
    }
}
