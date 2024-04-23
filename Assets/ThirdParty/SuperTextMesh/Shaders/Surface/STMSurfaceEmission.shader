Shader "Super Text Mesh/Surface/Emission" 
{
	
	//Temporary shader!!
	//pixel snap not yet supported
	//uses full emission, but can be changed at the end of this code.


	
	Properties 
	{
		_MainTex ("Font Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
		[Toggle(SDF_MODE)] _SDFMode ("Toggle SDF Mode", Float) = 0
		[ShowIf(SDF_MODE)] _SDFCutoff ("SDF Cutoff", Range(0,1)) = 0.5
		[ShowIf(SDF_MODE)] _Blend ("Blend Width", Range(0.0001,1)) = 0.05
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_ShadowCutoff ("Shadow Cutoff", Range(0,1)) = 0.5
		_Cutoff ("Cutoff", Range(0,1)) = 0.0001 //text cutoff
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 0
		[Enum(Normal,2,On Top,6)] _ZTestMode ("ZTest Mode", Float) = 2
	}
	SubShader 
	{
		Tags 
		{
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
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade addshadow
		#pragma shader_feature SDF_MODE


		sampler2D _MainTex;
		sampler2D _MaskTex;
		float _SDFCutoff;
		float _Blend;

		struct Input 
		{
			float2 uv_MainTex : TEXCOORD0;
			float2 uv2_MaskTex : TEXCOORD1;
			fixed4 color : COLOR;
			float4 vertex : POSITION;
		};

		float4 when_lt(float4 x, float4 y) 
		{
			return max(sign(y - x), 0.0);
		}

		float4 when_ge(float4 x, float4 y) 
		{
			return 1.0 - when_lt(x, y);
		}

		void surf (Input i, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 text = tex2D(_MainTex, i.uv_MainTex);
			fixed4 mask = tex2D(_MaskTex, i.uv2_MaskTex.xy);
			fixed4 col = fixed4(0,0,0,0);

			#if SDF_MODE
			//anything before this point is already cut by (0,0,0,0)
			//transparency to text
			col.rgb += (mask.rgb * i.color.rgb) * 
						when_ge(text.a, _SDFCutoff) * 
						when_lt(text.a, _SDFCutoff + _Blend);
			col.a += ((text.a - _SDFCutoff + (_Blend/100)) / _Blend * mask.a * i.color.a) * 
						//alpha greater or equal than cutoff
						when_ge(text.a, _SDFCutoff) * 
						//alpha less than blend point
						when_lt(text.a, _SDFCutoff + _Blend);
			//get color from mask & vertex
			col += (mask * i.color) * 
						//greater than blend point
						when_ge(text.a, _SDFCutoff + _Blend);
			#else
			col.rgb = mask.rgb * i.color.rgb;
    		col.a = text.a * mask.a * i.color.a;
			#endif

			o.Emission = col.rgb;
    		o.Alpha = col.a;
		}
		ENDCG
	}
	FallBack "GUI/Text Shader"
}
