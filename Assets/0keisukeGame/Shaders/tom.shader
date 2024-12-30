Shader "Custom/TOM"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Scale", Range(0, 2)) = 1

        _LimShadeColor1 ("リム陰の色 ベース", Color) = (0,0,0,1)
        _LimShadeColorWeight1 ("リム陰色の影響度 ベース", Range(0, 1)) = 0.5
        _LimShadeMinPower1 ("リム陰のグラデ範囲 ベース", Range(0, 1)) = 0.3
        _LimShadePowerWeight1 ("最濃リム陰の太さ ベース", Range(1, 10)) = 10

        _LimShadeColor2 ("リム陰の色 外側", Color) = (0,0,0,1)
        _LimShadeColorWeight2 ("リム陰色の影響度 外側", Range(0, 1)) = 0.8
        _LimShadeMinPower2 ("リム陰のグラデ範囲 外側", Range(0, 1)) = 0.3
        _LimShadePowerWeight2 ("最濃リム陰の太さ 外側", Range(1, 10)) = 2

        _LimShadeMaskMinPower ("リム陰マスクのグラデ範囲", Range(0, 1)) = 0.3
        _LimShadeMaskPowerWeight ("最濃リム陰マスクの太さ", Range(1, 10)) = 2

        _LimLightWeight ("リムライトの影響度", Range(0, 1)) = 0.5
        _LimLightPower ("リムライトのグラデ範囲", Range(1, 5)) = 3

        _AmbientColor ("Ambient Color", Color) = (0.5,0.5,0.5,1)

        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _SpecularRate ("スペキュラーの影響度", Range(0, 1)) = 0.3

        _OutlineWidth ("Outline Width", Range(0, 1)) = 0.1
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
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
            // 前面をカリング
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                half4 vertex : POSITION;
                half3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                half4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;

            half _OutlineWidth;
            half4 _OutlineColor;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;

                // アウトラインの分だけ法線方向に拡大する
                o.vertex = TransformObjectToHClip(v.vertex + v.normal * (_OutlineWidth / 100));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                return col * _OutlineColor;
            }
            ENDHLSL
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

             #include "Custom.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float fogFactor: TEXCOORD1;
                float4 vertex : SV_POSITION;

                float3 normal : NORMAL;
                float2 uvNormal : TEXCOORD2;
                float4 tangent  : TANGENT;
                float3 binormal : TEXCOORD3;

                float3 viewDir : TEXCOORD4;

                float3 toEye : TEXCOORD5;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;

            float4 _BumpMap_ST;
            float _BumpScale;

            float3 _LimShadeColor1;
            float _LimShadeColorWeight1;
            float _LimShadeMinPower1;
            float _LimShadePowerWeight1;

            float3 _LimShadeColor2;
            float _LimShadeColorWeight2;
            float _LimShadeMinPower2;
            float _LimShadePowerWeight2;

            float _LimShadeMaskMinPower;
            float _LimShadeMaskPowerWeight;

            float _LimLightPower;
            float _LimLightWeight;

            float3 _AmbientColor;

            float _Smoothness;
            float _SpecularRate;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(o.vertex.z);

                o.normal = TransformObjectToWorldNormal(v.normal);
                o.uvNormal = TRANSFORM_TEX(v.uv, _BumpMap);
                o.tangent = v.tangent;
                o.tangent.xyz = TransformObjectToWorldDir(v.tangent.xyz);
                o.binormal = normalize(cross(v.normal, v.tangent.xyz) * v.tangent.w * unity_WorldTransformParams.w);

                o.viewDir = normalize(-GetViewForwardDir());

                o.toEye = normalize(GetWorldSpaceViewDir(TransformObjectToWorld(v.vertex.xyz)));
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // ノーマルマップから法線情報を取得する
                float3 localNormal = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.uvNormal), _BumpScale);
                // タンジェントスペースの法線をワールドスペースに変換する
                i.normal = i.tangent * localNormal.x + i.binormal * localNormal.y + i.normal * localNormal.z;

                float4 albedo = col;

                // 陰1の計算をする
                float limPower = 1 - max(0, dot(i.normal, i.viewDir));
                float limShadePower = inverseLerp(_LimShadeMinPower1, 1, limPower);
                limShadePower = min(limShadePower * _LimShadePowerWeight1, 1);
                col.rgb = lerp(col.rgb, albedo.rgb * _LimShadeColor1, limShadePower * _LimShadeColorWeight1);

                // 陰2の計算をする
                limShadePower = inverseLerp(_LimShadeMinPower2, 1, limPower);
                limShadePower = min(limShadePower * _LimShadePowerWeight2, 1);
                col.rgb = lerp(col.rgb, albedo.rgb * _LimShadeColor2, limShadePower * _LimShadeColorWeight2);

                // 陰のマスク
                float limShadeMaskPower = inverseLerp(_LimShadeMaskMinPower, 1, limPower);
                limShadeMaskPower = min(limShadeMaskPower * _LimShadeMaskPowerWeight, 1);
                col.rgb = lerp(col.rgb, albedo.rgb, limShadeMaskPower);

                // リムライト
                Light light = GetMainLight();
                float limLightPower= 1 - max(0, dot(i.normal, -light.direction));
                float3 limLight = pow(saturate(limPower * limLightPower), _LimLightPower) * light.color;
                col.rgb += limLight * _LimLightWeight;

                // Half-Lambert拡散反射光
                float3 diffuseLight = CalcHalfLambertDiffuse(light.direction, light.color, i.normal);
                float shinePower = lerp(0.5, 10, _Smoothness);
                float3 specularLight = CalcPhongSpecular(-light.direction, light.color, i.toEye, i.normal, shinePower);
                specularLight = lerp(0, specularLight, _SpecularRate);
                col.rgb *= diffuseLight + _AmbientColor + specularLight;

                // apply fog
                col.rgb = MixFog(col.rgb, i.fogFactor);
                return col;
            }
            ENDHLSL
        }
    }
}