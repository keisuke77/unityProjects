using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
  public Image image;
  public Text text;
  // Start is called before the first frame update

  void Update()
  {
    if (image) image.sprite = CharactorUI.MainCharactor?.charactorResist?.ChatCharactor.icon;
    if (text) text.text = CharactorUI.MainCharactor?.charactorResist?.ChatCharactor.name;
  }
}
