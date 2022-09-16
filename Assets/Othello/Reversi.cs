using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Reversi : SingletonMonoBehaviour<Reversi>
{
    [SerializeField, Tooltip("セルとリバーシをまとめた親オブジェクト")]
    GameObject _cellsPosition = default;

    [SerializeField, Tooltip("棋譜を入れる")]
    string _gameRecord = null;

    public ReversiCell[,] _cells = new ReversiCell[8, 8];

    private List<ReversiCell> _reversiCheckList = new List<ReversiCell>();

    private List<ReversiCell> _animationReversiList = new List<ReversiCell>();

    private int _trunCount = 0;

    private int _placementcCellCount = 0;

    private bool _reverseBool = false;

    private int j = 0;

    private void Start()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                _cells[r, c] = _cellsPosition.transform.Find($"CellandReversi {r} {c}").GetComponent<ReversiCell>();
                _cells[r, c].ReversiState = ReversiState.None;
                _cells[r, c].OnReversiState();
            }
        }
        Setup();
    }

    /// <summary>
    /// ゲームの初期化
    /// </summary>
    void Setup()
    {
        _cells[3, 3].ReversiState = ReversiState.White; _cells[3, 3].OnReversiState();
        _cells[4, 4].ReversiState = ReversiState.White; _cells[4, 4].OnReversiState();
        _cells[3, 4].ReversiState = ReversiState.Black; _cells[3, 4].OnReversiState();
        _cells[4, 3].ReversiState = ReversiState.Black; _cells[4, 3].OnReversiState();
        Cursor.Instance.PotisionChange(4, 3);
        GameRecord();
        AllCheck();
    }

    private void AllCheck()
    {
        ReversiState rt;

        if (_trunCount % 2 == 0)
        {
            rt = ReversiState.Black;
        }
        else
        {
            rt = ReversiState.White;
        }

        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                if (_cells[x, z].ReversiState == ReversiState.None)
                    CellCheckFast(x, z, rt);
            }
        }
    }

    public void Arrangement(int x, int z)
    {
        if (_trunCount % 2 == 0)
        {
            _cells[x, z].ReversiState = ReversiState.Black;
        }
        else
        {
            _cells[x, z].ReversiState = ReversiState.White;
        }

        _cells[x, z].OnReversiState();

        _reversiCheckList = new List<ReversiCell>();

        CellCheckFast(x, z, _cells[x, z].ReversiState);

        Reversal(x, z, _cells[x, z].ReversiState, 1);
    }

    private void CellCheckFast(int x, int z, ReversiState rt)
    {
        ReversiCell[] rc = CellCheck(x, z, 1);

        for (int i = 0; i < 8; i++)
        {
            if (rc[i] != null)
            {
                if (rc[i].ReversiState != ReversiState.None && rc[i].ReversiState != rt)
                {
                    for (int r = 0; r < 8; r++)
                    {
                        for (int c = 0; c < 8; c++)
                        {
                            if (rc[i] == _cells[r, c])
                                rc[i].CellChack(x, z, r, c, rt);
                        }
                    }
                }
            }
        }
        if (_reverseBool == true)
        {
            _cells[x, z].ReversiCellState = (ReversiCellState)rt;
            _cells[x, z].OnCellState();
        }
        _reverseBool = false;
    }

    public void NextCheck(int Original_x, int Original_z, int me_x, int me_z, ReversiState reversi)
    {
        int x = me_x - Original_x;
        int z = me_z - Original_z;

        if (x < 0) x--;
        else if (x > 0) x++;

        if (z < 0) z--;
        else if (z > 0) z++;

        if (x == 0 && z == 0) return;

        if (Original_z + z < 8 && Original_z + z > -1 && Original_x + x < 8 && Original_x + x > -1)
        {
            _cells[Original_x + x, Original_z + z].CellChack(Original_x, Original_z, Original_x + x, Original_z + z, reversi);
        }
    }

    public void ReversalBool(int original_x, int original_z, int me_x, int me_z)
    {
        if (!_reverseBool)
        {
            _reverseBool = true;
        }

        if (me_x != original_x && me_z != original_z)
        {
            _reversiCheckList.Add(_cells[me_x, me_z]);
        }

        int x = me_x - original_x;
        int z = me_z - original_z;

        while (x != 0 || z != 0)
        {
            if (x < 0) x++;
            else if (x > 0) x--;

            if (z < 0) z++;
            else if (z > 0) z--;

            if (x == 0 && z == 0) { break; }

            _reversiCheckList.Add(_cells[original_x + x, original_z + z]);
        }
    }

    private void Reversal(int x, int z, ReversiState rs, int count)
    {

        Cursor.Instance._animationStop = true;
        _animationReversiList = new List<ReversiCell>();

        foreach (var reversiCell in CellCheck(x, z, count))
        {
            if (_reversiCheckList.Find(n => n == reversiCell))
            {
                reversiCell.ReversiState = rs;
                _animationReversiList.Add(reversiCell);
                _reversiCheckList.Remove(reversiCell);
            }
        }
        StartCoroutine(AnimationStop(x, z, rs, count));
    }

    private IEnumerator AnimationStop(int x, int z, ReversiState rs, int count)
    {
        foreach (var reversiCell in _animationReversiList)
        {
            reversiCell.OnReversiState();
        }

        if (count > 6 || _reversiCheckList.Count == 0)
        {
            TurnEnd();
        }
        else
        {

            yield return new WaitForSeconds(0.2f);
            count++;
            Reversal(x, z, rs, count);
        }

    }

    private void TurnEnd()
    {
        Cursor.Instance._animationStop = false;

        _placementcCellCount++;

        if (_placementcCellCount == 60) { GameSet(); }

        CellReset();

        _trunCount++;

        _reversiCheckList = new List<ReversiCell>();

        AllCheck();

        if (_reversiCheckList.Count == 0) 
        { 
            _trunCount++; AllCheck();

            if (_reversiCheckList.Count == 0)
            {
                GameSet();
            }
        }
    }

    private ReversiCell[] CellCheck(int x, int z, int addValue)
    {
        ReversiCell[] _opencellsArray = new ReversiCell[8];

        if (z - addValue >= 0) _opencellsArray[0] = _cells[x, z - addValue];

        if (z + addValue <= 7) _opencellsArray[1] = _cells[x, z + addValue];

        if (x - addValue >= 0) _opencellsArray[2] = _cells[x - addValue, z];

        if (x + addValue <= 7) _opencellsArray[3] = _cells[x + addValue, z];

        if (z + addValue <= 7 && x + addValue <= 7) _opencellsArray[4] = _cells[x + addValue, z + addValue];

        if (z - addValue >= 0 && x - addValue >= 0) _opencellsArray[5] = _cells[x - addValue, z - addValue];

        if (z + addValue <= 7 && x - addValue >= 0) _opencellsArray[6] = _cells[x - addValue, z + addValue];

        if (z - addValue >= 0 && x + addValue <= 7) _opencellsArray[7] = _cells[x + addValue, z - addValue];

        return _opencellsArray;

    }

    private void CellReset()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                _cells[r, c].CellReset();
            }
        }
    }

    private void GameSet()
    {
        int black = 0;
        int white = 0;

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                if (_cells[r, c].ReversiState == ReversiState.White) white++;
                else black++;
            }
        }

        if (white > black)
        {
            ReversiGameManager.Instance.Gameset(ReversiState.White);
        }
        else ReversiGameManager.Instance.Gameset(ReversiState.Black);
    }

    public void GameRecord()
    {
        if (_gameRecord != null)
        {
            for (int i = 0; i < _gameRecord.Length; i += 2)
            {

                if (_gameRecord.Substring(i, 2) != null)
                {
                    RecordConversion(_gameRecord.Substring(i, 2));
                    //j += 2;
                }
                else { Debug.Log("Error01"); /*j = 0;*/ break; }
            }
        }
        else { Debug.Log("Error02"); }
    }

    private void RecordConversion(string record)
    {
        string secoundLetter = record.Substring(0, 1);

        int secoundInt = 0;

        if (secoundLetter == "A") { secoundInt = 0; }
        else if (secoundLetter == "B") { secoundInt = 1; }
        else if (secoundLetter == "C") { secoundInt = 2; }
        else if (secoundLetter == "D") { secoundInt = 3; }
        else if (secoundLetter == "E") { secoundInt = 4; }
        else if (secoundLetter == "F") { secoundInt = 5; }
        else if (secoundLetter == "G") { secoundInt = 6; }
        else if (secoundLetter == "H") { secoundInt = 7; }
        else { Debug.Log("Error03"); return; }

        RecordArrangement(int.Parse(record.Substring(1, 1)) - 1, secoundInt);
    }

    private void RecordArrangement(int x, int z)
    {
        if (_cells[x, z].ReversiState != ReversiState.None)
        { Debug.Log("Error04"); return; }

        if (_trunCount % 2 == 0)
        {
            _cells[x, z].ReversiState = ReversiState.Black;
        }
        else
        {
            _cells[x, z].ReversiState = ReversiState.White;
        }

        _cells[x, z].OnReversiState();

        _reversiCheckList = new List<ReversiCell>();

        CellCheckFast(x, z, _cells[x, z].ReversiState);

        RecordReversal(x, z, _cells[x, z].ReversiState);
    }

    private void RecordReversal(int x, int z, ReversiState rs)
    {
        for (int r = 0; r < 8; r++) 
        {
            for (int c = 0; c < 8; c++) 
            {
                if (_reversiCheckList.Find(n => n == _cells[r , c])) 
                {
                    _cells[r, c].ReversiState = rs;
                    _cells[r, c].OnReversiState();
                }
            }
        }

        _placementcCellCount++;

        if (_placementcCellCount == 60) { GameSet(); }

        _trunCount++;
    }
}
