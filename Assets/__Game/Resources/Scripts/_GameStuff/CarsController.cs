using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.SOs;
using UnityEngine;
using static __Game.Resources.Scripts.EventBus.EventStructs;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarsController : MonoBehaviour
  {
    [SerializeField] private CorrectValuesContainerSo _correctValuesContainer;
    [Header("")]
    [SerializeField] private int _correctAmount = 1;

    private int _correctClicksCounter;

    private EventBinding<CarClickedEvent> _carClickedEvent;

    private void OnEnable() {
      _carClickedEvent = new EventBinding<CarClickedEvent>(CheckCorrectCar);
    }

    private void OnDisable() {
      _carClickedEvent.Remove(CheckCorrectCar);
    }

    private void CheckCorrectCar(CarClickedEvent carClickedEvent) {
      foreach (var correctValue in _correctValuesContainer.CorrectValues) {
        if (carClickedEvent.CarValue.Contains(correctValue)) {
          _correctClicksCounter++;

          if (_correctClicksCounter >= _correctAmount) {

          }
          return;
        }
      }
    }
  }
}