using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.SOs;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarHandler : MonoBehaviour, IPointerClickHandler
  {
    [SerializeField] private CorrectValuesContainerSo _correctValuesContainer;

    public string CarValue { get; private set; }

    private AudioClip _wordClip;

    private CarVisualHandler _carVisualHandler;

    private void Awake() {
      _carVisualHandler = GetComponent<CarVisualHandler>();
    }

    public void OnPointerClick(PointerEventData eventData) {
      EventBus<EventStructs.CarClickedEvent>.Raise(new EventStructs.CarClickedEvent {
        ID = transform.GetInstanceID(),
        CarValue = CarValue,
        WordClip = _wordClip,
        CarHandler = this
      });

      Destroy(gameObject);
    }

    public void InitCar(string carValue, AudioClip wordClip, bool tutorial = false) {
      CarValue = carValue;
      _wordClip = wordClip;

      EventBus<EventStructs.CarSettedEvent>.Raise(new EventStructs.CarSettedEvent {
        ID = transform.GetInstanceID(),
        CarValue = carValue,
        WordClip = _wordClip,
        Tutorial = tutorial
      });

      bool isCorrectValue = Array.Exists(_correctValuesContainer.CorrectValues, value => value == carValue);

      _carVisualHandler.EnableTutorialGlowing(isCorrectValue);
    }
  }
}