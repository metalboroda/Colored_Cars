using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarSpawner : MonoBehaviour
  {
    [SerializeField] private float _spawnRateMin = 1f;
    [SerializeField] protected float _spawnRateMax = 1.5f;
    [Space]
    [SerializeField] private CarSpawnItem[] _carSpawnItems;
    [Header("Car Settings")]
    [SerializeField] private float _movementSpeedMin = 5f;
    [SerializeField] private float _movementSpeedMax = 5.25f;
    [Header("")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _movementPoint;
    [Header("Tutorial")]
    [SerializeField] private bool _tutorial;

    private void Start() {
      StartCoroutine(DoSpawnAllCarsWithRate());
    }

    private IEnumerator DoSpawnAllCarsWithRate() {
      CarSpawnItem lastSpawnedItem = null;
      int remainingCarsToSpawn = _carSpawnItems.Sum(item => item.Amount);
      CarHandler spawnedCar = null;
      CarMovementHandler spawnedCarMovement = null;

      while (remainingCarsToSpawn > 0) {
        CarSpawnItem carToSpawn = GetRandomCarItem(lastSpawnedItem);

        if (carToSpawn != null) {
          for (int i = 0; i < carToSpawn.Amount; i++) {
            spawnedCar = Instantiate(
                carToSpawn.CarPrefab, _spawnPoint.position, _spawnPoint.rotation).GetComponent<CarHandler>();

            if (spawnedCar != null) {
              spawnedCar.InitCar(carToSpawn.CarValue, carToSpawn.WordClip, _tutorial);

              spawnedCarMovement = spawnedCar.transform.GetComponent<CarMovementHandler>();
            }

            float randomMovementSpeed = Random.Range(_movementSpeedMin, _movementSpeedMax);

            if (spawnedCarMovement != null)
              spawnedCarMovement.InitMovement(randomMovementSpeed, _spawnPoint.position, _movementPoint);

            yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));

            lastSpawnedItem = carToSpawn;
            remainingCarsToSpawn--;

            if (remainingCarsToSpawn == 0)
              break;
          }
        }
      }
    }

    private CarSpawnItem GetRandomCarItem(CarSpawnItem lastSpawnedItem) {
      var availableItems = _carSpawnItems.Where(item => item != lastSpawnedItem && item.Amount > 0).ToList();

      if (availableItems.Count == 0)
        availableItems = _carSpawnItems.Where(item => item.Amount > 0).ToList();

      if (availableItems.Count == 0)
        return null;

      var randomIndex = Random.Range(0, availableItems.Count);

      return availableItems[randomIndex];
    }
  }
}