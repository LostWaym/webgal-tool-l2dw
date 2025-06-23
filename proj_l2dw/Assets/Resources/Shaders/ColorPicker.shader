Shader "UI/ColorPicker"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        [KeywordEnum(Sv_Rect, Hue_Bar, Dragger)] _Mode ("Mode", Float) = 0
        _Hue ("Hue", Range(0, 1)) = 0
        _Saturation ("Saturation", Range(0, 1)) = 0
        _Value ("Value", Range(0, 1)) = 1
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

            #pragma multi_compile _MODE_SV_RECT _MODE_HUE_BAR _MODE_DRAGGER

            #include "UnityCG.cginc"
            #include "Library/ColorUtils.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Hue;
            float _Saturation;
            float _Value;

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
#if _MODE_SV_RECT
                // col = fixed4(1.0, 0.0, 0.0, 1.0);
                col.rgb = HsvToRgb(_Hue, i.uv.x, i.uv.y);
#elif _MODE_HUE_BAR
                // col = fixed4(0.0, 1.0, 0.0, 1.0);
                col.rgb = HsvToRgb(i.uv.x, 1.0, 1.0);
#elif _MODE_DRAGGER
                // col = fixed4(0.0, 0.0, 1.0, 1.0);
                col.rgb = HsvToRgb(_Hue, _Saturation, _Value);

                float normalizedSphere = length(i.uv - float2(0.5, 0.5)) * 2.0;
                col.rgb = lerp(float3(1.0, 1.0, 1.0), col.rgb, smoothstep(0.80, 0.70, normalizedSphere));
                col.rgb = lerp(float3(0.0, 0.0, 0.0), col.rgb, smoothstep(0.90, 0.80, normalizedSphere));

                col.a = smoothstep(1.0, 0.90, normalizedSphere);
#endif
                
                return col;
            }
            ENDCG
        }
    }
}
