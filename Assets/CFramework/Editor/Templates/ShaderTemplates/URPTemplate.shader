//#TIME
Shader "Custiom_URP/#URPBaseShader"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _TestFloat("TestFloat",float)=1
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "LightMode" = "UniversalForward" 
            }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //replace Unity.cginc
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //replace AutoLight.cginc
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float _TestFloat;

            CBUFFER_END
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv=v.uv;
                //o.vertex = TransformObjectToWorld(v.vertex.xyz);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.uv);
                return col;
            }
            ENDHLSL
        }
    }
}
//UNITY_MATRIX_M     replace: unity_ObjectToWorld
//UNITY_MATRIX_I_M   replace: unity_WorldToObject
//UNITY_MATRIX_V     replace: unity_MatrixV
//UNITY_MATRIX_I_V   replace: unity_MatrixInvV
//UNITY_MATRIX_VP    replace: unity_MatrixVP
//UNITY_MSTRIX_I_VP  replace: unity_MatrixInvVP