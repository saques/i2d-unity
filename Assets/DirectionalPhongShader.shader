Shader "Unlit/DirectionalPhongShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AmbientColor ("Ambient Color", Color) = (0.5, 0.5, 0.5, 1)
        _SpecularExponent ("Specular Component", float) = 20
        _ShadowMap ("Shadow Map", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "Lightmode" = "ForwardBase"}
            CGPROGRAM
            // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members normal)
            // #pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityShaderVariables.cginc"

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
				float3 vertexWorld :TEXCOORD1;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AmbientColor;
            float _SpecularExponent;
            sampler2D _ShadowMap;
            uniform float4x4 unity_WorldToLight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // PHONG
                fixed4 col = tex2D(_MainTex, i.uv);
                float4 l = _WorldSpaceLightPos0;
                float3 n = i.normal;
                float nDotL = dot(n, l);
                float4 diff =  max(nDotL, 0) * col;
                float3 r = -l + 2 * nDotL * n;
                float3 worldPos = i.vertexWorld;
                float3 v = normalize(_WorldSpaceCameraPos - worldPos);
                float4 spec = pow(max(dot(r, v), 0), _SpecularExponent) * col;

                // SHADOW MAPPING
                float4 shadowPos = mul(unity_WorldToLight, float4(worldPos, 1));
				float2 shadowUv = shadowPos.xy;
                if (tex2D(_ShadowMap, shadowUv).r <= (shadowPos.z / shadowPos.w) + 0.02) {
                    return 0;
                }
                return (diff + spec + _AmbientColor);
            }
            ENDCG
        }
    }
}
