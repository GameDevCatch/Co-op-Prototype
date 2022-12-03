using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{

    public GameObject shatter;

    public event Action<int> OnKilled;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void Kill()
    {
        OnKilled?.Invoke(_playerInput.playerIndex);
        var shatterEffect = Instantiate(shatter, transform.position, Quaternion.identity);
        shatterEffect.GetComponent<Exploder>().Trigger(_playerInput.playerIndex == 0 ? GameManager.Instance.player_1_Color : GameManager.Instance.player_2_Color);
        Destroy(gameObject);
    }
}