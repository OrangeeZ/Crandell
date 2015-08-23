Shader "Custom/Wave V2" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_NoiseTex ("Noise (RGB)", 2D) = "white" {}
		_Speed("Speed", Vector) = (1, 1, 1, 1)
		_XAmplitude("X Amplitude", float) = 0
		_XMoveSpeed("X Move speed", float) = 0
		_YAmplitude("Y Amplitude", float) = 0
		_Scale("Scale", Vector) = (1, 1, 1, 1)
		_Opacity("Opacity", Range(0, 1)) = 0.5
		_OpacityScale("Opacity scale", float) = 5
		_BlurAmount("Blur amount", int) = 2
	}
	SubShader {

		Tags { "RenderType"="Opaque" "Queue" = "Transparent+1" }
		LOD 200
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		
		CGPROGRAM
		#pragma surface surf Lambert keepalpha 

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		float4 _Speed;
		half _XAmplitude;
		half _XMoveSpeed;
		half _YAmplitude;
		float4 _Scale;
		
		half _Opacity;
		half _OpacityScale;
		int _BlurAmount;

		struct Input {

			float3 worldPos;
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		float2 calculate_uv( float2 baseUV , float3 vertexPos){
			
			float2 noiseValue = float2(_Time.y * _Speed.x + vertexPos.z * _XAmplitude, _Time.y * _Speed.y + vertexPos.x * _YAmplitude);
			float2 result = baseUV + tex2D(_NoiseTex, noiseValue) * _Scale - float2(0.04, 0.04);
		
			return result;
		}

		float4 blur(sampler2D tex, float2 uv){

			return tex2Dlod(tex, float4(uv.x, uv.y, 0, _BlurAmount));
		}

		void surf (Input IN, inout SurfaceOutput o) {

			float2 uv = IN.uv_MainTex;
			
			uv = calculate_uv(uv, IN.worldPos);
			
			half4 r = blur(_MainTex, uv);

			o.Albedo = IN.color.rgb;
			o.Alpha = r.a - tex2D(_NoiseTex, uv).r * (1 - IN.color.a) * _OpacityScale;
		}

		ENDCG


	} 
	FallBack "Sprites/Default"
}
