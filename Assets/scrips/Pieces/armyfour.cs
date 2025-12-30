using UnityEngine;
using System.Collections.Generic;
public class armyfour : Piece
{
    public override List<int> GetAvailableMoves(ref Piece[] board, int TILE_NUMBER)
    {
        List<int> temp = getNextMove(availableMovesPerIndex[currentN]);
        if (this.haveItem.Contains(itemType.car))
        {
            temp = getNextMove(temp);
        }
        List<int> moves = new List<int>(temp);
        Piece[] boardCopy = board;
        moves.RemoveAll(i => boardCopy[i] != null);
        boardObj = GameObject.Find("board");

        board bod = boardObj.GetComponent<board>();
        moves.RemoveAll(i => bod.tiles[i].layer == LayerMask.NameToLayer("unTile"));
        return moves;
    }
}
