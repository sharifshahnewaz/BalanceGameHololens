using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

public class GameController : MonoBehaviour
{
	public GameObject tennisball;
	
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	private GameObject head;
	
	
	//public GUIText scoreText;
	//public GUIText restartText;
	//public GUIText gameOverText;

	
	private int hit;
	private int miss;
	private bool play;
	

	private double elapsedTime = 0.0f;
	public int totalBall = 120;

	private string displayMessage = null;
	
	
	public int sampleRate = 10;	
	public String studyCondition;
	
	public GameObject hitText;
	public GameObject missText;
	public GameObject messageText;
	
	
	void Start ()
	{
		play = true;
		hit = 0;
		miss = 0;
				
		//Time.timeScale = 0;	

		head = GameObject.FindWithTag ("MainCamera");

		if (head == null) {
			Debug.Log ("Cannot find 'Head' of the avatar");
		}
		
		displayMessage = "'P' to \nplay";
		StartCoroutine (SpawnWaves ());
	}
	
	void Update ()
	{
		
		if (Input.GetKeyDown (KeyCode.P)) {
			if (play) {
				play = false;				
				displayMessage = "'P' to \nplay";
			} else {
				play = true;
				displayMessage = "'P' to \nstop";
			}
			
		} 
		
		if (hit + miss >= totalBall) {
			play = false;
			displayMessage = "Game\nOver";
		}
		hitText.GetComponent <TextMesh> ().text = "Hit: " + hit;
		missText.GetComponent <TextMesh> ().text = "Miss: " + miss;
		messageText.GetComponent <TextMesh> ().text = displayMessage;
		
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
	
		while (true) {
			
						
			for (int i = 0; i < hazardCount; i++) {
				GameObject bowlingMachineHead = GameObject.FindGameObjectsWithTag ("BowlingMachineHead") [0];
				Vector3 spawnPosition = new Vector3 (bowlingMachineHead.transform.position.x, 
								bowlingMachineHead.transform.position.y, bowlingMachineHead.transform.position.z);
				Quaternion spawnRotation = Quaternion.identity;
				if (head != null) {
					spawnRotation = Quaternion.LookRotation (spawnPosition - head.transform.position - new Vector3 (0, 0.099f, 0.134f));
				}	
				if (play) {							
					Instantiate (tennisball, spawnPosition, spawnRotation);
					
				}		
				yield return new WaitForSeconds (spawnWait);
			}
			
			yield return new WaitForSeconds (waveWait);
			
							
						
		}
	}
	
	
	
	public void AddHit ()
	{
		hit += 1;
		//UpdateScore ();
	}
	public void AddMiss ()
	{
		miss += 1;
		//UpdateScore ();
	}
		
}