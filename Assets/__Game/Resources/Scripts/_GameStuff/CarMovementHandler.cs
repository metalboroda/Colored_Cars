using __Game.Resources.Scripts.EventBus;
using UnityEngine;
using static __Game.Resources.Scripts.EventBus.EventStructs;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarMovementHandler : MonoBehaviour
  {
    public CarMovementComponent CarMovementComponent;

    private EventBinding<CarCompletedTheMove> _carCompletedTheMove;

    private void Awake() {
      transform.localScale = new Vector3(0f, 0f, 0f);
    }

    private void OnDestroy() {
      CarMovementComponent.Dispose();
    }

    public void InitMovement(float movementSpeed, Vector3 startPoint, Transform movementPoint) {
      CarMovementComponent = new CarMovementComponent(movementSpeed, startPoint, movementPoint, transform);

      CarMovementComponent.SpawnScale();
      CarMovementComponent.MoveToPoint();

      EventBus<CarMovementSettings>.Raise(new CarMovementSettings {
        ID = transform.GetInstanceID(),
        MovementSpeed = movementSpeed
      });
    }
  }
}