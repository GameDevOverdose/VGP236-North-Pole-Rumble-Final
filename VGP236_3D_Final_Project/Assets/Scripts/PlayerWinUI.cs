using UnityEngine;

public class PlayerWinUI : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _winText;

    private void Start()
    {
        _winText = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        //UpdateWinUI();
    }

    public void UpdateWinUI()
    {
        if(_winText == null)
        {
            _winText = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        }

        int winningPlayerIndex = GameStateManager.Instance.ReturnWinningPlayerIndex();
        _winText.text = $"Player {winningPlayerIndex + 1} Wins";
    }
}
