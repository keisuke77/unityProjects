Shader "Custom/RimLight" {
    Properties {
        _Color ("Color", Color) = (0, 0, 0)
        _RimColor ("RimColor", Color) = (1, 1, 1)
    }
    SubShader {
      
 Tags { "LightMode"="ForwardBase"
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"
        }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                half3 viewDir : TEXCOORD2;
            };

            float4 _Color;
            float4 _RimColor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // リムライトの計算。法線と視線のベクトルの内積を取り
                // 0~1の範囲で外側ほどリムライトの色が付くようにしている
                half rimRate = 1 - dot(i.normal, i.viewDir);
                return lerp(_Color, _RimColor, rimRate);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

