using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRoll : MonoBehaviour
{

    [SerializeField]
    private float rollSpeed;

    private Rigidbody _rb;
    private float _rollDir;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Rotate(_rollDir, rollSpeed);
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        _rollDir = context.ReadValue<Vector2>().x;
    }

    private void Rotate(float xDir, float speed)
    {
        _rb.AddTorque(Vector3.forward * -xDir * speed * Time.deltaTime);
    }
}
