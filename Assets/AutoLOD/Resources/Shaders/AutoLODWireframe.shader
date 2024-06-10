CGINCLUDE
#include "UnityCG.cginc"

struct v2g
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
};

struct g2f
{
    float4 vertex : SV_POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
    float3 wpos : TEXCOORD1;
    float visibility : TEXCOORD2;
};


half4 _Color;
float _Opacity;

v2g vert(v2g v) { return v; }

g2f getg2f(v2g v)
{
    g2f o;
    v.vertex.xyz += v.normal.xyz * 0.0001;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.normal = UnityObjectToWorldNormal(v.normal);
    o.uv = v.uv;
    o.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
    o.visibility = _Opacity;
    return o;
}

half4 frag(g2f i) : SV_Target
{
    float3 vd = normalize(i.wpos - _WorldSpaceCameraPos);
    float facing = -dot(i.normal.xyz , vd);
    float visibility = smoothstep(-0.1,0.5,facing) + smoothstep(0.1,-0.5,facing);
    visibility = saturate(visibility) * i.visibility;
    if (visibility < 0.001) discard;
    half4 col = half4(_Color.rgb , _Color.a * visibility);
    return col;
}
ENDCG

Shader "AutoLOD/Wireframe"
{
    Properties
    {
        [Header(RGB)]
        _Color("Color", Color) = (1,1,1,1)

        [Header(Alpha)]
        _Opacity("Opacity", Range(0.0,1.0)) = 0.1

    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" "Queue" = "Geometry+5" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest LEqual
            ZWrite Off
            Cull Off

            Pass
            {
                Name "LINES"
                CGPROGRAM
                #pragma vertex vert
                #pragma geometry geom
                #pragma fragment frag

                [maxvertexcount(32)]
                void geom(triangle v2g v[3] , uint pid : SV_PRIMITIVEID , inout LineStream<g2f> stream)
                {
                    g2f g0 = getg2f(v[0]);
                    g2f g1 = getg2f(v[1]);
                    g2f g2 = getg2f(v[2]);


                    stream.Append(g0);
                    stream.Append(g1);
                    stream.Append(g2);
                    stream.Append(g0);
                    stream.RestartStrip();


                    stream.Append(g0);
                    stream.RestartStrip();
                    stream.Append(g1);
                    stream.RestartStrip();
                    stream.Append(g2);
                    stream.RestartStrip();
                }
                ENDCG
            }
        }
}