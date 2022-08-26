using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class Minesweeper : MonoBehaviour
{
    [SerializeField]
    private MineHideController _mh = default;

    [SerializeField]
    private GameManager _gameManager = default;

    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = default;

    [SerializeField]
    internal int _rows = 10;

    [SerializeField]
    internal int _columns = 10;

    [SerializeField]
    private Cell _cellPrefab = default;

    [SerializeField]
    private int _mineCount = 10;

    public static Cell[,] _cells;

    private int[,] _cellsCheck;

    private void Awake()
    {
        if (_rows > 6 && _columns > 6 && _rows * _columns / 2 <= _mineCount)
            GameManager._bombCount = _mineCount - 9;
        else if (_rows * _columns / 2 <= _mineCount) GameManager._bombCount = _mineCount - 1;
        else GameManager._bombCount = _mineCount;
    }

    private void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;

        _mh.ColumnSet = _columns;

        _cells = new Cell[_rows, _columns];
        _cellsCheck = new int[_rows, _columns];
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                _cells[r, c] = cell;
            }
        }
    }

    public void Setup(int row, int col)
    {
        int i = 0;

        if (_rows * _columns <= _mineCount)  //”š’e‚Ì”‚ªƒZƒ‹‚Ì”‚æ‚è‘½‚©‚Á‚½‚ç”š’e‚Å–„‚ß‚ÄƒQ[ƒ€ƒI[ƒo[‚É‚·‚é
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    if (r != row || c != col)
                    {
                        _cells[r, c].CellState = CellState.Mine;
                        _cellsCheck[r, c] = -1;
                    }
                }
            }

            if (_rows > 6 && _columns > 6)
            {
                var noneCells = CellCheck(row, col);
                foreach (var noneCell in noneCells)
                {
                    noneCell.CellState = CellState.None;
                }
            }

            _gameManager.GameClear(false);
            return;
        }

        if (_rows > 6 && _columns > 6 && _rows * _columns / 2 <= _mineCount)//”š’e‚Ì‚Ù‚¤‚ª‘½‚¢‚Æ‚«”š’e‚©‚ç”z’u
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    if (r != row || c != col)
                    {
                        _cells[r, c].CellState = CellState.Mine;
                        _cellsCheck[r, c] = -1;
                    }
                }
            }

            if (_rows > 6 && _columns > 6)
            {
                var noneCells = CellCheck(row, col);
                foreach (var noneCell in noneCells)
                {
                    noneCell.CellState = CellState.None;
                }
            }

            while (i < _rows * _columns - _mineCount)
            {
                var r = Random.Range(0, _rows);
                var c = Random.Range(0, _columns);
                if (_cells[r, c].CellState != CellState.None)
                {
                    _cells[r, c].CellState = CellState.None;
                    _cellsCheck[r, c] = 0;
                    i++;
                }
            }
        }
        else if ( _rows * _columns / 2 <= _mineCount)
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    if (r != row && c != col)
                    {
                        _cells[r, c].CellState = CellState.Mine;
                        _cellsCheck[r, c] = -1;
                    }
                }
            }

            while (i < _rows * _columns - _mineCount -1)
            {
                var r = Random.Range(0, _rows);
                var c = Random.Range(0, _columns);
                if (_cells[r, c].CellState != CellState.None)
                {
                    _cells[r, c].CellState = CellState.None;
                    _cellsCheck[r, c] = 0;
                    i++;
                }
            }
        }
        else
        {
            if (_rows > 6 && _columns > 6)
            {
                var noneCells = CellCheck(row, col);

                while (i < _mineCount)
                {
                    var r = Random.Range(0, _rows);
                    var c = Random.Range(0, _columns);
                    if (r != row && c != col && _cells[r, c].CellState != CellState.Mine)
                    {
                        bool check = false;

                        foreach (var noneCell in noneCells)
                        {
                            if (noneCell == _cells[r, c])
                            {
                                check = true; break;
                            }
                        }

                        if (!check) 
                        {
                            _cells[r, c].CellState = CellState.Mine;
                            _cellsCheck[r, c] = -1;
                            i++;
                        }
                    }
                }
            }
            else 
            {
                while (i < _mineCount)
                {
                    var r = Random.Range(0, _rows);
                    var c = Random.Range(0, _columns);
                    if (r != row && c != col && _cells[r, c].CellState != CellState.Mine)
                    {
                        _cells[r, c].CellState = CellState.Mine;
                        _cellsCheck[r, c] = -1;
                        i++;
                    }
                }
            }
            
        }

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                if (_cells[r, c].CellState == CellState.Mine)
                {
                    if (r + 1 < _rows && _cells[r + 1, c].CellState != CellState.Mine)
                        _cells[r + 1, c].CellState++;

                    if (c + 1 < _columns && _cells[r, c + 1].CellState != CellState.Mine)
                        _cells[r, c + 1].CellState++;

                    if (r + 1 < _rows && c + 1 < _columns && _cells[r + 1, c + 1].CellState != CellState.Mine)
                        _cells[r + 1, c + 1].CellState++;

                    if (r - 1 >= 0 && _cells[r - 1, c].CellState != CellState.Mine)
                        _cells[r - 1, c].CellState++;

                    if (c - 1 >= 0 && _cells[r, c - 1].CellState != CellState.Mine)
                        _cells[r, c - 1].CellState++;

                    if (r - 1 >= 0 && c - 1 >= 0 && _cells[r - 1, c - 1].CellState != CellState.Mine)
                        _cells[r - 1, c - 1].CellState++;

                    if (r + 1 < _rows && c - 1 >= 0 && _cells[r + 1, c - 1].CellState != CellState.Mine)
                        _cells[r + 1, c - 1].CellState++;

                    if (r - 1 >= 0 && c + 1 < _columns && _cells[r - 1, c + 1].CellState != CellState.Mine)
                        _cells[r - 1, c + 1].CellState++;
                }
            }
        }
        _gameManager.GameStart();
    }

    public List<Cell> CellCheck(int row, int col)
    {
        List<Cell> _opencellsList = new List<Cell>();

        if (row > 0) _opencellsList.Add(_cells[row - 1, col]);

        if (row < _rows - 1) _opencellsList.Add(_cells[row + 1, col]);

        if (col > 0) _opencellsList.Add(_cells[row, col - 1]);

        if (col < _columns - 1) _opencellsList.Add(_cells[row, col + 1]);

        if (row + 1 < _rows && col + 1 < _columns) _opencellsList.Add(_cells[row + 1, col + 1]);

        if (row - 1 > 0 && col - 1 > 0) _opencellsList.Add(_cells[row - 1, col - 1]);

        if (row + 1 < _rows - 1 && col - 1 > 0) _opencellsList.Add(_cells[row + 1, col - 1]);

        if (row - 1 > 0 && col + 1 < _columns - 1) _opencellsList.Add(_cells[row - 1, col + 1]);

        return _opencellsList;

    }


    public bool MineCheck(int row, int col)
    {
        if (GameManager._inGame)
        {
            if (row > _rows && col > _columns && row < 0 && col < 0) return false; //“n‚³‚ê‚½’l‚ªƒZƒ‹‚Ì”ÍˆÍ“à‚©

            if (_cells[row, col].CellState == CellState.Mine) return true; //”š’e‚È‚çƒQ[ƒ€ƒI[ƒo[‚Å•Ô‚·

            _cellsCheck[row, col] = 1; //‰æ–Ê“à‚Å‰f‚Á‚½‰º‚ÌƒZƒ‹‚Í1‚É‚·‚é

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    if (_cellsCheck[r, c] == 0) return false; //‰æ–Ê“à‚Å‰f‚Á‚Ä‚¢‚È‚¢‰º‚ÌƒZƒ‹‚ª‚ ‚Á‚½‚ç‘±s
                }
            }
            _gameManager.GameClear(true); //‰æ–Ê“à‚Å‰f‚Á‚Ä‚¢‚È‚¢‰º‚ÌƒZƒ‹‚ª‚È‚¯‚ê‚ÎƒNƒŠƒA
            return false;
        }
        return false;
    }

    public void MineDisclosure()
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                if (!_mh._hideCells[r, c]._open)
                    _mh._hideCells[r, c].HideCellDisclosure(r, c);
            }
        }
    }
}
