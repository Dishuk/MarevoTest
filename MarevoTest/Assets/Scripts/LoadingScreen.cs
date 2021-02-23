using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [ SerializeField] private SceneController sceneController;

    void Awake() {
        sceneController.LoadScene("MainScene");
    }
}
