using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class yosoku : MonoBehaviour
{
    //public string dataName;    // 問題テーマ
    //private string title;       // タイトル
    private TextAsset csvFile;  // CSVファイル
    private List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト
    private int height = 0; // CSVの行数
    //private int i, j = 0;//debugループカウンタ
    //const int size = 3;
    public string filename = "unigram_freq";
   // private bool input_flag = false;
    public string[] wordlist = new string[10];
    private int cont = 0;
    private int cursor = 0;
    public string input_keys;
    public string input_word;
    private string input_sentence_tmp;
    public string input_sentence;

    public int space_pos;

    void Start()
    {
        csvFile = Resources.Load("CSV/" + filename) as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(',')); // リストに入れる
            height++; // 行数加算
        }
    }

    private void Update()
    {
        // キー入力
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (input_keys.Length > 0)
                {
                    input_keys = input_keys.Substring(0, input_keys.Length - 1);
                }
                else if (input_keys.Length == 0)
                {
                    input_sentence_tmp = input_sentence_tmp.Substring(0, space_pos);
                }
                cursor = 0;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (cursor > 0)
                {
                    cursor--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (cursor + 1 < cont)
                {
                    cursor++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                input_keys = "";
                input_sentence_tmp = input_sentence + "\n";
                cursor = 0;
            }
            else if (!Input.GetKeyDown(KeyCode.Space))
            {
                input_keys += input_key_num(Input.inputString);
                cursor = 0;
            }
            else
            {
                input_keys = "";
                input_sentence_tmp = input_sentence + " ";
                cursor = 0;
            }
            
            //　単語予測
            cont = 0;
            Array.Clear(wordlist, 0, wordlist.Length);

            // 通常探索
            for (int i = 0; i < height; i++)
            {
                if (csvDatas[i][1].Length == input_keys.Length)
                {
                    if (csvDatas[i][1].StartsWith(input_keys))
                    {
                        wordlist[cont] = csvDatas[i][0];
                        cont++;
                        if (cont > 9)
                        {
                            break;
                        }
                    }
                }
            }

            // 二分探索
            //var min = 0;
            //var max = csvDatas[0].Length - 1;
            //while (min <= max)
            //{
            //    var mid = min + (max - min) / 2;
            //    switch (input_keys.CompareTo(csvDatas[mid][1]))
            //    {
            //        case -1:
            //            max = mid - 1;
            //            break;
            //        case 1:
            //            min = mid + 1;
            //            break;
            //        case 0:
            //            wordlist[cont] = csvDatas[mid][0];
            //            //Debug.Log(wordlist[cont] + ":" + csvDatas[i][1]);
            //            cont++;
            //            break;
            //    }
            //    if (cont > 9)
            //    {
            //        break;
            //    }
            //}

            if (cont == 0 && input_keys.Length != 0)
            {
                wordlist[cont] = input_word;
            }

            // 入力処理
            input_word = wordlist[cursor];
            input_sentence = input_sentence_tmp + input_word;
            Debug.Log("input:" + input_sentence);

            space_pos = space_num(input_sentence_tmp);
        }
    }

    private static string input_key_num(string input)
    {
        string key_num = "";

        if (input == "q" || input == "a" || input == "z")
        {
            key_num = "1";
        }
        else if (input == "w" || input == "s" || input == "x")
        {
            key_num = "2";
        }
        else if (input == "e" || input == "d" || input == "c")
        {
            key_num = "3";
        }
        else if (input == "r" || input == "f" || input == "v" || input == "t" || input == "g" || input == "b")
        {
            key_num = "4";
        }
        else if (input == "y" || input == "h" || input == "n" || input == "u" || input == "j" || input == "m")
        {
            key_num = "5";
        }
        else if (input == "i" || input == "k")
        {
            key_num = "6";
        }
        else if (input == "o" || input == "l")
        {
            key_num = "7";
        }
        else if (input == "p")
        {
            key_num = "8";
        }

        return key_num;
    }

    private static int space_num(string input_sentence)
    {
        string space_char = " ";
        int num = 0;
        int num_tmp = 0;

        if (input_sentence != null)
        {
            num = input_sentence.IndexOf(" ");
        }

        while (num > 0)
        {
            num_tmp = num;
            if (input_sentence.Length > 0 && input_sentence.Length > num + 1)
            {
                num = input_sentence.Substring(0, input_sentence.Length - 1).IndexOf(space_char, num + 1);
            } else
            {
                num = -1;
            }
        }

        if (num_tmp > num)
        {
            num = num_tmp;
        }

        return num;
    }

}