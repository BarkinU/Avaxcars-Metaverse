using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsernameView : View
{
    [SerializeField] private Button _walletButton;
    public override void Initialize () {
        _walletButton.onClick.AddListener (() => ViewManager.Show<WalletConnectView>());
    }
}
