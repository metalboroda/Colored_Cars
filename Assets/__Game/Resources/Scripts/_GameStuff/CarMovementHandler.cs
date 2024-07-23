using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarMovementHandler : MonoBehaviour
  {
    public CarMovementComponent CarMovementComponent;

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
    }
  }
}