Shader "Custom/chr_outline"{
	Properties{
		_MainTex("Texture", 2D) = "white" {}

		[Header(Colorize)][Space(5)]  //colorize
		_Color("Color", COLOR) = (1,1,1,1)

		[HideInInspector] _ColIntense("Intensity", Range(0,3)) = 1
		[HideInInspector] _ColBright("Brightness", Range(-1,1)) = 0
		_AmbientCol("Ambient", Range(0,1)) = 0

		[Header(Detail)][Space(5)]  //detail
		[Toggle] _Segmented("Segmented", Float) = 1
		_Steps("Steps", Range(1,25)) = 3
		_StpSmooth("Smoothness", Range(0,1)) = 0
		_Offset("Lit Offset", Range(-1,1.1)) = 0

		[Header(Light)][Space(5)]  //light
		[Toggle] _Clipped("Clipped", Float) = 0
		_MinLight("Min Light", Range(0,1)) = 0
		_MaxLight("Max Light", Range(0,1)) = 1
		_Lumin("Luminocity", Range(0,2)) = 0

		[Header(Shine)][Space(5)]  //shine
		[HDR] _ShnColor("Color", COLOR) = (1,1,0,1)
		[Toggle] _ShnOverlap("Overlap", Float) = 0

		_ShnIntense("Intensity", Range(0,1)) = 0
		_ShnRange("Range", Range(0,1)) = 0.15
		_ShnSmooth("Smoothness", Range(0,1)) = 0

		[Header(Outline)][Space(5)] //outline
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth("Outline Width", Range(1,5)) = 1
	}



		CGINCLUDE

#include "UnityCG.cginc"

			struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : POSITION;
			float3 normal : NORMAL;
		};

		float _OutlineWidth;
		float4 _OutlineColor;

		v2f vert(appdata v)
		{
			v.vertex.xyz *= _OutlineWidth;

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}
		ENDCG

		Subshader
		{

			Tags { "Queue" = "Transparent"}
				LOD 3000
				Pass //Rendering Outlines 
				{
					Zwrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag 

			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}
					ENDCG
				}
				
				Pass // Normal Render
				{
					ZWrite On

					Material
					{
						Diffuse[_Color]
						Ambient[_Color]
					}

					Lighting Off

					SetTexture[_MainTex] {
						constantColor[_Color]
						Combine texture * constant, texture * constant
					}
				}
		}


}