using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERC721OwnerOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby";
        string contract = "0x2f46EB6E4a97557A4A2f74b4a4C493Bfedb96C66";
        string[] tokenIds = {"1", "11"};
        string multicall = ""; // optional: multicall contract https://github.com/makerdao/multicall
        string rpc = ""; // optional: custom rpc

        List<string> batchOwners = await ERC721.OwnerOfBatch(chain, network, contract, tokenIds, multicall, rpc);
        foreach (string owner in batchOwners)
        {
            print ("OwnerOfBatch: " + owner);
        } 
    }
}
