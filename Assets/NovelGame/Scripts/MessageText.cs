using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageText : MonoBehaviour
{
    [SerializeField , Tooltip("テキストを表示するMessageText")]
    private TextMeshProUGUI _messageText;

    [SerializeField, Tooltip("表示するテキスト一覧")]
    private string[] _messageTexts;

    [SerializeField, Tooltip("文字を送るスピード")]
    private　float _messageSpeed = 1.0f;

    /// <summary>何番目のテキストを表示しているのか</summary>
    private int _textCount = 0;

    private float _timer = 0;

    private int _displayTextCount = 0;

    private void Start()
    {
        _messageText.maxVisibleCharacters = 0;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_displayTextCount < _messageTexts[_textCount].Length) 
            {
                _displayTextCount = _messageTexts[_textCount].Length;

                _messageText.maxVisibleCharacters = _displayTextCount;

                _messageText.text = _messageTexts[_textCount];

            }
            else 
            {
                _messageText.maxVisibleCharacters = 0;
                _displayTextCount = 0;
                _timer = 0;
                _textCount++;
            }

            
        }

        if (_textCount < _messageTexts.Length && _displayTextCount < _messageTexts[_textCount].Length)
        {
            _timer += Time.deltaTime;

            if (_timer > _messageSpeed) 
            {
                _displayTextCount++;

                _messageText.maxVisibleCharacters = _displayTextCount;

                _messageText.text = _messageTexts[_textCount];

                _timer = 0;
            }
        }
    }
}
