using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManeger : MonoBehaviour
{
    [SerializeField] Text m_remainingBlack;
    [SerializeField] Text m_remainingWhite;
    [SerializeField] Text m_getBlack;
    [SerializeField] Text m_getWhite;
    [SerializeField] GameObject m_pathBlack;
    [SerializeField] GameObject m_pathWhite;
    [SerializeField] TextMeshProUGUI m_black;
    [SerializeField] TextMeshProUGUI m_white;

    [SerializeField] GameSystem m_gameSystem;
    bool m_isPath;

    [SerializeField] GameObject m_ui;

    public bool Path
    {
        get
        {
            return m_isPath;
        }
        set
        {
            m_isPath = value;
        }
    }

    void Update()
    {
        RemainingUI();
        GetUI();
        Player();
        IsPath();
        UI();
    }

    void RemainingUI()
    {
        m_remainingBlack.text = "Remaining : " + m_gameSystem.BlackRemaining;
        m_remainingWhite.text = "Remaining : " + m_gameSystem.WhiteRemaining;
    }

    void GetUI()
    {
        m_getBlack.text = "Get : " + GameSystem.BlackCount;
        m_getWhite.text = "Get : " + GameSystem.WhiteCount;
        if(m_gameSystem.WhiteRemaining < 6)
        {
            m_getBlack.text = "Get : ???";
            m_getWhite.text = "Get : ???";
        }
    }

    void Player()
    {
        if (m_gameSystem.Player == GameSystem.COLOR.BLACK)
        {
            m_black.fontStyle = FontStyles.Underline;
            m_white.fontStyle = FontStyles.Normal;
        }
        else if(m_gameSystem.Player == GameSystem.COLOR.WHITE)
        {
            m_white.fontStyle = FontStyles.Underline;
            m_black.fontStyle = FontStyles.Normal;
        }
    }

    void IsPath()
    {
        if(m_isPath)
        {
            StartCoroutine(PathProcess());
        }
    }

    IEnumerator PathProcess()
    {
        if(m_gameSystem.Player == GameSystem.COLOR.BLACK)
        {
            m_pathWhite.SetActive(true);
        }
        else if (m_gameSystem.Player == GameSystem.COLOR.WHITE)
        {
            m_pathBlack.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        m_pathBlack.SetActive(false);
        m_pathWhite.SetActive(false);
        m_isPath = false;
    }
    void UI()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            m_ui.SetActive(!m_ui.activeSelf);
        }
    }

    public void ButtonUI()
    {
        m_ui.SetActive(!m_ui.activeSelf);
    }
}
