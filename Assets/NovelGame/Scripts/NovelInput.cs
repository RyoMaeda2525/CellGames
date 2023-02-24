using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class NovelInput : MonoBehaviour
{
    [SerializeField, Tooltip("テキストウィンドウにメッセージを表示するScript")]
    MessagePrinter _printer = default;

    [SerializeField]
    private TextAsset _textAsset;

    private string[] _scenarios;

    /// <summary>現在読み込んでいる行</summary>
    private int _currentLine = 0;

    string[] commandWord = new string[] { "\\$image", "\\$backGround" };

    string[] processWord = new string[] 
    {"\\$FadeIn", "\\$FadeOut","\\$DontSkipFadeIn", "\\$DontSkipFadeIn",};

    string[] characterPositions = new string[] { "\\$0" , "\\$1" , "\\$2" };

    string[] endjudgment = new string[] { "\\$end" , "\\$continuity" };

    private NovelManager NovelManager => NovelManager.Instance;

    private CharaManager CharaManager => NovelManager.CharaManager;

    private BackGround BackGround => NovelManager.BackGround;

    private void Start()
    {
        _scenarios = _textAsset.text.Split(new string[] { "@br" }, StringSplitOptions.None);
        _currentLine = 0;
        MoveNext();
    }

    private void Update()
    {
        if (NovelManager.IsSkipRequested())
        {
            if (_printer.IsPrinting) { _printer.Skip(); }
            else if (CharaManager._fadeNow) {  return; } 
            else 
            {
                StartCoroutine(MoveNextCoroutine());
            }
        }
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
            //文章かコマンドかを判定
            Match command = Regex.Match(_scenarios[_currentLine], string.Format("({0})({1})({2})\\$([\\s\\S]+)({3})", string.Join("|", commandWord)
                                                                                                    , string.Join("|", processWord)
                                                                                                    , string.Join("|", characterPositions)
                                                                                                    , string.Join("|", endjudgment)));

            if (command.Groups[0].Value != "")
            {　 //コマンドならどの動きをするのか判別する
                Command(command);
            }
            else
            {   //コマンドでなければそのまま一文を出力
                _printer?.ShowMessage(_scenarios[_currentLine]);
            }
        }
    }

    private void Command(Match match)
    {
        //背景か画像か
        Group command = match.Groups[1];
        //何をするのか
        Group process = match.Groups[2];
        //画像の配置位置
        int index = int.Parse(match.Groups[3].Value.Substring(1 , 1));
        //次の文章を続けて読み込むか
        bool end = match.Groups[5].Value == "$end" ? true : false;

        //背景の切り替え
        if (command.Value == "$backGround")
        {
            switch (process.Value)
            {　　　　
                　　//スキップ可
                case "$FadeIn":
                    BackGround.FadeIn(match.Groups[4].Value , end);
                    Debug.Log($"backGroundFadeIn : {match.Groups[4].Value}");
                    break;
                　 //スキップ可
                case "$FadeOut":
                    BackGround.FadeOut(match.Groups[4].Value , end);
                    Debug.Log($"backGroundFadeOut : {match.Groups[4].Value}");
                    break;

                　//スキップ不可
                case "$DontSkipFadeIn":
                    BackGround.DontSkipFadeIn(match.Groups[4].Value, end);
                    Debug.Log($"backGroundDontSkipFadeIn : {match.Groups[4].Value}");
                    break;
                　//スキップ不可
                case "$DontSkipFadeOut":
                    BackGround.DontSkipFadeOut(match.Groups[4].Value, end);
                    Debug.Log($"backGroundDontSkipFadeOut : {match.Groups[4].Value}");
                    break;
            }
        }
        //画像の切り替え
        else if (command.Value == "$image")
        {
            switch (process.Value)
            {
                //スキップ可
                case "$FadeIn":
                    StartCoroutine(CharaManager.FadeIn(match.Groups[4].Value ,index , end));
                    Debug.Log($"Image : {match.Groups[4].Value}");
                    break;
                //スキップ可
                case "$FadeOut":
                    StartCoroutine(CharaManager.FadeOut(match.Groups[4].Value, index, end));
                    Debug.Log($"Image : {match.Groups[4].Value}");
                    break;

                //スキップ不可
                case "$DontSkipFadeIn":
                    CharaManager.DontSkipFadeIn(match.Groups[4].Value ,index , end);
                    Debug.Log($"backGroundDontSkipFadeIn : {match.Groups[4].Value}");
                    break;
                //スキップ不可
                case "$DontSkipFadeOut":
                    CharaManager.DontSkipFadeOut(match.Groups[4].Value, index , end);
                    Debug.Log($"backGroundDontSkipFadeOut : {match.Groups[4].Value}");
                    break;
            }
        }

        if (!end) 
        {
            MoveNext(); //次の行へ
        }
    }

    IEnumerator MoveNextCoroutine() 
    {
        yield return new WaitForEndOfFrame();

        MoveNext();

        yield return null;
    }
}
