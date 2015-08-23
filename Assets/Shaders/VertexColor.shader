Shader "Custom/VertexColor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma fragmentoption ARB_precision_hint_fastest

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		    float4 color : COLOR;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
						
			half gloss = IN.color.r == 0 && IN.color.g == 0 && IN.color.b == 0 ? 0.8 : 0;

			o.Albedo = c.rgb * IN.color + gloss;

			o.Metallic = gloss;
			o.Smoothness = gloss;

			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
