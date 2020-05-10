using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    // filedに配置されていくカードの枚数
    public static List<int> fieldCard = new List<int>();
    // filedの一番上に置かれている数
    public static int fieldNum = 0;
    // Fieldの中心座標
    Vector3 fieldPosition;
    // Cardとの距離
    Vector3 dir;

    // Cardクラスの変数
    Card cardScript;
    Vector3 cardPosition;
    // Playerクラスの変数
    GameObject playerObject;
    Player player;
    // GameMasterクラスの変数
    GameObject gameMasterObject;
    GameMaster gameMaster;

    // 場の数の表示
    public void ShowFieldNum()
    {
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject topNumText = canvas.transform.GetChild(0).gameObject;
        if(fieldCard.Count > 0)
        {
            int topNum = fieldCard.Count;
            fieldNum = fieldCard[topNum - 1];
            topNumText.GetComponent<Text>().text = fieldNum.ToString();
        }
        else
        {
            topNumText.GetComponent<Text>().text = fieldNum.ToString();
        }
    }

    // フィールドのリセット playerが出すことが出来ない時など呼ばれる
    public void Reset()
    {
        fieldCard.Clear();
        fieldNum = 0;
        Debug.Log("リセットされたよ");
    }

    void Awake()
    {
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<Player>();
        gameMasterObject = GameObject.Find("GameMaster");
        gameMaster = gameMasterObject.GetComponent<GameMaster>();
    }

    void Start()
    {
        fieldPosition = transform.position;
    }


    void Update()
    {
        // 場に出ているCardオブジェクトを取得
        GameObject[] cards = GameObject.FindGameObjectsWithTag("PlayerCard");
        // Cardの中心座標取得し,Fieldとの距離を計測
        // 各カードの距離を管理したいため、カードとの距離の変数を持たせる
        foreach(var card in cards)
        {
            cardScript = card.GetComponent<Card>();
            cardPosition = cardScript.Position;
            dir = fieldPosition - cardPosition;
            cardScript.distance = dir.magnitude;
        }

        // fieldNumの表示
        ShowFieldNum();
    }
}
