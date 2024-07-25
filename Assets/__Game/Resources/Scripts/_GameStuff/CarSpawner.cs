using __Game.Resources.Scripts.EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static __Game.Resources.Scripts.EventBus.EventStructs;

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

    private List<CarHandler> _spawnedCars = new List<CarHandler>();

    private EventBinding<CarCompletedTheMove> _carCompletedTheMove;

    private void OnEnable() {
      _carCompletedTheMove = new EventBinding<CarCompletedTheMove>(OnCarCompletedTheMove);
    }

    private void OnDisable() {
      _carCompletedTheMove.Remove(OnCarCompletedTheMove);
    }

    private void Start() {
      StartCoroutine(SpawnCars());
    }

    private void OnCarCompletedTheMove(CarCompletedTheMove carCompletedTheMove) {
      _spawnedCars.Remove(carCompletedTheMove.CarHandler);
    }

    private IEnumerator SpawnCars() {
      while (true) {
        foreach (var carSpawnItem in _carSpawnItems) {
          for (int i = 0; i < carSpawnItem.Amount; i++) {
            if (_spawnedCars.Contains(carSpawnItem.CarPrefab.GetComponent<CarHandler>())) {
              yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));
              continue;
            }

            SpawnCar(carSpawnItem);

            yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));
          }
        }
      }
    }

    private void SpawnCar(CarSpawnItem carSpawnItem) {
      GameObject carObject = Instantiate(carSpawnItem.CarPrefab, _spawnPoint.position, Quaternion.identity);
      CarHandler carHandler = carObject.GetComponent<CarHandler>();
      CarMovementHandler carMovementHandler = carObject.GetComponent<CarMovementHandler>();

      carHandler.InitCar(carSpawnItem.CarValue, carSpawnItem.WordClip, _tutorial);
      carMovementHandler.InitMovement(_movementSpeed, _spawnPoint.position, _movementPoint);

      _spawnedCars.Add(carHandler);
    }
  }
}