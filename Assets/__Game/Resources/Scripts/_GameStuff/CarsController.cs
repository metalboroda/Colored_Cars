using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Resources.Scripts.SOs;
using Assets.__Game.Scripts.Infrastructure;
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

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<CarClickedEvent> _carClickedEvent;

    private void Awake() {
      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable() {
      _carClickedEvent = new EventBinding<CarClickedEvent>(CheckCorrectCar);
    }

    private void OnDisable() {
      _carClickedEvent.Remove(CheckCorrectCar);
    }

    private void Start() {
      EventBus<ScoreEvent>.Raise(new ScoreEvent {
        CurrentScore = _correctClicksCounter,
        MaxScore = _correctAmount
      });

      EventBus<VariantsAssignedEvent>.Raise(new VariantsAssignedEvent());
    }

    private void CheckCorrectCar(CarClickedEvent carClickedEvent) {
      foreach (var correctValue in _correctValuesContainer.CorrectValues) {
        if (carClickedEvent.CarValue.Contains(correctValue)) {
          _correctClicksCounter++;

          EventBus<CorrectAnswerEvent>.Raise(new CorrectAnswerEvent {
            ID = carClickedEvent.CarHandler.transform.GetInstanceID()
          });

          if (_correctClicksCounter >= _correctAmount) {
            _correctClicksCounter = _correctAmount;

            if (_gameBootstrapper != null)
              _gameBootstrapper.StateMachine.ChangeStateWithDelay(new GameWinState(_gameBootstrapper), 1f, this);
          }

          EventBus<ScoreEvent>.Raise(new ScoreEvent {
            CurrentScore = _correctClicksCounter,
            MaxScore = _correctAmount
          });
          return;
        }
      }

      EventBus<IncorrectAnswerEvent>.Raise(new IncorrectAnswerEvent {
        ID = carClickedEvent.CarHandler.transform.GetInstanceID()
      });

      if (_gameBootstrapper != null)
        _gameBootstrapper.StateMachine.ChangeStateWithDelay(new GameLoseState(_gameBootstrapper), 1f, this);
    }
  }
}