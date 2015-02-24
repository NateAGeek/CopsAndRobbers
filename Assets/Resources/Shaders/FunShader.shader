Shader "Custom/FuncShader" {
	Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader {
		Pass{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
			
			#pragma vertex vertexShader
			#pragma fragment fragmentShader
			
			uniform float4 _Color;
			uniform float4 _LightColor0;
			
			
			
			struct vertexInput{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct vertexOutput{
				float4 position : SV_POSITION;
				float4 color : COLOR;
			};
			
			vertexOutput vertexShader(vertexInput input){
				vertexOutput o;
				
				float3 normalDirection = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
				float3 lightDirection;
				float atten = 1.0;
				
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				
				float3 diffuseReflection = atten * _LightColor0.xyz * max( 0.0, dot(normalDirection, lightDirection));
				
				float3 lightFinal = diffuseReflection + UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				o.color = float4(lightFinal * _Color.rgb, 1.0);
				o.position = mul(UNITY_MATRIX_MVP, input.vertex);
				return o;
			}
			
			float4 fragmentShader(vertexOutput i) : COLOR {
				return i.color;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
