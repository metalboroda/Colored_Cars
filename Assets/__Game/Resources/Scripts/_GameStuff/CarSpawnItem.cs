using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  [Serializable]
  public class CarSpawnItem
  {
    public GameObject CarPrefab;
    [Space]
    [Range(0f, 1f)]
    public float Posibility;
    [Space]
    public string CarValue;
    [Space]
    public AudioClip WordClip;
  }
}