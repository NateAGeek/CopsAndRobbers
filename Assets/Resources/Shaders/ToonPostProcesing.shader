Shader "Custom/ToonPostProcesing" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ResWidth ("ResWidth", Float) = 1000.0
		_ResHeight ("ResHeight", Float) = 1000.0
		_Tint ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineColor ("Outline Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineThickness ("Outline Thickness", Range(0.0, 1.0)) = 0.1
	}
	SubShader {
		Pass{
		CGPROGRAM
			#pragma vertex vertexShaderMain
			#pragma fragment fragmentShaderMain

			uniform sampler2D _MainTex;
			uniform float4 _Tint;
			uniform float4 _OutlineColor;
			uniform half _OutlineThickness;
			uniform float _ResWidth;
			uniform float _ResHeight;
			
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput {
				float4 position : SV_POSITION;
				float4 color : COLOR;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};

			vertexOutput vertexShaderMain(vertexInput input) {
				vertexOutput output;
				
				output.position = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
//				output.posWorld = mul(_Object2World, input.vertex);
//				float3 view = normalize(_WorldSpaceCameraPos - output.posWorld);
//				output.normalDir = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
//				
//				output.color.r = output.normalDir.x;
//				output.color.g = output.normalDir.y;
//				output.color.b = 1.0 - output.position.z/100.0;
				
				return output;
			}
			
			float4 fragmentShaderMain(vertexOutput o) : COLOR{
				float dx = 1.0/_ResWidth;
				float dy = 1.0/_ResHeight;
				
				  float4 center = tex2D(_MainTex, float2(0.0, 0.0));
				
				  // sampling just these 3 neighboring fragments keeps the outline thin.
				  float4 top = tex2D(_MainTex, float2(0.0, dy) );
				  float4 topRight = tex2D( _MainTex, float2(dx, dy) );
				  float4 right = tex2D( _MainTex, float2(dx, 0.0) );

				  // the rest is pretty arbitrary, but seemed to give me the
				  // best-looking results for whatever reason.

				  float4 t = center - top;
				  float4 r = center - right;
				  float4 tr = center - topRight;

				  t = abs( t );
				  r = abs( r );
				  tr = abs( tr );

				  float n;
				  n = max( n, t.x );
				  n = max( n, t.y );
				  n = max( n, t.z );
				  n = max( n, r.x );
				  n = max( n, r.y );
				  n = max( n, r.z );
				  n = max( n, tr.x );
				  n = max( n, tr.y );
				  n = max( n, tr.z );

				  // threshold and scale.
				  n = 1.0 - clamp( clamp((n * 2.0) - 0.8, 0.0, 1.0) * 1.5, 0.0, 1.0 );
				  float3 testing = tex2D(_MainTex, o.tex.xy).rgb * (0.1 + 0.9*n);
				  return tex2D(_MainTex, o.tex.xy);
				
				//float3 view = normalize(_WorldSpaceCameraPos - o.posWorld);
				//bool edgeDetection = (dot(view, o.normalDir) > _OutlineThickness) ? true : false;
				
				
				//if(edgeDetection){
					//return float4(o.normalDir, 1.0);
					//return _Tint * float4(tex.xyz, 1.0);
				//}else{
					//return float4(o.normalDir, 1.0);
				//}
				//return texCen;
			}
		
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
