using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenView : View
{
    [SerializeField] private Button _loginButton, _signButton;
    public override void Initialize () {
        _loginButton.onClick.AddListener (() => ViewManager.Show<LoginView>());
        _loginButton.onClick.AddListener (() => ChangeState());
        _signButton.onClick.AddListener (() => ViewManager.Show<SignView>());
        
    }

    private void ChangeState()
    {   
        GameManager.Instance.isLoginOrRegister = true;
    }
}
