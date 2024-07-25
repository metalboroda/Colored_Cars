using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  [Serializable]
  public class CarSpawnItem
  {
    public GameObject CarPrefab;
    [Space]
    public int CarNumber;
    public string CarValue;
    [Space]
    public int Amount;
    [Space]
    public AudioClip WordClip;
  }
}