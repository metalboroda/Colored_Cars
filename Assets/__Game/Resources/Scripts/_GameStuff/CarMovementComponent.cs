using __Game.Resources.Scripts.EventBus;
using DG.Tweening;
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

    public void DestroyScale() {
      _transform.DOScale(0, 0.15f);

      Object.Destroy(_transform.gameObject, 0.151f);
    }

    public void MoveToPoint() {
      _transform.DOLookAt(_movementPoint.position, 0);
      _transform.DOMove(_movementPoint.position, _movementSpeed)
        .SetSpeedBased(true)
        .OnComplete(() => {
          EventBus<EventStructs.CarCompletedTheMove>.Raise(new EventStructs.CarCompletedTheMove {
            ID = _transform.GetInstanceID(),
            CarHandler = _transform.GetComponent<CarHandler>()
          });

          DestroyScale();
        });
    }

    public void Dispose() {
      DOTween.Kill(_transform);
    }
  }
}