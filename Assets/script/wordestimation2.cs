using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Globalization;

public class wordestimation2 : MonoBehaviour
{
    private TextAsset csvFile;  // CSVファイル
    private List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト
    private int height = 0; // CSVの行数
    public string filename = "unigram_freq";
    private string[] wordlist = new string[10];
    private int cont = 0;
    private int cursor = 0;
    private int space_pos;
    public bool firstflag = true;
    private int[] word_num = new int[40];

    public GameObject input;
    public GameObject candidate;

    public string input_keys;
    public string input_word;
    public string input_sentence_tmp;
    public string input_sentence;
    //public string input_numlog;

    void Start()
    {
        csvFile = Resources.Load("CSV/" + filename) as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);
        int i = 0;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(',')); // リストに入れる
            height++; // 行数加算

            if (i < csvDatas[height - 1][1].Length)
            {
                word_num[i] = height;
                i++;
            }
        }
    }

    private void Update()
    {
        // キー入力
        if (Input.anyKeyDown)
        {
            // 入力処理
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (input_keys.Length > 0)
                {
                    input_keys = input_keys.Substring(0, input_keys.Length - 1);
                }
                else if (input_keys.Length == 0 && input_sentence_tmp.Length > 0)
                {
                    input_sentence_tmp = input_sentence_tmp.Substring(0, space_pos + 1);
                    if (input_sentence_tmp.Length == 1)
                    {
                        input_sentence_tmp = "";
                        firstflag = true;
                    }
                }
                cursor = 0;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (cursor > 0)
                {
                    cursor--;
                } 
                else {
                    cursor = cont - 1; // 一番下に
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (cursor + 1 < cont)
                {
                    cursor++;
                }
                else
                {
                    cursor = 0; // 一番上に
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                specialinput("\n");
            }
            else if (Input.GetKeyDown(KeyCode.Period))
            {
                specialinput(".");
            }
            else if (Input.GetKeyDown(KeyCode.Comma))
            {
                specialinput(",");
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                specialinput("_");
            }
            else
            {
                input_keys += input_key_num(Input.inputString);
                cursor = 0;
            }

            //　単語予測
            cont = 0;
            Array.Clear(wordlist, 0, wordlist.Length);
            int input_num = input_keys.Length;

            if (input_num != 0)
            {
                for (int i = word_num[input_num - 1] - 1; i < word_num[input_num]; i++)
                {
                    if (csvDatas[i][1].StartsWith(input_keys))
                    {
                        wordlist[cont] = csvDatas[i][0];
                        cont++;
                        if (cont > wordlist.Length - 1)
                        {
                            break;
                        }
                    }
                }
            }

            if (cont == 0 && input_keys.Length != 0)
            {
                wordlist[cont] = input_word;
            }

            // 入力処理
            input_word = wordlist[cursor];
            input_sentence = input_sentence_tmp + input_word;
            
            // 削除用処理
            space_pos = space_num(input_sentence_tmp);

            // 出力
            input.GetComponent<TextMesh>().text = input_sentence;
            candidate.GetComponent<TextMesh>().text = "";
            for (int i = 0; i < cont; i++)
            {
                candidate.GetComponent<TextMesh>().text += (i + 1) + ". " + wordlist[i] + "\n";
            }
        }
    }

    // 入力処理用の関数
    private static string input_key_num(string input)
    {
        string key_num = "";

        if (input == "q" || input == "a" || input == "z")
        {
            key_num = "2";
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
            key_num = "7";
        }

        if (input == "1")
        {
            key_num = "1";
        }
        else if (input == "2")
        {
            key_num = "2";
        }
        else if (input == "3")
        {
            key_num = "3";
        }
        else if (input == "4")
        {
            key_num = "4";
        }
        else if (input == "5")
        {
            key_num = "5";
        }
        else if (input == "6")
        {
            key_num = "6";
        }
        else if (input == "7")
        {
            key_num = "7";
        }
        else if (input == "8")
        {
            key_num = "8";
        }

        return key_num;
    }

    // 削除処理用の関数
    private static int space_num(string input_sentence)
    {
        string space_char = " ";
        int num = 0;
        int num_tmp = 0;

        if (input_sentence != null && input_sentence.Length > 0)
        {
            num = input_sentence.Substring(0, input_sentence.Length - 1).IndexOf("_");
        }

        while (num > 0)
        {
            num_tmp = num;
            if (input_sentence.Length > 0 && input_sentence.Length > num + 1)
            {
                num = input_sentence.Substring(0, input_sentence.Length - 1).IndexOf(space_char, num + 1);
            }
            else
            {
                num = -1;
            }
        }

        num = num_tmp;

        return num;
    }

    // アルファベット以外の入力用の関数
    private void specialinput(string schar)
    {
        input_keys = "";
        input_sentence_tmp = input_sentence + schar;
        cursor = 0;
        //firstupper();
    }

    private void firstupper()
    {
        if (firstflag)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            input_sentence_tmp = ti.ToTitleCase(input_sentence_tmp);
            firstflag = false;
        }
    }
}

