using UnityEngine;

[DefaultExecutionOrder(-20)]
public class PlayerCosmeticsManager : MonoBehaviour
{
    private static PlayerCosmeticsManager _instance = null;
    public static PlayerCosmeticsManager Instance { get { return _instance; } }

    public enum AccessoryType
    {
        NONE,
        HAT,
        BAG
    }

    [System.Serializable]
    public struct CosmeticChanges
    {
        public Material _penguinMaterial;
        public AccessoryType _accessory;
    }

    [SerializeField] private CosmeticChanges[] _cosmeticTypes = new CosmeticChanges[4];

    private void Awake()
    {
        if(Instance == null)
        {
            _instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public CosmeticChanges GetPlayerCosmetic(int index)
    {
        if (index < 0 || index >= _cosmeticTypes.Length)
        {
            Debug.LogError("Invalid cosmetic index");
            //return null;
        }

        return _cosmeticTypes[index];
    }
}
