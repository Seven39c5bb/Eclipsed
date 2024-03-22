using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class down : Card
{
    public Transform playerTransform;
    private new void Start()
    {
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private new void Update()
    {

    }
    public void MoveDown()
    {
        playerTransform.DOMove(new Vector3(0, playerTransform.position.y - 1, 0), 0.5f);
    }
}
