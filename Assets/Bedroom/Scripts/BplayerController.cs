using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditorInternal;
using UnityTutorial.Manager;



namespace UnityTutorial.PlayerControl
{
    public class BplayerController : MonoBehaviour

    {
        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;
        private Rigidbody _playerRigidbody;
        private InputManager _�nputManager;
        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
        private float _xRotation;
        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;


        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidbody = GetComponent<Rigidbody>();
            _�nputManager = GetComponent<InputManager>();

            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void LateUpdate()
        {
            CamMovements();
        }


        private void Move()
        {
            if (!_hasAnimator) return;

            float targetSpeed = _�nputManager.Run ? _runSpeed : _walkSpeed;
            if (_�nputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _�nputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _�nputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

            _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void CamMovements()
        {
            if (!_hasAnimator) return;
            var Mouse_X = _�nputManager.Look.x;
            var Mouse_Y = _�nputManager.Look.y;
            Camera.position = CameraRoot.position;

            _xRotation -= Mouse_Y * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            transform.Rotate(Vector3.up, Mouse_X * MouseSensitivity * Time.deltaTime);
           
        }
    }
}



