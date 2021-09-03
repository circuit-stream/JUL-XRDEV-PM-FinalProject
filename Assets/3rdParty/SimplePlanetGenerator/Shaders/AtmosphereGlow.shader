Shader "Custom/AtmosphereGlow" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_Color ("Color", Color) = (1,1,1,0)
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Glow Opacity", Range(1.0, 20.0)) = 10.0
	}
	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "ForceNoShadowCasting"="True"}


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float4 color : Color;
			float3 viewDir;
			float2 uv_MainTex;
		};

		half _Glossiness;
        half _Metallic;

		float4 _Color;
		float4 _RimColor;
		float _RimPower;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			IN.color = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = IN.color.rgb;
			o.Alpha = IN.color.a;

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		}
		ENDCG
	}
    FallBack "Standard"


}
