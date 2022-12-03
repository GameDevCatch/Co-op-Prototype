using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{

    [Header("Values")]

    [SerializeField]
    private GameObject projectileMax;
    [SerializeField]
    private GameObject projectileNormal;
    [Space]
    [SerializeField]
    private Vector3 chargedSize;
    [SerializeField]
    private Vector3 normalSize;
    [Space]
    [SerializeField]
    private float reloadDuration;
    [SerializeField]
    private float chargeDuration;
    [Space]
    [SerializeField]
    private float minRecoilForce;
    [SerializeField]
    private float maxRecoilForce;
    [Space]
    [SerializeField]
    private float normShotSpeed;
    [Space]
    [SerializeField]
    private float maxShotSpeed;
    [Space]
    [SerializeField]
    private float minDrag;
    [SerializeField]
    private float maxDrag;

    [Header("DEBUG")]

    [SerializeField] [ReadOnly]
    private float _charge;
    [SerializeField] [ReadOnly]
    private bool _canShoot = true;
    [SerializeField] [ReadOnly]
    private bool _shotQued;

    private PlayerInput _playerInput;
    private PlayerLaser _playerLaser;
    private Rigidbody _rb;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerLaser = GetComponent<PlayerLaser>();
        _rb = GetComponent<Rigidbody>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_canShoot)
                StartCoroutine(nameof(Charge));
            else
                _shotQued = true;
        }

        if (context.canceled)
        {
            if (_charge > 0 && _canShoot)
            {
                StopCoroutine(nameof(Charge));
                Shoot();
            }

            if (_shotQued)
                _shotQued = false;
        }
    }

    private IEnumerator Charge()
    {
        float time = 0;

        while (time < chargeDuration)
        {
            _charge = Mathf.Lerp(0, 1, time / chargeDuration);
            transform.localScale = Vector3.Lerp(normalSize, chargedSize, time / chargeDuration);
            _rb.drag = Mathf.Lerp(minDrag, maxDrag, time / chargeDuration);
            _playerLaser.length = Mathf.Lerp(_playerLaser.minLength, _playerLaser.maxLength, time / chargeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        _charge = 1f;
        transform.localScale = chargedSize;
        _rb.drag = maxDrag;

        StopCoroutine(nameof(Charge));
    }

    private IEnumerator Reload()
    {
        float time = 0;
        float startCharge = _charge;
        float startLength = _playerLaser.length;
        Vector3 startSize = transform.localScale;

        while (time < reloadDuration)
        {
            _charge = Mathf.Lerp(startCharge, 0, time / reloadDuration);
            transform.localScale = Vector3.Lerp(startSize, normalSize, time / reloadDuration);
            _playerLaser.length = Mathf.Lerp(startLength, _playerLaser.minLength, time / reloadDuration);
            time += Time.deltaTime;
            yield return null;
        }

        _charge = 0f;
        transform.localScale = normalSize;
        _canShoot = true;

        if (_shotQued)
        {
            StartCoroutine(nameof(Charge));
            _shotQued = false;
        }

        StopCoroutine(nameof(Reload));
    }

    private void Shoot()
    {
        _rb.drag = minDrag;

        var recoil = Mathf.Lerp(minRecoilForce, maxRecoilForce, _charge);
        var speed = 0f;
        GameObject prjct = null;

        if (_charge >= .9f)
        {
            speed = maxShotSpeed;
            prjct = Instantiate(projectileMax, transform.position, Quaternion.identity);
        }
        else
        {
            speed = normShotSpeed;
            prjct = Instantiate(projectileNormal, transform.position, Quaternion.identity);
        }
          
        prjct.GetComponent<Projectile>().Fire(speed, transform.up, _playerInput.playerIndex);
        prjct.GetComponent<MeshRenderer>().material.color = _playerInput.playerIndex == 0 ? GameManager.Instance.player_1_Color : GameManager.Instance.player_2_Color;
        _rb.AddForce(-transform.up.normalized * recoil, ForceMode.Impulse);

        _canShoot = false;
        StartCoroutine(nameof(Reload));
    }
}