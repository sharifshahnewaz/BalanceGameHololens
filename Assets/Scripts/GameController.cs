﻿using UnityEngine;
using System.Collections;
using System;
using HoloToolkit.Sharing;

public class GameController : MonoBehaviour
{
    public GameObject tennisball;

    public int hazardCount = 10;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    private GameObject head;

    //public GUIText scoreText;
    //public GUIText restartText;
    //public GUIText gameOverText;


    private int hit;
    private int miss;
    public bool play = false;
    private bool gameover = false;


    private double elapsedTime = 0.0f;
    public int totalBall = 120;

    private string displayMessage = null;


    public int sampleRate = 10;
   // public String studyCondition;

    public GameObject hitText;
    public GameObject missText;
    public GameObject messageText;



    void Start()
    {

        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.NetworkMessage] = this.handleNetworkMessage;

        SharingSessionTracker.Instance.SessionJoined += Instance_SessionJoined;

        //play = false;
        hit = 0;
        miss = 0;

        //Time.timeScale = 0;	

        head = GameObject.FindWithTag("MainCamera");

        if (head == null)
        {
            Debug.Log("Cannot find 'Head' of the avatar");
        }
        Debug.Log("after head");

        displayMessage = "'P' to \nplay";
        StartCoroutine(SpawnWaves());
        
    }


    void Update()
    {
        if (!play)
        {
            displayMessage = "'P' to \nplay";
        }
        else
        {
            displayMessage = "'P' to \nstop";
        }


        if (hit + miss >= totalBall)
        {
            play = false;
            displayMessage = "Game\nOver";
            if (!gameover)
            {
                gameover = true;
                CustomMessages.Instance.SendNetworkMessage("gameover");
                CustomMessages.Instance.SendNetworkMessage("hit+"+hit+"+miss+"+miss);
            }

        }
        hitText.GetComponent<TextMesh>().text = "Hit: " + hit;
        missText.GetComponent<TextMesh>().text = "Miss: " + miss;
        messageText.GetComponent<TextMesh>().text = displayMessage;

    }

    IEnumerator SpawnWaves()
    {
        Debug.Log("starting spwanwaves");
        yield return new WaitForSeconds(startWait);

        while (true)
        {


            for (int i = 0; i < hazardCount; i++)
            {
                GameObject bowlingMachineHead = GameObject.FindGameObjectsWithTag("BowlingMachineHead")[0];
                Vector3 spawnPosition = new Vector3(bowlingMachineHead.transform.position.x,
                                bowlingMachineHead.transform.position.y + 0.1f, bowlingMachineHead.transform.position.z);
                Quaternion spawnRotation = Quaternion.identity;
                if (head != null)
                {
                    spawnRotation = Quaternion.LookRotation(spawnPosition - head.transform.position - new Vector3(0, 0.099f, 0.134f));
                }

                if (play)
                {
                    Instantiate(tennisball, spawnPosition, spawnRotation);

                }
                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);



        }
    }
    void handleStringMessage(NetworkInMessage msg)
    {
        Debug.Log(msg.ReadString());
    }

    //handles command that is coming from the server 
    void handleNetworkMessage(NetworkInMessage msg)
    {
        Debug.Log("handle command");
        msg.ReadInt64();// important! the id of the message.
        string message = msg.ReadString();
        string command = message.Split('+')[0]; //the messages from the server;
        Debug.Log("Command received - "+command);
        GameObject srf;
        switch (command)
        {
            case "play":
                play = true;
                CustomMessages.Instance.SendNetworkMessage("startedPlaying");
                break;
            case "pause":
                play = false;
                CustomMessages.Instance.SendNetworkMessage("stoppedPlaying");
                break;
            case "nosrfstatic":
            case "nosrfdynamic":
                 srf = GameObject.Find("Static base");
                if ( srf!= null)
                {
                    foreach (Renderer r in srf.GetComponentsInChildren<Renderer>()) {
                        r.enabled = false;
                    }
                    CustomMessages.Instance.SendNetworkMessage("srfDisabled");
                }
                break;
            case "srfstatic":
            case "srfdynamic":
                 srf = GameObject.Find("Static base");
                if (srf != null)
                {
                    foreach (Renderer r in srf.GetComponentsInChildren<Renderer>())
                    {
                        r.enabled = true;
                    }
                    CustomMessages.Instance.SendNetworkMessage("srfEnabled");
                }
                break;

            case "totalball":
                totalBall = Convert.ToInt32(message.Split('+')[1]);
                CustomMessages.Instance.SendNetworkMessage("totalBallUpdated");
                break;
            default:
                break;
        }
       

    }

    private void Instance_SessionJoined(object sender, SharingSessionTracker.SessionJoinedEventArgs e)
    {
        /*if (GotTransform)
		{
			CustomMessages.Instance.SendStageTransform(transform.localPosition, transform.localRotation);
		}*/
        Debug.Log("instance_sessionjoined called");
    }





    public void AddHit()
    {
        hit += 1;
        //UpdateScore ();
    }
    public void AddMiss()
    {
        miss += 1;
        //UpdateScore ();
    }

    private void OnApplicationQuit()
    {
        if (!gameover)
        {
            gameover = true;
            CustomMessages.Instance.SendNetworkMessage("gameover");
            CustomMessages.Instance.SendNetworkMessage("hit+" + hit + "+miss+" + miss);
        }
    }

}