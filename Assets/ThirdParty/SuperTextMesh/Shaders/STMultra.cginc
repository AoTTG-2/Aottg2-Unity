//Copyright (c) 2016-2023 Kai Clavier [kaiclavier.com] Do Not Distribute
//OMPUCO

//base stuff that can be used by any STM shader

struct appdata {
    float4 vertex : POSITION;
    fixed4 color : COLOR;
    float2 uv_MainTex : TEXCOORD0;
    float2 uv2_MaskTex : TEXCOORD1;
    float4 uv3 : TEXCOORD2;
    float4 uv4 : TEXCOORD3;
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
    float4 dist : TEXCOORD2;
    float2 scale : TEXCOORD4;
    float2 quality : TEXCOORD5;
    #if defined(UNITY_STEREO_INSTANCING_ENABLED)
    UNITY_VERTEX_OUTPUT_STEREO
    #endif
};

//Custom frag output struct that includes depth
//lets us control the depth/zbuffer output.
struct fragOut
{
    fixed4 col : SV_TARGET;
    float depth : DEPTH;
};
//This allows us to make use of depth testing
//to prevent outlines from rendering over other
//character faces that they may occlude

sampler2D _MainTex;
uniform float4 _MainTex_ST;
uniform float4 _MainTex_TexelSize;
sampler2D _MaskTex;
uniform float4 _MaskTex_ST;
uniform float4 _MaskTex_TexelSize;
float _Cutoff;

float _SDFCutoff;
float _Blend;

//float2 shadowOffset; //change all references of this to whatever the shadow offset will be
float4 _DropshadowColor; //change all references of this to whatever the shadow color will be
float _DropshadowAngle;
float3 _DropshadowAngle2;
float _DropshadowType;
float _DropshadowDistance;


float4 _OutlineColor; //change all references of this to whatever the shadow color will be
float _OutlineWidth; 
float _OutlineType; //circle or square
float _OutlineSamples; //taps that are sampled...
//int _ExpansionEffectActive; //only run extra steps if using
//int _UseOutline; //0 = dropshadow mode, 1 = outline mode (changes expansion rules as well)
//int _ExpansionEffect;

float _Effect; //0 = none, 1 = dropshadow mode, 2 = outline

static const float doublepi = 6.28318530718;

static const float2 quadOffset[4] = {
    float2(-1,1),
    float2(1,1),
    float2(1,-1),
    float2(-1,-1)
};



float2 ratio(float2 r)
{
    return float2(1.0,r.y/r.x);
}



void expandForEffects(inout appdata v, uint id, out float4 dist, out float2 qScale){

    float texelFix;
    if(_MainTex_TexelSize.z < _MainTex_TexelSize.w)
    {
        texelFix = _MainTex_TexelSize.z / 8;
    }
    else
    {
        texelFix = _MainTex_TexelSize.z / 64;
    }
    
    float2 shadowOffset;
    if(_DropshadowType == 0)
    {
        shadowOffset = float2(sin((_DropshadowAngle / 360.0)*doublepi),cos((_DropshadowAngle / 360.0)*doublepi)) * _DropshadowDistance * texelFix / 128 ;
    }
    else
    {
        shadowOffset.x = -_DropshadowAngle2.x * texelFix / 128;
        shadowOffset.y = _DropshadowAngle2.y * texelFix / 128;
    }

    float2 os = 0;

    //kai: this seems to fix it...? for dropshadows
    if(_MainTex_TexelSize.w > _MainTex_TexelSize.z) //if the texture is taller than it is wide
    {
        os.x = shadowOffset.x * _MainTex_TexelSize.x * 2;// * _MaskTex_TexelSize.xy;
        os.y = shadowOffset.y * _MainTex_TexelSize.y * 8;
    }
    else
    {
        os = shadowOffset * _MainTex_TexelSize.xy * 16;// * _MainTex_TexelSize.xy * 8;// * _MaskTex_TexelSize.xy;
    }

    //WARNING: 8 IS ARBITRARY, NEED TO REPLACE TO ADJUST TO DIFFERENT SIZES

    //float2 ratio = v.uv3.xy;
    //ok so turns out that it's better to not scale via ratio.
    //By uniformly expanding the verts, we get enough extra space
    //for making outlines. Using ratios would cause any tall or
    //short (non-square) letters to have disproportionate outline area

    //v.uv3.xy can be retired now.


    float2 adj = quadOffset[(id)%4];

    
    qScale = v.uv4.yy; //grab size of text
    bool upsideDown = qScale.x < 0;
    if(upsideDown)
    {
        qScale = -qScale;
    }
    
    float2 quality = v.uv4.zw; //grab quality of text atlas
    //os *= quality; //adjust vertex offset for quality
    qScale /= quality; //divide size by quality




   
    

    float2 scale = 0;
    

    //os /= qScale;



    if(_Effect==1)
    //minimize the number of pixels we raster onto
    //by only scaling in the direction of the shadow
    {

        //dropshadow expansion





        if(sign(os.x) == sign(-adj.x))
        scale.x = abs(os.x)*8;
        if(sign(os.y) == sign(adj.y))
        scale.y = abs(os.y)*8;

            
        //scale.y *= _MainTex_TexelSize.x/_MainTex_TexelSize.y;


        scale *= ratio(_MainTex_TexelSize.xy);
        scale *=8;



        //}

    }
    //not sure if it's much faster to run all this
    //or instead just rasterize across more pixels,
    //but these only run once per vert.

    //this only works for simple dropshadow though,
    //so for uniform expansion, we can use this
    else
    if(_Effect==2)
    {
        //outline expansion

        os = _OutlineWidth * texelFix / 128  * min(_MainTex_TexelSize.x,_MainTex_TexelSize.y) * 16;
        
        scale = abs(os)*8; //kai: not perfect but changing this from 4 to 16 keeps it ahead of the outline
        //scale.y *= _MainTex_TexelSize.x/_MainTex_TexelSize.y;
        scale *= ratio(_MainTex_TexelSize.xy);
        scale *= 8;

    }
    //scale = abs(shadowOffset.y)*4; //uniform scaling
    //replace shadowOffset with what ever value we want to scale up by.
    

    //scale *= qScale; //adjust arbitrary scaling by size of text quad
    //this allows our quad mods to maintain relative scale with large/small text
    

    v.vertex.xy += adj * scale; //expand each quad around it's center


    if(abs(v.uv4.x)==1)//if rotated,
    {
        adj = adj.yx; //rotate our offsets for UV calculation
        scale = scale.yx;
    }
    adj.x*=-1;
    if(upsideDown)
    {
        adj.y*=-1;
    }
    //flip x (or y) of our offset since it doesn't behave if I don't

    //Could probably clean up this multiplication bit but not sure how yet.
    //Should really figure out what's happening here sooner or later...

    v.uv_MainTex.xy -= v.uv3.zw; //center UVs around (0,0) for scaling
    

    float2 ogUV = v.uv_MainTex.xy; //store this unscaled centered UV for clipping later


    v.uv_MainTex.xy -= adj / qScale * scale / 4;
    //also apply to mask
    v.uv2_MaskTex.xy -= adj / qScale * scale / 4;


    dist.xy = v.uv_MainTex.xy; //send scaled centered UV to frag (not abs yet since we need per-pixel interpolated data to compare)

    dist.zw = abs(ogUV) * sign (.5-v.uv4.x); 
    //send abs of unscaled centered UV to frag (abs since we only need the constant scale on all pixels)
    //also multiplided it by a sign to cheaply transfer over the rotation state
    //We could simplify this step by making a float2 in the V2F struct with
    //the nointerpolation tag (nointerpolation float2 ogUV : TEXCOORD3;) and send the raw data.
    //However, this isn't compatible with all of Unity's shader compilers
    //despite it being supported in most graphics platforms.

    v.uv_MainTex.xy += v.uv3.zw;//move UV back to letter space
    //v.uv2_MaskTex.xy += v.uv3.zw;
    if(upsideDown)
    {
        //flip BACK so this info can be used by the dropshadow/outline!
        qScale = -qScale;
    }
}







v2f vert (appdata v, uint id : SV_VERTEXID)
{
    v2f o;

    if(_Effect!=0)
        expandForEffects(v, id, o.dist, o.scale);
    else
    {
        //still have to set the full struct
        //to prevent compilation errors.
        o.dist = 0;
        //not sure if this abs is needed
        o.scale = abs(v.uv4.yy)/v.uv4.zw;
    }
    
    o.quality = v.uv4.zw;


    //single-pass stereo rendering:
    #if defined(UNITY_STEREO_INSTANCING_ENABLED)
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    #endif
    #if UNITY_VERSION < 540
    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); //UNITY_SHADER_NO_UPGRADE
    #else
    o.vertex = UnityObjectToClipPos(v.vertex);
    #endif

    o.color = v.color;
    o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
    o.uv2_MaskTex = TRANSFORM_TEX(v.uv2_MaskTex, _MaskTex);
    #ifdef PIXELSNAP_ON
    o.vertex = UnityPixelSnap(o.vertex);
    #endif
    return o;
}












//fragment functions

float offsetClip (float4 dist){
    return step(0,abs(dist.z)-abs(dist.x)) * step(0,abs(dist.w)-abs(dist.y));
}

void outlineDraw(inout fixed4 col, v2f i, inout float zed, int dropShadowMode)
{
    //only run if text alpha isn't fully opaque
    if(col.a!=1.0)
    {



        zed -= .05 * (1.0-col.a);
        //adjust depth to avoid drawing over other letters


        fixed4 droptext = 0;
        fixed4 dropMask = 0;

        float4 outline = float4(_OutlineColor.rgb,0);


        //float2 os = float2(sin((_DropshadowAngle/360.0)*doublepi), cos((_DropshadowAngle/360.0)*doublepi)) * _DropshadowDistance * texelFix / 128  * _MainTex_TexelSize.y * 4 * (max(i.scale.x,i.scale.y)/min(i.scale.x,i.scale.y)) * i.quality;
        bool upsideDown = i.scale.y < 0;
        if(upsideDown)
        {
            i.scale.y = -i.scale.y;
            i.scale.x = -i.scale.x;
        }
        float texelFix;
        if(_MainTex_TexelSize.z < _MainTex_TexelSize.w)
        {
            texelFix = _MainTex_TexelSize.z / 8;
        }
        else
        {
            texelFix = _MainTex_TexelSize.z / 64;
        }
        //outline samples = taps
        for(int n = 0; n < _OutlineSamples; n++)
        {
            
            //float circscale = _OutlineWidth * texelFix / 128  * _MainTex_TexelSize.y * 256 * (min(i.scale.x,i.scale.y)/max(i.scale.x,i.scale.y)) * ratio(i.quality) /  max(i.scale.x,i.scale.y);
            float2 circ = float2(sin((float(n)/(_OutlineSamples))*doublepi), cos((float(n)/(_OutlineSamples))*doublepi)) * _OutlineWidth * texelFix / 128  * _MainTex_TexelSize.y * 256 * (min(i.scale.x,i.scale.y)/max(i.scale.x,i.scale.y)) * ratio(i.quality) /  max(i.scale.x,i.scale.y);

            if(_OutlineType > 0)
            {
                circ = clamp(circ*2, -_OutlineWidth * texelFix / 128  * _MainTex_TexelSize.y * 256 * (min(i.scale.x,i.scale.y)/max(i.scale.x,i.scale.y)) * ratio(i.quality) /  max(i.scale.x,i.scale.y),
                    _OutlineWidth * texelFix / 128  * _MainTex_TexelSize.y * 256 * (min(i.scale.x,i.scale.y)/max(i.scale.x,i.scale.y)) * ratio(i.quality) /  max(i.scale.x,i.scale.y));
            }
            circ/=2; //keep it even with outlines (why did this break? find out later)
            if(i.dist.z<.0)
            {
                if(_MainTex_TexelSize.w > _MainTex_TexelSize.z) //if the texture is taller than it is wide
                {
                    //kai: this seems to correct for over-shaped offset
                    circ.x /= 2;
                    circ.y *=2;
                }
                circ=circ.yx; 
                circ.x = -circ.x; //flip sideways uvs
            }
            if(upsideDown)
            {
                circ.y = -circ.y;
            }
            droptext = tex2Dlod(_MainTex, float4(i.uv_MainTex.xy + circ,  0, 0));
            dropMask = tex2Dlod(_MaskTex, float4(i.uv2_MaskTex.xy + circ, 0, 0));
        
        
            //float4 shadowCol = float4(dropMask.rgb * i.color.rgb, droptext.a * dropMask.a * i.color.a) * _DropshadowColor;
            float outlineAlpha = droptext.a * dropMask.a * i.color.a;
            //SHEEPISH COMMENT: Old line multiplies shadow color by base color, making some combinations impossible (IE red & green)
            //and means the shadow can never be brighter than the base. Was that intentional? Commented out for now.
            outlineAlpha *= offsetClip(i.dist + float4(circ, 0,0));

            //outline = lerp(outline, _OutlineColor, outlineAlpha);
            //lerp from alpha/zero to ShadowColor as we sample along the circle & succeed
            //just kidding: new method
            outline.a = max(outline.a, outlineAlpha);
        }
        //max

        //don't use this one, creates a gap in HDRP:
        //col=lerp(saturate(outline), col, (col.a));
        //instead...
        //col=float4(lerp(saturate(outline), col, (col.a)).rgb, max(col.a,outline.a));
        outline.rgb *= lerp(1.0,col.rgb,saturate(col.a));
        outline.a *= lerp(_OutlineColor.a, outline.a, col.a);
        if(col.a < outline.a)
        {
            col=float4(lerp(saturate(outline), col, col.a).rgb, max(col.a,outline.a));
        }
        //col = max(col, outline);

        if(col.a<=0)
        discard;
        //since zwrite is on, we want to discard pixels so as to not
        //occlude anything with this quad's presence in depth buffer

        //cutoff excess letters with offset
    }

}




void dropShadow(inout fixed4 col, v2f i, inout float zed)
{
    if(col.a!=1.0)
    {
        //float2 circ = float2(sin(((_DropshadowAngle)/360.0)*doublepi), cos(((_DropshadowAngle)/360.0)*doublepi)) * _MainTex_TexelSize.x * 4 * _OutlineWidth * texelFix / 128  * i.quality /i.scale.x;
        //float2 shadowOffset = float2(sin((_DropshadowAngle / 360.0)*doublepi),cos((_DropshadowAngle / 360.0)*doublepi)) * _DropshadowDistance * texelFix / 128 ;
        //float2 os = shadowOffset * sign(i.dist.z) * _MainTex_TexelSize.x * 4 *(i.scale.y/i.scale.x) * i.quality / i.scale.x;
        // * _MainTex_TexelSize.x * 4 * _ShadowDistance * i.quality /i.scale.x
        float texelFix;
        bool upsideDown = i.scale.y < 0;
        if(upsideDown)
        {
            i.scale.y = -i.scale.y;
            i.scale.x = -i.scale.x;
        }
        if(_MainTex_TexelSize.z < _MainTex_TexelSize.w)
        {
            texelFix = _MainTex_TexelSize.z / 8;
        }
        else
        {
            texelFix = _MainTex_TexelSize.z / 64;
        }
        float2 os;
        if(_DropshadowType == 0)
        {
            os = float2(sin((_DropshadowAngle/360.0)*doublepi), cos((_DropshadowAngle/360.0)*doublepi)) * _DropshadowDistance * texelFix / 128  * _MainTex_TexelSize.y * 256 * (min(i.scale.x,i.scale.y)/max(i.scale.x,i.scale.y)) * ratio(i.quality) /  max(i.scale.x,i.scale.y);  
        }
        else
        {
            float angle = atan2(-_DropshadowAngle2.x, _DropshadowAngle2.y);
            float distance = length(_DropshadowAngle2);
            os = float2(sin((angle)), cos((angle))) * distance * texelFix / 128  * _MainTex_TexelSize.y * 256 * (min(i.scale.x,i.scale.y)/max(i.scale.x,i.scale.y)) * ratio(i.quality) /  max(i.scale.x,i.scale.y); 
        }
        os /= 2; //stay consistent with outline
        
        if(upsideDown) //upside-down?
        {
            os.y = -os.y;
        }

        /*
         *kai: thought this would work but it doesnt
        if(_MainTex_TexelSize.w > _MainTex_TexelSize.z) //if the texture is taller than it is wide
        {
            os=os.yx;
            os.x = -os.x; //flip sideways uvs
            os.y = -os.y;
            //os.y *= 16;
        }
        */
        if(i.dist.z<.0) //sideways
        {
            if(_MainTex_TexelSize.w > _MainTex_TexelSize.z) //if the texture is taller than it is wide
            {
                //kai: this seems to correct for over-shaped offset
                os.x /= 2;
                os.y *=2;
            }
            os=os.yx;
            os.x = -os.x; //flip sideways uvs
            os.y = -os.y;
        }
        zed -= .05 * (1.0-col.a);

        //if we're only doing this once

        fixed4 dropText = tex2D(_MainTex, i.uv_MainTex + os);
        fixed4 dropMask = tex2D(_MaskTex, i.uv2_MaskTex.xy + os);
        float4 shadow = float4(_DropshadowColor.rgb,0);

        
        //float4 shadowCol = float4(dropMask.rgb * i.color.rgb, dropText.a * dropMask.a * i.color.a) * _DropshadowColor;
        //SHEEPISH COMMENT: Old line multiplies shadow color by base color (i.color), making some combinations impossible (IE red & green)
        //and also means the shadow can never be brighter than the base. Was that intentional? Commented out for now, replaced with below.
        //float4 shadowCol = float4(dropMask.rgb, dropText.a * dropMask.a * i.color.a) * _DropshadowColor;

        float shadowAlpha = dropText.a * dropMask.a * i.color.a;
        shadowAlpha *= offsetClip(i.dist + float4(os, 0,0));
        //shadowCol.a*=saturate(offsetClip(i.dist + float4(os.xy, 0,0)));
        //shadow = lerp(shadow, _DropshadowColor, shadowAlpha);
        shadow.a = max(shadow.a, shadowAlpha);

        shadow.rgb *= lerp(1.0,col.rgb,saturate(col.a));

        //shadow.a *= _DropshadowColor.a; //this works but looks jank
        shadow.a *= lerp(_DropshadowColor.a, shadow.a, col.a);
        
        if(col.a < shadow.a)
        {
            col=float4(lerp(saturate(shadow), col, col.a).rgb, max(col.a, shadow.a));
            
        }

        
        if(col.a<=0)
        discard;
        //since zwrite is on, we want to discard pixels so as to not
        //occlude anything with this quad's presence in depth buffer

        //cutoff excess letters with offset
    }

}











float4 when_lt(float4 x, float4 y) {
    return max(sign(y - x), 0.0);
}

float4 when_ge(float4 x, float4 y) {
    return 1.0 - when_lt(x, y);
}

inline float LinearEyeDepthToOutDepth(float z)
{
    return (1 - _ZBufferParams.w * z) / (_ZBufferParams.z * z);
}



//render normal text
fragOut frag(v2f i)
{
    fragOut o;


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

    
    col.a *= offsetClip(i.dist);
    //stop other letters on the atlas from rendering

    float zed = 0;
    //used to offset depth

    if(_Effect!=0)
    {
        if(_Effect==1)
            dropShadow(col, i, zed);
        else
        if(_Effect==2)
            outlineDraw(col, i, zed, _Effect-1);

        zed -= (1.0 - col.a)*.1;
    }

    o.depth = LinearEyeDepthToOutDepth(LinearEyeDepth(i.vertex.z) - zed * .01);


    //If rendering on top & using expansion effects (dropshadows/outlines),
    //do not use ZTest always!

    //Instead, set the depth to the front & use ZTest Less.
    //This allows for the same functionality, while preventing
    //the expansion effects from drawing over other text faces.

    //o.depth = 1 + zed * .01;//1.0/(_ZBufferParams.x*(i.vertex.w-zed)+_ZBufferParams.y);




    //calculate base depth

    o.col = col;
    
    clip(col.a - _Cutoff);



    return o;
}