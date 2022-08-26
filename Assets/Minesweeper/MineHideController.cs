using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(GridLayoutGroup))]
public class MineHideController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Minesweeper _minesweeper = default;

    [SerializeField]
    private GameManager _gameManager = default;

    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = default;

    [SerializeField]
    internal int _rows = 10;

    [SerializeField]
    internal int _columns = 10;

    [SerializeField]
    private HideCell _cellPrefab = default;

    Cell[,] _cells;

    internal HideCell[,] _hideCells;

    public int ColumnSet
    {
        get => _columns;
        set
        {
            _columns = value;
            _rows = value;
            Setup();
        }
    }

    private void Setup()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;

        _hideCells = new HideCell[_rows, _columns];

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                _hideCells[r, c] = cell;
            }
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name != "HideText") return;

        HideCell cell = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<HideCell>();

        if (eventData.pointerId == -2) //爆弾の数から旗の数を引いた数を表示する
        {
            Text text = eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>();
            if (text.text.ToString() == "")
            {
                eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text = "〇";
                GameManager._bombCount--;
            }
            else
            {
                eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text = "";
                GameManager._bombCount++;
            }
            return;
        }

        if (!GameManager._inGame && eventData.pointerId == -1) //ゲームを開始してセルを一つ開けたら爆弾を配置する
        {
            _minesweeper.Setup((int)CellCheck(cell).x, (int)CellCheck(cell).y);
            _cells = Minesweeper._cells;
        }

        if (eventData.pointerId == -1)//左クリック
        {
            int r = (int)CellCheck(cell).x;
            int c = (int)CellCheck(cell).y;

            if (_minesweeper.MineCheck(r, c))
            {
                _gameManager.GameClear(false);
                return;
            }

            _hideCells[r, c].HideCellDisclosure(r, c);
        }
    }

    private Vector2 CellCheck(HideCell cell)
    {
        int row = 0;
        int col = 0;

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                if (_hideCells[r, c] == cell)
                {
                    row = r;
                    col = c;
                    break;
                }
            }
        }
        return new Vector2(row, col);
    }

    public void HideOpen(int row, int col)
    {
        foreach (var hidecell in CellCheck(row, col))
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    if (_hideCells[r, c] == hidecell && _cells[r, c].CellState != CellState.Mine && !hidecell._open)
                    {
                        hidecell.HideCellDisclosure(r, c);
                    }
                }
            }
        }
    }

    public List<HideCell> CellCheck(int row, int col)
    {
        List<HideCell> _opencellsList = new List<HideCell>();

        if (row > 0) _opencellsList.Add(_hideCells[row - 1, col]);

        if (row < _rows - 1) _opencellsList.Add(_hideCells[row + 1, col]);

        if (col > 0) _opencellsList.Add(_hideCells[row, col - 1]);

        if (col < _columns - 1) _opencellsList.Add(_hideCells[row, col + 1]);

        if (row + 1 < _rows && col + 1 < _columns) _opencellsList.Add(_hideCells[row + 1, col + 1]);

        if (row - 1 > 0 && col - 1 > 0) _opencellsList.Add(_hideCells[row - 1, col - 1]);

        if (row + 1 < _rows - 1 && col - 1 > 0) _opencellsList.Add(_hideCells[row + 1, col - 1]);

        if (row - 1 > 0 && col + 1 < _columns - 1) _opencellsList.Add(_hideCells[row - 1, col + 1]);

        return _opencellsList;

    }
}
