using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemandPos : MonoBehaviour {

    SpriteRenderer sr;
    public Animator animator;
    public GameObject demandBG;
    public enum Type {Man,Woman,Child }
    public Type type;
    public enum Status { Open, Filled, Failed }
    public Status status;
    public Sprite[] sprMan;
    public Sprite[] sprWoman;
    public Sprite[] sprChild;
    public Sprite[] sprBg;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void ChangeDemandType(Type newType)
    {
        // Reset status
        status = Status.Open;
        ChangeBG(sprBg[0]);
        if (newType == Type.Man)
            ChangeSprite(sprMan[0]);    // Gray sprite
        if (newType == Type.Woman)
            ChangeSprite(sprWoman[0]); // Gray sprite
        if (newType == Type.Child)
            ChangeSprite(sprChild[0]); // Gray sprite
        type = newType;
    }
    public void DemandFailed()
    {
        GameManager.instance.SpawnCurse();
        status = Status.Failed;
        animator.SetBool("Idle", true);
        ChangeBG(sprBg[2]);
    }
    public void DemandMet()
    {
        AudioManager.instance.PlaySoul();
        status = Status.Filled;
        animator.SetBool("Idle", true);
        ChangeBG(sprBg[1]);
        if (type == Type.Man)
            ChangeSprite(sprMan[1]);
        if (type == Type.Woman)
            ChangeSprite(sprWoman[1]);
        if (type == Type.Child)
            ChangeSprite(sprChild[1]);
    }
    public int CalculatePower()
    {
        if (type == Type.Man)
        {
            return 1;
        }
        if (type == Type.Woman)
        {
            return 2;
        }
        if (type == Type.Child)
        {
            return 4;
        }
        return 0;
    }
    public void ChangeBG(Sprite sprite)
    {
        demandBG.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public void ChangeSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }
}
