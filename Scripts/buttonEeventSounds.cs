using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonEeventSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{


    public void OnPointerEnter(PointerEventData eventData)
    {
        soundManager.playSound(gameAssets.getInstance().buttonHover);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        soundManager.playSound(gameAssets.getInstance().buttonClick);
    }
}
