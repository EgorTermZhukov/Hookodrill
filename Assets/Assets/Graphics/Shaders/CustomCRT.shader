Shader "CustomRenderTexture/CustomCRT"
{
   Properties
    {
        [PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.1
        _NoiseScale ("Noise Scale", Float) = 50.0
        _NoiseSpeed ("Noise Speed", Float) = 10.0
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.08
        _ScanlineDensity ("Scanline Density", Float) = 200.0
        _ScanlineSpeed ("Scanline Speed", Float) = 10.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenUV : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _NoiseIntensity;
            float _NoiseScale;
            float _NoiseSpeed;
            float _ScanlineIntensity;
            float _ScanlineDensity;
            float _ScanlineSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenUV = ComputeScreenPos(o.vertex);
                return o;
            }

            float random (float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898,78.233))) * 43758.5453123);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // Calculate screen UV coordinates
                float2 screenUV = i.screenUV.xy / i.screenUV.w;
                
                // Generate animated noise
                float noise = random(screenUV * _NoiseScale + _Time.y * _NoiseSpeed);
                noise = (noise * 2.0 - 1.0) * _NoiseIntensity;
                col.rgb += noise;
                
                // Create moving scanlines
                float scanline = sin(screenUV.y * _ScanlineDensity + _Time.y * _ScanlineSpeed);
                scanline = 1.0 - (scanline * 0.5 + 0.5) * _ScanlineIntensity;
                col.rgb *= scanline;
                
                // Ensure valid color range
                col.rgb = clamp(col.rgb, 0.0, 1.0);
                
                return col;
            }
            ENDCG
        }
    }
}
