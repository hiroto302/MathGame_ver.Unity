using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CPクラス Playerクラスの操作と同等のことを自動で判断することができるCPプレイヤー
public class CP : Player
{
    public CP() : base("相手")
    {
      // コンストラクタ CPのnameを、相手とする
    }

    // 保持するカードの表示
    public override void ShowCard()
    {
    //   Console.WriteLine("{0}のカード", Name);
    //   for(int i = 0; i < card.Count; i++)
    //   {
    //     Console.Write("|?|" + " ");
    //     // Console.Write("|{0}| ",card[i]);
    //   }
    //   Console.WriteLine();
    }
    // CPが処理を実行にかける時間
    public void ThinkingTime(int second)
    {
    //   // 思考時間 ３秒 (３秒間sleepにするか時間を計測する方法)
    //   // Thread.Sleep(3000);
    // //   Console.WriteLine("{0}が思考中....", Name);
    //   stopWatch.Start();
    //   TimeSpan ts = stopWatch.Elapsed;
    //   while(ts.Seconds < second)
    //   {
    //     ts  = stopWatch.Elapsed;
    //   }
    //   stopWatch.Stop();
    //   stopWatch.Reset();
    }

    // CPが保持しているカードを出すメソッド
    public override void DiscardCard()
    {
        // 場に出すカード選択
        int n = 0;
        int num = 0;
        bool discard = false;

        // 同等のカードを扱う変数群
        int sameNum = 0;
        bool sameNumber = false;                  // 同等のカードがある時, true
        int yesOrNo;
        List<int> sameNumbers = new List<int>();  // 同じカードの値がある値を格納する

        // 手札に同じ値のカードが複数あるか
        for(int i = 0; i < card.Count; i++)
        {
            sameNum = card[i];
            card.RemoveAt(i);
            for(int j = 0; j < card.Count; j++)
            {
            // 同等のカードがあるか判定
            if(card[j] == sameNum)
            {
                // sameNumber = true;
                if(sameNumbers.Contains(sameNum) == false)
                {
                sameNumbers.Add(sameNum);
                }
            }
            }
            card.Insert(i, sameNum);
        }

        // 場に出す以上の数が手札にあるか
        int m = 0;
        while(m < card.Count)
        {
            if(card[m] > GameMaster.fieldNum)
            {
            // Console.WriteLine("{0} > {1}", card[m], GameMaster.fieldNum);
            discard = true;
            // Console.WriteLine("場に出せる数があるよ");
            break;
            }
            else if(card[m] <= GameMaster.fieldNum) // 場の数「以下」ならm++ (8 < 8 のような時,場に出すことが出来ない時無限ループになる)
            {
            m++;
            }
        }
        // sameNumbersの数がある時、その２倍した数が場の数を越した時もdiscardをtrueとする
        if(sameNumbers.Count > 0)
        {
            for(int i = 0; i < sameNumbers.Count; i++)
            {
            if(sameNumbers[i] * 2 > GameMaster.fieldNum)
            {
                discard = true;
                // Console.WriteLine("場に、数をを同時に出せば置くことが出来るよ");
            }
            }
        }
        // 場に出せる数がある時
        if(discard == true)
        {
            while(true)
            {
            n = Random.Range(0, card.Count);
            num = card.Find(i => i == card[n]);
            // 選択した数がsameNumbersに含まれる数が
            if(sameNumbers.Contains(num))
            {
                sameNumber = true;
            }
            // sameNumbersの時、1枚のみでも場に出せる時、1枚のみ or 2枚 同時に出すか 判定処理
            if(sameNumber == true && num > GameMaster.fieldNum)
            {
                yesOrNo = Random.Range(0, 1);
                // 同時に出す
                if(yesOrNo == 0)
                {
                DiscardNum(2, num);
                GameMaster.nextPlay = "player";
                break;
                }
                // 1枚のみ出す
                else if(yesOrNo == 1)
                {
                DiscardNum(1, num);
                GameMaster.nextPlay = "player";
                break;
                }
            }
            // sameNumbersの時、2枚同時に出せば場に出すことが出来る時の判定処理
            else if(sameNumber == true && num * 2 > GameMaster.fieldNum)
            {
                DiscardNum(2, num);
                GameMaster.nextPlay = "player";
                break;
            }
            // radomに選択した数が場の数以上のカードを選択した時
            else if(num > GameMaster.fieldNum)
            {
                DiscardNum(1, num);
                GameMaster.nextPlay = "player";
                break;
            }
            }
        }
        // 場に出せる数が無い時
        else if(discard == false)
        {
            // Console.WriteLine("{0}は場に出すことが出来なかった", Name);
            GameMaster.nextPlay = "cpRestart";
        }
    }

    // 場に出すカードの数 手札から出した数(第二引数)を場が保持するカードに加えるメソッド
    public void DiscardNum(int n, int num)
    {
        switch(n)
        {
            case 1:
            // Console.WriteLine("{0}が選択した数 : {1}", Name, num);
            break;
            case 2:
            // Console.WriteLine("{0}が選択した数を同時に出した : {1}, {2}", Name, num, num);
            break;
        }
        for(int i = 0; i < n; i++)
        {
            card.Remove(num);
            GameMaster.fieldCard.Add(num);
        }
    }

    // Draw と AddPoint メソッドなどのPlayerクラスとメソッドの処理内容が同じの時、記述は消すこと
    public override void Draw(List<int> number)
    {
        // 手札が６枚でない時、かつ、山札にカードが残っている時下記の処理を実行
        // if(card.Count != 6 && number != null)
        if(card.Count != 6 && number.Count > 0)
        {
            // 手札が6枚になるまで山札から引く
            while(card.Count < 6)
            {
            if(number.Count ==0)
            {
                break;
            }
            int r = Random.Range(0, number.Count);
            card.Add(number[r]);
            number.RemoveAt(r);
            }
        }
    }

    public override void AddPoint(int n)
    {
        point += n;
    //   Console.WriteLine("{0}現在の失点 : {1}", Name, point);
    }
}
