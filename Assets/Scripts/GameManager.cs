using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

enum Layers
{   
    Default,
    TransparentFX,
    IgnoreRaycast,
    Piece,
    Water,
    UI
}

public class GameManager : MonoBehaviour
{
    public List<PieceController> player1Pieces = new List<PieceController>();
    public List<PieceController> player2Pieces = new List<PieceController>();
    public BoardManager boardManager;


    public int currentPlayer = 0;
    public float lateStart = 1.0f;
    private PieceController currentPiece;
    
    public static GameManager instance;

    void Awake(){
        if (instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    void Start(){
        StartCoroutine(LateChangePlayer());
    }

    void Update(){
        if (currentPiece != null) {
            if (currentPiece.isMoving) return;

            PieceKeySelection();
            PieceMouseSelection();
            PieceMovement();
        }
    }

    private void PieceKeySelection(){
        //Piece Selection
        if(Input.GetKeyDown(KeyCode.A)){
            boardManager.SelectNextPiece((int)PieceTypes.A);
        }
        if(Input.GetKeyDown(KeyCode.B)){
            boardManager.SelectNextPiece((int)PieceTypes.B);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            boardManager.SelectNextPiece((int)PieceTypes.C);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            boardManager.SelectNextPiece((int)PieceTypes.D);
        }
    }

    private void PieceMouseSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Raycast
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if(hit)
            {
                if (hitInfo.transform.gameObject.layer == (int)Layers.Piece)
                {
                    boardManager.SelectNextPiece(hitInfo.transform.GetComponent<PieceController>());
                }
            }
        }
    }

    private void PieceMovement(){
        //Piece Movement
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            boardManager.MoveCurrentPiece(currentPiece, 1, PrepareChangePlayer());
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            boardManager.MoveCurrentPiece(currentPiece,2, PrepareChangePlayer());
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            boardManager.MoveCurrentPiece(currentPiece,3, PrepareChangePlayer());
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            boardManager.ChangeBoards(currentPiece,1, PrepareChangePlayer());
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            boardManager.ChangeBoards(currentPiece,-1, PrepareChangePlayer());
        }
    }

    IEnumerator LateChangePlayer()
    {
        yield return new WaitForSeconds(lateStart);
        ChangePlayer();
    }

    IEnumerator PrepareChangePlayer(){
        yield return new WaitWhile(currentPiece.GetIsMoving);
        ChangePlayer();
    }



    private void ChangePlayer(){
        if(currentPlayer == 0 || currentPlayer == 2){
            foreach (PieceController item in player2Pieces)
            {
                item.outline.enabled = false;
                item.SetIsActive(false);
            }
            currentPlayer = 1;
            boardManager.CheckBoardStatus();
        }
        else{
            foreach (PieceController item in player1Pieces)
            {
                item.outline.enabled = false;
                item.SetIsActive(false);
            }
            currentPlayer = 2;
            boardManager.CheckBoardStatus();
        }
    }

    public void SetCurrentPiece(PieceController newPiece)
    {
        currentPiece = newPiece;
        currentPiece.outline.enabled = true;
        currentPiece.SetIsActive(true);
    }

    public PieceController GetCurrentPiece()
    {
        return currentPiece;
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

}
