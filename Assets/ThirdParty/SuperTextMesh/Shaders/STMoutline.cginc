//Copyright (c) 2016-2018 Kai Clavier [kaiclavier.com] Do Not Distribute

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

float4 _OutlineColor;
float _OutlineWidth;

//uniform float ANGLE = 135;
//uniform float3 ANGLE3 = (1,1,0)

v2f vert (appdata v)
{
    v2f o;
    //single-pass stereo rendering:
    #if defined(UNITY_STEREO_INSTANCING_ENABLED)
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    #endif
    o.vertex = v.vertex;
    #ifdef SQUARE_OUTLINE
    o.vertex.x += ANGLE3.x * _OutlineWidth;
    o.vertex.y += ANGLE3.y * _OutlineWidth;
    o.vertex.z += ANGLE3.z * _OutlineWidth;
    #else
    o.vertex.x += sin(ANGLE * 0.01745327777) * _OutlineWidth;
    o.vertex.y += cos(ANGLE * 0.01745327777) * _OutlineWidth;
    #endif
    #if UNITY_VERSION < 540
    o.vertex = mul(UNITY_MATRIX_MVP, o.vertex); //UNITY_SHADER_NO_UPGRADE
    #else
    o.vertex = UnityObjectToClipPos(o.vertex);
    #endif
    o.color.rgb = _OutlineColor.rgb;
    o.color.a = v.color.a * _OutlineColor.a;
    o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
    o.uv2_MaskTex = TRANSFORM_TEX(v.uv2_MaskTex, _MaskTex);
    #ifdef PIXELSNAP_ON
    o.vertex = UnityPixelSnap(o.vertex);
    #endif
    return o;
}

float4 when_lt(float4 x, float4 y) {
    return max(sign(y - x), 0.0);
}

float4 when_ge(float4 x, float4 y) {
    return 1.0 - when_lt(x, y);
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

/*
    older SDF code, with SDF outline support. 
    keeping it around just in case, but it doesn't work too well
    with how STM uses generated SDFs. (no extra space for outlines)
    if(text.a < _SDFCutoff - _SDFOutlineWidthOut - _Blend)
    {
        col.rgb = i.color.rgb;
        col.a = 0; //cut!
    }
    else if(text.a < _SDFCutoff - _SDFOutlineWidthOut + _Blend)
    { //blend between cutoff and edge of letter
        //blend
        if(_SDFOutlineWidthIn > 0 || _SDFOutlineWidthOut > 0) //transparency to outline
        {//get color from outline, or inside depending on if there's an outline or not
            col.rgb = _OutlineColor.rgb;
            col.a = (text.a - _SDFCutoff + _SDFOutlineWidthOut + (_Blend/100)) / _Blend * _OutlineColor.a;
        }
        else //transparency to text
        {
            col.rgb = mask.rgb * i.color.rgb;
            col.a = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * mask.a * i.color.a;
        }
    }
    else if(text.a < _SDFCutoff + _SDFOutlineWidthIn)
    { //inside outline
        col = _OutlineColor; //get color from outline
    }
    else if(text.a < _SDFCutoff + _SDFOutlineWidthIn + _Blend)
    { //blend between inside outline and inside color
        col = lerp(_OutlineColor, mask * i.color, (text.a - _SDFCutoff - _SDFOutlineWidthIn) / _Blend);
    }
    else
    {
        col = mask * i.color; //get color from mask & vertex
    }
*/
    #else
    col.rgb = mask.rgb * i.color.rgb;
    col.a = text.a * mask.a * i.color.a;
    #endif
    clip(col.a - _Cutoff);
    return col;
}