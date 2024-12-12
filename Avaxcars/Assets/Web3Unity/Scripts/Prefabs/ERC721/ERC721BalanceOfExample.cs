using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC721BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string contract = "0x2f46EB6E4a97557A4A2f74b4a4C493Bfedb96C66";
        string account = "0x8690f008be974a82cd46850b5e7675340d5cd9fe";

        int balance = await ERC721.BalanceOf(chain, network, contract, account);
        print(balance);
    }
}
