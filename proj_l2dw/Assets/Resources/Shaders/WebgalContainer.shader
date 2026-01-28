Shader "Webgal/WebgalContainer"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        
        [KeywordEnum(Normal, Add, Multiply, Screen)] _BlendMode ("BlendMode", Float) = 0
        
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Float) = 10
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcAlphaBlend ("SrcAlphaBlend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstAlphaBlend ("DstAlphaBlend", Float) = 10
        
        _SampleScaleX ("SampleScaleX", Float) = 1
        _SampleScaleY ("SampleScaleY", Float) = 1
        
        _Alpha ("Alpha", Range(0, 1)) = 1
        [Toggle(_BLUR_FILTER)] _BlurFilter ("BlurFilter", Float) = 0
        _Blur ("Blur", Float) = 0
        
        _Brightness ("Brightness", Float) = 1
        _Contrast ("Contrast", Float) = 1
        _Saturation ("Saturation", Float) = 1
        _Gamma ("Gamma", Float) = 1
        _ColorRed ("ColorRed", Float) = 255
        _ColorGreen ("ColorGreen", Float) = 255
        _ColorBlue ("ColorBlue", Float) = 255
        
        _Bloom ("Bloom", Float) = 0
        _BloomBrightness ("BloomBrightness", Float) = 1
        _BloomBlur ("BloomBlur", Float) = 0
        [Toggle(_BLOOM_FILTER_BLUR)] _BloomFilterBlur ("BloomFilterBlur", Float) = 0
        _BloomThreshold ("BloomThreshold", Range(0, 1)) = 0
        
        [Toggle(_BEVEL_FILTER_LEGACY)] _BevelFilterLegacy ("BevelFilterLegacy", Float) = 0
        _Bevel ("Bevel", Range(0, 1)) = 0
        _BevelThickness ("BevelThickness", Float) = 0
        _BevelRotation ("BevelRotation", Float) = 0
        [Toggle(_BEVEL_FILTER_SOFTNESS)] _BevelFilterSoftness ("BevelFilterSoftness", Float) = 0
        _BevelSoftness ("BevelSoftness", Range(0, 1)) = 0
        _BevelRed ("BevelRed", Float) = 255
        _BevelGreen ("BevelGreen", Float) = 255
        _BevelBlue ("BevelBlue", Float) = 255
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100
        Blend [_SrcBlend] [_DstBlend], [_SrcAlphaBlend] [_DstAlphaBlend]
        ZWrite Off
        ZTest LEqual
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile _BLEND_MODE_NORMAL _BLEND_MODE_ADD _BLEND_MODE_MULTIPLY _BLEND_MODE_SCREEN

            #include "UnityCG.cginc"
            #include "Library/AlphaFilter.cginc"
            #include "Library/BlurFilter.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0.5, 0.8, 0.6 ,1);
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);

                // 应用滤镜
                col = ApplyBlurFilter(i.uv);
                col = ApplyAlphaFilter(col);

                // col.rgb = pow(col.rgb, 2.0);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                #if defined(_BLEND_MODE_MULTIPLY)
                    col.rgb = lerp(1, col.rgb, col.a);
                #elif defined(_BLEND_MODE_SCREEN)
                    col.rgb *= col.a;
                #endif
                
                return col;
            }
            ENDCG
        }
    }
}
