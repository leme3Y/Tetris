using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLines : MonoBehaviour
{
    // config params
    [SerializeField] int _lines;
    [SerializeField] int _lineWidth;

    // state
    [SerializeField] int[] _blockInLines;

    public int Lines { get => _lines; set => _lines = value; }
    public int LineWidth { get => _lineWidth; set => _lineWidth = value; }

    // Start is called before the first frame update
    void Start()
    {
        _blockInLines = new int[_lines + 4];
    }

    // please minus the value of PosY 0.5;
    public bool BreakCheck(float posY)
    {
        posY += _lines / 2;
        _blockInLines[(int)(posY)]++;
        if (_blockInLines[(int)(posY)] >= _lineWidth)
        {
            return true;
        }
        return false;
    }

    public void ResetCheckLines()
    {
        _blockInLines = new int[_lines + 4];
    }
}
