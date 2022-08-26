using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class LifeGame : MonoBehaviour , IPointerClickHandler
{
    [SerializeField]
    private LifeCell _lifeCell = default;
    [SerializeField]
    private int _rows = 0;
    [SerializeField]
    private int _columns = 0;

    [SerializeField]
    int _baseRowCount = 0;
    [SerializeField]
    int _baseColumnCount = 0;

    private GridLayoutGroup _gridLayoutGroup = default;

    LifeCell[,] _lifeCells;

    // Start is called before the first frame update
    void Start()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;

        _gridLayoutGroup.cellSize = new Vector2((float)_baseRowCount * _gridLayoutGroup.cellSize.x / _rows , (float)_baseColumnCount * _gridLayoutGroup.cellSize.y / _columns);

        _lifeCells = new LifeCell[_rows , _columns];
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_lifeCell, _gridLayoutGroup.transform);
                cell.LifeState = LifeState.dead;
                _lifeCells[r , c] = cell;
            }
        }
    }


    /// <summary>
    /// クリックしたセルのライフを逆にする
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "LifeCell(Clone)") 
        {
           LifeCell lifecell = eventData.pointerCurrentRaycast.gameObject.GetComponent<LifeCell>();
            
            if (lifecell.LifeState == LifeState.alive) lifecell.LifeState = LifeState.dead;
            else lifecell.LifeState = LifeState.alive;
        }
    }

    /// <summary>
    /// 次のループ処理
    /// </summary>
    private void NextLoop()
        {
        LifeState[,] nextLiefStates = new LifeState[_rows ,_columns];

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                int aliveCount = 0;
                foreach (var lifeCell in CellCheck(row, col))
                {
                    if (lifeCell.LifeState == LifeState.alive) aliveCount++;
                }

                nextLiefStates[row, col] = LifeState.dead;

                if (_lifeCells[row, col].LifeState == LifeState.dead && aliveCount == 3)
                    nextLiefStates[row, col] = LifeState.alive;
                else if (_lifeCells[row, col].LifeState == LifeState.alive && aliveCount == 2 || aliveCount == 3)
                {
                    nextLiefStates[row, col] = LifeState.alive;
                }
            }
        }

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                _lifeCells[row, col].LifeState = nextLiefStates[row, col];
            }
        }
              
    }

    /// <summary>
    /// 周囲8マスのセルを取得する関数
    /// </summary>
    public List<LifeCell> CellCheck(int row, int col)
    {
        List<LifeCell> _opencellsList = new List<LifeCell>();

        if (row > 0) _opencellsList.Add(_lifeCells[row - 1, col]);

        if (row < _rows - 1) _opencellsList.Add(_lifeCells[row + 1, col]);

        if (col > 0) _opencellsList.Add(_lifeCells[row, col - 1]);

        if (col < _columns - 1) _opencellsList.Add(_lifeCells[row, col + 1]);

        if (row + 1 < _rows && col + 1 < _columns) _opencellsList.Add(_lifeCells[row + 1, col + 1]);

        if (row - 1 > 0 && col - 1 > 0) _opencellsList.Add(_lifeCells[row - 1, col - 1]);

        if (row + 1 < _rows - 1 && col - 1 > 0) _opencellsList.Add(_lifeCells[row + 1, col - 1]);

        if (row - 1 > 0 && col + 1 < _columns - 1) _opencellsList.Add(_lifeCells[row - 1, col + 1]);

        return _opencellsList;

    }

    /// <summary>
    /// セルステートを再配置
    /// </summary>
    public void Reset()
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                _lifeCells[r, c].LifeState = Random.Range(0, 100) < 12 ? LifeState.alive : LifeState.dead;
            }
        }
        LifeLoopStop();
    }

    /// <summary>
    /// セルステートをクリア
    /// </summary>
    public void Clear() 
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                _lifeCells[r, c].LifeState = LifeState.dead;
            }
        }
        LifeLoopStop();
    }

    private bool _isLoop = false;

    /// <summary>
    /// ループ開始
    /// </summary>
    public void LifeLoopStart()
    {
        if (_isLoop) return;
        StartCoroutine(LifeLoopCoroutine());
    }

    /// <summary>
    /// ループ停止
    /// </summary>
    public void LifeLoopStop()
    {
        _isLoop = false;
    }

    /// <summary>
    /// ループコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator LifeLoopCoroutine()
    {
        // ループを停止するまで一定間隔で次の世帯を呼び出す
        _isLoop = true;
        while (_isLoop)
        {
            yield return new WaitForSeconds(0.1f);
            NextLoop();
        }
    }
}
