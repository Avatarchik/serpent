Shader "Example/Diffuse Simple" {
	Properties {
		_HeightCubemap ("Height Cubemap", Cube) = "white" {}
	}

	SubShader {
		Tags {
			"RenderType" = "Opaque"
		}

		CGPROGRAM
		#pragma surface surf Standard vertex:vert
		
		struct Input {
			float4 color : COLOR;
			half3 position;
		};

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.position = v.vertex.xyz;
		}

		samplerCUBE _HeightCubemap;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = texCUBE(_HeightCubemap, IN.position).rgb;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
