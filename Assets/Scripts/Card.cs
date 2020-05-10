using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 数字カードのクラス
// GameMasterにより作成されるカード
// 作成された数字に対応する数をカード表面に表示する
public class Card : MonoBehaviour
{
    // カードの値(数)
    private int num;
    public int Num
    {
        get{return num;}
    }

    // カードの初期位置
    float x, y, z;
    Vector3 startPosition;
    // 移動中の中心座標
    private Vector3 position;
    public Vector3 Position
    {
        get{return position;}
    }
    // カードとFieldの距離
    public float distance;
    // カードに対応する数を表示・値をセットするメソッド(Player)
    public void ShowNum(int num)
    {
        this.num = num;
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject numText = canvas.transform.GetChild(0).gameObject;
        numText.GetComponent<Text>().text = this.num.ToString();
    }
    // カードの値を非表示にし、値をセットするメソッド(PC)
    public void NoShowNum(int num)
    {
        this.num = num;
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject numText = canvas.transform.GetChild(0).gameObject;
        numText.GetComponent<Text>().text = "?";
    }

    // カードを初期位置に戻すメソッド
    public void ReturnPosition()
    {
        transform.position = startPosition;
    }
    // カードを移動させとき、フィールドの位置に置かれなければ元の位置に戻すメソッド
    public void NoField()
    {
        // 高さが初期位置と同じ時、元の位置に戻す
        if(startPosition.y == transform.position.y)
        {
            ReturnPosition();
        }
    }
    void Start()
    {
        // 初期位置の初期化
        startPosition = transform.position;
        y = startPosition.y;
    }

    void Update()
    {
        NoField();
        // 中心座標の取得
        position = transform.position;
    }
}
