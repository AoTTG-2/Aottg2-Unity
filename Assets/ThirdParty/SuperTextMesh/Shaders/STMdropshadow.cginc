//Copyright (c) 2016-2021 Kai Clavier [kaiclavier.com] Do Not Distribute

//base stuff that can be used by any STM shader

struct appdata {
    float4 vertex : POSITION;
    fixed4 color : COLOR;
    float2 uv_MainTex : TEXCOORD0;
    float2 uv2_MaskTex : TEXCOORD1;
    #if defined(UNITY_STEREO_INSTANCING_ENABLED)
    UNITY_VERTEX_INPUT_INSTANCE_ID
    #endif
};

struct v2f
{
    float4 vertex : SV_POSITION;
    fixed4 color : COLOR;
    float2 uv_MainTex : TEXCOORD0;
    float2 uv2_MaskTex : TEXCOORD1;
    #if defined(UNITY_STEREO_INSTANCING_ENABLED)
    UNITY_VERTEX_OUTPUT_STEREO
    #endif
};

sampler2D _MainTex;
uniform float4 _MainTex_ST;
sampler2D _MaskTex;
uniform float4 _MaskTex_ST;
float _Cutoff;

float _SDFCutoff;
float _Blend;

float4 _ShadowColor;
float _ShadowDistance;
float _ShadowAngle;

float _UseVector3;
float3 _ShadowAngle3;

float4 when_lt(float4 x, float4 y) {
    return max(sign(y - x), 0.0);
}

float4 when_ge(float4 x, float4 y) {
    return 1.0 - when_lt(x, y);
}

v2f vert (appdata v)
{
    v2f o;
    //single-pass stereo rendering:
    #if defined(UNITY_STEREO_INSTANCING_ENABLED)
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    #endif
    o.vertex = v.vertex;
    #ifdef VECTOR3_DROPSHADOW
    o.vertex.x += (_ShadowAngle3.x * _ShadowDistance);
    o.vertex.y += (_ShadowAngle3.y * _ShadowDistance);
    o.vertex.z += (_ShadowAngle3.z * _ShadowDistance);
    #else
    o.vertex.x += (sin(_ShadowAngle * 0.01745327777) * _ShadowDistance);
    o.vertex.y += (cos(_ShadowAngle * 0.01745327777) * _ShadowDistance);
    #endif
    #if UNITY_VERSION < 540
    o.vertex = mul(UNITY_MATRIX_MVP, o.vertex); //UNITY_SHADER_NO_UPGRADE
    #else
    o.vertex = UnityObjectToClipPos(o.vertex);
    #endif
    o.color.rgb = _ShadowColor.rgb;
    o.color.a = v.color.a * _ShadowColor.a;
    o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
    o.uv2_MaskTex = TRANSFORM_TEX(v.uv2_MaskTex, _MaskTex);
    #ifdef PIXELSNAP_ON
    o.vertex = UnityPixelSnap(o.vertex);
    #endif
    return o;
}

//render normal text
fixed4 frag(v2f i) : SV_Target
{
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
    clip(col.a - _Cutoff);
    return col;
}