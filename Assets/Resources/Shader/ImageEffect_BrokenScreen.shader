
Shader "PengLu/ImageEffect/Unlit/BrokenScreen" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}


	CGINCLUDE

		#include "UnityCG.cginc"

		struct appdata_t {
			float4 vertex : POSITION;
			half2 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			half2 texcoord : TEXCOORD0;
		};

		sampler2D _MainTex;
		uniform sampler2D _BumpTex;
		uniform float _satCount;
		uniform float _scaleX,_scaleY;

		v2f vert (appdata_t v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.texcoord = v.texcoord;
			return o;
		}
		
		fixed4 frag (v2f i) : COLOR
		{
			half2 bumpUV = i.texcoord -0.5;
			bumpUV *= float2(_scaleX, _scaleY);
			bumpUV += 0.5;
			half2 bump = UnpackNormal(tex2D( _BumpTex, bumpUV)).rg;
			i.texcoord = bump * 0.5  + i.texcoord.xy;
			fixed4 col = tex2D(_MainTex , i.texcoord);
			fixed4 lum = Luminance(col);
			col = lerp(col, lum, _satCount);
			return col;
		}


	ENDCG

	SubShader {
	  ZTest Always  ZWrite Off Cull Off Blend Off

	  Fog { Mode off } 
	//0  
	Pass { 
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG	 
		}	
			
	
	}	


}
