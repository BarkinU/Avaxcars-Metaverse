using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using FirebaseWebGL.Examples.Database;

public class WalletConnectSettings : MonoBehaviour
{

    string chain = "avalanche";
    string network = "testnet";
    string contract = "0xf7c6Fd183c52439E4bda26B06C3452dcf35F49f7";
    string account = "0xcc96ee8091f5db65a95e61ea2c0d88eb6e06fbc7";
    public GameManager _gameManager;
    public UIManager _uiManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _uiManager = UIManager.Instance;
    }
    public async void StartGetNFTs()
    {
        //Tranport Json to Class
        //string account = PlayerPrefs.GetString("Account");
        string response = await EVM.AllErc721(chain, network, account, contract);
        print(response);
        NFTs[] erc721s = JsonConvert.DeserializeObject<NFTs[]>(response);

        var tasks = new Task[erc721s.Length];

        for (int i = 0; i < erc721s.Length; i++)
        {

            await NFTFeatures(erc721s[i].tokenId);
            StartCoroutine(ProgressBarCoroutine(i, erc721s.Length));
        }

        // await Task.WhenAll(tasks);


        _gameManager.AddWalletIDServer(account);
        _gameManager.GetAllNFTServer();
        
    }

    IEnumerator ProgressBarCoroutine(int i, int erclength)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            _uiManager.currentValue = elapsedTime;
            float currentOffset = _uiManager.currentValue - _uiManager.minimumValue;
            _uiManager.progressBar.fillAmount = currentOffset / 1;

            yield return null;
        }
    }

    async Task NFTFeatures(string tokenId)
    {
        string uriIpfs = await ERC721.URI(chain, network, contract, tokenId);
        print(uriIpfs);
        ////////////////FOR IPFS SERVER//////////////
        print(uriIpfs);
        string[] uriHttps = uriIpfs.Split("//"[1]);
        string uri = "https://ipfs.io/ipfs/" + uriHttps[2] + "/" + uriHttps[3];
        print(uri);
        ////////////////////////////////////////////
        //print("https://uat.minego.co/nfts/json/"+ tokenId + ".json");
        //string uri="";
        //UnityWebRequest webRequest = UnityWebRequest.Get("https://uat.minego.co/nfts/json/"+ tokenId + ".json");
        
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

     //   await webRequest.SendWebRequest();
        var data = JsonUtility.FromJson<CarAttributesInGame>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        print(webRequest.downloadHandler.text);

        _gameManager.carSpect.nfts.Add(data);

        await Task.Yield();

    }

    [System.Serializable]
    private class NFTs
    {
        public string contract { get; set; }
        public string tokenId { get; set; }
        public string uri { get; set; }
        public string balance { get; set; }
    }

}