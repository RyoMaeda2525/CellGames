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

    private float _elapsed = 0; // ������\�����Ă���̌o�ߎ���

    private int[] _alphaArray;

    // _message �t�B�[���h����\�����錻�݂̕����C���f�b�N�X�B
    // �����w���Ă��Ȃ��ꍇ�� -1 �Ƃ���B
    private int _currentIndex = -1;

    /// <summary>
    /// �����o�͒����ǂ����B
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
    /// �w��̃��b�Z�[�W��\������B
    /// </summary>
    /// <param name="message">�e�L�X�g�Ƃ��ĕ\�����郁�b�Z�[�W�B</param>
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
    /// ���ݍĐ����̕����o�͂��ȗ�����B
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