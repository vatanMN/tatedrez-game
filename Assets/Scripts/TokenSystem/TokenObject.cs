using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class TokenObject : DragAndDrop
{
    public Pawn Pawn;
    public string TokenInfo;
    [SerializeField] private Image Image;
    public Coordinate Coordinate;
    public TetraBoard Board;
    private float CellSize;
    private Transform DefaultParent;

    public void FillTokenData(TetraBoard board, PawnColor pawnColor, PawnType pawnType, float cellX, Transform defaultParent)
    {
        Pawn = new Pawn()
        {
            PawnColor = pawnColor,
            PawnType = pawnType
        };
        Coordinate = CoordiantePool.GetCoordinate(-1,-1);
        Board = board;
        Image.sprite = Pawn.Sprite;
        CellSize = cellX;
        Image.rectTransform.sizeDelta = new Vector2(cellX, cellX);
        DefaultParent = defaultParent;
        transform.SetParent(DefaultParent);
        transform.position = Vector3.zero;
    }

    public void SetDraggable(bool isDraggable)
    {
        Image.raycastTarget = isDraggable;
    }

    public void UpdateCoordinate(Coordinate coordinate)
    {
        if(coordinate == CoordiantePool.GetCoordinate(-1, -1))
        {
            transform.SetParent(DefaultParent);
            transform.position = Vector3.zero;
            return;
        }

        Coordinate = coordinate;
        var pos = Board.GetPosition(Coordinate);

        transform.DOLocalMove(pos, 0.3f).OnComplete(() => {
            transform.localPosition = pos;
        });
    }

    public void Dispose(int order, Action Callback)
    {
        transform.DOJump(Vector3.zero, 2f, 1, 0.3f + 0.01f *order).OnComplete(() => {
            Callback.Invoke();
            SystemLocator.Instance.PoolController.Destroy(PoolEnum.Token, gameObject);
        });
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        var tmpCoor = Board.GetCoordinate(transform.localPosition);
        if(!Board.IsCoordinateAvailable(tmpCoor))
        {
            UpdateCoordinate(Coordinate);
        }
        else
        {
            Board.Place(this, tmpCoor);
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(Board.transform);
        transform.SetSiblingIndex(15);
    }

}

public class Pawn
{
    public PawnColor PawnColor;
    public PawnType PawnType;
    public Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>("Pawns/" + PawnType.ToString().ToUpper() + "_" + PawnColor.ToString().ToUpper());
            }

            return sprite;
        }
    }
} 

public enum PawnColor
{
    WHITE,
    BLACK
}

public enum PawnType
{
    KNIGHT,
    BISHOP,
    ROOK
}
