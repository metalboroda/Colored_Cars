using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarMovementComponent
  {
    private float _movementSpeed;
    private Vector3 _startPoint;
    private Transform _movementPoint;
    private Transform _transform;

    public CarMovementComponent(float movementSpeed, Vector3 startPoint, Transform movementPoint, Transform transform) {
      _movementSpeed = movementSpeed;
      _startPoint = startPoint;
      _movementPoint = movementPoint;
      _transform = transform;
    }

    public void SpawnScale() {
      _transform.DOScale(1, 0.15f);
    }

    public void MoveToPoint() {
      _transform.DOLookAt(_movementPoint.position, 0);
      _transform.DOMove(_movementPoint.position, _movementSpeed)
        .SetSpeedBased(true);
    }

    public void Dispose() {
      DOTween.Kill(_transform);
    }
  }
}