using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryDoor : MonoBehaviour
{
    public string destinationSceneName = "Victory"; // Name of the destination scene.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D playerCollider = Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0);
            if (playerCollider != null && playerCollider.CompareTag("Code Cluck"))
            {
                LoadDestinationScene();
            }
        }
    }

    private void LoadDestinationScene()
    {
        SceneManager.LoadScene(destinationSceneName);
    }
}