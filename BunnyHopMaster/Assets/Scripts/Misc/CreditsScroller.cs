using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    public GameObject textContainer;
    public Button exitButton;
    public float endingY = 0;
    public float scrollSpeed = 0.75f;

    void Start()
    {
        exitButton.onClick.AddListener(GoToMainMenu);
        exitButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if(textContainer.transform.localPosition.y < endingY)
        {
            textContainer.transform.position += new Vector3(0, scrollSpeed, 0);
        }
        else
        {
            exitButton.gameObject.SetActive(true);
        }
    }

    public void GoToMainMenu()
    {
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
