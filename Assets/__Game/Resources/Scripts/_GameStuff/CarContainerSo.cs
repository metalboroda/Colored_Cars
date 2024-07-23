using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  [CreateAssetMenu(fileName = "CarContainer", menuName = "SOs/Containers/CarContainer")]

  public class CarContainerSo : ScriptableObject
  {
    public CarHandler[] Cars;

    private int _lastIndex = -1;

    public CarHandler GetRandomCar() {
      if (Cars == null || Cars.Length == 0) return null;
      if (Cars.Length == 1) return Cars[0];

      int randomIndex = _lastIndex;

      while (randomIndex == _lastIndex) {
        randomIndex = Random.Range(0, Cars.Length);
      }

      _lastIndex = randomIndex;

      return Cars[randomIndex];
    }
  }
}