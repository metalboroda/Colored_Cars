using Assets.__Game.Resources.Scripts.SOs;
using System.Collections;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarSpawner : MonoBehaviour
  {
    [SerializeField] private CorrectValuesContainerSo _correctValuesContainer;
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
          if (Instantiate(carToSpawn.CarPrefab, _spawnPoint.position, _spawnPoint.rotation).TryGetComponent<CarHandler>(out var spawnedCar)) {
            spawnedCar.InitCar(carToSpawn.CarValue, carToSpawn.WordClip, _tutorial);

            if (spawnedCar.TryGetComponent<CarMovementHandler>(out var spawnedCarMovement))
              spawnedCarMovement.InitMovement(_movementSpeed, _spawnPoint.position, _movementPoint);
          }
          yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));
        }
      }
    }

    private CarSpawnItem GetRandomCarItem() {
      return _carSpawnItems[Random.Range(0, _carSpawnItems.Length)];
    }
  }
}