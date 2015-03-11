Shader "Custom/IRPostProcessing" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RampTex ("Ramp Texture", 2D) = "white" {}
	}
	SubShader {
	
		Pass{
		Tags {"LightMode" = "ForwardBase"}
		CGPROGRAM
			#pragma multi_compile_fwd
			#pragma vertex vertexShaderMain
			#pragma fragment fragmentShaderMain
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _CameraDepthNormalsTexture;
			uniform sampler2D _RampTex;
			uniform float4 _LightColor0;
			
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput {
				float4 position : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 scrPos : TEXCOORD1;
				LIGHTING_COORDS(2,3)
			};

			vertexOutput vertexShaderMain(vertexInput input) {
				vertexOutput output;
				
				output.position = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				output.scrPos = ComputeScreenPos(output.position);
				
				TRANSFER_VERTEX_TO_FRAGMENT(output);
				
				return output;
			}
			
			float4 fragmentShaderMain(vertexOutput o) : COLOR{
				
			 	float depthValue;
			 	float3 normalValue;
			 	DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, o.scrPos.xy), depthValue, normalValue);
			 	
			 	float atten = 1.0f;
				float3 lightDir;
				
				lightDir = normalize(_WorldSpaceLightPos0.xyz);
				

                float3 diffuseRef = atten * _LightColor0.xyz * saturate(dot(normalValue, lightDir));
                float intensity = dot(lightDir, normalValue);
   
                float3 rampColor = tex2D(_RampTex, float2(-depthValue*15.0f, 0.0));
                
                float3 lightFinal = rampColor * UNITY_LIGHTMODEL_AMBIENT.rgb;
			 	
				return float4(lightFinal, 1.0) * 10.0f;
			}
		
		ENDCG
		}
	}
	FallBack "Diffuse"
}
