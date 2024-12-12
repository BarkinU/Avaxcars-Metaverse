using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

#region Class Lists

[System.Serializable]
public class FirstTimeList
{

    public int first;
    public int second;

    public int third;

    public int fourth;

    public int fifth;

    public int sixth;

    public int seventh;

    public int eighth;

    public int ninth;

    public int tenth;


}

[System.Serializable]
public class RookieSystemList
{
    public int first;
    public int second;

    public int third;

    public int fourth;

    public int fifth;

    public int sixth;

    public int seventh;

    public int eighth;

    public int ninth;

    public int tenth;
}

[System.Serializable]
public class SemiProList
{
    public int first;
    public int second;

    public int third;

    public int fourth;

    public int fifth;

    public int sixth;

    public int seventh;

    public int eighth;

    public int ninth;

    public int tenth;
}

[System.Serializable]
public class ProList
{
    public int first;
    public int second;

    public int third;

    public int fourth;

    public int fifth;

    public int sixth;

    public int seventh;

    public int eighth;

    public int ninth;

    public int tenth;
}

[System.Serializable]
public class MasterList
{
    public int first;
    public int second;

    public int third;

    public int fourth;

    public int fifth;

    public int sixth;

    public int seventh;

    public int eighth;

    public int ninth;

    public int tenth;
}

[System.Serializable]
public class ChallengerList
{
    public int first;
    public int second;

    public int third;

    public int fourth;

    public int fifth;

    public int sixth;

    public int seventh;

    public int eighth;

    public int ninth;

    public int tenth;

}

#endregion


public class RankSystem : MonoBehaviour
{
    #region  Lists
    public List<FirstTimeList> firstTimeLists = new List<FirstTimeList>();
    public List<RookieSystemList> rookieLists = new List<RookieSystemList>();
    public List<SemiProList> semiProLists = new List<SemiProList>();
    public List<ProList> proLists = new List<ProList>();
    public List<MasterList> masterLists = new List<MasterList>();
    public List<ChallengerList> challengerLists = new List<ChallengerList>();

    #endregion
    
    
    
    public List<int> nftValues;
    public List<int> averageMinusNft;
    public int totalValue;
    public int averageValue;
    private int playerCount = 10;

    public int rookiePlacedPlayerPoint;
    private int matchesCompletedCount = 10;



    void Start()
    {

        for (int j = 0; j < playerCount; j++)
        {
            nftValues.Add(Random.Range(1200, 1400));
            totalValue += nftValues[j];
        }

        averageValue = totalValue / playerCount;

        for (int g = 0; g < playerCount; g++)
        {
            averageMinusNft.Add(averageValue - nftValues[g]);
        }

        firstTimeLists[0].first = 55;
        firstTimeLists[0].second = 45;
        firstTimeLists[0].third = 40;
        firstTimeLists[0].fourth = 30;
        firstTimeLists[0].fifth = 25;
        firstTimeLists[0].sixth = 20;
        firstTimeLists[0].seventh = 15;
        firstTimeLists[0].eighth = 10;
        firstTimeLists[0].ninth = 5;
        firstTimeLists[0].tenth = 0;

        rookieLists[0].first = (int)(((averageMinusNft[0] / 20) + 30) / 1.5f);
        rookieLists[0].second = (int)(((averageMinusNft[1] / 20) + 20) / 1.5f);
        rookieLists[0].third = (int)(((averageMinusNft[2] / 20) + 15) / 1.5f);
        rookieLists[0].fourth = (int)(((averageMinusNft[3] / 20) + 10) / 1.5f);
        rookieLists[0].fifth = 0;
        rookieLists[0].sixth = 0;
        rookieLists[0].seventh = (int)(((averageMinusNft[8] / 20) - 10) / 1.5f);
        rookieLists[0].eighth = (int)(((averageMinusNft[8] / 20) - 15) / 1.5f);
        rookieLists[0].ninth = (int)(((averageMinusNft[8] / 20) - 20) / 1.5f);
        rookieLists[0].tenth = (int)(((averageMinusNft[9] / 20) - 30) / 1.5f);

        semiProLists[0].first = ((int)(((averageMinusNft[0] / 20) + 30) / 2f));
        semiProLists[0].second = ((int)(((averageMinusNft[1] / 20) + 20) / 2f));
        semiProLists[0].third = ((int)(((averageMinusNft[2] / 20) + 15) / 2f));
        semiProLists[0].fourth = ((int)(((averageMinusNft[3] / 20) + 10) / 2f));
        semiProLists[0].fifth = 0;
        semiProLists[0].sixth = 0;
        semiProLists[0].seventh = ((int)(((averageMinusNft[8] / 20) - 10) / 2f));
        semiProLists[0].eighth = ((int)(((averageMinusNft[8] / 20) - 15) / 2f));
        semiProLists[0].ninth = ((int)(((averageMinusNft[8] / 20) - 20) / 2f));
        semiProLists[0].tenth = ((int)(((averageMinusNft[9] / 20) - 30) / 2f));

        proLists[0].first = (int)(((averageMinusNft[0] / 20) + 30) / 2.5f);
        proLists[0].second = (int)(((averageMinusNft[1] / 20) + 20) / 2.5f);
        proLists[0].third = (int)(((averageMinusNft[2] / 20) + 15) / 2.5f);
        proLists[0].fourth = (int)(((averageMinusNft[3] / 20) + 10) / 2.5f);
        proLists[0].fifth = 0;
        proLists[0].sixth = 0;
        proLists[0].seventh = (int)(((averageMinusNft[8] / 20) - 10) / 2.5f);
        proLists[0].eighth = (int)(((averageMinusNft[8] / 20) - 15) / 2.5f);
        proLists[0].ninth = (int)(((averageMinusNft[8] / 20) - 20) / 2.5f);
        proLists[0].tenth = (int)(((averageMinusNft[9] / 20) - 30) / 2.5f);

        masterLists[0].first = (int)(((averageMinusNft[0] / 20) + 30) / 3f);
        masterLists[0].second = (int)(((averageMinusNft[1] / 20) + 20) / 3f);
        masterLists[0].third = (int)(((averageMinusNft[2] / 20) + 15) / 3f);
        masterLists[0].fourth = (int)(((averageMinusNft[3] / 20) + 10) / 3f);
        masterLists[0].fifth = 0;
        masterLists[0].sixth = 0;
        masterLists[0].seventh = (int)(((averageMinusNft[8] / 20) - 10) / 3f);
        masterLists[0].eighth = (int)(((averageMinusNft[8] / 20) - 15) / 3f);
        masterLists[0].ninth = (int)(((averageMinusNft[8] / 20) - 20) / 3f);
        masterLists[0].tenth = (int)(((averageMinusNft[9] / 20) - 30) / 3f);

        challengerLists[0].first = (int)(((averageMinusNft[0] / 20) + 30) / 4f);
        challengerLists[0].second = (int)(((averageMinusNft[1] / 20) + 20) / 4f);
        challengerLists[0].third = (int)(((averageMinusNft[2] / 20) + 15) / 4f);
        challengerLists[0].fourth = (int)(((averageMinusNft[3] / 20) + 10) / 4f);
        challengerLists[0].fifth = 0;
        challengerLists[0].sixth = 0;
        challengerLists[0].seventh = (int)(((averageMinusNft[8] / 20) - 10) / 4f);
        challengerLists[0].eighth = (int)(((averageMinusNft[8] / 20) - 15) / 4f);
        challengerLists[0].ninth = (int)(((averageMinusNft[8] / 20) - 20) / 4f);
        challengerLists[0].tenth = (int)(((averageMinusNft[9] / 20) - 30) / 4f);

    }

}
