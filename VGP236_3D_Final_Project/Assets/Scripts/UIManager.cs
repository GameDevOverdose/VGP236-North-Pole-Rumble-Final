using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DefaultExecutionOrder(-20)]
public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField] private Transform _menuParent = null;
    [SerializeField] private Transform _playersHealthUIParent = null;
    [SerializeField] private Transform _gameWinUIParent = null;
    [SerializeField] private Transform _gamePauseUIParent = null;

    public List<GameObject> _playersHealthUI = new List<GameObject>();

    private Dictionary<GameObject, Vector3> _originalScales = new Dictionary<GameObject, Vector3>();

    [SerializeField] private float _uIScaleDuration = 0.5f;

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

        RecordOriginalScale(_menuParent.gameObject);
        RecordOriginalScale(_playersHealthUIParent.gameObject);
        RecordOriginalScale(_gameWinUIParent.gameObject);
        RecordOriginalScale(_gamePauseUIParent.gameObject);

        foreach (Transform healthUI in _playersHealthUIParent)
        {
            _playersHealthUI.Add(healthUI.gameObject);

            RecordOriginalScale(healthUI.gameObject);
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

    public void EnableUIElement(GameObject uiElement)
    {
        StartCoroutine(EnableUI(uiElement));
    }

    public IEnumerator DisableUI(GameObject uiElement)
    {
        yield return null;

        yield return StartCoroutine(DoScaleUI(uiElement, false));
    }

    public IEnumerator EnableUI(GameObject uiElement)
    {
        yield return null;

        uiElement.SetActive(true);
        yield return StartCoroutine(DoScaleUI(uiElement, true));
    }

    IEnumerator DoScaleUI(GameObject uiElement, bool scaleUp)
    {
        if (uiElement == null)
        {
            yield break;
        }

        Vector3 targetScale = Vector3.one;

        if (_originalScales.TryGetValue(uiElement, out var savedScale))
        {
            targetScale = savedScale;
        }

        if (scaleUp)
        {
            uiElement.transform.localScale = Vector3.zero;
            Tween scale = uiElement.transform.DOScale(targetScale, _uIScaleDuration).SetEase(Ease.InOutBack);

            yield return scale.WaitForCompletion();
        }
        else
        {
            //uiElement.SetActive(true);

            Tween scale = uiElement.transform.DOScale(Vector3.zero, _uIScaleDuration).SetEase(Ease.InOutSine);
            yield return scale.WaitForCompletion();

            uiElement.SetActive(false);
            uiElement.transform.localScale = targetScale;
        }
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

    public IEnumerator EnablePauseUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        //EnableUIElement (_gamePauseUIParent.gameObject);
        _gamePauseUIParent.gameObject.SetActive(true);
    }

    public IEnumerator DisablePauseUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        DisableUIElement (_gamePauseUIParent.gameObject);
        _gamePauseUIParent.gameObject.SetActive(false);
    }

    /////////////////////////////////////////////////////////////////////
    
    private void RecordOriginalScale(GameObject uiElement)
    {
        if(uiElement != null && !_originalScales.ContainsKey(uiElement))
        {
            _originalScales[uiElement] = uiElement.transform.localScale;
        }
    }
}
