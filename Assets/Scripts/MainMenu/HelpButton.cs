using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{

    Button button;
    public CanvasGroup controlsPanel;
    CanvasGroup mainPanel;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        mainPanel = transform.parent.gameObject.GetComponent<CanvasGroup>();
    }

    public void ShowHelpMenu() {
        mainPanel.alpha = 0;
        mainPanel.interactable = false;
        mainPanel.blocksRaycasts = false;
        
        controlsPanel.alpha = 1;
        controlsPanel.interactable = true;
        controlsPanel.blocksRaycasts = true;
    }
}
