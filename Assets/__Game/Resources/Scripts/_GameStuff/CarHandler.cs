using __Game.Resources.Scripts.EventBus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarHandler : MonoBehaviour, IPointerClickHandler
  {
    public string CarValue { get; private set; }

    private AudioClip _wordClip;

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
    }
  }
}