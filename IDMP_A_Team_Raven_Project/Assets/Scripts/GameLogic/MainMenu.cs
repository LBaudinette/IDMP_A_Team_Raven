using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //public AudioSource mapMusic;
    //public MusicScript playMusic;
    public string gameSceneToLoad;

    private void Start()
    {
        //playMusic.MenuMusic();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(gameSceneToLoad);
    }

    public void QuitToDeskTop()
    {
        Application.Quit();
    }
}
