using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // cached reference
    BoxCollider2D _boxCollider2D;
    Blocks _blocks;
    float _diff;

    public float Diff { get => _diff; set => _diff = value; }

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _blocks = GetComponentInParent<Blocks>();
        _diff = _blocks.transform.position.y - transform.position.y;
    }


    public float ReturnPosY()
    {
        return transform.position.y;
    }

    public void RotationCollision()
    {
        _boxCollider2D.size = new Vector2(_boxCollider2D.size.y, _boxCollider2D.size.x);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_blocks.IsConflicted)
        {
            // call new block Conflict()
            _blocks.Conflict(collision, _diff);
        }
    }
}
