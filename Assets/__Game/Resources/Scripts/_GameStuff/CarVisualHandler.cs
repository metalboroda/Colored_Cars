﻿using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using __Game.Resources.Scripts.EventBus;
using static __Game.Resources.Scripts.EventBus.EventStructs;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarVisualHandler : MonoBehaviour
  {
    [SerializeField] private float _rotationSpeedMultiplier = 25f;

    private float _rotationSpeed;
    private Transform _frontLeft;
    private Transform _rearLeft;
    private Transform _frontRight;
    private Transform _rearRight;

    private List<Transform> _wheels;

    private EventBinding<CarMovementSettings> _carMovementSettingsEvent;

    private void Awake() {
      AddWheelsToList();
    }

    private void OnEnable() {
      _carMovementSettingsEvent = new EventBinding<CarMovementSettings>(SetRotationSpeed);
    }

    private void OnDisable() {
      _carMovementSettingsEvent.Remove(SetRotationSpeed);
    }

    private void Start() {
      RotateWheels();
    }

    private void OnDestroy() {
      foreach (Transform wheel in _wheels) {
        DOTween.Kill(wheel);
      }
    }

    private void AddWheelsToList() {
      _wheels = new List<Transform>();

      foreach (Transform child in transform) {
        if (child.name.Contains("FL")) {
          _frontLeft = child;
          _wheels.Add(_frontLeft);
        }
        else if (child.name.Contains("RL")) {
          _rearLeft = child;
          _wheels.Add(_rearLeft);
        }
        else if (child.name.Contains("FR")) {
          _frontRight = child;
          _wheels.Add(_frontRight);
        }
        else if (child.name.Contains("RR")) {
          _rearRight = child;
          _wheels.Add(_rearRight);
        }
      }
    }

    private void SetRotationSpeed(CarMovementSettings carMovementSettings) {
      if (carMovementSettings.ID == transform.GetInstanceID()) {
        _rotationSpeed = carMovementSettings.MovementSpeed * _rotationSpeedMultiplier;
      }
    }

    private void RotateWheels() {
      foreach (Transform wheel in _wheels) {
        float initialYRotation = wheel.localEulerAngles.y;
        float initialZRotation = wheel.localEulerAngles.z;

        float rotationDirection = (wheel == _frontRight || wheel == _rearRight) ? -1 : 1;

        wheel.DOLocalRotate(new Vector3(360 * rotationDirection, initialYRotation, initialZRotation),
          _rotationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetSpeedBased(true);
      }
    }
  }
}