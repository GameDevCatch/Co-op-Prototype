using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{

    public float delay;

    private Rigidbody _rb;
    private bool _fired;
    private float _moveSpeed;

    private int _ownerID;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Invoke("Destroy", delay);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (_fired)
            Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInput input))
        {
            if (_ownerID != input.playerIndex)
                input.GetComponent<Player>().Kill();
        }
        else if (other.CompareTag("Ground"))
            Destroy();
    }

    public void Fire(float speed, Vector3 dir, int playerID)
    {
        _fired = true;
        _moveSpeed = speed;
        _ownerID = playerID;

        transform.up = dir;
    }

    private void Move()
    {
        _rb.velocity = _moveSpeed * 2f * Time.deltaTime * transform.up;
    }
}