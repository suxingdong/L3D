Shader "Custom/ImageSwirl"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha


		//-----add
		GrabPass
	{
		"_MyGrabTex"
	}
		//---------

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
	};

	fixed4 _Color;

	//------------add
	float _Radius;
	float _Angle;
	sampler2D _MyGrabTex;
	float2 swirl(float2 uv);
	float2 swirl(float2 uv)
	{
		//先减去贴图中心点的纹理坐标,这样是方便旋转计算 
		uv -= float2(0.5, 0.5);

		//计算当前坐标与中心点的距离。 
		float dist = length(uv);

		//计算出旋转的百分比 
		float percent = (_Radius - dist) / _Radius;

		if (percent < 1.0 && percent >= 0.0)
		{
			//通过sin,cos来计算出旋转后的位置。 
			float theta = percent * percent * _Angle * 8.0;
			float s = sin(theta);
			float c = cos(theta);
			//uv = float2(dot(uv, float2(c, -s)), dot(uv, float2(s, c))); 
			uv = float2(uv.x*c - uv.y*s, uv.x*s + uv.y*c);
		}

		//再加上贴图中心点的纹理坐标，这样才正确。 
		uv += float2(0.5, 0.5);

		return uv;
	}
	//---------------

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}

	sampler2D _MainTex;
	sampler2D _AlphaTex;
	float _AlphaSplitEnabled;

	fixed4 SampleSpriteTexture(float2 uv)
	{
		fixed4 color = tex2D(_MainTex, uv);

		//----------modify
		//fixed4 color = tex2D(_MyGrabTex, float2(uv.x,1-uv.y));
		//-----------


#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
		if (_AlphaSplitEnabled)
			color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

		return color;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		//---add
		IN.texcoord = swirl(IN.texcoord);
	//--------

	fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
	c.rgb *= c.a;
	return c;
	}
		ENDCG
	}
	}
}