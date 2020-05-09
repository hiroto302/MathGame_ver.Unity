using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // CardのPrefab
    public GameObject Card;

    // Player変数
    // プレイヤーが保持するカード, 手札
    public List<int> card;
    // フィールドに各手札のカードを表示する位置の親
    Transform cardPositionParent = null;
    // プレイヤ-の名前 Playerクラスを継承するクラスでも共通の値
    private string name;
    // 勝敗を分ける失点ポイント
    protected int point = 0;
    // Tapして,選択しているカードの値
    int selectNum;
    // 各プロパティ
    public string Name
    {
        set{name = value;}
        get{return name ;}
    }
    // プロパティを静的(static)にしたい場合、pointもstaticである必要がある
    public int Point
    {
        set{point = value;}
        get{return point;}
    }

    // Filedクラスの変数
    GameObject fieldObject;
    Field field;

    // コンストラクタ
    public Player() : this("player")
    {
    }
    public Player(string name)
    {
        this.name = name;
    }

    // 保持するカードの表示するメソッド
    // Prefab化したカードを生成し、手札に並べる
    // 位置に関しては、生成する位置を格納したTransform配列を用意して、そこに各配置する方法でもいいかも
    public virtual void ShowCard()
    {
        for(int i = 0; i < card.Count; i++)
        {
            GameObject numCard = Instantiate(Card, new Vector3( -2.0f + i * 0.8f, 0.3f, -1.3f), Quaternion.identity) as GameObject;
            // tag を PlayerCardとして生成
            numCard.tag = "PlayerCard";
            numCard.GetComponent<Card>().ShowNum(card[i]);
        }
    }
    // 手札のカードを格納・取得
    protected GameObject[] cardObjects;
    public virtual void GetCard()
    {
        cardObjects = GameObject.FindGameObjectsWithTag("PlayerCard");
    }


    // タップしたカードを場に移動させるメソッド
    float x, y, z;
    GameObject tapCard;
    bool rayHit = true;

    public void TapCard()
    {
        float distance = 100; // 飛ばす&表示するRayの長さ
        int layerMask = 1 << 8;  // cardのレイヤー設定
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit, distance, layerMask))
            {
                // クリック中の y の高さ
                y =  hit.collider.gameObject.transform.position.y + 0.1f;
                // tapしたカード以外にrayが衝突しないようにする
                // tapしたカードの値取得
                tapCard = hit.collider.gameObject;
                selectNum = tapCard.GetComponent<Card>().Num;
                GetCard();
                foreach(GameObject cardObject in cardObjects)
                {
                    cardObject.GetComponent<Collider>().enabled = false;
                }
            }
            tapCard.GetComponent<Collider>().enabled= true;
        }
        // マウスをクリックし続けている時
        if(Input.GetMouseButton(0))
        {
            // cardとrayが当たっている時
            if(Physics.Raycast(ray, out hit, distance, layerMask))
            {
                x = hit.point.x;
                z = hit.point.z;
                // cardの位置を更新 xとzのみ変更
                hit.collider.gameObject.transform.position = new Vector3(x, y, z);
                rayHit = false;
            }
            // マウスをクリック中にrayが外れた時の処理
            else if(rayHit == false)
            {
                tapCard.GetComponent<Card>().ReturnPosition();
                rayHit = true;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            // if(Physics.Raycast(ray, out hit, distance, layerMask) && Field.Distance < 0.8f)
            if(Physics.Raycast(ray, out hit, distance, layerMask))
            {
                // tapしたカードをフィールドに移動し、場に出す判定
                if(hit.collider.gameObject.GetComponent<Card>().distance < 0.8f
                    && hit.collider.gameObject.GetComponent<Card>().Num > Field.fieldNum)
                {
                    DiscardCard();
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    // y の位置を元に戻す
                    y = hit.collider.gameObject.transform.position.y - 0.1f;
                    hit.collider.gameObject.transform.position = new Vector3(x, y, z);
                }
                }
            GetCard();
            // colliderのリセット
            foreach(GameObject cardObject in cardObjects)
            {
                cardObject.GetComponent<Collider>().enabled = true;
            }
            rayHit = true;
        }
    }
    // 保持しているカードを場に出すメソッド
    // 場の値以上のカードのみ出すこが可能
    // 出したカードを手持ちから削除し、場に表示する
    // 同じ値のカードは同時に出すことが出来る、ただし、その合計の値が場の数値以上であれ必要がある。
    // そして、新たに場に表示される値は合計値ではなく一枚あたりの数字のみ表示する
    // 場に出すことが出来た時、山札から手札が6枚になるようにする。メソッドを分けていいかも
    // 場に出すことが出来なかった時、場が保持している数を失点に追加
    // そして、改めて場をリセットし自由な数からだす
    int n; // 場に出す数
    bool discard = false; // 場に出すことが出来るか判定のフラグ
    int num; // 数の判断
    bool sameNumber = false; // 同じ値の数を保持しているか判定フラグ
    // int sameNum = 0;
    int yesOrNo;
    public virtual void DiscardCard()
    {
        // 場にだす数の選択
        // tapカードの値 selectNum
        if(Field.fieldNum < selectNum)
        {
            // 場の数以上のカードを選択し、場に出した時、そのカードを場に追加する。
            card.Remove(selectNum);
            Field.fieldCard.Add(selectNum);
            GameMaster.fieldCard.Add(selectNum);
            GameMaster.nextPlay = "cp";
            // 手札を出すことが出来たら山札からカードを引く
            turn = 2;
        }
    }
    
    public virtual void DDiscardCard()
    {
        List<int> sameNumbers = new List<int>(); // 同じ値の数値を格納

        // 手札に同じ値のカードが複数あるか
        for(int i = 0; i < card.Count; i++)
        {
            // i 番目の数を取り出す 1~6
            num = card[i];
            card.RemoveAt(i);
            // 同等のカードが手札にあるか判定
            for(int j = 0; j < card.Count; j++)
            {
            if(card[j] == num)
            {
                sameNumber = true;
                // 同等のカードを複数持つことを考慮する
                if(sameNumbers.Contains(num) == false)
                {
                sameNumbers.Add(num);
                }
            }
            }
            // 取り出した値を戻す
            card.Insert(i, num);
        }

        // センターの場に出すカード選択
        while(true)
        {
            // Console.Write("場に出す数を選択 : ");
            // n = int.Parse(Console.ReadLine());
            //選択した値が手札にあるか 0を選択したか
            for(int i = 0; i < card.Count; i++)
            {
            // if(card[i] == n || n == 0)
            // {
            //     discard = true;
            // }
            }
            // 選択した値が手札にある時
            if(discard)
            {
            //変数のリセット
            //場に出ている数以上 0入力 判定
            if(n > GameMaster.fieldNum || n == 0 || (discard == true && sameNumbers.Find(i => i == n) * 2 > GameMaster.fieldNum))
            {
                discard = false;
                break;
            }
            else
            {
                discard = false;
                // Console.WriteLine("場に出ている数より大きい数を選択 or 無い場合は,「0」を入力");
            }
            }
            else if( discard == false)
            {
            // Console.WriteLine("手札にあるカードを選択せよ");
            }
        }
        // 場にカードを出せた時
        if(n > 0)
        {
            // 同じ値がある時、その値を同時に出すか
            if(sameNumbers.Contains(n))
            {
            if( n > GameMaster.fieldNum)
            {
                // Console.WriteLine("同じ値のカードを同時に出しますか? Yes : 0, No : 1");
                // Console.Write("値を入力 : ");
                // yesOrNo = int.Parse(Console.ReadLine());
                sameNumber = false; //リセット
                switch(yesOrNo)
                {
                case 0:
                    // 2つの n を削除 & フィールドカードに追加
                    for(int i = 0; i < 2; i++)
                    {
                    card.Remove(n);
                    GameMaster.fieldCard.Add(n);
                    }
                    GameMaster.nextPlay = "cp";
                    break;
                case 1:
                    card.Remove(n);
                    GameMaster.fieldCard.Add(n);
                    GameMaster.nextPlay = "cp";
                    break;
                }
            }
            else if(n <= GameMaster.fieldNum)
            {
                // Console.WriteLine("同等のカードを同時に出します");
                for(int i = 0; i < 2; i++)
                {
                card.Remove(n);
                GameMaster.fieldCard.Add(n);
                }
                GameMaster.nextPlay = "cp";
            }
            }
            // 同じ値がない時
            else
            {
                card.Remove(n);
                GameMaster.fieldCard.Add(n);
                GameMaster.nextPlay = "cp";
            }
        }
        // 入力 0 skip
        // Lv4では, 0入力を場に出すことができなことを意味する
        else if(n == 0)
        {
            // Console.WriteLine("スキップします");
            // GameMaster.nextPlay = "playerSkip";
            // skipNum --;
            GameMaster.nextPlay = "playerRestart";
        }
    }

    // 場に出した時手札が６枚になるように、山札からカードを手札に加えるメソッド
    public virtual void Draw(List<int> number)
    {
        // 手札が６枚でない時、かつ、山札にカードが残っている時下記の処理を実行
        if(card.Count != 6 && number != null)
        {
            // 手札が6枚になるまで山札から引く
            while(card.Count < 6)
            {
                // 山札numberから引いている途中に山札が0になった時
                if(number.Count == 0)
                {
                    break;
                }
                int r = Random.Range(0, number.Count);  // 0~引数未満の数を生成
                card.Add(number[r]);
                //   Console.WriteLine(number[r] + "をドローした");
                number.RemoveAt(r);
            }
        }
        // ドローしたら手札の表示更新
        turn = 3;
    }
    // 手札の更新
    public virtual void UpdateHand()
    {
        // 現在場にあるカードを削除
        GetCard();
        foreach(Object cardObject in cardObjects)
        {
            Destroy(cardObject);
        }
        // ドローして更新した手札を表示
        ShowCard();
        // ターンの終了
        playerTurn = false;
        turn = 1;
    }
    // 場に出すことが出来なかった時の処理 場が保持するカードの枚数分失点に追加
    public virtual void AddPoint(int n)
    {
        // Console.WriteLine("場に出すことが出来ないので{0}失点が追加されました", GameMaster.fieldCard.Count);
        // point += n;
        // Console.WriteLine("現在の失点 : {0}", point);
    }

    // 変数の初期化
    public void Reset()
    {
        point = 0;
        if(card.Count > 0)
        {
            card.Clear();
        }
    }

    void Awake()
    {
        fieldObject = GameObject.Find("Field");
        field = fieldObject.GetComponent<Field>();
    }
    public virtual void Start()
    {
        playerTurn = true;
    }

    public bool playerTurn;
    protected int turn = 1;
    public virtual void Update()
    {
        // Debug.Log(playerTurn + " : playerTurn");
        // Debug.Log(turn + ": plyer turn");
        if(playerTurn == false)
        {
            return;
        }
        else
        {
            switch(turn)
            {
                // 手札から場に出す
                case 1:
                    TapCard();
                    break;
                // 山札からカードを引く
                case 2:
                    Draw(GameMaster.number);
                    break;
                // 手札を更新
                case 3:
                    UpdateHand();
                    GameMaster.nextTurn = 3;
                    break;
            }
        }
    }
}
