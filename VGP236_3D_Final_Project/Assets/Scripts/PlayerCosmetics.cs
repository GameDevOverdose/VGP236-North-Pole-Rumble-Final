using UnityEngine;

public class PlayerCosmetics : MonoBehaviour
{
    private int _playerNumber = 0;
    private PlayerCosmeticsManager.CosmeticChanges _currentPlayerCosmetics;

    [SerializeField] private Renderer _PenguinMeshRenderer;
    [SerializeField] private GameObject _hatAccessory;
    [SerializeField] private GameObject _bagAccessory;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _playerNumber = PlayerManager.Instance.GetPlayerNumberIndex(gameObject);
        _currentPlayerCosmetics = PlayerCosmeticsManager.Instance.GetPlayerCosmetic(_playerNumber);

        if (_currentPlayerCosmetics._penguinMaterial != null)
        {
            _PenguinMeshRenderer.material = _currentPlayerCosmetics._penguinMaterial;
        }

        switch (_currentPlayerCosmetics._accessory)
        {
            case PlayerCosmeticsManager.AccessoryType.HAT:
                _hatAccessory.SetActive(true);
                break;
            case PlayerCosmeticsManager.AccessoryType.BAG:
                _bagAccessory.SetActive(true);
                break;
            case PlayerCosmeticsManager.AccessoryType.NONE:
                // Do nothing
                break;
        }
    }
}
