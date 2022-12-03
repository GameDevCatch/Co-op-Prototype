using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //For use by other scripts
    public Color player_1_Color, player_2_Color;
    //For use by other scripts
    public Transform player_1_SpawnPoint, player_2_SpawnPoint;

    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private TextMeshProUGUI winnerText;

    [SerializeField]
    private TextMeshProUGUI player_1_ScoreText, player_2_ScoreText;

    private PlayerInput _player_1, _player_2;

    public static GameManager Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += (x) => { SetupPlayer(x); };
        ScoreManager.Instance.OnWin += Won;
    }

    private void SetupPlayer(PlayerInput player)
    {
        if (_player_1 == null)
        {
            _player_1 = player;
            _player_1.name = "Player_1";
            _player_1.transform.position = player_1_SpawnPoint.position;
            _player_1.GetComponent<MeshRenderer>().material.color = player_1_Color;
            player_1_ScoreText.color = player_1_Color;
        }
        else
        {
            _player_2 = player;
            _player_2.name = "Player_2";
            _player_2.transform.position = player_2_SpawnPoint.position;
            _player_2.GetComponent<MeshRenderer>().material.color = player_2_Color;
            player_2_ScoreText.color = player_2_Color;
        }
    }

    private void Won(int winnerID)
    {
        Time.timeScale = .4f;
        PlayerInputManager.instance.DisableJoining();
        _player_1.DeactivateInput();
        _player_2.DeactivateInput();
        winnerText.color = (winnerID == 0 ? player_1_Color : player_2_Color);
        winScreen.SetActive(true);
        winnerText.SetText("Player " + (winnerID == 0 ? 1 : 2) + " Wins!");
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}