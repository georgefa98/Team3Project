using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacktoMainButton : MonoBehaviour
{

    CanvasGroup controlsPanel;
    public CanvasGroup mainPanel;

    // Start is called before the first frame update
    void Start()
    {
        controlsPanel = transform.parent.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    public void ShowMainMenu()
    {
        controlsPanel.alpha = 0;
        controlsPanel.interactable = false;
        controlsPanel.blocksRaycasts = false;
        
        mainPanel.alpha = 1;
        mainPanel.interactable = true;
        mainPanel.blocksRaycasts = true;
    }
}
