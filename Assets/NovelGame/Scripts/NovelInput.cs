using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Unity.VisualScripting;

public class NovelInput : MonoBehaviour
{
    [SerializeField, Tooltip("テキストウィンドウにメッセージを表示するScript")]
    MessagePrinter _printer = default;

    [SerializeField, Tooltip("背景を操作するScript")]
    BackGround _bg = default;

    [SerializeField, Tooltip("メッセージ情報を持つTextファイル")]
    private string _loadFileName;

    private string[] _scenarios;

    /// <summary>現在読み込んでいる行</summary>
    private int _currentLine = 0;

    string[] commandWord = new string[] { "\\$image", "\\$backGround" };

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
            Debug.LogError("シナリオファイルが見つかりませんでした");
            Debug.LogError("ScenarioManagerを無効化します");
            enabled = false;
            return;
        }
        _scenarios = scenarioText.text.Split(new string[] { "@br" }, StringSplitOptions.None);
        _currentLine = 0;

        Resources.UnloadAsset(scenarioText);
    }

    /// <summary>
    /// 次のページに進む。
    /// 次のページが存在しない場合は無視する。
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

            Match match = Regex.Match(_scenarios[_currentLine], string.Format("({0})(\\S+)", string.Join("|", commandWord)));

            int indexofCommand = _scenarios[_currentLine].IndexOf(match.Groups[0].Value);

            if (indexofCommand != -1)
            {
                _scenarios[_currentLine] = _scenarios[_currentLine].Substring(0, indexofCommand);
            }

            Command(match);

            _printer?.ShowMessage(_scenarios[_currentLine]);
        }
    }

    private void Command(Match match) 
    {
        Group g = match.Groups[1];

        switch (g.Value) 
        {
            case "$image":
                Debug.Log($"Image : {match.Groups[3].Value}");
                break;

            case "$backGround":
                Debug.Log($"backGround : {match.Groups[3].Value}");
                break;
        }
    }
}
