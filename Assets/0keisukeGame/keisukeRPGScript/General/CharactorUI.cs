using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
public class CharactorUI : MonoBehaviour
{

  public static CharactorUI MainCharactor;
  public charactorResist charactorResist;
  public Text nameText;
  public Text ExplainText;
  public Image IconImage;
  public Image Hpvar;
  public Text HpText;
  public ObjectChange modelChange;
  public ChatCharactor chatCharactor;
  public bool isMainCharactor;

  public character character;



  void MainLoop()
  {
    if (charactorResist != null)
      chatCharactor = charactorResist.ChatCharactor;
    if (character != null)
    {
      if (Hpvar != null)
        Hpvar.fillAmount = character.HPRate();
      if (HpText != null)

        HpText.text = (character.HPRate() * 100).ToString("F0") + "%";

    }
    else
    {

      if (Hpvar != null)

        Hpvar.fillAmount = chatCharactor.HpPer;


      if (HpText != null)

        HpText.text = (chatCharactor.HpPer * 100).ToString("F0") + "%";


    }




    if (nameText != null)
    {
      nameText.text = chatCharactor.name;

    }
    if (IconImage != null)
    {
      IconImage.sprite = chatCharactor.icon;

    }
    if (ExplainText != null)
    {
      ExplainText.text = chatCharactor.Explain;

    }
    if (modelChange?.objList != null)
    {
      modelChange.ChangeByName(chatCharactor.name);
    }

  }
  public void MainSet()
  {
    var temp = MainCharactor.charactorResist;


    MainCharactor.charactorResist = charactorResist;

    charactorResist = temp;

    MainCharactor.charactorResist.Set();
    MainLoop();
  }
  void Start()
  {

    if (isMainCharactor
  )
    {
      MainCharactor = this;
      MainSet();
    }
  }
  private void Update()
  {
    MainLoop();
  }


}
