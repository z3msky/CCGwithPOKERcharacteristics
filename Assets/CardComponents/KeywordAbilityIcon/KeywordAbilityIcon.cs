using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeywordAbilityIcon : InfoPanelItem
{
    public Image IconImage;

    public void Setup(KeywordAbilityData data)
    {
        IconImage.sprite = data.IconSprite;
        InfoPanelSprite = data.IconSprite;
        InfoPanelMessage = ZTMPHelper.Bold(data.name) + "\n" + data.ExplainerText;
    }
}
