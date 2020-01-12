Shader "Custom/LobbyBackground"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_NoiseTex ("Noise", 2D) = "white" {}
		_NoiseSpeed ("Noise Speed", Float) = 1
		_NoiseAmplitude ("Noise Amplitude", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:noiseDisplacement
        #pragma target 3.0

        sampler2D _NoiseTex;

        struct Input
        {
            float2 uv_NoiseTex;
        };

        fixed4 _Color;
		float _NoiseSpeed;
		float _NoiseAmplitude;
        half _Glossiness;
        half _Metallic;

		void noiseDisplacement(inout appdata_full v) {
			v.vertex.xyz += sin(_NoiseSpeed * (_Time.y + tex2Dlod(_NoiseTex, v.texcoord1).r * 6.28318531)) * _NoiseAmplitude; // 6.28318531 = pi * 2
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
