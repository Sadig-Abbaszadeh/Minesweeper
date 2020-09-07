using UnityEngine;

public enum CommandType { Open, Flag, Suspect }

public class GridManager
{
    private int width, height;
    private float cellSize;
    private Vector2 origin;
    private Sprite[] sprites;
    private Cell[,] grid;

    // ctor
    public GridManager(int width, int height, float cellSize, Vector2 origin, Sprite[] sprites, int bombCount)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.sprites = sprites;

        grid = new Cell[width, height];

        // settings up cells
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                GameObject obj = new GameObject(x + "; " + y);
                obj.transform.position = GridToWorldPosition(new Vector2Int(x, y));
                grid[x, y] = new Cell(obj.AddComponent<SpriteRenderer>(), sprites[9], this);
            }

        // assigning bombs
        while (bombCount > 0)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (grid[x, y].containsBomb == false)
            {
                grid[x, y].containsBomb = true;
                ActOnNeighbours(x, y, (int cellX, int cellY) => grid[cellX, cellY].IncreaseValue());
                bombCount--;
            }
        }
    }

    // receive command from master
    public void ReceiveCommand(Vector2 worldPos, CommandType type)
    {
        int x, y;
        WorldToGridPosition(worldPos, out x, out y);

        if (x < 0 || y < 0 || x >= width || y >= height)
            return;
        
        if(grid[x, y].isOpen)
        {
            if (!grid[x, y].isUsedUp && grid[x, y].cellValue == grid[x, y].flaggedNeighbours)
            {
                ActOnNeighbours(x, y, (int cellX, int cellY) => { if (!grid[cellX, cellY].isOpen) OpenACell(cellX, cellY); });
                grid[x, y].isUsedUp = true;
            }

            return;
        }

        switch (type)
        {
            case CommandType.Open:
                OpenACell(x, y);
                break;
            case CommandType.Flag:
                FlagACell(x, y);
                break;
            case CommandType.Suspect:
                SuspectACell(x, y);
                break;
            default:
                break;
        }
    }

    // callback from a cell to check whether the player has won. Parameter checks if the last cell opened is bomb
    public void CheckGameWin(bool cellHasABomb)
    {
        GameMaster.Instance.CheckGameWin(cellHasABomb);
    }

    // get grid x&y from world position
    private void WorldToGridPosition(Vector2 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPos - origin).y / cellSize);
    }

    // get world pos from grid x&y
    private Vector2 GridToWorldPosition(Vector2Int gridPos)
    {
        return (Vector2)gridPos * cellSize + origin;
    }

    // open one cell
    private void OpenACell(int x, int y)
    {
        if (grid[x, y].isFlagged || grid[x, y].isSuspected)
            return;

        if (grid[x, y].containsBomb == true)
        {
            GameMaster.Instance.GameOver(false);
        }
        else if (grid[x, y].cellValue != 0)
        {
            grid[x, y].Open(sprites[grid[x, y].cellValue]);
        }
        else
        {
            OpenSafeCells(x, y);
        }
    }

    // open a 0 cell and its neighbouring cells
    private void OpenSafeCells(int x, int y)
    {
        grid[x, y].Open(sprites[grid[x, y].cellValue]);
        grid[x, y].isUsedUp = true;
        ActOnNeighbours(x, y, (int cellX, int cellY) => { if (!grid[cellX, cellY].isOpen) { if (grid[cellX, cellY].cellValue == 0) { OpenSafeCells(cellX, cellY); } else { grid[cellX, cellY].Open(sprites[grid[cellX, cellY].cellValue]); } } });
    }

    // flag a cell
    private void FlagACell(int x, int y)
    {
        if (grid[x, y].isSuspected)
            return;
        
        int value;
        Sprite s;

        if (grid[x, y].isFlagged)
        {
            value = -1;
            s = sprites[9];
        }
        else
        {
            value = 1;
            s = sprites[10];
        }

        ActOnNeighbours(x, y, (int cellX, int cellY) => grid[cellX, cellY].flaggedNeighbours += value);
	GameMaster.Instance.UpdateBombCount(-value);
        grid[x, y].Flag(s);
    }

    // suspect a cell
    private void SuspectACell(int x, int y)
    {
        if (grid[x, y].isFlagged)
            return;

        Sprite s;

        if (grid[x, y].isSuspected)
            s = sprites[9];
        else
            s = sprites[11];
        
        grid[x, y].Suspect(s);
    }

    // do sth with neighboring cells
    private void ActOnNeighbours(int x, int y, System.Action<int, int> actionOnNeighbours)
    {
        // left
        if (x - 1 >= 0)
        {
            actionOnNeighbours(x - 1, y);
            if (y - 1 >= 0)
                actionOnNeighbours(x - 1, y - 1);
            if (y + 1 < height)
                actionOnNeighbours(x - 1, y + 1);
        }
        // right
        if (x + 1 < width)
        {
            actionOnNeighbours(x + 1, y);
            if (y - 1 >= 0)
                actionOnNeighbours(x + 1, y - 1);
            if (y + 1 < height)
                actionOnNeighbours(x + 1, y + 1);
        }
        // down
        if (y - 1 >= 0)
            actionOnNeighbours(x, y - 1);
        // up 
        if (y + 1 < height)
            actionOnNeighbours(x, y + 1);
    }
}