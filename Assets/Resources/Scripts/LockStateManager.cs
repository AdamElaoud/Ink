using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStateManager : MonoBehaviour {

    [SerializeField]
    GameObject key;

    void Update() {
        if (key != null)
            LockCheck();
    }

    void LockCheck() {
        if (!key.activeSelf)
            gameObject.SetActive(false);
    }
}
