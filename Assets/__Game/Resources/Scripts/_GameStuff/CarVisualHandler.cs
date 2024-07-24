using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using __Game.Resources.Scripts.EventBus;
using static __Game.Resources.Scripts.EventBus.EventStructs;
using EPOOutline;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarVisualHandler : MonoBehaviour
  {
    [Header("Wheels Settings")]
    [SerializeField] private float _rotationSpeedMultiplier = 25f;
    [Header("VFX")]
    [SerializeField] private GameObject _correctVfx;
    [SerializeField] private GameObject _incorrectVfx;
    [Header("Tutorial")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _correctColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color _incorrectColor;

    private float _rotationSpeed;
    private Transform _frontLeft;
    private Transform _rearLeft;
    private Transform _frontRight;
    private Transform _rearRight;

    private List<Transform> _wheels;

    private Outlinable _outlinable;

    private EventBinding<CarMovementSettings> _carMovementSettingsEvent;
    private EventBinding<CorrectAnswerEvent> _correctAnswerEvent;
    private EventBinding<IncorrectAnswerEvent> _incorrectAnswerEvent;

    private void Awake() {
      _outlinable = GetComponent<Outlinable>();

      _outlinable.enabled = false;

      AddWheelsToList();
    }

    private void OnEnable() {
      _carMovementSettingsEvent = new EventBinding<CarMovementSettings>(SetRotationSpeed);
      _correctAnswerEvent = new EventBinding<CorrectAnswerEvent>(OnCorrect);
      _incorrectAnswerEvent = new EventBinding<IncorrectAnswerEvent>(OnIncorrect);
    }

    private void OnDisable() {
      _carMovementSettingsEvent.Remove(SetRotationSpeed);
      _correctAnswerEvent.Remove(OnCorrect);
      _incorrectAnswerEvent.Remove(OnIncorrect);
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

    private void OnCorrect(CorrectAnswerEvent correctAnswerEvent) {
      if (correctAnswerEvent.ID == transform.GetInstanceID()) {
        Instantiate(_correctVfx, transform.position, Quaternion.identity);
      }
    }

    private void OnIncorrect(IncorrectAnswerEvent incorrectCancelEvent) {
      if (incorrectCancelEvent.ID == transform.GetInstanceID()) {
        Instantiate(_incorrectVfx, transform.position, Quaternion.identity);
      }
    }

    public void EnableTutorialGlowing(bool correct) {
      _outlinable.enabled = true;

      if (correct == true) {
        _outlinable.OutlineParameters.Color = _correctColor;
      }
      else {
        _outlinable.OutlineParameters.Color = _incorrectColor;
      }
    }
  }
}