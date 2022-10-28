using TMPro;
using UnityEditor.VersionControl;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessagePrinter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textUi = default;

    [SerializeField]
    private string _message = null;

    [SerializeField]
    private float _interval = 1.0f;

    private float _elapsed = 0; // 文字を表示してからの経過時間

    private int[] _alphaArray;

    // _message フィールドから表示する現在の文字インデックス。
    // 何も指していない場合は -1 とする。
    private int _currentIndex = -1;

    /// <summary>
    /// 文字出力中かどうか。
    /// </summary>
    public bool IsPrinting
    {
        get
        {
            if (_currentIndex + 1 >= _message.Length)
            {
                return false;
            }
            else { return true; }
        }
    }

    private void Update()
    {
        if (_textUi is not null && _message is not null && _currentIndex + 1 < _message.Length)
        {
            _elapsed += Time.deltaTime;
            if (_elapsed > _interval)
            {
                _elapsed = 0;
                _currentIndex++;
                _textUi.text += _message[_currentIndex];
            }
        }

        if (_alphaArray[_currentIndex] != 255) 
        {
            _textUi.text = "";
            for (int i = 0; i < _currentIndex + 1; i++)
            {
                if (_alphaArray[i] < 250)
                {
                    if (_alphaArray[_currentIndex] < 10) { _alphaArray[_currentIndex] = 10; }

                    _alphaArray[i] += 5;
                    _textUi.text += $"<alpha=#" +_alphaArray[i].ToString("x2")+">"+_message[_currentIndex];

                }
                else
                {
                    _alphaArray[i] = 255;
                    _textUi.text += $"<alpha=#ff>"+_message[_currentIndex];
                }
            }
        }
    }

    /// <summary>
    /// 指定のメッセージを表示する。
    /// </summary>
    /// <param name="message">テキストとして表示するメッセージ。</param>
    public void ShowMessage(string message)
    {
        _message = message;
        _alphaArray = new int[message.Length];
        _textUi.text = "";
        _currentIndex = 0;
        _alphaArray[_currentIndex] = 0;
        _elapsed = 0;
    }

    /// <summary>
    /// 現在再生中の文字出力を省略する。
    /// </summary>
    public void Skip()
    {
        _currentIndex = _message.Length - 1;
        for (int i = 0; i < _alphaArray.Length; i++)
        {
            _alphaArray[i] = 254;
        }
    }
}