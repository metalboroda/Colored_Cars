using Assets.__Game.Resources.Scripts._GameStuff;
using Assets.__Game.Scripts.Enums;
using UnityEngine;

namespace __Game.Resources.Scripts.EventBus
{
  public class EventStructs
  {
    #region FiniteStateMachine
    public struct StateChanged : IEvent
    {
      public State State;
    }
    #endregion

    #region LevelManager
    public struct LastLevelEvent : IEvent
    {
      public bool LastLevel;
    }
    #endregion

    #region Variants&Answers
    public struct VoiceButtonAudioEvent : IEvent
    {
      public AudioClip AudioClip;
    }
    public struct VariantsAssignedEvent : IEvent { }
    public struct VariantClickedEvent : IEvent
    {
      public string VariantValue;
    }
    public struct VariantAudioClickedEvent : IEvent
    {
      public AudioClip AudioClip;
    }
    public struct CorrectAnswerEvent : IEvent
    {
      public int ID;
    }
    public struct IncorrectAnswerEvent : IEvent
    {
      public int ID;
    }
    public struct QuestTextEvent : IEvent
    {
      public string QuestText;
    }

    public struct CarSettedEvent : IEvent
    {
      public int ID;
      public string CarValue;
      public AudioClip WordClip;
      public bool Tutorial;
    }

    public struct CarClickedEvent : IEvent
    {
      public int ID;
      public string CarValue;
      public AudioClip WordClip;
      public CarHandler CarHandler;
    }

    public struct CarCompletedTheMove : IEvent
    {
      public int ID;
      public CarHandler CarHandler;
    }

    public struct CarMovementSettings : IEvent
    {
      public int ID;
      public float MovementSpeed;
    }
    #endregion

    #region Game
    public struct WinEvent : IEvent { }
    public struct LoseEvent : IEvent { }
    public struct StuporEvent : IEvent { }
    public struct Win : IEvent { }
    public struct Lose : IEvent { }
    #endregion

    #region ScoreManager
    public struct LevelPointEvent : IEvent
    {
      public int LevelPoint;
    }

    public struct ScoreEvent : IEvent
    {
      public int MaxScore;
      public int CurrentScore;
    }
    #endregion

    #region Ui
    public struct LevelCounterEvent : IEvent
    {
      public int OverallLevelIndex;
    }
    public struct UiButtonEvent : IEvent
    {
      public UiEnums UiEnums;
    }
    #endregion

    #region Audio
    public struct AudioSwitchedEvent : IEvent { }
    #endregion

    #region Components
    public struct ComponentEvent<T> : IEvent
    {
      public T Data { get; set; }
    }
    #endregion

    #region Timer
    public struct TimerEvent : IEvent
    {
      public int Time;
    }
    #endregion
  }
}