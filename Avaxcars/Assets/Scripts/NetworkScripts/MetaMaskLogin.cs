using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_WEBGL
public class MetaMaskLogin : MonoBehaviour
{
    private static MetaMaskLogin instance = null;
    public Slider swipeSlider;
    public GameObject connectedGO;
    public static MetaMaskLogin Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("MetaMaskLogin").AddComponent<MetaMaskLogin>();
            }

            return instance;
        }
    }
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private string account;
    public WalletConnectSettings wallet;

    public void OnLogin()
    {
        Web3Connect();
        OnConnected();
    }

    public void WhenSWipeComplete()
    {
        if (swipeSlider.value >= 100)
        {
            connectedGO.SetActive(true);
            StartCoroutine(DelayForConnectGO());
        }
    }

    async private void  OnConnected()
    {
        //account = ConnectAccount();

        while (account == "")
        {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };

        // save account for next scene
        //PlayerPrefs.SetString("Account", account);
        PlayerPrefs.Save();
        // reset login message
        //SetConnectAccount("");
        ViewManager.Show<LoadingView>();
        wallet.StartGetNFTs();

    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
    }

    private IEnumerator DelayForConnectGO()
    {
        yield return new WaitForSeconds(1f);
        OnConnected();
    }
}
#endif
