using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance { get; private set; }
    
    public Camera _camera;
    private Vector3 _cameraRotation;

    private Vector3 _defaultRotation;

    public bool IsActive = true;
    [SerializeField] private float _maxRotation = 4f;

    [Space(10)]
    [SerializeField] private float _turnThreshold = 100f;
    [SerializeField] private float _turnAmount = 90f;
    [SerializeField] private float _turnTime = 0.2f;

    [Space(10)]
    [SerializeField] private Animator _bookAnimator;
    [SerializeField] private float _bookCooldown = 0.5f;
    private float _bookCooldownTime;
    private bool _bookActive = false;
    private bool _bookBuffer = false;

    [Space(10)]
    [SerializeField] private float _turnSFXVolume = 0.75f;
    [SerializeField] private AudioClip _turnRightSFX;
    [SerializeField] private AudioClip _turnLeftSFX;
    [SerializeField] private AudioClip _bookGrabSFX;
    [SerializeField] private AudioClip _bookPutAwaySFX;

    [Space(10)]
    [SerializeField] private GameObject _turnIndicatorDown;
    [SerializeField] private GameObject _turnIndicatorRight;
    [SerializeField] private GameObject _turnIndicatorLeft;

    public bool CanHover = false;

    private bool _isTurned = false;
    private bool _isTurning = false;
    private Coroutine _turnCoroutine;

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _defaultRotation = transform.eulerAngles;
        _cameraRotation = _camera.transform.localEulerAngles;

        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        float mouseX = Mathf.Clamp(Input.mousePosition.x, 0, Screen.width);
        float mouseY = Mathf.Clamp(Input.mousePosition.y, 0, Screen.height);
        MouseCameraRotate(mouseX, mouseY);

        if (!IsActive)
            return;

        if (CanHover)
        {
            if (mouseX > Screen.width - _turnThreshold)
                Turn(true);
            else if (mouseX < _turnThreshold)
                Turn(false);

            if (mouseY < _turnThreshold && !_bookBuffer)
            {
                ToggleBook();
                _bookBuffer = true;
            }
            else if (mouseY > _turnThreshold)
            {
                _bookBuffer = false;
            }
            _bookCooldownTime = _bookCooldownTime <= 0 ? 0 : _bookCooldownTime - Time.deltaTime;
        }
    }

    private void MouseCameraRotate(float x, float y)
    {
        x = 1 - (x / (Screen.width / 2));
        float rotValueHorizontal = _cameraRotation.y - _maxRotation * x;

        y = 1 - (y / (Screen.height / 2));
        y = -y; // inverted because idk
        float rotValueVertical = _cameraRotation.x - _maxRotation * y;

        _camera.transform.localRotation = Quaternion.Euler(rotValueVertical, rotValueHorizontal, _cameraRotation.z);
    }

    private void Turn(bool turned)
    {
        if (_isTurning)
            return;
        else if (_bookActive)
            return;
        else if (turned && _isTurned || !turned && !_isTurned)
            return;

        float targetRot = turned ? _defaultRotation.y + _turnAmount : _defaultRotation.y;

        if (_defaultRotation.y + _turnAmount > 360 && !turned)
        {
            // I dont get why this works but a simple "_defaultRotation.y - 360" doesn't. Someone PLEASE throw a brick at Unity's CEO.
            targetRot = (_defaultRotation.y + _turnAmount) - 360 - _turnAmount;
        }
        // Debug.Log("turning: " + targetRot);

        ToggleUI(turned, !turned, !turned);
        AudioManager.Instance.PlaySound(turned ? _turnRightSFX : _turnLeftSFX, _turnSFXVolume, true);

        if (_turnCoroutine != null)
            StopCoroutine(_turnCoroutine);
        _turnCoroutine = StartCoroutine(TurnCoroutine(targetRot));
    }

    private IEnumerator TurnCoroutine(float targetRot)
    {
        _isTurning = true;
        
        WaitForEndOfFrame wait = new();
        float currentRot = transform.eulerAngles.y;
        float t = 0f;

        // Debug.Log("stuff");

        while (t < _turnTime)
        {
            float newRot = Mathf.Lerp(currentRot, targetRot, t / _turnTime);
            transform.rotation = Quaternion.Euler(_defaultRotation.x, newRot, _defaultRotation.z);

            t += Time.deltaTime;
            yield return wait;
        }

        transform.rotation = Quaternion.Euler(_defaultRotation.x, targetRot, _defaultRotation.z);

        _isTurning = false;
        _isTurned = targetRot > currentRot;
    }

    private void ToggleBook()
    {
        if (_isTurned || _isTurning || _bookCooldownTime > 0)
            return;

        if (!_bookActive)
        {
            _bookAnimator.SetTrigger("Enter");
            AudioManager.Instance.PlaySound(_bookGrabSFX, 1, true);
            ToggleUI(false, false, true);
        }
        if (_bookActive)
        {
            _bookAnimator.SetTrigger("Exit");
            AudioManager.Instance.PlaySound(_bookPutAwaySFX, 0.75f, true);
            ToggleUI(false, true, true);
        }


        _bookActive = !_bookActive;
        _bookCooldownTime = _bookCooldown;
    }
    public void ForceRemoveBook()
    {
        if (_bookActive)
            ToggleBook();
    }

    public void ToggleUI(bool leftVisible, bool rightVisible, bool downVisible)
    {
        _turnIndicatorLeft.SetActive(leftVisible);
        _turnIndicatorRight.SetActive(rightVisible);
        _turnIndicatorDown.SetActive(downVisible);
    }
}
