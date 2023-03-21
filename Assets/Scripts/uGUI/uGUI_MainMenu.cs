using UnityEngine;
using UnityEngine.SceneManagement;

public class uGUI_MainMenu : MonoBehaviour
{
    public void BeginGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
