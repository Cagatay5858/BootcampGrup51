using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private float _distanceFromCamera = 8.0f; 
    [SerializeField] private float _minHeight = 4.0f; 

    private void Start()
    {
        _mainCam = Camera.main;
        _uiPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        if (_mainCam == null)
        {
            _mainCam = Camera.main;
            if (_mainCam == null) return; 
        }

        var rotation = _mainCam.transform.rotation;
        Vector3 targetPosition = _mainCam.transform.position + _mainCam.transform.forward * _distanceFromCamera;

        if (targetPosition.y < _minHeight)
        {
            targetPosition.y = _minHeight;
        }

        transform.position = targetPosition;
        transform.rotation = rotation;

       
        float distance = Vector3.Distance(_mainCam.transform.position, transform.position);
        float scale = Mathf.Clamp(distance / 10.0f, 0.5f, 1.0f);
        _uiPanel.transform.localScale = new Vector3(scale, scale, scale);
    }

    public bool IsDisplayed = false;

    public void SetUp(string promptText)
    {
        _promptText.text = promptText; 
        _uiPanel.SetActive(true);
        IsDisplayed = true;
    }

    public void Close()
    {
        _uiPanel.SetActive(false);
        IsDisplayed = false; 
    }
}