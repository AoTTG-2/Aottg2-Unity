Shader "Super Text Mesh/Unlit/Dropshadow" {
	Properties 
	{
		_MainTex ("Font Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}

		_ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _ShadowDistance ("Shadow Distance", Range(0,1)) = 0.05
		[Toggle(VECTOR3_DROPSHADOW)] _Vector3Dropshadow ("Vector3 Dropshadow", Float) = 0
		//[Toggle(SQUARE_OUTLINE)] _SquareOutline ("Vector3 Outline", Float) = 0
        [HideIf(VECTOR3_DROPSHADOW)] _ShadowAngle ("Shadow Angle", Range(0,360)) = 135
		[ShowIf(VECTOR3_DROPSHADOW)] _ShadowAngle3 ("Shadow Angle3", Vector) = (1,-1,0)

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
			#include "UnityCG.cginc"
			#include "../STMdropshadow.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature VECTOR3_DROPSHADOW
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
