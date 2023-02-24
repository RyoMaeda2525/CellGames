using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageText : MonoBehaviour
{
    [SerializeField, Tooltip("�e�L�X�g��\������MessageText")]
    private TextMeshProUGUI _messageText;

    [SerializeField, Tooltip("�\������e�L�X�g�ꗗ")]
    private string[] _messageTexts;

    [SerializeField, Tooltip("�����𑗂�X�s�[�h")]
    private float _messageSpeed = 1.0f;

    /// <summary>���Ԗڂ̃e�L�X�g��\�����Ă���̂�</summary>
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
        //�������o���؂��Ă��Ȃ���ΑS�ďo��
        if (_displayTextCount < _messageTexts[_textCount].Length)
        {
            _displayTextCount = _messageTexts[_textCount].Length;

            _messageText.maxVisibleCharacters = _displayTextCount;

            _messageText.text = _messageTexts[_textCount];

        }
        else //�o���؂��Ă����玟�̕��͂�
        {
            _messageText.maxVisibleCharacters = 0;
            _displayTextCount = 0;
            _timer = 0;
            _textCount++;
            MessageStart();
        }
    }

    private void MessageStart() //�������o���؂�܂ŏo��
    {
        if (_textCount < _messageTexts.Length && _displayTextCount < _messageTexts[_textCount].Length)
        {
            StartCoroutine(SendMessage());
        }
    }

    private IEnumerator SendMessage()//Inspector��Ō��߂����x�ŕ������o��
    {
        _displayTextCount++;

        _messageText.maxVisibleCharacters = _displayTextCount;

        _messageText.text = _messageTexts[_textCount];

        yield return new WaitForSecondsRealtime(_messageSpeed);

        MessageStart();
    }
}
