using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{

    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(PlayGame);
    }

    public void PlayGame() {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
