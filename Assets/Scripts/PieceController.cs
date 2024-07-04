using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum PieceTypes{
    A,
    B,
    C,
    D,
    Count
}

public class PieceController : MonoBehaviour
{
    public PieceTypes type;
    public float duration;
    public int currentBoard;
    public int currentTile;

    public bool isMoving = false;

    public Outline outline;
    public Material teamMaterial;

    public Action<PieceController> outOfGameEvent;

    public Animator animator;

    private Vector3 outofGamePosition;

    private void Awake()
    {
        if (outline == null)
            outline = GetComponent<Outline>();
    }

    void Start (){
        GetComponentInChildren<MeshRenderer>().material = teamMaterial;
        outOfGameEvent += (PieceController piece) => {print("Do action of: " + piece.name);};
    }

    public bool GetIsMoving(){
        return isMoving;
    }

    public void SetOutofGamePosition(Vector3 position){
        outofGamePosition = position;
    }

    public void SetIsActive(bool isActive){
        
        if (animator == null) return;
        animator.SetBool("IsActive", isActive);
    }
    public bool GetIsActive(){
        if (animator == null) return false;
        
        return animator.GetBool("IsActive");
    }

    public void MoveToTile(Vector3 newPosition, int newTile = -1){
        if(isMoving) return;

        isMoving = true;
        if(newTile == -1){
            transform.position = newPosition;
            isMoving = false;
            return;
        }      
        currentTile = newTile;
        animator.SetBool("IsActive", true);
        DoLerpTo(newPosition, duration);
    }

    public void MoveToNextBoard(Vector3 newPosition){
        if(isMoving) return;

        isMoving = true;
        DoLerpTo(newPosition, duration);
    }

    /// <summary>
    /// Moves the piece to the middle of the boards.
    /// </summary>
    public void MoveToOutOfGame(){
        if(isMoving) return;

        isMoving = true; 
        print("I should move out of game");
        DoLerpTo(outofGamePosition, duration);
        outOfGameEvent.Invoke(this);
    }

    public void DoLerpTo(Vector3 positionToMoveTo, float duration){
        StartCoroutine(LerpPosition(positionToMoveTo,duration));
    }

    /// <summary>
    /// Coroutine for lerping position.
    /// </summary>
    /// <param name="targetPosition">The position to move to.</param>
    /// <param name="duration">How long it takes</param>
    /// <returns></returns>
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        animator.SetTrigger("Move");

        while(time < duration){
            transform.position = Vector3.Lerp(startPosition, targetPosition, time/duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}




