using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField] GameObject[] _blocks;
    [SerializeField] CheckLines _checkLines;
    [SerializeField] GameOver _gameOverTrigger;
    [Range(0.3f, 10f)] [SerializeField] float _gameSpeed;

    private bool _hasStarted;

    public bool HasStarted { get => _hasStarted; }



    // Start is called before the first frame update
    void Start()
    {
        _hasStarted = false;
        _gameSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = _gameSpeed;
        if (!_hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _hasStarted = true;
                NextBlock();
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            _gameSpeed = 10f;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            _gameSpeed = 1f;
        }

    }

    public void NextBlock()
    {
        int index = Random.Range(0, _blocks.Length);
        Instantiate(
            _blocks[index],
            new Vector2(0f, 5f),
            Quaternion.identity,
            transform
            );
    }
    
    public void ReturnAllBlocksPosY() // TODO set method name
    {
        // reset checklines
        _checkLines.ResetCheckLines();
        Block[] blocks = GetComponentsInChildren<Block>();
        var destroySet = new SortedSet<int>();
        for (int i = 0; i < blocks.Length; i++)
        {
            float posY = blocks[i].ReturnPosY();
            if (posY > _checkLines.Lines / 2)
            {
                _gameOverTrigger.LoadGameOver();
            }
            bool destroyable = _checkLines.BreakCheck(posY);
            if (destroyable)
            {
                destroySet.Add((int)blocks[i].transform.position.y);
            }
        }

        // if has destroyable block than break it
        if (destroySet.Count != 0)
        {
            DestroyAllBlocks(destroySet);
        }

    }

    private void DestroyAllBlocks(SortedSet<int> destroySet)
    {
        Blocks[] allBlocks = GetComponentsInChildren<Blocks>();
        foreach (Blocks blocks in allBlocks)
        {
            blocks.DestroyBlocks(destroySet);
        }
    }

    
}
