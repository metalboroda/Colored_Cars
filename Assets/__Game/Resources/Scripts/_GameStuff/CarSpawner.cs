using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.SOs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private float _movementSpeedMin = 5f;
    [SerializeField] private float _movementSpeedMax = 5.25f;
    [Header("")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _movementPoint;
    [Header("Tutorial")]
    [SerializeField] private bool _tutorial;

    private HashSet<string> _clickedCarValues = new HashSet<string>();
    private Dictionary<string, int> _remainingCorrectCars = new Dictionary<string, int>();
    private Dictionary<string, int> _activeCorrectCars = new Dictionary<string, int>();

    private EventBinding<EventStructs.CarClickedEvent> _carClickedEvent;
    private EventBinding<EventStructs.CarCompletedTheMove> _carCompletedEvent;

    private void Start() {
      foreach (var carItem in _carSpawnItems) {
        if (_correctValuesContainer.CorrectValues.Contains(carItem.CarValue)) {
          _remainingCorrectCars[carItem.CarValue] = carItem.Amount;
          _activeCorrectCars[carItem.CarValue] = 0;
        }
      }

      StartCoroutine(DoSpawnCarsContinuously());
    }

    private void OnEnable() {
      _carClickedEvent = new EventBinding<EventStructs.CarClickedEvent>(OnCarClicked);
      _carCompletedEvent = new EventBinding<EventStructs.CarCompletedTheMove>(OnCarCompleted);
    }

    private void OnDisable() {
      _carClickedEvent.Remove(OnCarClicked);
      _carCompletedEvent.Remove(OnCarCompleted);
    }

    private void OnCarClicked(EventStructs.CarClickedEvent e) {
      if (_correctValuesContainer.CorrectValues.Contains(e.CarValue)) {
        _clickedCarValues.Add(e.CarValue);
      }
    }

    private void OnCarCompleted(EventStructs.CarCompletedTheMove e) {
      var car = FindObjectsOfType<CarHandler>().FirstOrDefault(ch => ch.transform.GetInstanceID() == e.ID);
      if (car != null && _correctValuesContainer.CorrectValues.Contains(car.CarValue)) {
        _activeCorrectCars[car.CarValue]--;
      }
    }

    private IEnumerator DoSpawnCarsContinuously() {
      while (true) {
        CarSpawnItem carToSpawn = GetRandomCarItem();

        if (carToSpawn != null) {
          if (Instantiate(carToSpawn.CarPrefab, _spawnPoint.position, _spawnPoint.rotation).TryGetComponent<CarHandler>(out var spawnedCar)) {
            spawnedCar.InitCar(carToSpawn.CarValue, carToSpawn.WordClip, _tutorial);

            if (_correctValuesContainer.CorrectValues.Contains(carToSpawn.CarValue)) {
              _activeCorrectCars[carToSpawn.CarValue]++;
            }

            CarMovementHandler spawnedCarMovement = spawnedCar.GetComponent<CarMovementHandler>();

            float randomMovementSpeed = Random.Range(_movementSpeedMin, _movementSpeedMax);

            if (spawnedCarMovement != null) {
              spawnedCarMovement.InitMovement(randomMovementSpeed, _spawnPoint.position, _movementPoint);
            }
          }

          yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));
        }
      }
    }

    private CarSpawnItem GetRandomCarItem() {
      var availableItems = _carSpawnItems
          .Where(item => (!_clickedCarValues.Contains(item.CarValue) &&
                          (!_remainingCorrectCars.ContainsKey(item.CarValue) || _remainingCorrectCars[item.CarValue] > _activeCorrectCars[item.CarValue])) ||
                         !_correctValuesContainer.CorrectValues.Contains(item.CarValue))
          .ToList();

      if (availableItems.Count == 0)
        return null;

      var randomIndex = Random.Range(0, availableItems.Count);
      return availableItems[randomIndex];
    }
  }
}