using UnityEngine;
using UnityEngine.UI;

public class ResultManeger : MonoBehaviour
{
    [SerializeField] GameSystem m_gameSystem;
    [SerializeField] GameObject m_board;
    [SerializeField] Text m_getBlack;
    [SerializeField] Text m_getWhite;

    [SerializeField] GameObject[] m_result;
    void Start()
    {
        Initialize();
        Result();
    }

    public void Initialize()
    {
        m_gameSystem.Board = new GameSystem.COLOR[GameSystem.WIDTH, GameSystem.HEIGHT];
        int count = 0;
        for (int i = 0; i < GameSystem.HEIGHT; i++)
        {
            for (int j = 0; j < GameSystem.WIDTH; j++)
            {
                count++;
                if(count <= GameSystem.BlackCount)
                {
                    m_gameSystem.Board[j, i] = GameSystem.COLOR.BLACK;
                }
                else if(GameSystem.BlackCount < count && count <= GameSystem.BlackCount + GameSystem.WhiteCount)
                {
                    m_gameSystem.Board[j, i] = GameSystem.COLOR.WHITE;
                }
                else
                {
                    m_gameSystem.Board[j, i] = GameSystem.COLOR.NONE;
                }
            }
        }
        ShowBoard();
    }

    void ShowBoard()
    {
        for (int i = 0; i < GameSystem.HEIGHT; i++)
        {
            for (int j = 0; j < GameSystem.WIDTH; j++)
            {
                GameObject piece = m_gameSystem.GetPrefab(m_gameSystem.Board[j, i]);
                piece.transform.SetParent(m_board.transform, false);
            }
        }
    }

    void Result()
    {
        if(GameSystem.BlackCount > GameSystem.WhiteCount)
        {
            m_result[0].SetActive(true);
            m_result[4].SetActive(true);
        }
        else if(GameSystem.WhiteCount > GameSystem.BlackCount)
        {
            m_result[1].SetActive(true);
            m_result[3].SetActive(true);
        }
        else
        {
            m_result[2].SetActive(true);
            m_result[5].SetActive(true);
        }
        m_getBlack.text = "Get : " + GameSystem.BlackCount;
        m_getWhite.text = "Get : " + GameSystem.WhiteCount;
    }
}
