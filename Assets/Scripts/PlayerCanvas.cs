using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    GameObject point;
    // プレイヤーのpointの表示
    void ShowPlayerPoint()
    {
        point.GetComponent<Text>().text = "Point " + Player.point.ToString();
    }
    void Awake()
    {
        point = transform.GetChild(1).gameObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowPlayerPoint();
    }
}
