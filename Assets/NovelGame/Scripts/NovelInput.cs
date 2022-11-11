using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class NovelInput : MonoBehaviour
{
    [SerializeField, Tooltip("�e�L�X�g�E�B���h�E�Ƀ��b�Z�[�W��\������Script")]
    MessagePrinter _printer = default;

    [SerializeField, Tooltip("�w�i�𑀍삷��Script")]
    BackGround _bg = default;

    [SerializeField, Tooltip("���b�Z�[�W��������Text�t�@�C��")]
    private string _loadFileName;

    private string[] _scenarios;

    /// <summary>���ݓǂݍ���ł���s</summary>
    private int _currentLine = 0;

    private Regex regex = new Regex("@(\\S+)\\s");

    // Start is called before the first frame update
    void Awake()
    {
        UpdateLines(_loadFileName);
    }

    private void Start()
    {
        MoveNext();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_printer.IsPrinting) { _printer.Skip(); }
            else { MoveNext(); }
        }
    }

    public void UpdateLines(string fileName)
    {
        var scenarioText = Resources.Load<TextAsset>("Scenario/" + fileName);

        if (scenarioText == null)
        {
            Debug.LogError("�V�i���I�t�@�C����������܂���ł���");
            Debug.LogError("ScenarioManager�𖳌������܂�");
            enabled = false;
            return;
        }
        _scenarios = scenarioText.text.Split(new string[] { "@br" }, StringSplitOptions.None);
        _currentLine = 0;

        Resources.UnloadAsset(scenarioText);
    }

    /// <summary>
    /// ���̃y�[�W�ɐi�ށB
    /// ���̃y�[�W�����݂��Ȃ��ꍇ�͖�������B
    /// </summary>
    private void MoveNext()
    {
        if (_scenarios is null or { Length: 0 }) { return; }

        if (_currentLine + 1 < _scenarios.Length)
        {
            _currentLine++;

            int indexof = _scenarios[_currentLine].IndexOf("//");

            if (indexof != -1) 
            {
                _scenarios[_currentLine] = _scenarios[_currentLine].Substring(0, indexof);
            }

            String[] commandText;

            commandText = _scenarios[_currentLine].Split(new string[] { "@" }, StringSplitOptions.None);

            Command(commandText);

            _printer?.ShowMessage(commandText[commandText.Length - 1]);
        }
    }

    private void Command(string[] commandText) 
    {
        for(int i = 0; i < commandText.Length - 1; i++) 
        {
            Debug.Log(commandText[i]);
        }
    }
}
