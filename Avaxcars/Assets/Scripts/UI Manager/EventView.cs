using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventView : View
{
     [SerializeField] private Button _backButton;
      EventManager _eventManager;

    public override void Initialize()
    {
        _eventManager = EventManager.Instance;
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _backButton.onClick.AddListener(() => CloseEventView());
    }

    public void EnterGarage()
    {
        ViewManager.Show<GarageView>();
    }

    public void CloseEventView()
    {
        _eventManager.isInEvent=false;
    }
}
