using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private Image _promptImage;
    [SerializeField] private Vector3 _initialScale = Vector3.one;
    [SerializeField] private Vector3 _targetScale = Vector3.one * 1.5f;
    [SerializeField] private float _animationDuration = 1.0f;
    private Transform _targetTransform;

    private Coroutine _currentAnimation;

    private void Start()
    {
        _mainCam = Camera.main;
        _uiPanel.SetActive(false);
        _uiPanel.transform.localScale = _initialScale; 
    }

    private void LateUpdate()
    {
        if (_mainCam == null)
        {
            _mainCam = Camera.main;
            if (_mainCam == null) return; 
        }

        if (_targetTransform != null)
        {
            Vector3 screenPosition = _mainCam.WorldToScreenPoint(_targetTransform.position + Vector3.up * 1f);
            _uiPanel.transform.position = screenPosition;
        }
    }

    public bool IsDisplayed { get; private set; } = false;

    public void SetUp(Transform targetTransform)
    {
        _targetTransform = targetTransform;
        _uiPanel.SetActive(true);
        IsDisplayed = true;

        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
        }
        _currentAnimation = StartCoroutine(AnimatePrompt());
    }

    public void Close()
    {
        _uiPanel.SetActive(false);
        IsDisplayed = false;

        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _uiPanel.transform.localScale = _initialScale; 
        }
    }

    private IEnumerator AnimatePrompt()
    {
        float elapsedTime = 0f;
        while (true)
        {
            while (elapsedTime < _animationDuration)
            {
                _uiPanel.transform.localScale = Vector3.Lerp(_initialScale, _targetScale, (Mathf.Sin(elapsedTime / _animationDuration * Mathf.PI * 2) + 1) / 2);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;
        }
    }
}
