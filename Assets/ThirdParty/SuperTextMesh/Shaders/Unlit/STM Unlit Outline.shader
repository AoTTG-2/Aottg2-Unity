Shader "Super Text Mesh/Unlit/Outline" {
	Properties 
	{
		_MainTex ("Font Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0,1)) = 0.05

		[Toggle(SQUARE_OUTLINE)] _SquareOutline ("Vector3 Outline", Float) = 0

		[Toggle(SDF_MODE)] _SDFMode ("Toggle SDF Mode", Float) = 0
		[ShowIf(SDF_MODE)] _SDFCutoff ("SDF Cutoff", Range(0,1)) = 0.5
		[ShowIf(SDF_MODE)] _Blend ("Blend Width", Range(0.0001,1)) = 0.05
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_ShadowCutoff ("Shadow Cutoff", Range(0,1)) = 0.5
		_Cutoff ("Cutoff", Range(0,1)) = 0.0001 //text cutoff
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 0
		[Enum(Normal,4,On Top,8)] _ZTestMode ("ZTest Mode", Float) = 4
	}
	SubShader {
		Tags { 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"STMUberShader"="Yes"
		}
		LOD 100

		Lighting Off
		Cull [_CullMode]
		ZTest [_ZTestMode]
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass 
		{
			CGPROGRAM
			static float3 ANGLE3 = float3(0,1,0);
			static float ANGLE = 0;
			#include "UnityCG.cginc"
			#include "../STMoutline.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature SQUARE_OUTLINE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		Pass 
		{
			CGPROGRAM
			static float3 ANGLE3 = float3(1,1,0);
			static float ANGLE = 90;
			#include "UnityCG.cginc"
			#include "../STMoutline.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature SQUARE_OUTLINE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		Pass 
		{
			CGPROGRAM
			static float3 ANGLE3 = float3(1,0,0);
			static float ANGLE = 180;
			#include "UnityCG.cginc"
			#include "../STMoutline.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature SQUARE_OUTLINE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		Pass 
		{
			CGPROGRAM
			static float3 ANGLE3 = float3(1,-1,0);
			static float ANGLE = 270;
			#include "UnityCG.cginc"
			#include "../STMoutline.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature SQUARE_OUTLINE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		Pass 
		{
			CGPROGRAM
			static float3 ANGLE3 = float3(0,-1,0);
			static float ANGLE = 45;
			#include "UnityCG.cginc"
			#include "../STMoutline.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature SQUARE_OUTLINE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		Pass 
		{
			CGPROGRAM
			static float3 ANGLE3 = float3(-1,-1,0);
			static float ANGLE = 135;
			#include "UnityCG.cginc"
			#include "../STMoutline.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature SQUARE_OUTLINE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		Pass 
		{
			CGPROGRAM
			static float3 ANGLE3 = float3(-1,0,0);
			static float ANGLE = 225;
			#include "UnityCG.cginc"
			#include "../STMoutline.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature SQUARE_OUTLINE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		Pass 
		{
			CGPROGRAM
			static float3 ANGLE3 = float3(-1,1,0);
			static float ANGLE = 315;
			#include "UnityCG.cginc"
			#include "../STMoutline.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature SQUARE_OUTLINE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "../STM.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature PIXELSNAP_ON
			ENDCG
		}
		ZWrite On
		Pass
		{
			Tags {"LightMode"="ShadowCaster"}

			CGPROGRAM
			#include "UnityCG.cginc"
			#include "../STMshadow.cginc"
            #pragma vertex vert
            #pragma fragment frag
			#pragma shader_feature SDF_MODE
			ENDCG
		}
	}
	FallBack "GUI/Text Shader"
}
