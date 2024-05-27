// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Polytope Studio/PT_Water_Shader_WebGl"
{
	Properties
	{
		_DeepColor("Deep Color", Color) = (0.3114988,0.5266015,0.5283019,0)
		_ShallowColor("Shallow Color", Color) = (0.5238074,0.7314408,0.745283,0)
		_Depth("Depth", Range( 0 , 1)) = 0.3
		_DepthStrength("Depth Strength", Range( 0 , 1)) = 0.3
		_Smootness("Smootness", Range( 0 , 1)) = 1
		_WaveSpeed("Wave Speed", Range( 0 , 1)) = 0.5
		_WaveTile("Wave Tile", Range( 0 , 0.9)) = 0.5
		_WaveAmplitude("Wave Amplitude", Range( 0 , 1)) = 0.2
		[NoScaleOffset][Normal]_NormalMapTexture("Normal Map Texture ", 2D) = "bump" {}
		_NormalMapWavesSpeed("Normal Map Waves Speed", Range( 0 , 1)) = 0.1
		_NormalMapsWavesSize("Normal Maps Waves Size", Range( 0 , 10)) = 5
		_FoamColor("Foam Color", Color) = (0.3066038,1,0.9145772,0)
		_FoamAmount("Foam Amount", Range( 0 , 10)) = 1.5
		_FoamPower("Foam Power", Range( 0.1 , 5)) = 0.5
		_FoamNoiseScale("Foam Noise Scale", Range( 0 , 1000)) = 150
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" }
		Cull Off
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform float _WaveAmplitude;
		uniform float _WaveSpeed;
		uniform float _WaveTile;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FoamAmount;
		uniform float _FoamPower;
		uniform float _FoamNoiseScale;
		uniform sampler2D _NormalMapTexture;
		uniform float _NormalMapsWavesSize;
		uniform float _NormalMapWavesSpeed;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float4 _ShallowColor;
		uniform float4 _DeepColor;
		uniform float _DepthStrength;
		uniform float _Depth;
		uniform float4 _FoamColor;
		uniform float _Smootness;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float2 UnityGradientNoiseDir( float2 p )
		{
			p = fmod(p , 289);
			float x = fmod((34 * p.x + 1) * p.x , 289) + p.y;
			x = fmod( (34 * x + 1) * x , 289);
			x = frac( x / 41 ) * 2 - 1;
			return normalize( float2(x - floor(x + 0.5 ), abs( x ) - 0.5 ) );
		}
		
		float UnityGradientNoise( float2 UV, float Scale )
		{
			float2 p = UV * Scale;
			float2 ip = floor( p );
			float2 fp = frac( p );
			float d00 = dot( UnityGradientNoiseDir( ip ), fp );
			float d01 = dot( UnityGradientNoiseDir( ip + float2( 0, 1 ) ), fp - float2( 0, 1 ) );
			float d10 = dot( UnityGradientNoiseDir( ip + float2( 1, 0 ) ), fp - float2( 1, 0 ) );
			float d11 = dot( UnityGradientNoiseDir( ip + float2( 1, 1 ) ), fp - float2( 1, 1 ) );
			fp = fp * fp * fp * ( fp * ( fp * 6 - 15 ) + 10 );
			return lerp( lerp( d00, d01, fp.y ), lerp( d10, d11, fp.y ), fp.x ) + 0.5;
		}


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 appendResult71 = (float4(0.23 , -0.8 , 0.0 , 0.0));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult81 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float2 panner79 = ( ( _Time.y * _WaveSpeed ) * appendResult71.xy + ( ( appendResult81 * float4( float2( 6.5,0.9 ), 0.0 , 0.0 ) ) * _WaveTile ).xy);
			float simplePerlin2D77 = snoise( panner79 );
			simplePerlin2D77 = simplePerlin2D77*0.5 + 0.5;
			float WAVESDISPLACEMENT69 = ( ( float3(0,0.05,0).y * _WaveAmplitude ) * simplePerlin2D77 );
			float3 temp_cast_3 = (WAVESDISPLACEMENT69).xxx;
			v.vertex.xyz += temp_cast_3;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth33 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth33 = abs( ( screenDepth33 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamAmount ) );
			float saferPower42 = abs( distanceDepth33 );
			float temp_output_42_0 = pow( saferPower42 , _FoamPower );
			float2 temp_cast_0 = (_FoamNoiseScale).xx;
			float2 temp_cast_1 = (( _Time.y * 0.2 )).xx;
			float2 uv_TexCoord34 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float gradientNoise40 = UnityGradientNoise(uv_TexCoord34,1.0);
			gradientNoise40 = gradientNoise40*0.5 + 0.5;
			float temp_output_48_0 = step( temp_output_42_0 , gradientNoise40 );
			float FoamMask59 = temp_output_48_0;
			float4 appendResult5 = (float4(_NormalMapsWavesSize , _NormalMapsWavesSize , 0.0 , 0.0));
			float mulTime3 = _Time.y * 0.1;
			float2 temp_cast_3 = (( mulTime3 * _NormalMapWavesSpeed )).xx;
			float2 uv_TexCoord7 = i.uv_texcoord * appendResult5.xy + temp_cast_3;
			float2 temp_output_2_0_g9 = uv_TexCoord7;
			float2 break6_g9 = temp_output_2_0_g9;
			float temp_output_25_0_g9 = ( pow( 0.5 , 3.0 ) * 0.1 );
			float2 appendResult8_g9 = (float2(( break6_g9.x + temp_output_25_0_g9 ) , break6_g9.y));
			float4 tex2DNode14_g9 = tex2D( _NormalMapTexture, temp_output_2_0_g9 );
			float temp_output_4_0_g9 = 1.0;
			float3 appendResult13_g9 = (float3(1.0 , 0.0 , ( ( tex2D( _NormalMapTexture, appendResult8_g9 ).g - tex2DNode14_g9.g ) * temp_output_4_0_g9 )));
			float2 appendResult9_g9 = (float2(break6_g9.x , ( break6_g9.y + temp_output_25_0_g9 )));
			float3 appendResult16_g9 = (float3(0.0 , 1.0 , ( ( tex2D( _NormalMapTexture, appendResult9_g9 ).g - tex2DNode14_g9.g ) * temp_output_4_0_g9 )));
			float3 normalizeResult22_g9 = normalize( cross( appendResult13_g9 , appendResult16_g9 ) );
			float3 NORMALMAPWAVES15 = normalizeResult22_g9;
			float4 color104 = IsGammaSpace() ? float4(0.4980392,0.4980392,1,0) : float4(0.2122307,0.2122307,1,0);
			float layeredBlendVar90 = FoamMask59;
			float4 layeredBlend90 = ( lerp( float4( NORMALMAPWAVES15 , 0.0 ),color104 , layeredBlendVar90 ) );
			float4 normalizeResult72 = normalize( layeredBlend90 );
			o.Normal = normalizeResult72.rgb;
			float screenDepth35 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth35 = abs( ( screenDepth35 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 100.0 ) );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor39 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( (ase_grabScreenPosNorm).xyzw + float4( ( NORMALMAPWAVES15 * 1.0 ) , 0.0 ) ).xy);
			float4 FAKEREFRACTIONS50 = ( ( 1.0 - distanceDepth35 ) * screenColor39 );
			float eyeDepth20 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float clampResult44 = clamp( ( _DepthStrength * ( eyeDepth20 - ( ase_screenPos.w + _Depth ) ) ) , 0.0 , 1.0 );
			float4 lerpResult47 = lerp( _ShallowColor , _DeepColor , clampResult44);
			float4 DeepShallowColor51 = lerpResult47;
			float4 lerpResult55 = lerp( FAKEREFRACTIONS50 , DeepShallowColor51 , float4( 0.6132076,0.6132076,0.6132076,0 ));
			float4 FoamColor54 = ( temp_output_48_0 * _FoamColor );
			o.Albedo = ( lerpResult55 + FoamColor54 ).rgb;
			float4 temp_cast_9 = (_Smootness).xxxx;
			float4 color92 = IsGammaSpace() ? float4(0.2264151,0.2264151,0.2264151,0) : float4(0.04193995,0.04193995,0.04193995,0);
			float layeredBlendVar101 = FoamMask59;
			float4 layeredBlend101 = ( lerp( temp_cast_9,color92 , layeredBlendVar101 ) );
			o.Smoothness = layeredBlend101.r;
			float DeepShallowMask68 = clampResult44;
			float smoothstepResult96 = smoothstep( 0.2 , 1.2 , FoamMask59);
			float clampResult75 = clamp( ( smoothstepResult96 * 0.05 ) , 0.0 , 1.0 );
			float TRANSPARENCYFINAL105 = ( DeepShallowMask68 + (1.0 + (0.95 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) + clampResult75 );
			o.Alpha = TRANSPARENCYFINAL105;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float4 tSpace0 : TEXCOORD4;
				float4 tSpace1 : TEXCOORD5;
				float4 tSpace2 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.CommentaryNode;1;-2860.221,-2767.032;Inherit;False;1847.655;579.5157;Comment;9;15;11;8;7;6;5;4;3;2;Normal Map Waves;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;13;-318.5545,-2479.646;Inherit;False;1745.648;737.933;Foam;15;95;59;54;49;48;45;42;40;34;33;32;27;26;24;19;Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-2738.214,-2282.876;Inherit;False;Property;_NormalMapWavesSpeed;Normal Map Waves Speed;12;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-2813.985,-2554.105;Inherit;False;Property;_NormalMapsWavesSize;Normal Maps Waves Size;13;0;Create;True;0;0;0;False;0;False;5;5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;3;-2577.033,-2380.765;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;5;-2442.628,-2531.67;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;19;-269.8217,-1935.53;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-324.3314,-1830.444;Inherit;False;Constant;_FoamSpeed;Foam Speed;20;0;Create;True;0;0;0;False;0;False;0.2;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-2334.804,-2399.479;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-2245.326,-2532.602;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-280.4615,-2287.179;Inherit;False;Property;_FoamAmount;Foam Amount;15;0;Create;True;0;0;0;False;0;False;1.5;1.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-24.61484,-1912.608;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-278.7315,-2082.779;Inherit;False;Property;_FoamNoiseScale;Foam Noise Scale;17;0;Create;True;0;0;0;False;0;False;150;150;0;1000;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;8;-2442.107,-2758.511;Inherit;True;Property;_NormalMapTexture;Normal Map Texture ;11;2;[NoScaleOffset];[Normal];Create;True;0;0;0;True;0;False;a78adb8868cccbe4a92d9d81db916e6e;a78adb8868cccbe4a92d9d81db916e6e;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.CommentaryNode;9;-2726.994,-552.2872;Inherit;False;1982.289;589.8825;Comment;14;68;51;47;44;41;38;37;31;30;23;20;16;12;10;Deep&ShallowColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;11;-1982.756,-2744.316;Inherit;True;NormalCreate;0;;9;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;10;-2695.149,-309.5551;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;34;38.04077,-2080.626;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;33;-28.18245,-2436.97;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;4.163269,-2224.65;Inherit;False;Property;_FoamPower;Foam Power;16;0;Create;True;0;0;0;False;0;False;0.5;0.5;0.1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;56;-2680.193,267.1243;Inherit;False;1957.563;1136.648;Comment;21;102;99;94;91;87;85;83;81;79;78;77;73;71;70;69;67;66;65;63;61;60;WavesVertexOffset;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;14;-2661.164,-1795.197;Inherit;False;1714.404;696.3644;Comment;12;50;46;43;39;36;35;29;28;25;22;21;17;FAKE REFRACTIONS;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;40;414.8806,-2085.356;Inherit;True;Gradient;True;True;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;42;342.3517,-2372.323;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;16;-2467.752,-300.6801;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;12;-2607.954,-119.3663;Inherit;False;Property;_Depth;Depth;4;0;Create;True;0;0;0;False;0;False;0.3;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-1324.422,-2732.54;Inherit;False;NORMALMAPWAVES;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2461.04,-1220.55;Inherit;False;Constant;_Float3;Float 3;18;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-2423.155,-1322.014;Inherit;False;15;NORMALMAPWAVES;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-2341.202,-227.3137;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;20;-2511.162,-401.6147;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;48;588.4208,-2252.122;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;78;-2630.193,519.4303;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GrabScreenPosition;21;-2611.164,-1537.815;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;29;-2342.759,-1531.594;Inherit;False;True;True;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2311.551,-422.7628;Inherit;False;Property;_DepthStrength;Depth Strength;5;0;Create;True;0;0;0;False;0;False;0.3;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-2128.249,-1325.15;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;31;-2208.881,-285.3358;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;73;-2416.326,686.3053;Inherit;False;Constant;_Vector0;Vector 0;15;0;Create;True;0;0;0;False;0;False;6.5,0.9;1,10;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;81;-2407.841,525.8712;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-2237.695,-1704.798;Inherit;False;Constant;_Float4;Float 4;22;0;Create;True;0;0;0;False;0;False;100;1;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;59;1184.976,-2371.161;Inherit;False;FoamMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;35;-1936.723,-1745.197;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;62;-211.352,198.9783;Inherit;False;59;FoamMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-1843.745,-1376.259;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;57;144.4692,-84.96806;Inherit;False;1198.132;582.9205;Comment;9;105;103;96;93;80;76;75;74;64;TRANSPARENCY FINAL;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-2496.096,977.5432;Inherit;False;Constant;_wavedirectionx;wave direction x;17;0;Create;True;0;0;0;False;0;False;0.23;-1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-2148.827,528.4573;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-2212.851,753.6283;Inherit;False;Property;_WaveTile;Wave Tile;9;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-2108.119,1287.772;Inherit;False;Property;_WaveSpeed;Wave Speed;8;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2014.39,-310.4016;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;61;-2007.354,1185.461;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-2488.928,1063.094;Inherit;False;Constant;_wavedirectiony;wave direction y;24;0;Create;True;0;0;0;False;0;False;-0.8;0.067;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-1895.786,530.8673;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;93;251.166,452.8219;Inherit;False;Constant;_Float0;Float 0;29;0;Create;True;0;0;0;True;0;False;0.05;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;44;-1848.948,-307.1082;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-1783.354,1235.461;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;96;335.9149,235.6729;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;43;-1640.374,-1744.727;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;39;-1668.698,-1375.208;Inherit;False;Global;_GrabScreen0;Grab Screen 0;21;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;94;-1765.857,343.2173;Inherit;False;Constant;_Vector1;Vector 1;14;0;Create;True;0;0;0;False;0;False;0,0.05,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;41;-1579.718,-200.467;Inherit;False;Property;_DeepColor;Deep Color;2;0;Create;True;0;0;0;False;0;False;0.3114988,0.5266015,0.5283019,0;1,0.8726415,0.8726415,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;71;-2119.817,1001.461;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;38;-1606.009,-409.5605;Inherit;False;Property;_ShallowColor;Shallow Color;3;0;Create;True;0;0;0;False;0;False;0.5238074,0.7314408,0.745283,0;0.2003827,0.745283,0.3747112,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;64;167.6422,30.29788;Inherit;False;Constant;_Transparency;Transparency;2;0;Create;True;0;0;0;False;0;False;0.95;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;551.5494,304.3638;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;68;-1629.68,-489.6985;Inherit;False;DeepShallowMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1418.169,-1406.905;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;83;-1550.686,368.8113;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;70;-1661.686,577.8113;Inherit;False;Property;_WaveAmplitude;Wave Amplitude;10;0;Create;True;0;0;0;False;0;False;0.2;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;79;-1638.964,969.9133;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;45;746.9266,-1941.397;Inherit;False;Property;_FoamColor;Foam Color;14;0;Create;True;0;0;0;False;0;False;0.3066038,1,0.9145772,0;0.2971698,1,0.9126425,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;47;-1298.001,-346.3586;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-1419.686,456.8113;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-984.2501,-352.5178;Inherit;False;DeepShallowColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;77;-1379.427,893.3003;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;74;481.9349,62.44095;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;75;689.1746,260.2238;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;-1177.811,-1408.146;Inherit;False;FAKEREFRACTIONS;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;76;468.2339,-34.96836;Inherit;False;68;DeepShallowMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;997.0488,-2172.865;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;657.2927,-1389.869;Inherit;False;50;FAKEREFRACTIONS;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;752.6437,-999.4344;Inherit;False;15;NORMALMAPWAVES;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;54;1182.43,-2166.723;Inherit;False;FoamColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;792.5258,-1082.094;Inherit;False;59;FoamMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-1215.872,341.8492;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;104;764.5768,-917.7066;Inherit;False;Constant;_Color0;Color 0;15;0;Create;True;0;0;0;False;0;False;0.4980392,0.4980392,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;52;650.9476,-1306.726;Inherit;False;51;DeepShallowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;103;807.732,28.93192;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;69;-982.236,352.5482;Inherit;False;WAVESDISPLACEMENT;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;92;866.0418,-543.4797;Inherit;False;Constant;_Color1;Color 1;15;0;Create;True;0;0;0;False;0;False;0.2264151,0.2264151,0.2264151,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;105;1021.635,20.11087;Inherit;False;TRANSPARENCYFINAL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;1148.053,-1188.369;Inherit;False;54;FoamColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;89;833.1308,-623.7841;Inherit;False;Property;_Smootness;Smootness;6;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;90;1169.526,-1070.094;Inherit;False;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;815.9908,-706.8666;Inherit;False;59;FoamMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;55;942.1397,-1415.504;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.6132076,0.6132076,0.6132076,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;1539.755,-571.0422;Inherit;False;69;WAVESDISPLACEMENT;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;101;1172.991,-712.8666;Inherit;False;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;1520.12,-680.3422;Inherit;False;105;TRANSPARENCYFINAL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;99;-1083.049,908.4213;Inherit;False;OffsetWavesMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;98;1374.15,-1408.553;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;97;1178.376,-927.0734;Inherit;False;Property;_Mettalic;Mettalic;7;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;95;550.2086,-2448.055;Inherit;False;newfoammask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;72;1398.325,-1061.994;Inherit;False;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1988.372,-1292.972;Float;False;True;-1;2;;0;0;Standard;Polytope Studio/PT_Water_Shader_WebGl;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;2;0
WireConnection;5;1;2;0
WireConnection;6;0;3;0
WireConnection;6;1;4;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;24;0;19;0
WireConnection;24;1;18;0
WireConnection;11;1;8;0
WireConnection;11;2;7;0
WireConnection;34;0;27;0
WireConnection;34;1;24;0
WireConnection;33;0;26;0
WireConnection;40;0;34;0
WireConnection;42;0;33;0
WireConnection;42;1;32;0
WireConnection;16;0;10;0
WireConnection;15;0;11;0
WireConnection;23;0;16;3
WireConnection;23;1;12;0
WireConnection;48;0;42;0
WireConnection;48;1;40;0
WireConnection;29;0;21;0
WireConnection;25;0;22;0
WireConnection;25;1;17;0
WireConnection;31;0;20;0
WireConnection;31;1;23;0
WireConnection;81;0;78;1
WireConnection;81;1;78;3
WireConnection;59;0;48;0
WireConnection;35;0;28;0
WireConnection;36;0;29;0
WireConnection;36;1;25;0
WireConnection;65;0;81;0
WireConnection;65;1;73;0
WireConnection;37;0;30;0
WireConnection;37;1;31;0
WireConnection;102;0;65;0
WireConnection;102;1;66;0
WireConnection;44;0;37;0
WireConnection;85;0;61;0
WireConnection;85;1;60;0
WireConnection;96;0;62;0
WireConnection;43;0;35;0
WireConnection;39;0;36;0
WireConnection;71;0;67;0
WireConnection;71;1;63;0
WireConnection;80;0;96;0
WireConnection;80;1;93;0
WireConnection;68;0;44;0
WireConnection;46;0;43;0
WireConnection;46;1;39;0
WireConnection;83;0;94;0
WireConnection;79;0;102;0
WireConnection;79;2;71;0
WireConnection;79;1;85;0
WireConnection;47;0;38;0
WireConnection;47;1;41;0
WireConnection;47;2;44;0
WireConnection;91;0;83;1
WireConnection;91;1;70;0
WireConnection;51;0;47;0
WireConnection;77;0;79;0
WireConnection;74;0;64;0
WireConnection;75;0;80;0
WireConnection;50;0;46;0
WireConnection;49;0;48;0
WireConnection;49;1;45;0
WireConnection;54;0;49;0
WireConnection;87;0;91;0
WireConnection;87;1;77;0
WireConnection;103;0;76;0
WireConnection;103;1;74;0
WireConnection;103;2;75;0
WireConnection;69;0;87;0
WireConnection;105;0;103;0
WireConnection;90;0;86;0
WireConnection;90;1;84;0
WireConnection;90;2;104;0
WireConnection;55;0;53;0
WireConnection;55;1;52;0
WireConnection;101;0;88;0
WireConnection;101;1;89;0
WireConnection;101;2;92;0
WireConnection;99;0;77;0
WireConnection;98;0;55;0
WireConnection;98;1;58;0
WireConnection;95;0;42;0
WireConnection;72;0;90;0
WireConnection;0;0;98;0
WireConnection;0;1;72;0
WireConnection;0;4;101;0
WireConnection;0;9;100;0
WireConnection;0;11;82;0
ASEEND*/
//CHKSM=60FAB36643FAEFD0568125BEE76DF3E8466292A3