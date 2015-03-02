Shader "Custom/Vignette" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Radius ("Radius", Float) = 1.0
	}
	SubShader {
		Pass{
			CGPROGRAM
			#pragma vertex vertexShaderMain
			#pragma fragment fragmentShaderMain
		
			sampler2D _MainTex;
			float _Radius;
			
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput {
				float4 position : SV_POSITION;
				float4 tex : TEXCOORD0;
			};
		
			vertexOutput vertexShaderMain(vertexInput input) {
				vertexOutput output;
    	        
				output.position = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				
				return output;
			}
		
			float4 fragmentShaderMain(vertexOutput o) : COLOR {
				float2 inTex = o.tex - 0.5f;
				float vignette = 1 - dot(inTex, inTex);
				
				return tex2D(_MainTex, o.tex.xy) * saturate(pow(vignette, _Radius));
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
