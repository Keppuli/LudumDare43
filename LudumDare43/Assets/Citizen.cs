using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour {

    public SpriteRenderer sr;

    public AudioClip dieSound;
    public float moveSpeed = 1f;
    public enum Side {left,right};
    public Side side;
    public enum Type {Man, Woman, Child };
    public Type type;
    [SerializeField]
    GameObject myPosObj;
    public int myPosIndex;
    public int myCitizenIndex;
    public bool atTheFinalPosition;
    public int power;

    void Awake () {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        // Flip sprite when spawning to the right side
        if (side == Side.right)
            FlipSprite();
    }
    void Update () {
        var d = Vector3.Distance(transform.position, myPosObj.transform.position);
        if ((d < .1f) && myPosIndex == 4)
            atTheFinalPosition = true;
        else if (d < .1f)
        {
            if (myPosObj.name == "PosSpawnLeft" || myPosObj.name == "PosSpawnRight")
                Destroy(gameObject);

            else if (myPosObj.name == "PositionRelease")
            {
                MoveOffScreenLeft();
                if (side == Side.left)
                    FlipSprite();
            }
            else if (myPosIndex != 4 && !CheckNextPositionAvailability()) // Check if false
            {
                ReserveNextPosition();
                ReleaseCurrentPosition();
                MoveToNextPosition();
            }
        }

    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, myPosObj.transform.position, moveSpeed * Time.deltaTime);
    }

    public void AssignPosition(int i)
    {
        myPosIndex = i; // Store index locally
        if (side == Side.left)
            myPosObj = GameManager.instance.positionsLeft[i];
        else if (side == Side.right)
            myPosObj = GameManager.instance.positionsRight[i];
        // Reserve assigned pos
        myPosObj.GetComponent<Position>().TakePosition();
    }

    bool CheckNextPositionAvailability()
    {
        if (side == Side.left)
        {
                return GameManager.instance.positionsLeft[myPosIndex + 1].GetComponent<Position>().CheckStatus();
        }
        else if (side == Side.right)
        {
                return GameManager.instance.positionsRight[myPosIndex + 1].GetComponent<Position>().CheckStatus();
        }
        return false;
    }

    void ReleaseCurrentPosition()
    {
        // Only if myPos exists
        if (myPosObj != null)
        {
            myPosObj.GetComponent<Position>().ReleasePosition();
        }
    }
    void ReserveNextPosition()
    {
        if (side == Side.left)
            GameManager.instance.positionsLeft[myPosIndex + 1].GetComponent<Position>().TakePosition();
        else if (side == Side.right)
            GameManager.instance.positionsRight[myPosIndex + 1].GetComponent<Position>().TakePosition();
    }
    public void MoveToNextPosition()
    {
        if (side == Side.left)
            myPosObj = GameManager.instance.positionsLeft[myPosIndex+1];
        else if (side == Side.right)
            myPosObj = GameManager.instance.positionsRight[myPosIndex + 1];
        myPosIndex += 1; // Update stored index number
    }
    public void MoveToAltar()
    {
        ReleaseCurrentPosition();
        myPosObj = GameManager.instance.altar;
        GameManager.instance.sacrificeCitizen = gameObject;
    }
    public void MoveToReleasePosition()
    {
        myPosObj = GameManager.instance.releasePosition;
    }
    public void MoveOffScreenLeft()
    {
        myPosObj = GameManager.instance.positionsLeft[0];
    }
    public void MoveOffScreenRight()
    {
        myPosObj = GameManager.instance.positionsRight[0];
    }
    private void OnDestroy() // Before destroying lets do some things
    {
        // Clean list
        GameManager.instance.CheckCitizenList();
        // Also spawn shit
    }

    void FlipSprite() 
    {
        sr.flipX = !sr.flipX;
    }
    void SortSpriteBack()
    {
        sr.sortingOrder = -1;
    }
}
