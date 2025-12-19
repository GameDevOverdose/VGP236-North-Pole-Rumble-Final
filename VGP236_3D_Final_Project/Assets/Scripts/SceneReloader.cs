using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    private Vector3 cameraInitialPosition;
    private Quaternion cameraInitialRotation;

    [SerializeField] private GameObject uiToDisable;
    [SerializeField] private GameObject uiToEnable;

    private int reloadCount = 0;

    private void Update()
    {
        if(reloadCount >= 2)
        {
            uiToDisable.SetActive(false);
            uiToEnable.SetActive(true);

            if (Camera.main.transform.position == cameraInitialPosition && Camera.main.transform.rotation == cameraInitialRotation)
            {
                StartCoroutine(SceneDelay());
            }
        }
    }

    private void OnEnable()
    {
        if(reloadCount == 0)
        {
            cameraInitialPosition = Camera.main.transform.position;
            cameraInitialRotation = Camera.main.transform.rotation;
        }

        reloadCount++;
    }

    private IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
