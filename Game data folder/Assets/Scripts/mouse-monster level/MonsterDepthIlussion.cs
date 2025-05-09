using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDepthIlussion : MonoBehaviour
{
    public SpriteRenderer monsterSprite;
    public SpriteRenderer playerSprite;
    public Collider2D monsterCollider; 
    public Transform hillTransform; 

    private bool isPlayerAboveHill = false;
    private bool isPlayerInsideMonster = false;

    private int monsterDefaultSortingOrder = 3;
    private int playerDefaultSortingOrder = 5;

    private float hillYThreshold;


    private void Start()
    {
        hillYThreshold = hillTransform.position.y + 2f;
        monsterSprite.sortingOrder = monsterDefaultSortingOrder;
        playerSprite.sortingOrder = playerDefaultSortingOrder;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInsideMonster = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInsideMonster = false;
        }
    }

    private void Update()
    {
        CheckPlayerPosition();

        if (isPlayerAboveHill)
        {
            if (!isPlayerInsideMonster)
            {
                monsterSprite.sortingOrder = playerDefaultSortingOrder + 1;
            }
        }
        else
        {
            if (!isPlayerInsideMonster)
            {
                monsterSprite.sortingOrder = monsterDefaultSortingOrder;
            }
        }
    }

    private void CheckPlayerPosition()
    {
        isPlayerAboveHill = playerSprite.transform.position.y > hillYThreshold;
    }
}
