using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  [Serializable]
  public class CarSpawnItem
  {
    public GameObject CarPrefab;
    [Space]
    public string CarValue;
    public int Amount;
    [Space]
    public AudioClip WordClip;
  }
}