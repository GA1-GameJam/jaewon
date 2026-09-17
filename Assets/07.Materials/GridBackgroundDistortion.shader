Shader "Custom/GridBackgroundDistortion"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _GridTexture ("Grid Texture", 2D) = "white" {}
        _GridColor ("Grid Color", Color) = (1,0,0,0.4)
        _GroundColor ("Ground Color", Color) = (0,0,0,0)
        _Tiling ("Tiling", Vector) = (9,5,0,0)
        _GridSpeed ("Grid Speed", Vector) = (-0.05,0,0,0)
        _GridDistortionTex ("Grid Trail", 2D) = "black" {}
        _DistortionStrength ("Distortion Strength", Range(0,1)) = 0.35
        _TrailInfluence ("Trail Influence", Range(0,2)) = 0.8
        _GridRotation ("Grid Rotation", Range(0,360)) = 180
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "CanUseSpriteAtlas"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_GridTexture); SAMPLER(sampler_GridTexture);
            TEXTURE2D(_GridDistortionTex); SAMPLER(sampler_GridDistortionTex);
            float4 _GridTrailTexelSize;
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST; float4 _GridTexture_ST;
                float4 _GridColor, _GroundColor, _Tiling, _GridSpeed;
                float _DistortionStrength, _TrailInfluence, _GridRotation;
            CBUFFER_END
            struct Attributes { float3 positionOS:POSITION; float4 color:COLOR; float2 uv:TEXCOORD0; };
            struct Varyings { float4 positionCS:SV_POSITION; float4 color:COLOR; float2 uv:TEXCOORD0; };
            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color;
                return output;
            }
            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                float2 texel = _GridTrailTexelSize.xy;
                float trail = SAMPLE_TEXTURE2D(_GridDistortionTex, sampler_GridDistortionTex, uv).r;
                float trailX = SAMPLE_TEXTURE2D(_GridDistortionTex, sampler_GridDistortionTex, uv + float2(texel.x,0)).r - SAMPLE_TEXTURE2D(_GridDistortionTex, sampler_GridDistortionTex, uv - float2(texel.x,0)).r;
                float trailY = SAMPLE_TEXTURE2D(_GridDistortionTex, sampler_GridDistortionTex, uv + float2(0,texel.y)).r - SAMPLE_TEXTURE2D(_GridDistortionTex, sampler_GridDistortionTex, uv - float2(0,texel.y)).r;
                float2 distortion = float2(trailX, trailY);
                uv += distortion * _DistortionStrength * (1.0 + trail * 2.0);
                float2 gridUV = uv * _Tiling.xy + _Time.y * _GridSpeed.xy;
                float angle = radians(_GridRotation);
                float2 rotation = float2(cos(angle), sin(angle));
                gridUV = float2(gridUV.x * rotation.x - gridUV.y * rotation.y, gridUV.x * rotation.y + gridUV.y * rotation.x);
                half grid = SAMPLE_TEXTURE2D(_GridTexture, sampler_GridTexture, gridUV).r;
                half3 color = lerp(_GroundColor.rgb, _GridColor.rgb, grid);
                half alpha = saturate(lerp(_GroundColor.a, _GridColor.a, grid) + trail * _TrailInfluence);
                return half4(color, alpha) * input.color;
            }
            ENDHLSL
        }
    }
}
