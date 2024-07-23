using System.Collections;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarSpawner : MonoBehaviour
  {
    [Header("Settings")]
    [SerializeField] private float _spawnRateMin = 1f;
    [SerializeField] private float _spawnRateMax = 1.5f;
    [Space]
    [SerializeField] private CarSpawnItem[] _carSpawnItems;
    [Header("Car Settings")]
    [SerializeField] private float _movementSpeed = 5f;
    [Header("")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _movementPoint;
    [Header("Tutorial")]
    [SerializeField] private bool _tutorial;

    private void Start() {
      StartCoroutine(DoSpawnCarsContinuously());
    }

    private IEnumerator DoSpawnCarsContinuously() {
      while (true) {
        CarSpawnItem carToSpawn = GetRandomCarItem();

        if (carToSpawn != null) {
          if (Instantiate(carToSpawn.CarPrefab, _spawnPoint.position, _spawnPoint.rotation, transform)
            .TryGetComponent<CarHandler>(out var spawnedCar)) {
            spawnedCar.InitCar(carToSpawn.CarValue, carToSpawn.WordClip, _tutorial);

            if (spawnedCar.TryGetComponent<CarMovementHandler>(out var spawnedCarMovement))
              spawnedCarMovement.InitMovement(_movementSpeed, _spawnPoint.position, _movementPoint);
          }
          yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));
        }
      }
    }

    private CarSpawnItem GetRandomCarItem() {
      float totalPosibility = 0f;

      foreach (var item in _carSpawnItems) {
        if (item.Posibility > 0) {
          totalPosibility += item.Posibility;
        }
      }

      if (totalPosibility == 0) {
        return null;
      }

      float randomPoint = Random.value * totalPosibility;

      foreach (var item in _carSpawnItems) {
        if (item.Posibility > 0) {
          if (randomPoint < item.Posibility) {
            return item;
          }
          else {
            randomPoint -= item.Posibility;
          }
        }
      }
      return null;
    }
  }
}