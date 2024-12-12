using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeView : View
{
    [SerializeField] private Button _backButton;
    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _backButton.onClick.AddListener(() => UpgradeClose());
    }

    public void UpgradeClose()
    {
        GameManager.isMenuCameraFree = false;
        GameManager.Instance.camTransition.ChangeCameraPositionByIndex(1);
    }
}
