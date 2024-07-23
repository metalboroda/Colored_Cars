using __Game.Resources.Scripts.EventBus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CarHandler : MonoBehaviour, IPointerClickHandler
  {
    private string _carValue;
    private AudioClip _wordClip;

    public void OnPointerClick(PointerEventData eventData) {
      EventBus<EventStructs.CarClickedEvent>.Raise(new EventStructs.CarClickedEvent {
        ID = transform.GetInstanceID(),
        CarValue = _carValue,
        WordClip = _wordClip
      });
    }

    public void InitCar(string carValue, AudioClip wordClip, bool tutorial = false) {
      _carValue = carValue;
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