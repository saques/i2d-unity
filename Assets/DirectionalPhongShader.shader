// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

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
                float4 vertex : POSITION;
				float3 vertexWorld :TEXCOORD3;
                float3 normal : NORMAL;
                float4 shadowCoords: TEXCOORD4;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AmbientColor;
            float _SpecularExponent;
            sampler2D _ShadowMap;
            float4x4 _lightProjectionMatrix;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.shadowCoords = mul(_lightProjectionMatrix, float4(o.vertexWorld, 1));
                o.normal = v.normal;
                return o;
            }

            float2 poissonDisk[4] = {
                float2( -0.94201624, -0.39906216 ),
                float2( 0.94558609, -0.76890725 ),
                float2( -0.094184101, -0.92938870 ),
                float2( 0.34495938, 0.29387760 )
            };
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
                float visibility = 1;
                float bias = clamp(0.005 * tan(acos(clamp(nDotL, 0, 1))), 0, 0.01);
                // float bias = 0.005;
                for (int c = 0; c < 4; c++) {
                    float shadowProjectionDepth = 1 - tex2Dproj(_ShadowMap, float4(i.shadowCoords.xy + poissonDisk[c] / 700.0, i.shadowCoords.zw)).r;
                    if (shadowProjectionDepth < i.shadowCoords.z - bias) {
                        visibility -= 0.2;
                    }
                }
                return visibility *  (diff + spec + _AmbientColor);
            }
            ENDCG
        }
    }
}
