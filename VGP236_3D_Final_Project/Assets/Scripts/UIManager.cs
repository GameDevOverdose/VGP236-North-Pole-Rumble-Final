using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField] private Transform _menuParent = null;
    [SerializeField] private Transform _playersHealthUIParent = null;
    [SerializeField] private Transform _gameWinUIParent = null;
    public List<GameObject> _playersHealthUI = new List<GameObject>();

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        foreach (Transform child in _playersHealthUIParent)
        {
            _playersHealthUI.Add(child.gameObject);
        }

        for (int i = 0; i < _playersHealthUI.Count; i++)
        {
            if (i < PlayerManager.Instance.PlayersSupported)
            {
                _playersHealthUI[i].SetActive(true);
            }
            else
            {
                _playersHealthUI[i].SetActive(false);
            }
        }
    }

    public int GetPlayerHealthUIIndex(GameObject healthUIObject)
    {
        for (int i = 0; i < _playersHealthUI.Count; i++)
        {
            if (_playersHealthUI[i] == healthUIObject)
            {
                return i;
            }
        }

        Debug.LogError($"Health UI {healthUIObject} not found in assigned health UI elements.");
        return -1;
    }

    public void UpdateUI()
    {
        UpdateHealthUIElements();
    }

    public void UpdateHealthUIElements()
    {
        for (int i = 0; i < _playersHealthUI.Count; i++)
        {
            if (_playersHealthUI[i].activeSelf)
            {
                PlayerHealthUI healthUIComponent = _playersHealthUI[i].GetComponent<PlayerHealthUI>();
                healthUIComponent.UpdateHealthUI();
            }
        }
    }

    /////////////////////////////////////////////////////////////////////

    public void DisableUIElement(GameObject uiElement)
    {
        StartCoroutine(DisableUI(uiElement));
    }

    public IEnumerator DisableUI(GameObject uiElement)
    {
        yield return null;
        uiElement.SetActive(false);
    }

    public void EnableUIElement(GameObject uiElement)
    {
        StartCoroutine(EnableUI(uiElement));
    }

    public IEnumerator EnableUI(GameObject uiElement)
    {
        yield return null;
        uiElement.SetActive(true);
    }

    /////////////////////////////////////////////////////////////////////

    public IEnumerator EnableMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);

        EnableUIElement(_menuParent.gameObject);
    }

    public IEnumerator DisableMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);

        DisableUIElement(_menuParent.gameObject);
    }

    public IEnumerator EnableGameUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        EnableUIElement(_playersHealthUIParent.gameObject);

        foreach(GameObject healthUI in _playersHealthUI)
        {
            EnableUIElement(healthUI);
        }
    }

    public IEnumerator DisableGameUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        DisableUIElement(_playersHealthUIParent.gameObject);
    }

    public IEnumerator EnableWinUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        _gameWinUIParent.GetComponentInChildren<PlayerWinUI>().UpdateWinUI();
        EnableUIElement(_gameWinUIParent.gameObject);
    }

    public IEnumerator DisableWinUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        DisableUIElement(_gameWinUIParent.gameObject);
    }
}
