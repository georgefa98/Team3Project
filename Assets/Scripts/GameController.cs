using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private CharacterController playerController;
	[SerializeField] private FPSController fpsCon;
	//[SerializeField] private CharacterController player;
	[SerializeField] private Player player;
	
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private AudioSource music;
    public float timeInDay = 5f;

    public Light sun;
    public Light moon;
    public float timeOfDay;
	public bool isPaused = false;

    void Start()
    {
    }

    void Update()
    {
        timeOfDay += Time.deltaTime;
        if(timeOfDay >= timeInDay) {
            timeOfDay = 0f;
        }

        sun.transform.rotation = Quaternion.AngleAxis((1f - timeOfDay/timeInDay) * 360f, Vector3.forward) * Quaternion.FromToRotation(Vector3.forward, Vector3.right);
        moon.transform.rotation = Quaternion.AngleAxis((1f - timeOfDay/timeInDay) * 360f, Vector3.forward) * Quaternion.FromToRotation(Vector3.forward, Vector3.left);
		
		// Pause
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (isPaused == false)
				PauseGame();
			else
				UnpauseGame();
		}
        
    }
	
	public void PauseGame()
	{
		music.Pause();
		isPaused = true;
		//Cursor.visible = true;
		
		Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
		
		player.enabled = false;
		playerController.enabled = false;
		fpsCon.enabled = false;
		
		Time.timeScale = 0f;
		pausePanel.SetActive(true);
	}
	
	public void UnpauseGame()
	{
		music.UnPause();
		isPaused = false;
		
		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		
		player.enabled = true;
		playerController.enabled = true;
		fpsCon.enabled = true;
		
		Time.timeScale = 1f;
		pausePanel.SetActive(false);
		
	}
	
	public void LoadMainMenu()
	{
		Debug.Log("Load man menu.");
	}
}
