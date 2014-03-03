using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProfileInView {
   
	public int Id { get; set;}
	public string PlayerName { get; set;}
	public string DOB { get; set;}
	public string City { get; set;}
	public string Country { get; set;}
	public BitArray ProfilePicture { get; set;}
	public List<string> ChatMessages = new List<string> ();
	public float X { 
		get{
			return ProfileWindowRect.x;
		}
		set{
			ProfileWindowRect.x = value;
		}
	}
	public float Y { 
		get{
			return ProfileWindowRect.y;
		}
		set{
			ProfileWindowRect.y = value;
		}
	}
	public float Width { 
		get{
			return ProfileWindowRect.width;
		}
		set{
			ProfileWindowRect.width = value;
		}
	}
	public float Height { 
		get{
			return ProfileWindowRect.height;
		}
		set {
			ProfileWindowRect.height = value;
		}
	}
	public Rect ProfileWindowRect;
	public List<string> PrivateChatText;
	public string PrivateTypedText { get; set;}

    private float previousY;
    private float DefaultDockY;

    private float DefaultWindowY;
    private float DefaultWindowX;


	public ProfileInView(){

		X = 0;
		Y = (float)Screen.height/11;
		Width = (float)(Screen.width/5);
		Height = (float)(Screen.height/1.4);
		ProfileWindowRect = new Rect(X,Y,Width,Height);
		PrivateChatText = new List<string>();
		PrivateTypedText = "";
	
	}

    public void MoveDown(float x,float y,float width,float height)
    {

        X = x;
        Y = y;
        Width = width / 5;
        Height = height / 10;
    }

}
