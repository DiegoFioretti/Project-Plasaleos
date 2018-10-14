﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceHolderUIManager : MonoBehaviour {
    private void Awake() {
        Screen.sleepTimeout = 15;
    }

    public void LoadLevel1() {
        SceneManager.LoadScene("testLevel_00");
    }

    public void LoadLevel2() {
        SceneManager.LoadScene("testLevel_01");
    }

    public void LoadLevel3() {
        SceneManager.LoadScene("testLevel_02");
    }

    public void LoadLevel4() {
        SceneManager.LoadScene("testLevel_03");
    }

    public void LoadLevel5() {
        SceneManager.LoadScene("testLevel_04");
    }

    public void LoadLevel(string level) {
        SceneManager.LoadScene(level);
    }

    public void ResetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame() {
        Application.Quit();
    }
}