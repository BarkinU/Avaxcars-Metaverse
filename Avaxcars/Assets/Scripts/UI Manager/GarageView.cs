using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Examples.Database;

public class GarageView : View
{
    [SerializeField] private Button _backButton;
    EventManager _eventManager;
    public override void Initialize()
    {
        _eventManager = EventManager.Instance;
        _backButton.onClick.AddListener(() => ViewManager.Show<MainMenuView>());
        _backButton.onClick.AddListener(() => GarageClose());

    }

    public void GarageClose()
    {
        if (_eventManager.isInEvent == true)
        {
            _eventManager.CancelRoomsRefresh();
            GameManager.isMenuCameraFree = false;
            _eventManager.GetRequestedPageServer();
            ViewManager.Show<EventView>();
            DatabaseHandler.Instance.EventMessagesButton.interactable = false;

        }
        else
        {
            _eventManager.joinLeaveGO.SetActive(true);
            GameManager.Instance.camTransition.ChangeCameraPositionByIndex(1);

        }
    }
}