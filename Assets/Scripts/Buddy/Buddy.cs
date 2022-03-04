using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buddy : MonoBehaviour
{
    private int atk = 21;
    private int def = 21;
    private int initiative = 21;
    [SerializeField]
    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField]
    private Sprite[] currentSkin;
    private bool sprite1 = false;
    private bool sprite2 = false;

    public int Atk { get => atk; set => atk = value; }
    public int Def { get => def; set => def = value; }
    public int Initiative { get => initiative; set => initiative = value; }

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (currentSkin[0] != null)
        {
            if (sprite1)
            {
                sr.sprite = currentSkin[0];
            }
            else
            {
                sr.sprite = currentSkin[1];
            }
        }
    }

    public void LeaveForQuest()
    {
        //set leaving anim here >>>>>

        sr.enabled = false;
    }

    public void ReturnFromQuest()
    {
        //set returning anim here >>>>>>
        sr.enabled = true;
    }

    public void UpdateCurrentSkin(Sprite sprite1, Sprite sprite2)
    {
        currentSkin[0] = sprite1;
        currentSkin[1] = sprite2;
    }

    public void ReplaceAnimSprite1()
    {
        sprite1 = true;
        sprite2 = false;
    }

    public void ReplaceAnimSprite2()
    {
        sprite2 = true;
        sprite1 = false;
    }
}
