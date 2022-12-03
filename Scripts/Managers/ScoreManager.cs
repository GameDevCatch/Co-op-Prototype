using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI player_1_ScoreText, player_2_ScoreText;
    [SerializeField]
    private int scoreToWin;

    [SerializeField] [ReadOnly]
    private int _player_1_Score, _player_2_Score;

    public event Action<int> OnWin;

    public static ScoreManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += (x) => { x.GetComponent<Player>().OnKilled += Scored; };
    }

    private void Scored(int playerID)
    {
        switch (playerID)
        {
            case 0:
                _player_2_Score++;
                player_2_ScoreText.SetText(_player_2_Score.ToString("0"));
                break;

            case 1:
                _player_1_Score++;
                player_1_ScoreText.SetText(_player_1_Score.ToString("0"));
                break;

            default:
                break;
        }

        if (_player_1_Score >= scoreToWin)
            OnWin?.Invoke(0);
        else if (_player_2_Score >= scoreToWin)
            OnWin?.Invoke(1);
    }
}