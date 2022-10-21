using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageText : MonoBehaviour
{
    [SerializeField, Tooltip("テキストを表示するMessageText")]
    private TextMeshProUGUI _messageText;

    [SerializeField, Tooltip("表示するテキスト一覧")]
    private string[] _messageTexts;

    [SerializeField, Tooltip("文字を送るスピード")]
    private float _messageSpeed = 1.0f;

    /// <summary>何番目のテキストを表示しているのか</summary>
    private int _textCount = 0;

    private float _timer = 0;

    private int _displayTextCount = 0;

    private void Start()
    {
        _messageText.maxVisibleCharacters = 0;
        MessageStart();
    }

    public void SkipOrNext() 
    {
        //文字を出し切っていなければ全て出す
        if (_displayTextCount < _messageTexts[_textCount].Length)
        {
            _displayTextCount = _messageTexts[_textCount].Length;

            _messageText.maxVisibleCharacters = _displayTextCount;

            _messageText.text = _messageTexts[_textCount];

        }
        else //出し切っていたら次の文章へ
        {
            _messageText.maxVisibleCharacters = 0;
            _displayTextCount = 0;
            _timer = 0;
            _textCount++;
            MessageStart();
        }
    }

    private void MessageStart() //文字を出し切るまで出力
    {
        if (_textCount < _messageTexts.Length && _displayTextCount < _messageTexts[_textCount].Length)
        {
            StartCoroutine(SendMessage());
        }
    }

    private IEnumerator SendMessage()//Inspector上で決めた速度で文字を出力
    {
        _displayTextCount++;

        _messageText.maxVisibleCharacters = _displayTextCount;

        _messageText.text = _messageTexts[_textCount];

        yield return new WaitForSecondsRealtime(_messageSpeed);

        MessageStart();
    }
}
