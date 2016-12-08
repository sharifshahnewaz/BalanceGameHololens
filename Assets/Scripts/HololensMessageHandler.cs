using UnityEngine;

using HoloToolkit.Sharing;


public class HololensMessageHandler : MonoBehaviour {

   
	void Start () {
		
		CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.NetworkMessage] = this.handleNetworkMessage;
       
        SharingSessionTracker.Instance.SessionJoined += Instance_SessionJoined;
    }


	void handleStringMessage (NetworkInMessage msg){
		Debug.Log (msg.ReadString());
	}

    //handles command that is coming from the server 
    void handleNetworkMessage(NetworkInMessage msg)
    {
        Debug.Log("handle command");
        msg.ReadInt64();// important! the id of the message.
        string command = msg.ReadString(); //the messages from the server;
       
        Debug.Log(command);

	}

	private void Instance_SessionJoined(object sender, SharingSessionTracker.SessionJoinedEventArgs e)
	{
		/*if (GotTransform)
		{
			CustomMessages.Instance.SendStageTransform(transform.localPosition, transform.localRotation);
		}*/
		Debug.Log ("instance_sessionjoined called");
	}



}
