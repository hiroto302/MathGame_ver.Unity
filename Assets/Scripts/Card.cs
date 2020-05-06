using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 数字カードのクラス
// GameMasterにより作成されるカード
// 作成された数字に対応する数をカード表面に表示する
public class Card : MonoBehaviour
{
    // 表示する数
    int num;
    // カードに対応する数を表示するメソッド
    public void ShowNum(int num)
    {
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject numText = canvas.transform.GetChild(0).gameObject;
        numText.GetComponent<Text>().text = num.ToString();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
