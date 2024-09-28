using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TetraBoard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TurnText;
    [SerializeField] Image Image;

    [SerializeField] Transform Player1Area;
    [SerializeField] Transform Player2Area;

    public int Width => 3;
    public int Height => 3;
    public float CellSize => cellSize;
    private float cellSize;
    private float screenSize;
    private List<TokenObject> CreatedTokens = new List<TokenObject>();

    private const float EMPTY_TICKNESS = 20f;

    public PawnColor TurnColor;
    private int turnNumber;

    public void Create()
    {
        SetScreenSize();
        CreatePawns();
        turnNumber = 0;
        TurnColor = (PawnColor)UnityEngine.Random.Range(0, 2);
        SetTurn();
    }

    private void ChangeTurn()
    {
        TurnColor = (PawnColor) (1 - (int)TurnColor);
        turnNumber++;
        SetTurn();
    }

    private void SetTurn()
    {
        TurnText.text = TurnColor == PawnColor.WHITE ? "First Player's Turn" : "Second Player's Turn";
        foreach (var item in CreatedTokens)
        {
            if(turnNumber < 6 && item.Coordinate != CoordiantePool.GetCoordinate(-1,-1))
                item.SetDraggable(false);
            else
                item.SetDraggable(TurnColor == item.Pawn.PawnColor);
        }
        if (!CheckIfMoveAvailable(TurnColor))
            ChangeTurn();
    }

    public void CreatePawns()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var obj = SystemLocator.Instance.PoolController.Create<TokenObject>(PoolEnum.Token, i == 0? Player1Area : Player2Area);
                obj.FillTokenData(this, (PawnColor)i, (PawnType)j, cellSize, i == 0 ? Player1Area : Player2Area);
                CreatedTokens.Add(obj);
            }
        }
    }

    public Vector3 GetPosition(Coordinate coordinate)
    {
        return new Vector3(((coordinate.X + 0.5f - Width / 2f) * cellSize), ((coordinate.Y + 0.5f - Height / 2f) * cellSize), 0);
    }

    public Coordinate GetCoordinate(Vector3 position)
    {
        int x = Mathf.RoundToInt ((position.x / cellSize) + (Width / 2f) - 0.5f);
        int y = Mathf.RoundToInt((position.y / (cellSize)) + (Height / 2f) - 0.5f);


        if (x >= Width || x < 0) x = -1;
        if (y >= Height || y < 0) y = -1;

        return CoordiantePool.GetCoordinate(x, y);
    }

    public bool IsCoordinateAvailable(Coordinate coordinate)
    {
        if (coordinate.X < 0 || coordinate.Y < 0) return false;

        bool isEmpty = true;
        foreach (var item in CreatedTokens)
        {
            if (item.Coordinate == coordinate)
            {
                isEmpty = false;
            }
        }

        return isEmpty;
    }

    private bool CheckWin(PawnColor pawnColor)
    {
        CoordinateDictionary<TokenObject> coords = new CoordinateDictionary<TokenObject>();
        foreach (var item in CreatedTokens)
        {
            if(item.Coordinate != CoordiantePool.GetCoordinate(-1, -1) && item.Pawn.PawnColor == pawnColor)
                coords.Add(item.Coordinate, item);
        }

        if (coords.ContainsKey(CoordiantePool.GetCoordinate(1, 1)))
        {
            if (coords.ContainsKey(CoordiantePool.GetCoordinate(0, 0)) && coords.ContainsKey(CoordiantePool.GetCoordinate(2, 2)))
                return true;
            if (coords.ContainsKey(CoordiantePool.GetCoordinate(1, 0)) && coords.ContainsKey(CoordiantePool.GetCoordinate(1, 2)))
                return true;
            if (coords.ContainsKey(CoordiantePool.GetCoordinate(0, 1)) && coords.ContainsKey(CoordiantePool.GetCoordinate(2, 1)))
                return true;
            if (coords.ContainsKey(CoordiantePool.GetCoordinate(0, 2)) && coords.ContainsKey(CoordiantePool.GetCoordinate(2, 0)))
                return true;
        }
        else
        {
            if(coords.ContainsKey(CoordiantePool.GetCoordinate(2, 2)))
            {
                if (coords.ContainsKey(CoordiantePool.GetCoordinate(2, 1)) && coords.ContainsKey(CoordiantePool.GetCoordinate(2, 0)))
                    return true;
                if (coords.ContainsKey(CoordiantePool.GetCoordinate(0, 2)) && coords.ContainsKey(CoordiantePool.GetCoordinate(1, 2)))
                    return true;
            }
            if (coords.ContainsKey(CoordiantePool.GetCoordinate(0, 0)))
            {
                if (coords.ContainsKey(CoordiantePool.GetCoordinate(0, 1)) && coords.ContainsKey(CoordiantePool.GetCoordinate(0, 2)))
                    return true;
                if (coords.ContainsKey(CoordiantePool.GetCoordinate(1, 0)) && coords.ContainsKey(CoordiantePool.GetCoordinate(2, 0)))
                    return true;
            }
        }


        return false;
    }

    private bool CheckIfMoveAvailable(PawnColor pawnColor)
    {
        foreach (var item in CreatedTokens)
        {
            if(item.Pawn.PawnColor == pawnColor)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var tmpCoor = CoordiantePool.GetCoordinate(i, j);
                        if (IsCoordinateAvailable(tmpCoor) && IsPlaceable(item, tmpCoor))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool IsPlaceable(TokenObject tokenObject, Coordinate coordinate)
    {
        bool isPlaceable = false;

        if (tokenObject.Coordinate == CoordiantePool.GetCoordinate(-1, -1))
            isPlaceable = true;

        if (tokenObject.Pawn.PawnType == PawnType.BISHOP)
        {
            if (Mathf.Abs(coordinate.X - tokenObject.Coordinate.X) == Mathf.Abs(coordinate.Y - tokenObject.Coordinate.Y))
            {
                if (Mathf.Abs(coordinate.X - tokenObject.Coordinate.X) == 1 ||
                (Mathf.Abs(coordinate.X - tokenObject.Coordinate.X) == 2 && IsCoordinateAvailable(CoordiantePool.GetCoordinate((coordinate.X + tokenObject.Coordinate.X) / 2, (coordinate.Y + tokenObject.Coordinate.Y) / 2))))
                    isPlaceable = true;
            }
        }
        if (tokenObject.Pawn.PawnType == PawnType.ROOK)
        {
            if (coordinate.X == tokenObject.Coordinate.X && (Mathf.Abs(coordinate.Y - tokenObject.Coordinate.Y) == 1 ||
                (Mathf.Abs(coordinate.Y - tokenObject.Coordinate.Y) == 2 && IsCoordinateAvailable(CoordiantePool.GetCoordinate(coordinate.X, (coordinate.Y + tokenObject.Coordinate.Y) / 2)))
                ))
                isPlaceable = true;

            if (coordinate.Y == tokenObject.Coordinate.Y && (Mathf.Abs(coordinate.X - tokenObject.Coordinate.X) == 1 ||
                (Mathf.Abs(coordinate.X - tokenObject.Coordinate.X) == 2 && IsCoordinateAvailable(CoordiantePool.GetCoordinate((coordinate.X + tokenObject.Coordinate.X) / 2, coordinate.Y)))
                ))
                isPlaceable = true;
        }
        if (tokenObject.Pawn.PawnType == PawnType.KNIGHT)
        {
            if (Mathf.Abs(coordinate.X - tokenObject.Coordinate.X) + Mathf.Abs(coordinate.Y - tokenObject.Coordinate.Y) == 3)
                isPlaceable = true;
        }
        return isPlaceable;
    }

    public void Place(TokenObject tokenObject, Coordinate coordinate)
    {
        var isPlaceable = IsPlaceable(tokenObject, coordinate);

        if (isPlaceable)
        {
            tokenObject.UpdateCoordinate(coordinate);
            if(!CheckWin(tokenObject.Pawn.PawnColor))
                ChangeTurn();
            else
            {
               // Debug.Log(tokenObject.Pawn.PawnColor.ToString() + " wins!!");
                SystemLocator.Instance.PanelController.Show(PanelType.WinPanel, tokenObject.Pawn.PawnColor.ToString() + " wins!!");
            }
        }
        else
            tokenObject.UpdateCoordinate(tokenObject.Coordinate);
    }

    public void ClearBoard(Action callback)
    {
        int order = 0;
        foreach (var item in CreatedTokens)
        {
            item.Dispose(order, () => { });
            order++;
        }
        CreatedTokens.Clear();
        callback.Invoke();
    }

    private void SetScreenSize()
    {
        screenSize = Image.rectTransform.rect.width;
        cellSize = (screenSize - (EMPTY_TICKNESS * 2f)) / Width;
    }

}
