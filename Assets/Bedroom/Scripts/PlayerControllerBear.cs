using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditorInternal;
using UnityTutorial.Manager;

namespace UnityTutorial.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _playerRigidbody;
        private InputManager _�nputManager;
        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
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

        private void Move()
        {
            if(!_hasAnimator) return;

            float targetSpeed = _�nputManager.Run ? _runSpeed : _walkSpeed;
            if (_�nputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            _currentVelocity.x = targetSpeed * _�nputManager.Move.x;
            _currentVelocity.y = targetSpeed * _�nputManager.Move.y;

            var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

            _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }
    }
}

