using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    // filedに配置されていくカードの枚数
    public List<int> fieldCard = new List<int>();
    // filedの一番上に置かれている数
    public int fieldNum = 0;
    // Fieldの中心座標
    Vector3 fieldPosition;
    // Cardとの距離
    Vector3 dir;
    float d;

    // Cardの変数
    Card cardScript;
    Vector3 cardPosition;
    // cardが一定の範囲に入ったら呼び出されるメソッド
    public void Discard()
    {
        Debug.Log("呼ばれたよ");
    }

    void Start()
    {
        fieldPosition = transform.position;
    }


    void Update()
    {
        // 場に出ているCardオブジェクトを取得
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        // Cardの中心座標取得
        foreach(var card in cards)
        {
            cardScript = card.GetComponent<Card>();
            cardPosition = cardScript.position;
            dir = fieldPosition - cardPosition;
            d = dir.magnitude;
            // Debug.Log(d);
            // dの範囲設定 0.2 ~ 0.8
            if(d < 0.8)
            {
                Discard();
            }
        }

    }
}
