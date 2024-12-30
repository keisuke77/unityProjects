Shader "Custom/HalfTone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HalftoneScale ("Halftone Scale", Range(0.001, 0.1)) = 0.02
        _ShadeColor ("Shade Color", Color) = (0.5, 0.5, 0.5, 1)
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _LightColor ("Light Color", Color) = (1, 1, 1, 1) // Default to white
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
                float3 normal : NORMAL;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float _HalftoneScale;
            float4 _ShadeColor;
            float4 _MainColor;
            float4 _LightColor; // Adding the LightColor
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.normal = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // If the texture appears white, use only the main color
                if (col.r == 1 && col.g == 1 && col.b == 1 && col.a == 1)
                {
                    col = _MainColor;
                }
                else
                {
                    col *= _MainColor;
                }

                // Compute lighting
                float2 screenPos = i.screenPos.xy / i.screenPos.w;
                float aspect = _ScreenParams.x / _ScreenParams.y;
                float2 cellSize = float2(_HalftoneScale, _HalftoneScale * aspect);
                float2 cellCenter;
                cellCenter.x = floor(screenPos.x / cellSize.x) * cellSize.x + cellSize.x / 2;
                cellCenter.y = floor(screenPos.y / cellSize.y) * cellSize.y + cellSize.y / 2;
                float2 diff = screenPos - cellCenter;
                diff.x /= cellSize.x;
                diff.y /= cellSize.y;
                Light light = GetMainLight();
                float threshold = 1 - dot(i.normal, light.direction);

                col.rgb *= lerp(_LightColor.rgb, _ShadeColor.rgb, step(length(diff), threshold));

                return col;
            }
            ENDHLSL
        }
    }
}
