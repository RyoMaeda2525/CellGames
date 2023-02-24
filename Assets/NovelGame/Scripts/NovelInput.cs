using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class NovelInput : MonoBehaviour
{
    [SerializeField, Tooltip("�e�L�X�g�E�B���h�E�Ƀ��b�Z�[�W��\������Script")]
    MessagePrinter _printer = default;

    [SerializeField]
    private TextAsset _textAsset;

    private string[] _scenarios;

    /// <summary>���ݓǂݍ���ł���s</summary>
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
    /// ���̃y�[�W�ɐi�ށB
    /// ���̃y�[�W�����݂��Ȃ��ꍇ�͖�������B
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
            //���͂��R�}���h���𔻒�
            Match command = Regex.Match(_scenarios[_currentLine], string.Format("({0})({1})({2})\\$([\\s\\S]+)({3})", string.Join("|", commandWord)
                                                                                                    , string.Join("|", processWord)
                                                                                                    , string.Join("|", characterPositions)
                                                                                                    , string.Join("|", endjudgment)));

            if (command.Groups[0].Value != "")
            {�@ //�R�}���h�Ȃ�ǂ̓���������̂����ʂ���
                Command(command);
            }
            else
            {   //�R�}���h�łȂ���΂��̂܂܈ꕶ���o��
                _printer?.ShowMessage(_scenarios[_currentLine]);
            }
        }
    }

    private void Command(Match match)
    {
        //�w�i���摜��
        Group command = match.Groups[1];
        //��������̂�
        Group process = match.Groups[2];
        //�摜�̔z�u�ʒu
        int index = int.Parse(match.Groups[3].Value.Substring(1 , 1));
        //���̕��͂𑱂��ēǂݍ��ނ�
        bool end = match.Groups[5].Value == "$end" ? true : false;

        //�w�i�̐؂�ւ�
        if (command.Value == "$backGround")
        {
            switch (process.Value)
            {�@�@�@�@
                �@�@//�X�L�b�v��
                case "$FadeIn":
                    BackGround.FadeIn(match.Groups[4].Value , end);
                    Debug.Log($"backGroundFadeIn : {match.Groups[4].Value}");
                    break;
                �@ //�X�L�b�v��
                case "$FadeOut":
                    BackGround.FadeOut(match.Groups[4].Value , end);
                    Debug.Log($"backGroundFadeOut : {match.Groups[4].Value}");
                    break;

                �@//�X�L�b�v�s��
                case "$DontSkipFadeIn":
                    BackGround.DontSkipFadeIn(match.Groups[4].Value, end);
                    Debug.Log($"backGroundDontSkipFadeIn : {match.Groups[4].Value}");
                    break;
                �@//�X�L�b�v�s��
                case "$DontSkipFadeOut":
                    BackGround.DontSkipFadeOut(match.Groups[4].Value, end);
                    Debug.Log($"backGroundDontSkipFadeOut : {match.Groups[4].Value}");
                    break;
            }
        }
        //�摜�̐؂�ւ�
        else if (command.Value == "$image")
        {
            switch (process.Value)
            {
                //�X�L�b�v��
                case "$FadeIn":
                    StartCoroutine(CharaManager.FadeIn(match.Groups[4].Value ,index , end));
                    Debug.Log($"Image : {match.Groups[4].Value}");
                    break;
                //�X�L�b�v��
                case "$FadeOut":
                    StartCoroutine(CharaManager.FadeOut(match.Groups[4].Value, index, end));
                    Debug.Log($"Image : {match.Groups[4].Value}");
                    break;

                //�X�L�b�v�s��
                case "$DontSkipFadeIn":
                    CharaManager.DontSkipFadeIn(match.Groups[4].Value ,index , end);
                    Debug.Log($"backGroundDontSkipFadeIn : {match.Groups[4].Value}");
                    break;
                //�X�L�b�v�s��
                case "$DontSkipFadeOut":
                    CharaManager.DontSkipFadeOut(match.Groups[4].Value, index , end);
                    Debug.Log($"backGroundDontSkipFadeOut : {match.Groups[4].Value}");
                    break;
            }
        }

        if (!end) 
        {
            MoveNext(); //���̍s��
        }
    }

    IEnumerator MoveNextCoroutine() 
    {
        yield return new WaitForEndOfFrame();

        MoveNext();

        yield return null;
    }
}
