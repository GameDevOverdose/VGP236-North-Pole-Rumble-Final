using UnityEngine;
using UnityEngine.UI;

public enum HeartStatus
{
    Empty = 0,
    Full = 1
}

public class HealthHeart : MonoBehaviour
{


    [SerializeField] private Sprite _fullHeart, _emptyHeart;
    private Image _heartImage;

    private void Awake()
    {
        _heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus heartStatus)
    {
        switch (heartStatus)
        {
            case HeartStatus.Full:
                {
                    _heartImage.sprite = _fullHeart;
                }
                break;
            case HeartStatus.Empty:
                {
                    _heartImage.sprite = _emptyHeart;
                }
                break;
        }
    }
}
