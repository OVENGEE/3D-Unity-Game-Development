Shader "Custom/Blended Sky"
{
       Properties
    {
        _Tex1("Skybox 1", Cube) = "" {}
        _Tex2("Skybox 2", Cube) = "" {}
        _Blend("Blend", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _Tex1;
            samplerCUBE _Tex2;
            float _Blend;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 dir : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.dir = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 c1 = texCUBE(_Tex1, i.dir);
                fixed4 c2 = texCUBE(_Tex2, i.dir);
                return lerp(c1, c2, _Blend);
            }
            ENDCG
        }
    }

    FallBack "RenderFX/Skybox"
}

// Code references
// Title: Skybox transitions
//Author: Chatgpt
//Date accessed: 2025/11/09
//Code version:
// Availability: https://chatgpt.com/c/6910ec1a-7c94-832d-aa33-c0bece66e8b5

// Title: Unity3D Dynamic Weather Tutorial #29 Blended Skybox
//Author: Moon Cat Laboratory
//Date accessed: 2025/11/09
//Code version:
// Availability: https://www.youtube.com/watch?v=wC_4VoBvpQc
