using UnityEngine;

public class Cell 
{
    private SpriteRenderer sr;
    private GridManager gridManager;

    public int cellValue { get; private set; }
    public int flaggedNeighbours { get; set; }
    public bool containsBomb { get; set; }
    public bool isOpen { get; private set; }
    public bool isUsedUp { get;  set;}
    public bool isFlagged { get; private set; }
    public bool isSuspected { get; private set; }

    // ctor
    public Cell(SpriteRenderer sr, Sprite initialSprite, GridManager gridManager)
    {
        this.sr = sr;
        this.gridManager = gridManager;
        sr.sprite = initialSprite;
    }

    public void Open(Sprite s)
    {
        sr.sprite = s;
	isOpen = true;
        gridManager.CheckGameWin(containsBomb);
    }

    public void Flag(Sprite s)
    {
        sr.sprite = s;
        isFlagged = !isFlagged;
    }

    public void Suspect(Sprite s)
    {
        sr.sprite = s;
        isSuspected = !isSuspected;
    }

    public void IncreaseValue()
    {
        cellValue++;
    }
}