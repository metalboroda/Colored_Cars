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
    [SerializeField] private CarSpawnRoute[] _carSpawnRoutes;
    [Header("Tutorial")]
    [SerializeField] private bool _tutorial;

    private List<GameObject> _spawnedCars = new List<GameObject>();

    private EventBinding<CarCompletedTheMove> _carCompletedTheMove;
    private EventBinding<CarClickedEvent> _carClickedEvent;

    private void OnEnable() {
      _carCompletedTheMove = new EventBinding<CarCompletedTheMove>(OnCarCompletedTheMove);
      _carClickedEvent = new EventBinding<CarClickedEvent>(OnCarClicked);
    }

    private void OnDisable() {
      _carCompletedTheMove.Remove(OnCarCompletedTheMove);
      _carClickedEvent.Remove(OnCarClicked);
    }

    private void Start() {
      StartCoroutine(SpawnCars());
    }

    private void OnCarCompletedTheMove(CarCompletedTheMove carCompletedTheMove) {
      _spawnedCars.Remove(carCompletedTheMove.CarHandler.gameObject);
    }

    private void OnCarClicked(CarClickedEvent carClickedEvent) {
      UpdateCarAmount(carClickedEvent.CarHandler.CarValue);
    }

    private IEnumerator SpawnCars() {
      while (true) {
        foreach (var carSpawnItem in _carSpawnItems) {
          for (int i = 0; i < carSpawnItem.Amount; i++) {
            if (_spawnedCars.Contains(carSpawnItem.CarPrefab)) {
              yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));
              continue;
            }

            CarSpawnRoute randomRoute = _carSpawnRoutes[Random.Range(0, _carSpawnRoutes.Length)];

            SpawnCar(carSpawnItem, randomRoute);

            yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));
          }
        }
      }
    }

    private void SpawnCar(CarSpawnItem carSpawnItem, CarSpawnRoute carSpawnRoute) {
      GameObject carObject = Instantiate(carSpawnItem.CarPrefab, carSpawnRoute.SpawnPoint.position, Quaternion.identity, transform);
      CarHandler carHandler = carObject.GetComponent<CarHandler>();
      CarMovementHandler carMovementHandler = carObject.GetComponent<CarMovementHandler>();
      CarVisualHandler carVisualHandler = carObject.GetComponent<CarVisualHandler>();

      carHandler.InitCar(carSpawnItem.CarValue, carSpawnItem.WordClip, _tutorial);
      carMovementHandler.InitMovement(_movementSpeed, carSpawnRoute.SpawnPoint.position, carSpawnRoute.MovementPoint);
      carVisualHandler.SetNumber(carSpawnItem.CarNumber);

      _spawnedCars.Add(carObject);
    }

    private void UpdateCarAmount(string carValue) {
      foreach (var carSpawnItem in _carSpawnItems) {
        if (carSpawnItem.CarValue == carValue) {
          carSpawnItem.Amount--;
          break;
        }
      }
    }
  }
}