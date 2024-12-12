using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : MonoBehaviour
{

    [SerializeField] private GameObject[] frontWheels;
    private float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 3)
        {
            transform.position += new Vector3(0, 0, +0.03f);
            transform.eulerAngles += new Vector3(0, +0.1f, 0);
            for (int i = 0; i < frontWheels.Length; i++)
            {
                frontWheels[i].transform.eulerAngles += new Vector3(0, 0.05f, 0);
            }
        }


    }

}
