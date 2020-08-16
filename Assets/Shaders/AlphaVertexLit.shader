Shader "Custom/Transparent VertexLit"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecCol ("Specular Color", Color) = (1,1,1,1)
		_Emission ("Emmisive Color", Color) = (0,0,0,0)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.7
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}
 
	SubShader
	{
		Tags {"RenderType"="Transparent" "Queue"="Transparent"}

		Pass
		{
			Blend One One
			Blend One One
			Blend One One
			ColorMask 0
		}
    
		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			ColorMask RGB

			Lighting On
			SeparateSpecular On
			
			Material
			{
				Diffuse [_Color]
				Ambient [_Color]
				Shininess [_Shininess]
				Specular [_SpecCol]
				Emission [_Emission]
			}

			SetTexture [_MainTex]
			{
				Combine texture * primary
			}
		}
	}
}