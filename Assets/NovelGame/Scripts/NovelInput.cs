using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class NovelInput : MonoBehaviour
{
    [SerializeField, Tooltip("テキストウィンドウにメッセージを表示するScript")]
    MessagePrinter _printer = default;

    [SerializeField, Tooltip("メッセージ情報を持つTextファイル")]
    private string _loadFileName;

    private string[] _scenarios;

    /// <summary>現在読み込んでいる行</summary>
    private int _currentLine = 0;

    string[] commandWord = new string[] { "\\$image", "\\$backGround" };

    string[] processWord = new string[] 
    { "\\$FadeInComplete", "\\$FadeOutComplete" , "\\$FadeIn", "\\$FadeOut",
      "\\$FadeStand0"
    };

    private CharaManager CharaManager => NovelManager.Instance.CharaManager;

    private BackGround BackGround => NovelManager.Instance.BackGround;

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
    public void MoveNext()
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

            Match command = Regex.Match(_scenarios[_currentLine], string.Format("({0})({1})(\\S+)", string.Join("|", commandWord)
                                                                                                    , string.Join("|", processWord)));

            if (command.Groups[0].Value != "")
            {
                Command(command);
            }
            else
            {
                _printer?.ShowMessage(_scenarios[_currentLine]);
            }
        }
    }

    private void Command(Match match)
    {
        Group g = match.Groups[1];

        Group p = match.Groups[2];

        //背景の切り替え
        if (g.Value == "$backGround")
        {
            switch (p.Value)
            {
                case "$FadeIn":
                    BackGround.FadeIn(match.Groups[3].Value);
                    Debug.Log($"backGroundFadeIn : {match.Groups[3].Value}");
                    break;

                case "$FadeOut":
                    BackGround.FadeOut(match.Groups[3].Value);
                    Debug.Log($"backGroundFadeOut : {match.Groups[3].Value}");
                    break;

                case "$FadeInComplete":
                    BackGround.FadeInComplete(match.Groups[3].Value);
                    Debug.Log($"backGroundFadeInComplete : {match.Groups[3].Value}");
                    return;

                case "$FadeOutComplete":
                    BackGround.FadeOutComplete(match.Groups[3].Value);
                    Debug.Log($"backGroundFadeOutComplete : {match.Groups[3].Value}");
                    return;
            }
        }
        //characterなどの切り替え
        else if (g.Value == "$image")
        {
            switch (p.Value)
            {
                case "$FadeStand0":
                    CharaManager.CharactorFadeStand(match.Groups[3].Value , 0);
                    Debug.Log($"Image : {match.Groups[3].Value}");
                    break;
            }
        }

        MoveNext(); //returnしなければ次の行へ
    }
}
