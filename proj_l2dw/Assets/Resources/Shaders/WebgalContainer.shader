Shader "Webgal/WebgalContainer"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _Alpha ("Alpha", Range(0, 1)) = 1
        [Toggle(_BLUR_FILTER)] _BlurFilter ("BlurFilter", Float) = 0
        _Blur ("Blur", Float) = 0
        _BlurSampleScaleX ("BlurSampleScaleX", Float) = 1
        _BlurSampleScaleY ("BlurSampleScaleY", Float) = 1
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
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
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
