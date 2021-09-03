using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    // config parameters

    // cached reference
    GameSystem _gameSystem;     // parent game object
    GameObject _blocks;         // child gameobject for rotation
    Block[] _allBlocks;         // child Block object

    // state variables
    // How many new block make when this block's collsion entered?
    const float s_feildWidth = 8;
    [SerializeField] Block _topBlock;
    [SerializeField] Block _bottomBlock;
    Block _leftBlock;
    Block _rightBlock;

    int _newBlock;
    float _sec;
    float _secZ;
    float _angle;       // the angle in degrees
    private bool _isConflicted;

    public GameSystem GameSystem { get => _gameSystem; set => _gameSystem = value; }
    public int NewBlock { get => _newBlock; set => _newBlock = value; }
    public Block TopBlock { get => _topBlock; set => _topBlock = value; }
    public Block BottomBlock { get => _bottomBlock; set => _bottomBlock = value; }
    public Block LeftBlock { get => _leftBlock; set => _leftBlock = value; }
    public Block RightBlock { get => _rightBlock; set => _rightBlock = value; }
    public bool IsConflicted { get => _isConflicted; set => _isConflicted = value; }


    // Start is called before the first frame update
    void Start()
    {
        GetReference();
        SetState();
        _isConflicted = false;
        _newBlock = 1;
        _blocks = transform.Find("Center Rotation").gameObject;
    }

    public void SetState()
    {
        _allBlocks = GetComponentsInChildren<Block>();
        if (_allBlocks.Length == 0)
        {
            DestroyGameObject(gameObject);
            return;
        }
        
        foreach (Block block in _allBlocks)
        {
            // if last block tag == Destroyed than _bottomBlock will be null
            if (block.gameObject.CompareTag("Destroyed"))
            {
                continue;
            }
            if (_topBlock == null || _bottomBlock == null || _leftBlock == null || _rightBlock == null)
            {
                _topBlock = block;
                _bottomBlock = block;
                _leftBlock = block;
                _rightBlock = block;
            }
            if (_bottomBlock.gameObject.CompareTag("Destroyed"))
            {
                _bottomBlock = block;
            }
            if (_topBlock.transform.position.y < block.transform.position.y)
            {
                _topBlock = block;
            }
            if (_bottomBlock.transform.position.y > block.transform.position.y)
            {
                _bottomBlock = block;
            }
            if (_leftBlock.transform.position.x > block.transform.position.x)
            {
                _leftBlock = block;
            }
            if (_rightBlock.transform.position.x < block.transform.position.x)
            {
                _rightBlock = block;
            }

        }

        if (_topBlock == null || _bottomBlock == null || _leftBlock == null || _rightBlock == null)
        {
            return;
        }
    }

    private void GetReference()
    {
        _gameSystem = transform.parent.gameObject.GetComponent<GameSystem>();
    }


    // Update is called once per frame
    void Update()
    {
#pragma warning disable IDE0039
#pragma warning disable UNT0025
        System.Action rotateBlock = () =>
        {
            _angle += 90f;
            _blocks.transform.localRotation = Quaternion.Euler(0f, 0f, _angle);
            foreach (Block block in _allBlocks)
            {
                block.RotationCollision();
                block.Diff = transform.position.y - block.transform.position.y;
            }
            SetState();
        };

        if (Input.GetKeyDown("up"))
        {
            rotateBlock();
        }

        _sec += Time.deltaTime;
        if (_sec > 1f)
        {
            transform.Translate(0f, -1f, 0f);
            _sec = 0;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            _secZ += Time.deltaTime;
            if (_secZ > 0.05f)
            {
                transform.Translate(0f, -0.5f, 0f);
                _secZ = 0;
            }
        }

        if (Input.GetKeyDown("right") && _rightBlock.transform.position.x < s_feildWidth / 2 - 0.5f)
        {
            transform.Translate(1f, 0f, 0f);
        }
        if (Input.GetKeyDown("left") && _leftBlock.transform.position.x > -s_feildWidth / 2 + 0.5f)
        {
            transform.Translate(-1f, 0f, 0f);
        }
#pragma warning restore UNT0025
#pragma warning restore IDE0039
    }


    // Only active in object that has a tag calls "Block"
    public void Conflict(Collision2D collision, float diff)
    {
        _isConflicted = true;

        if (!collision.gameObject.CompareTag("Bottom"))
        {
            transform.position = new Vector2(transform.position.x, collision.transform.position.y + 1f + diff);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, collision.transform.position.y + 1f + diff);
        }
        _gameSystem.ReturnAllBlocksPosY();
        gameObject.tag = "OldBlock";
        TurnOffUpdate();

        if (_newBlock > 0)
        {
            _newBlock--;
            _gameSystem.NextBlock();
        }
    }

    public void TurnOffUpdate()
    {
        this.enabled = false;
    }

    public void DestroyBlocks(SortedSet<int> destroySet)
    {
        Block[] blocks = GetComponentsInChildren<Block>();
        int bottomPosY = (int) _bottomBlock.transform.position.y;
        int downSteps = 0;
        foreach (int temp in destroySet)
        {
            if (bottomPosY >= temp)
            {
                downSteps++;
            }
            for (int i = 0; i < blocks.Length; i++)
            {
                if ((int)blocks[i].transform.position.y == temp)
                {
                    DestroyGameObject(blocks[i].gameObject);
                }
            }
            SetState();
        }

        if (downSteps > 0)
        {
            transform.Translate(0, -downSteps, 0);
        }
    }

    private void DestroyGameObject(GameObject obj)
    {
        obj.tag = "Destroyed";
        obj.SetActive(false);
        Destroy(obj);
    }
}
