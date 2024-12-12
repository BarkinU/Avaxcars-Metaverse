using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SwitchToggle : MonoBehaviour
{
    private int switchState = 1;
    public GameObject switchBtn;
    public CameraFollow myCamera;
    private int increase = 1;

    public void OnSwitchButtonClicked()
    {
        increase = -increase;

        switchBtn.transform.DOLocalMoveX(increase * -26.9f + 3, .3f);
        switchState = Math.Sign(increase * 26.9f);


        if (increase > 0)
        {

            myCamera.autoCameraToggle.isOn = true;

        }
        else
        {

            myCamera.autoCameraToggle.isOn = false;

        }


    }

}