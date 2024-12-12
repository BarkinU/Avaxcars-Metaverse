using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuView : View
{
    [SerializeField] private Button _garageViewButton, _eventViewButton, _upgradeViewButton, _settingsViewButton, _leaderboardButton, _profileButton;
    [SerializeField] private Camera uiCam;
    [SerializeField] private Camera carCam;
    EventManager _eventManager;


    public override void Initialize()
    {
        _eventManager = EventManager.Instance;
        _garageViewButton.onClick.AddListener(() => GarageOpen());
        _eventViewButton.onClick.AddListener(() => EventOpen());
        _garageViewButton.onClick.AddListener(() => ViewManager.Show<GarageView>());
        _upgradeViewButton.onClick.AddListener(() => ViewManager.Show<UpgradeView>());
        _settingsViewButton.onClick.AddListener(() => ViewManager.Show<SettingsMenuView>());
        _leaderboardButton.onClick.AddListener(() => ViewManager.Show<LeaderboardView>());
        _profileButton.onClick.AddListener(() => ViewManager.Show<ProfileView>());
        uiCam.enabled = false;
        carCam.enabled = true;

    }

    public void GarageOpen()
    {
        _eventManager.joinLeaveGO.SetActive(false);
    }

    public void EventOpen()
    {
        _eventManager.isInEvent = true;

    }

}
