using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームの管理・進行を行うクラス
public class GameMaster : MonoBehaviour
{
    // 生成する数字カードを格納
    public static List<int> number;
    // 作成する枚数
    protected int n ;
    // フィールド(中央)に配置されていくカード
    public static List<int> fieldCard = new List<int>();
    // フィールドの一番上に現在置かれている数
    public static int fieldNum = 0;
    // case3に置いて、次の処理判の断を行う文字列の変数
    public static string nextPlay = "";
    // PlayGameにおける進行管理
    public static int turn;
    //
    public bool playGame;


    // カードを作成するメソッド 引数に10を与える
    public void MakeCard(int n)
    {
        number = new List<int>();
        for(int i = 1; i < n + 1; i++)
        {
            switch(i)
            {
            case 1:
                MakeNum(2, i, number);
                break;
            case 2:
                MakeNum(3, i, number);
                break;
            case 3:
                MakeNum(3, i, number);
                break;
            case 4:
                MakeNum(3, i, number);
                break;
            case 5:
                MakeNum(4, i, number);
                break;
            case 6:
                MakeNum(3, i, number);
                break;
            case 7:
                MakeNum(2, i, number);
                break;
            case 8:
                MakeNum(2, i, number);
                break;
            case 9:
                MakeNum(1, i, number);
                break;
            case 10:
                MakeNum(1, i, number);
                break;
            // 計24枚
            }
        }
    }
    // ある数のカードを作る枚数
    // 第一引数 : 作る枚数, 第二引数: 作るカードの値, 第三引数 : 作成したカード追加する山札
    protected void MakeNum(int n, int m,List<int> number)
    {
        for(int i = 0; i < n; i++)
        {
            number.Add(m);
        }
    }
    // カードの配布
    public void DistributeCard(Player player, CP cp, List<int> number)
    {
        // 各プレイヤーが保持するカード
        player.card = new List<int>();
        cp.card = new List<int>();
        DistributionTarget(player.card);
        DistributionTarget(cp.card);
    }
    // 配布する対象にカードを配る
    protected void DistributionTarget(List<int> card)
    {
        // 一人あたり持つ手札の数
        int cardNum = 6;
        // 引数への配布
        while(true)
        {
            // 山札が0枚でなければ下記を実行
            if(number != null)
            {
                int r = Random.Range(0, number.Count);
                card.Add(number[r]);
                cardNum --;
                // 配布したカードを山札から削除
                number.RemoveAt(r);
                if(cardNum == 0)
                {
                    cardNum = 6;
                    break;
                }
            }
        }
    }
    // フィールド 中央のカード置き場所
    public static void FieldCard(int n)
    {
        if(n > 0 && fieldCard.Count > 0)
        {
            // 現在フィールドにある数の表示
            int topNum = fieldCard.Count;
            fieldNum = fieldCard[topNum - 1];
            // Console.WriteLine("　　　　___");
        //     Console.WriteLine();
        //     Console.WriteLine("場の数　|{0}|", fieldNum);
        //     Console.WriteLine();
        }
        else
        {
            // Console.WriteLine();
            // Console.WriteLine("場の数　|{0}|", fieldNum);
            // Console.WriteLine();
        }
    }

    // 現在のフィールドが保持しているカードの表示
    public static void CountFieldCard()
    {
    //   Console.Write("フィールドが保持している数 : ");
    //   foreach(int i in fieldCard)
    //   {
    //     Console.Write(i + " ,");
    //   }
    //   Console.WriteLine();
    }

    // 線
    public static void Line()
    {
    //   Console.WriteLine("-------------------------");
    }

    // 誰のターンか表示
    public void TurnName(Player player)
    {
    //   Console.WriteLine("{0}のターン", player.Name);
    }
    // ゲームの終了
    public void FinishGame()
    {
        // Console.WriteLine("ゲーム終了");
        playGame = false;
    }

    // ゲーム結果の表示
    public void GameResult(Player player, CP cp)
    {
    //   Console.WriteLine("-------- 結果 --------");
      player.Point += player.card.Count;
      cp.Point += cp.card.Count;
      if(player.Point < cp.Point)
      {
        // Console.WriteLine("{0}の勝ち!!!", player1.Name);
      }
      else if(player.Point > cp.Point)
      {
        // Console.WriteLine("CPの勝ち!!!");
      }
      else if(player.Point == cp.Point)
      {
        // Console.WriteLine("引き分け....");
      }
    //   Console.WriteLine("{0}の失点 : {1}", player.Name, player.Point);
    //   Console.WriteLine("CPの失点 : {0}", cp.Point);
    }
    // Unityに置いて下記のゲームを実行する処理をMonoBehaviourクラス継承しているクラスで実行する処理を記述する
    // 全て書き直した方が手取り早いかも
    // ゲームを実行するメソッド
    // 場に出すことが出来なかった時、場が保持しているカードを失点に追加し、場の数を０にリセットする。
    // そして、改めて最初のplayerとして自由な数からだす
    public void PlayGame(Player player, CP cp)
    {
        turn = 0;
        playGame = true;
        while(playGame)
        {
            switch(turn)
            {
            // ゲーム開始場面
            case 0:
                cp.ShowCard();
                FieldCard(turn);
                player.ShowCard();
                turn = 1;
                Line();
                break;
            // Playerがカードを出す場面
            case 1:
                TurnName(player);
                player.DiscardCard();
                player.Draw(number);
                turn = 3;
                break;
            // CPがカードを出す場面
            case 2:
                cp.ThinkingTime(1);
                cp.DiscardCard();
                cp.Draw(number);
                turn = 3;
                break;
            // フィールドと手札の更新 と その後の処理
            case 3:
                // ゲーム終了判定
                // どちらかの手札が0枚になった時点でゲーム終了
                if(player.card.Count == 0 || cp.card.Count == 0)
                {
                nextPlay = "finish";
                }
                // 条件分岐でどのplayerがプレーするか判断
                if(nextPlay.Equals("player"))
                {
                turn = 1;
                }
                else if(nextPlay.Equals("cp"))
                {
                turn = 2;
                }
                else if(nextPlay.Equals("playerRestart")) // プレイヤーが場に出さないことを選択した時の処理
                {
                if(fieldCard.Count > 0)
                {
                    player.AddPoint(fieldCard.Count);
                }
                FieldReset();
                turn = 1; // 再び自分のターン
                }
                else if(nextPlay.Equals("cpRestart"))     // CPが場に出さないこと選択した時の処理
                {
                if(fieldCard.Count > 0)
                {
                    cp.AddPoint(fieldCard.Count);
                }
                FieldReset();
                turn = 2; // 再び相手のターン
                }
                else if(nextPlay.Equals("finish"))
                {
                turn = 4;
                }
                if(fieldCard.Count > 0)
                {
                // 場が保持しているカードの表示
                CountFieldCard();
                }
                Line();
                cp.ShowCard();
                FieldCard(turn);
                player.ShowCard();
                Line();
                break;
            case 4:
                // playGame = false;
                FinishGame();
                break;
            }
        }
        GameResult(player, cp);
        Reset(player, cp);
    }

    // ゲームの状態をリセット 繰り返し遊べる仕様
    public void Reset(Player player1, CP cp)
    {
        fieldNum = 0;
        number.Clear();
        fieldCard.Clear();
        player1.Reset();
        cp.Reset();
    }

    // fieldの状態をリセットするメソッド カードを出すことができな時に呼び出される
    protected void FieldReset()
    {
        fieldCard.Clear();
        fieldNum = 0;
    }

        GameObject p;
        Player player;
        GameObject c;
        CP cp;
    void Awake()
    {
        // Unityの上のインスタンスを参照
        p =  GameObject.Find("Player");
        c = GameObject.Find("CP");
        player = p.GetComponent<Player>();
        cp = c.GetComponent<CP>();
    }

    // 各メソッドをUnity で 実行できるように作成し、最後につなぎ合わせる
    // ゲーム実行するための処理
    void Start()
    {
        // カードの生成
        MakeCard(10);
        // カードの配布
        DistributeCard(player, cp, GameMaster.number);
        // プレイヤー・PCのカード表示
        player.ShowCard();
        cp.ShowCard();
        cp.DiscardCard();
        // cp.ShowCard();
        // turn = 1;
        //
    }
    void Update()
    {

    }
}
