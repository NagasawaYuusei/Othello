using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    public enum COLOR
    {
        NONE,
        BLACK,
        WHITE
    }
    public const int WIDTH = 8;
    public const int HEIGHT = 8;

    [SerializeField] GameObject m_blackPiece;
    [SerializeField] GameObject m_whitePiece;
    [SerializeField] GameObject m_nonePiece;

    [SerializeField] GameObject m_board;

    COLOR[,] m_boardNumber = new COLOR[WIDTH, HEIGHT];

    COLOR m_player = COLOR.BLACK;

    static int m_black;
    static int m_white;

    int m_remainingBlack = 30;
    int m_remainingWhite = 30;

    [SerializeField] UIManeger m_uiManeger;

    [SerializeField] SceneManeger m_sceneManeger;
    [SerializeField] string m_scene;

    [SerializeField] bool m_result;
    public static int BlackCount
    {
        get
        {
            return m_black;
        }
        set
        {
            m_black = value;
        }
    }

    public static int WhiteCount
    {
        get
        {
            return m_white;
        }
        set
        {
            m_white = value;
        }
    }

    public int BlackRemaining
    {
        get
        {
            return m_remainingBlack;
        }
    }

    public int WhiteRemaining
    {
        get
        {
            return m_remainingWhite;
        }
    }

    public COLOR Player
    {
        get
        {
            return m_player;
        }
    }

    public COLOR[,] Board
    {
        get
        {
            return m_boardNumber;
        }
        set
        {
            m_boardNumber = value;
        }
    }

    void Start()
    {
        if(!m_result)
        {
            Initialize();
        }
    }

    /// <summary>盤面の初期値を設定</summary>
    public void Initialize()
    {
        m_boardNumber = new COLOR[WIDTH, HEIGHT];
        m_remainingBlack = 30;
        m_remainingWhite = 30;
        m_boardNumber[3, 3] = COLOR.WHITE;
        m_boardNumber[3, 4] = COLOR.BLACK;
        m_boardNumber[4, 3] = COLOR.BLACK;
        m_boardNumber[4, 4] = COLOR.WHITE;
        m_player = COLOR.BLACK;
        ShowBoard();
    }

    /// <summary>盤面を表示する</summary>
    void ShowBoard()
    {
        foreach (Transform child in m_board.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                GameObject piece = GetPrefab(m_boardNumber[j, i]);
                int x = j;
                int y = i;
                piece.GetComponent<Button>().onClick.AddListener(() => { PutStone(x, y); });
                piece.transform.SetParent(m_board.transform, false);
            }
        }
        Get();
    }

    /// <summary>色によって適切なprefabを取得して返す</summary>
    public GameObject GetPrefab(COLOR color)
    {
        GameObject prefab;
        switch (color)
        {
            case COLOR.NONE:
                prefab = Instantiate(m_nonePiece);
                break;
            case COLOR.BLACK:
                prefab = Instantiate(m_blackPiece);
                break;
            case COLOR.WHITE:
                prefab = Instantiate(m_whitePiece);
                break;
            default:
                prefab = null;
                break;
        }
        return prefab;
    }
    /// <summary>駒が置かれた際の処理</summary>
    public void PutStone(int h, int v)
    {
        if(!m_result)
        {
            if (m_boardNumber[h, v] == COLOR.NONE)
            {
                ReverseAll(h, v);
                ShowBoard();
                if (m_boardNumber[h, v] == m_player)
                {
                    if (m_player == COLOR.BLACK)
                    {
                        m_remainingBlack--;
                        if(m_remainingBlack == 0 && m_remainingWhite > 1)
                        {
                            m_remainingBlack++;
                            m_remainingWhite--;
                        }
                        if(m_remainingWhite == 0 && m_remainingBlack > 1)
                        {
                            m_remainingBlack--;
                            m_remainingWhite++;
                        }
                    }
                    else if (m_player == COLOR.WHITE)
                    {
                        m_remainingWhite--;
                    }

                    m_player = m_player == COLOR.BLACK ? COLOR.WHITE : COLOR.BLACK;
                    if (CheckPass())
                    {
                        m_player = m_player == COLOR.BLACK ? COLOR.WHITE : COLOR.BLACK;
                        m_uiManeger.Path = true;

                        if (CheckPass())
                        {
                            StartCoroutine(CheckGame());
                        }
                    }
                }
            }
        }
    }

    void Reverse(int x1, int y1, int directionX, int directionY)
    {
        int x = x1 + directionX, y = y1 + directionY;
        while (x < WIDTH && x >= 0 && y < HEIGHT && y >= 0)
        {
            if (m_boardNumber[x, y] == m_player)
            {
                if (m_boardNumber[x, y] == m_player)
                {
                    int x2 = x1 + directionX, y2 = y1 + directionY;
                    int count = 0;
                    while (!(x2 == x && y2 == y))
                    {
                        m_boardNumber[x2, y2] = m_player;
                        x2 += directionX;
                        y2 += directionY;
                        count++;
                    }
                    if (count > 0)
                    {
                        m_boardNumber[x1, y1] = m_player;
                    }
                    break;
                }
            }
            else if (m_boardNumber[x, y] == COLOR.NONE)
            {
                break;
            }

            x += directionX;
            y += directionY;
        }
    }

    void ReverseAll(int x, int y)
    {
        Reverse(x, y, 1, 0);
        Reverse(x, y, -1, 0);
        Reverse(x, y, 0, -1);
        Reverse(x, y, 0, 1);
        Reverse(x, y, 1, -1);
        Reverse(x, y, -1, -1);
        Reverse(x, y, 1, 1);
        Reverse(x, y, -1, 1);
    }

    bool CheckPass()
    {
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                if (m_boardNumber[j, i] == COLOR.NONE)
                {
                    COLOR[,] boardTemp = new COLOR[WIDTH, HEIGHT];
                    Array.Copy(m_boardNumber, boardTemp, m_boardNumber.Length);
                    ReverseAll(j, i);

                    if (m_boardNumber[j, i] == m_player)
                    {
                        m_boardNumber = boardTemp;
                        return false;
                    }
                }
            }
        }
        return true;
    }

    void Get()
    {
        m_black = 0;
        m_white = 0;
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                switch (m_boardNumber[j, i])
                {
                    case COLOR.BLACK:
                        m_black++;
                        break;
                    case COLOR.WHITE:
                        m_white++;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    IEnumerator CheckGame()
    {
        BlackCount = m_black;
        WhiteCount = m_white;
        yield return new WaitForSeconds(2);
        m_sceneManeger.Scene(m_scene);
    }
}
