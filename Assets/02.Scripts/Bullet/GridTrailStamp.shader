Shader "Hidden/GridTrailStamp"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            float4 _StampPosition; float _StampSize; float _StampStrength; float _Fade;
            struct Attributes { uint vertexID : SV_VertexID; };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; };
            Varyings Vert(Attributes input)
            {
                Varyings output;
                float2 uv = float2((input.vertexID << 1) & 2, input.vertexID & 2);
                output.uv = uv;
                output.positionCS = float4(uv * 2.0 - 1.0, 0.0, 1.0);
                return output;
            }
            half4 Frag(Varyings input) : SV_Target
            {
                float distanceFromStamp = length(input.uv - _StampPosition.xy);
                float radius = max(_StampSize, 0.0001);
                float stamp = 1.0 - smoothstep(radius * 0.15, radius, distanceFromStamp);
                float oldValue = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).r;
                return saturate(max(oldValue * _Fade, stamp * _StampStrength));
            }
            ENDHLSL
        }
    }
}
