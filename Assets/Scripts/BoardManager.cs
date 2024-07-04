using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum Boards
{
    Large,
    Middle,
    Small
}

public class BoardManager : MonoBehaviour
{
    private const int MAX_DISTANCE = 4;
    public List<PieceController> player1MovablePieces = new List<PieceController>();
    public List<PieceController> player2MovablePieces = new List<PieceController>();
    public List<List<Transform>> tiles = new List<List<Transform>>();
    public Transform outOfGame;
    private int selectedPiece;


    void Start(){
        SetupBoards();
        SetupPieces();
    }

    
    private void SetupBoards(){
        var boards = FindObjectsByType<IBoard>(FindObjectsSortMode.None).ToList();
        boards.Sort((p1, p2) => p1.name.CompareTo(p2.name));

        for (int i = 0; i < boards.Count; i++)
        {
            if(tiles.Count == i)
                tiles.Add(new List<Transform>());

            foreach (var item in boards[i].GetComponentsInChildren<ITile>())
            {
                tiles[i].Add(item.transform);
            } 
        }
        
    }

    public void CheckBoardStatus()
    {
        if (GameManager.instance.GetCurrentPlayer() == 1)
        {
            player1MovablePieces.Clear();
            var ownATypePieces = GameManager.instance.player1Pieces.FindAll((v) => v.type == PieceTypes.A);
            var ownBTypePieces = GameManager.instance.player1Pieces.FindAll((v) => v.type == PieceTypes.B);
            var ownCTypePiece = GameManager.instance.player1Pieces.Find((v) => v.type == PieceTypes.C);
            var ownDTypePiece = GameManager.instance.player1Pieces.Find((v) => v.type == PieceTypes.D);

            if (ownDTypePiece.currentTile - ownCTypePiece.currentTile >= MAX_DISTANCE)
            {
                player1MovablePieces.AddRange(ownATypePieces);
                player1MovablePieces.AddRange(ownBTypePieces);
                player1MovablePieces.Add(ownCTypePiece);
            }
            else if (ownDTypePiece.currentTile - ownCTypePiece.currentTile <= -MAX_DISTANCE)
            {
                player1MovablePieces.AddRange(ownATypePieces);
                player1MovablePieces.AddRange(ownBTypePieces);
                player1MovablePieces.Add(ownDTypePiece);
            }
            else
            {
                player1MovablePieces.AddRange(ownATypePieces);
                player1MovablePieces.AddRange(ownBTypePieces);
                player1MovablePieces.Add(ownCTypePiece);
                player1MovablePieces.Add(ownDTypePiece);
            }
        }else if (GameManager.instance.GetCurrentPlayer() == 2)
        {
            player2MovablePieces.Clear();
            var ownATypePieces = GameManager.instance.player2Pieces.FindAll((v) => v.type == PieceTypes.A);
            var ownBTypePieces = GameManager.instance.player2Pieces.FindAll((v) => v.type == PieceTypes.B);
            var ownCTypePiece = GameManager.instance.player2Pieces.Find((v) => v.type == PieceTypes.C);
            var ownDTypePiece = GameManager.instance.player2Pieces.Find((v) => v.type == PieceTypes.D);

            if (ownDTypePiece.currentTile - ownCTypePiece.currentTile >= MAX_DISTANCE)
            {
                player2MovablePieces.AddRange(ownATypePieces);
                player2MovablePieces.AddRange(ownBTypePieces);
                player2MovablePieces.Add(ownCTypePiece);
            }
            else if (ownDTypePiece.currentTile - ownCTypePiece.currentTile <= -MAX_DISTANCE)
            {
                player2MovablePieces.AddRange(ownATypePieces);
                player2MovablePieces.AddRange(ownBTypePieces);
                player2MovablePieces.Add(ownDTypePiece);
            }
            else
            {
                player2MovablePieces.AddRange(ownATypePieces);
                player2MovablePieces.AddRange(ownBTypePieces);
                player2MovablePieces.Add(ownCTypePiece);
                player2MovablePieces.Add(ownDTypePiece);
            }
        }
        SelectFirstPiece();
    }

    private void SelectFirstPiece()
    {
        if (GameManager.instance.GetCurrentPlayer() == 1)
        {
            GameManager.instance.SetCurrentPiece(player1MovablePieces[0]);
        }
        else if (GameManager.instance.GetCurrentPlayer() == 2)
        {
            GameManager.instance.SetCurrentPiece(player2MovablePieces[0]);
        }
    }

    public void SelectNextPiece(int piece)
    {
        if (piece == selectedPiece)
            return;

        if (GameManager.instance.GetCurrentPlayer() == 1)
        {
            if (piece >= player1MovablePieces.Count || piece < 0)
                return;
            if (GameManager.instance.GetCurrentPiece().GetIsActive())
            { 
                GameManager.instance.GetCurrentPiece().SetIsActive(false);
                GameManager.instance.GetCurrentPiece().outline.enabled = false;
            }

            selectedPiece = piece;

            GameManager.instance.SetCurrentPiece(player1MovablePieces[selectedPiece]);
        }
        else if (GameManager.instance.GetCurrentPlayer() == 2)
        {
            if (piece >= player2MovablePieces.Count || piece < 0)
                return;
            if (GameManager.instance.GetCurrentPiece())
            {
                GameManager.instance.GetCurrentPiece().SetIsActive(false);
                GameManager.instance.GetCurrentPiece().outline.enabled = false;
            }

            selectedPiece = piece;

            GameManager.instance.SetCurrentPiece(player2MovablePieces[selectedPiece]);
        }
    }

    public void SelectNextPiece(PieceController pieceController)
    {
        int piece = GameManager.instance.GetCurrentPlayer() == 1 ? player1MovablePieces.IndexOf(pieceController) : player2MovablePieces.IndexOf(pieceController);

        if (piece == selectedPiece || piece <= -1)
            return;

        if (GameManager.instance.GetCurrentPlayer() == 1)
        {
            if (piece >= player1MovablePieces.Count || piece < 0)
                return;
            if (GameManager.instance.GetCurrentPiece().GetIsActive())
            {
                GameManager.instance.GetCurrentPiece().SetIsActive(false);
                GameManager.instance.GetCurrentPiece().outline.enabled = false;
            }

            selectedPiece = piece;

            GameManager.instance.SetCurrentPiece(player1MovablePieces[selectedPiece]);
        }
        else if (GameManager.instance.GetCurrentPlayer() == 2)
        {
            if (piece >= player2MovablePieces.Count || piece < 0)
                return;
            if (GameManager.instance.GetCurrentPiece())
            {
                GameManager.instance.GetCurrentPiece().SetIsActive(false);
                GameManager.instance.GetCurrentPiece().outline.enabled = false;
            }

            selectedPiece = piece;

            GameManager.instance.SetCurrentPiece(player2MovablePieces[selectedPiece]);
        }
    }

    public void SetupPieces(){
        foreach (var item in GameManager.instance.player1Pieces)
        {
            item.currentBoard = 0;
            item.outOfGameEvent += (PieceController piece) => {GameManager.instance.player1Pieces.Remove(piece);};
            item.SetOutofGamePosition(outOfGame.position);
            item.MoveToTile(tiles[item.currentBoard][item.currentTile].position);
            item.outline.enabled = false;
        }

        foreach (var item in GameManager.instance.player2Pieces)
        {
            item.currentBoard = 0;
            item.outOfGameEvent += (PieceController piece) => {GameManager.instance.player2Pieces.Remove(piece);};
            item.SetOutofGamePosition(outOfGame.position);
            item.MoveToTile(tiles[item.currentBoard][item.currentTile].position);
            item.outline.enabled = false;
        }

    }

    public void ChangeBoards(PieceController currentPiece, int moveAmount, IEnumerator routine){
        if(currentPiece == null) return;

        var boardToMoveTo = currentPiece.currentBoard + moveAmount;
        if(boardToMoveTo >= tiles.Count)
            return;
        if(boardToMoveTo < 0)
            return;
        if(CheckForPiecesInNextBoard(currentPiece, boardToMoveTo ,currentPiece.currentTile))
        {   
            Debug.LogError("You cannot change boards when the tile is occupied.");
            return;
        }
        currentPiece.currentBoard = boardToMoveTo;
        currentPiece.MoveToNextBoard(tiles[boardToMoveTo][currentPiece.currentTile].position);
        StartCoroutine(routine);
    }
    
    public void MoveCurrentPiece(PieceController currentPiece, int moveAmount, IEnumerator routine){
        if(currentPiece == null) return;


        if(currentPiece.type == PieceTypes.C || currentPiece.type == PieceTypes.D)
        {
            var distance = CheckSpecialPiecesDistance(GameManager.instance.currentPlayer == 1 ? GameManager.instance.player1Pieces : GameManager.instance.player2Pieces);
            var difference = MAX_DISTANCE - Mathf.Abs(distance);
            if (distance > 0 && currentPiece.type == PieceTypes.D)
            {
                if (moveAmount > difference)
                {
                    Debug.LogError("You cannot that much.");
                    return;
                }
            }
            if(distance < 0 && currentPiece.type == PieceTypes.C)
            {
                if (moveAmount > difference)
                {
                    Debug.LogError("You cannot that much.");
                    return;
                }
            }
            
        }
        var tileToMoveTo = 0;
        if (currentPiece.currentBoard == (int)Boards.Small)
        {
            tileToMoveTo = currentPiece.currentTile - moveAmount;
            if (tileToMoveTo < 0)
            {
                tileToMoveTo += tiles[currentPiece.currentBoard].Count;
            }
        }
        else
        {
            tileToMoveTo = currentPiece.currentTile + moveAmount;
            if (tileToMoveTo >= tiles[currentPiece.currentBoard].Count)
            {
                tileToMoveTo -= tiles[currentPiece.currentBoard].Count;
            }
        }

        if(CheckForOwnPieces(currentPiece, tileToMoveTo)){
            Debug.LogError("You cannot move to an occupied tile by your own pieces.");
            return;
        }else if(CheckForOpponetPieces(currentPiece,tileToMoveTo)){
            if(CheckForPiecesInNextBoard(currentPiece,currentPiece.currentBoard + 1,tileToMoveTo) || CheckForSpecialRules())
            {   
                Debug.LogError("You cannot eat that piece.");
                return;
            }
            var eatenPiece = GetOpponetPieceFromTile(currentPiece,tileToMoveTo);
            if(eatenPiece != null){
                if(eatenPiece.type == PieceTypes.A)
                    eatenPiece.MoveToOutOfGame();
                else if(eatenPiece.currentBoard >= tiles.Count - 1){
                    eatenPiece.MoveToOutOfGame();
                }else{
                    eatenPiece.currentBoard += 1;
                    eatenPiece.MoveToNextBoard(tiles[eatenPiece.currentBoard][eatenPiece.currentTile].position);
                }
            }
        }
        currentPiece.MoveToTile(tiles[currentPiece.currentBoard][tileToMoveTo].position, tileToMoveTo);
        StartCoroutine(routine);
    }

     private bool CheckForPiecesInNextBoard(PieceController currentPiece,int nextBoard,int tileToMoveTo){
        var pieceInTile = false;
        
        pieceInTile = GameManager.instance.player1Pieces.Exists((PieceController v) => tileToMoveTo == v.currentTile && v.currentBoard == nextBoard? true : false);
        if(!pieceInTile)
            pieceInTile = GameManager.instance.player2Pieces.Exists((PieceController v) => tileToMoveTo == v.currentTile && v.currentBoard == nextBoard? true : false);
        
        return pieceInTile;
    }

    private bool CheckForOwnPieces(PieceController currentPiece, int tileToMoveTo){
        var pieceInTile = false;

        if(GameManager.instance.currentPlayer == 1){
            pieceInTile = GameManager.instance.player1Pieces.Exists((PieceController v) => tileToMoveTo == v.currentTile && currentPiece.currentBoard == v.currentBoard? true : false);
        }else if(GameManager.instance.currentPlayer == 2){
            pieceInTile = GameManager.instance.player2Pieces.Exists((PieceController v) => tileToMoveTo == v.currentTile && currentPiece.currentBoard == v.currentBoard? true : false);
        }

        return pieceInTile;
    }

    private bool CheckForOpponetPieces(PieceController currentPiece,int tileToMoveTo){
        var pieceInTile = false;


        if(GameManager.instance.currentPlayer == 1){
            pieceInTile = GameManager.instance.player2Pieces.Exists((PieceController v) => tileToMoveTo == v.currentTile && currentPiece.currentBoard == v.currentBoard? true : false);
        }else if(GameManager.instance.currentPlayer == 2){
            pieceInTile = GameManager.instance.player1Pieces.Exists((PieceController v) => tileToMoveTo == v.currentTile && currentPiece.currentBoard == v.currentBoard? true : false);
        }

        return pieceInTile;
    }

    private PieceController GetOwnPieceFromTile(PieceController currentPiece,int tileToMoveTo){
        PieceController pieceInTile = null;

        if(GameManager.instance.currentPlayer == 1){
            pieceInTile = GameManager.instance.player1Pieces.Find((PieceController v) => tileToMoveTo == v.currentTile && currentPiece.currentBoard == v.currentBoard? true : false);
        }else if(GameManager.instance.currentPlayer == 2){
            pieceInTile = GameManager.instance.player2Pieces.Find((PieceController v) => tileToMoveTo == v.currentTile && currentPiece.currentBoard == v.currentBoard? true : false);
        }

        return pieceInTile;
    }

    private PieceController GetOpponetPieceFromTile(PieceController currentPiece,int tileToMoveTo){
        PieceController pieceInTile = null;

        if(GameManager.instance.currentPlayer == 1){
            pieceInTile = GameManager.instance.player2Pieces.Find((PieceController v) => tileToMoveTo == v.currentTile && currentPiece.currentBoard == v.currentBoard? true : false);
        }else if(GameManager.instance.currentPlayer == 2){
            pieceInTile = GameManager.instance.player1Pieces.Find((PieceController v) => tileToMoveTo == v.currentTile && currentPiece.currentBoard == v.currentBoard? true : false);
        }

        return pieceInTile;
    }

    private bool CheckSpecialPiecesBoards(List<PieceController>playerPieces)
    {
        var cTypePiece = playerPieces.Find((v) => v.type == PieceTypes.C);
        var dTypePiece = playerPieces.Find((v) => v.type == PieceTypes.D);
        return dTypePiece.currentBoard == cTypePiece.currentBoard;
    }

    private float CheckSpecialPiecesDistance(List<PieceController> playerPieces, bool isAbs = false)
    {
        var cTypePiece = playerPieces.Find((v) => v.type == PieceTypes.C);
        var dTypePiece = playerPieces.Find((v) => v.type == PieceTypes.D);
        if (isAbs)
            return Mathf.Abs(dTypePiece.currentTile - cTypePiece.currentTile);
        else
            return dTypePiece.currentTile - cTypePiece.currentTile;
    }

    private bool CheckForSpecialRules()
    {
        if (GameManager.instance.currentPlayer == 1)
        {
            if(CheckSpecialPiecesBoards(GameManager.instance.player2Pieces))
            {
                if (CheckSpecialPiecesDistance(GameManager.instance.player2Pieces,true) == 2)
                {
                    return true;
                }
            }
        }
        else if (GameManager.instance.currentPlayer == 2)
        {
            if (CheckSpecialPiecesBoards(GameManager.instance.player1Pieces))
            {
                if (CheckSpecialPiecesDistance(GameManager.instance.player1Pieces,true) == 2)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
