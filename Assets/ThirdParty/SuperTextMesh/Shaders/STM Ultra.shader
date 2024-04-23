Shader "Super Text Mesh/Ultra" {
	Properties 
	{
		/*
		todo:
		[ ]outlines and dropshadows dont work on UI(this may just be because of pre-2017 versions of unity!! i'll check)
			ok yeah it wont work in versions before 2017 because that didn't have additional shader channels on the UI
			should be fine after since we have UV3 and UV4, but it isn't working yet anyway...
		[ ]editing with STM inspetor should update ALL STMs using same material
		[x]fix outlines being different for different fonts and sideways UVs, oblong texutres
		
		[x]ok so the outlines on sideways letters are actually going sideways, too! but they're going the same distance
		[ ]in stm, uv3.zw is being sent with the texel size (not quality) divided by 4. not sure why but it works
		[x] bounding boxes go out way too far
		  wait. this is fixed in 5.3.4, but in 2019 it's broken! wtf
		[ ] add square outline option
		[ ] sdf mode gotta effect outlines
		[ ] mark alpha as transparency on itim sdf texture in 5.3.4
		[ ] changing quality changes shadow distance
		[ ] uv3.xy goes unused
		*/
		_MainTex ("Font Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
		[Toggle(SDF_MODE)] _SDFMode ("Toggle SDF Mode", Float) = 0
		[ShowIf(SDF_MODE)] _SDFCutoff ("SDF Cutoff", Range(0,1)) = 0.5
		[ShowIf(SDF_MODE)] _Blend ("Blend Width", Range(0.0001,1)) = 0.05
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_ShadowCutoff ("Shadow Cutoff", Range(0,1)) = 0.5
		_Cutoff ("Cutoff", Range(0,1)) = 0.0001 //text cutoff
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 0
		[Enum(Normal,4,On Top,8)] _ZTestMode ("ZTest Mode", Float) = 4
		
		_StencilComp ("Stencil Comparison", Float) = 8 //_StencilComp
		_Stencil ("Stencil Mode", Float) = 0 //_Stencil
		_StencilOp ("Stencil Operation", Float) = 0 //_StencilOp
        _StencilWriteMask ("Stencil Write Mask", Float) = 255 //_StencilWriteMask
		_StencilReadMask ("Stencil Read Mask", Float) = 255 //_StencilReadMask
		_ColorMask ("Color Mask", Float) = 15

		//[Enum(None, 0, Drop Shadow, 1, Outline, 2)] _ExpansionEffect ("Enable Expansion Effects (outline/dropshadows)", int) = 0
		
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		//enum mode for now
		//https://docs.unity3d.com/ScriptReference/MaterialPropertyDrawer.html
		[KeywordEnum(None, Dropshadow, Outline)] _Effect ("Effect Mode", int) = 0
		//outline settings
		//[Toggle(EFFECT_OUTLINE)] _OutlineEnabled ("Outline Enabled", Float) = 0
		[ShowIf(EFFECT_OUTLINE)] _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        [ShowIf(EFFECT_OUTLINE)] _OutlineWidth ("Outline Width", Range(0,1)) = 0.05
		[ShowIf(EFFECT_OUTLINE)][Enum(Circle, 0, Square, 1)] _OutlineType ("Outline Type", int) = 0
		[ShowIf(EFFECT_OUTLINE)] _OutlineSamples  ("Outline Samples", Range(1, 256)) = 32
		//dropshadow
		//[Toggle(EFFECT_DROPSHADOW)] _DropshadowEnabled ("Dropshadow Enabled", Float) = 0
		[ShowIf(EFFECT_DROPSHADOW)] _DropshadowColor ("Dropshadow Color", Color) = (0,0,0,1)
		[ShowIf(EFFECT_DROPSHADOW)][Enum(Angle, 0, Vector, 1)] _DropshadowType ("Dropshadow Type", int) = 0
		[ShowIf(EFFECT_DROPSHADOW)] _DropshadowAngle ("Dropshadow Angle", Range(0,360)) = 135
		[ShowIf(EFFECT_DROPSHADOW)] _DropshadowDistance ("Dropshadow Distance", Range(0,1)) = 1
		[ShowIf(EFFECT_DROPSHADOW)] _DropshadowAngle2 ("Dropshadow Vector", Vector) = (1,-1,0)
		/*
		what do i want for inspector...
		*/
	}
	SubShader {
		Tags { 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"STMUberShader2"="Yes"
			"STMMaskingSupport"="Yes"
		}
		Stencil
		{  
			Ref [_Stencil]  //Customize this value  
			Comp [_StencilComp] //Customize the compare function  
			Pass [_StencilOp]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}
		
		LOD 100

		Lighting Off
		Cull [_CullMode]
		ZTest [_ZTestMode]
		ZWrite On //this can be off if there's no dropshadow/outline. those use the z buffer.
		Blend SrcAlpha OneMinusSrcAlpha
		 ColorMask [_ColorMask]

		Pass 
		{
			//this page has an explanation of multi-compile:
			//https://docs.unity3d.com/2019.3/Documentation/Manual/SL-MultipleProgramVariants.html
			//https://docs.unity3d.com/Manual/SL-MultipleProgramVariants.html
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "STMultra.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature SDF_MODE
			#pragma shader_feature PIXELSNAP_ON
			//for now, only allow one at a time
			#pragma shader_feature __ _EFFECT_DROPSHADOW _EFFECT_OUTLINE
			//#pragma shader_feature_local 
			ENDCG
		}
		Zwrite On
		Pass
		{
			Tags {"LightMode"="ShadowCaster"}

			CGPROGRAM
			#include "UnityCG.cginc"
			#include "STMshadow.cginc"
            #pragma vertex vert
            #pragma fragment frag
			#pragma shader_feature SDF_MODE
			ENDCG
		}
	}
	FallBack "GUI/Text Shader"
}