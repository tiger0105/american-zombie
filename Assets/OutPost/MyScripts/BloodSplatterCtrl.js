#pragma strict

var fadeTexture : Texture;
var startColor : Color = Color(1,1,1,1);
var endColor : Color = Color(1,1,1,0);
var duration : float = 3.0;
private var t : float;
internal var currentColor : Color;

function Start()
{
	t = Time.time;
	currentColor = startColor;
	Destroy(gameObject,duration+1);
}

function OnGUI()
{
	GUI.depth = -1;
	GUI.color = currentColor;
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), fadeTexture);	
}

function Update()
{
	currentColor = Color.Lerp(startColor, endColor,(Time.time-t)/duration);
}