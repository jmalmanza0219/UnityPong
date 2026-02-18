using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class StartGameUI : MonoBehaviour
{
    public Button startButton;   
    public TextMeshProUGUI buttonText;

  void Update()
{
    if (NetworkManager.Singleton == null) return;
    if (!NetworkManager.Singleton.IsListening) return;

    // Clients never see it
    if (!NetworkManager.Singleton.IsServer)
    {
        if (startButton.gameObject.activeSelf)
            startButton.gameObject.SetActive(false);
        return;
    }

    // Server: wait for GameManager
    if (GameManager.Instance == null)
    {
        if (startButton.gameObject.activeSelf)
            startButton.gameObject.SetActive(false);
        return;
    }

    bool shouldShow = !GameManager.Instance.gameStarted.Value || GameManager.Instance.gameOver.Value;

    if (startButton.gameObject.activeSelf != shouldShow)
        startButton.gameObject.SetActive(shouldShow);

    if (buttonText != null)
        {
            buttonText.text = GameManager.Instance.gameOver.Value ? "Restart Game" : "Start Game";
        }
}

public void OnStartButtonPressed()
    {
        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsServer) return;
        if (GameManager.Instance == null) return;

        GameManager.Instance.StartGame();

        // Hide immediately; Update() will keep it hidden while game is running
        startButton.gameObject.SetActive(false);
    }
}

