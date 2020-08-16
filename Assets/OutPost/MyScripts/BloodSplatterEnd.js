#pragma strict

var texture : Texture;

function OnGUI()
{
	GUI.depth = 1;
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), texture);	
}