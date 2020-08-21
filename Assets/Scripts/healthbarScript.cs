using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthbarScript : MonoBehaviour
{
    private Transform bar;
    private SpriteRenderer rend;
    bool flip;
    bool critical;
    int timer;

    void Start()
    {
        bar = transform.Find("bar");
        rend = GetComponent<SpriteRenderer>();
        flip = true;
        critical = false;
        timer = 0;
    }
    public void SetValue(float size)
    {
        bar.localScale = new Vector3(size, 1f);
        
    }
    public void Update()
    {
        //Flash every few frames
        if (critical && timer>5)
        {
            flash();
            timer = 0;
        }
        timer++;
    }
    public void goCritical()
    {
        critical = true;
    }
    private void flash()
    {
        if (flip)
        {
            rend.color = Color.white;
            flip = !flip;
        }
        else
        {
            rend.color = Color.red;
            flip = !flip;
        }
    }
    public void die()
    {
        Destroy(gameObject);
    }
}
