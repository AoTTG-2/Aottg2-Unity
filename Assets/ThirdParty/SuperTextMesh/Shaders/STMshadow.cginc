//Copyright (c) 2016-2018 Kai Clavier [kaiclavier.com] Do Not Distribute

//base stuff that can be used by any STM shader

struct v2f { 
    V2F_SHADOW_CASTER;
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

float _SDFCutoff;

float _ShadowCutoff;

v2f vert(appdata_full v)
{
    v2f o;
    //single-pass stereo rendering:
    #if defined(UNITY_STEREO_INSTANCING_ENABLED)
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    #endif
    o.color = v.color;
    o.uv_MainTex = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
    o.uv2_MaskTex = TRANSFORM_TEX(v.texcoord1.xy, _MaskTex);
    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
    return o;
}

float4 when_lt(float4 x, float4 y) {
    return max(sign(y - x), 0.0);
}
float4 when_ge(float4 x, float4 y) {
    return 1.0 - when_lt(x, y);
}

float4 frag(v2f i) : COLOR
{
    fixed4 col = fixed4(0,0,0,0);
    fixed4 text = tex2D(_MainTex, i.uv_MainTex);
    fixed4 mask = tex2D(_MaskTex, i.uv2_MaskTex.xy);
    #if SDF_MODE
    col += (mask * i.color) * when_ge(text.a, _SDFCutoff);
    #else
    col = text * mask * i.color;
    #endif
    clip(col.a - _ShadowCutoff);
    //if(col.a < _ShadowCutoff) discard;
    SHADOW_CASTER_FRAGMENT(i)
    return col;
}