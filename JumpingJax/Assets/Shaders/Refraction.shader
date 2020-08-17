Shader "Custom/Refractive surface"
{
	Properties
	{
		[Header(Color)]
		_DarkColor("Dark water color",  Color) = (0,0,0,1)
		[HDR] _LitColor("Lit water color", Color) = (1,1,1,1)
		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5

		[Header(Fresnel)]
		_FPower("Fresnel power", Range(0.001, 10.0)) = 5.0
		_FScale("Fresnel scale", Range(0.0, 1.0)) = 1.0
		_FBias("Fresnel bias", Range(0.0, 1.0)) = 0.0

		[Header(Dissortion)]
		_DissortAmt("Dissortion amount", Range(0.0, 1.0)) = 0.3
		[Normal] _BumpMap("Normal dissortion map", 2D) = "bump" {}
		_BumpScale("Normal strenght", Float) = 1.0

		_SpeedX("Waves speed (X)", float) = 0.5
		_SpeedY("Waves speed (Y)", float) = 0.5
	}

		CGINCLUDE
			// Some helpers
#define DISSORTION_MAX 127
#define SPEED_UV(c) _Time.c * float2(_SpeedX, _SpeedY)
#define TEX(name) tex2D(name, i.uv##name)
			ENDCG

			SubShader
		{
			Tags{ "Queue" = "Transparent+1" "RenderType" = "Transparent" }

			GrabPass { }

			Zwrite Off
			CGPROGRAM
			#pragma surface surf Standard nolightmap noshadow
			#pragma target 3.0
			struct Input
			{
				float2 uv_BumpMap;
				float4 screenPos;
				float3 viewDir;
			};
			uniform sampler2D _GrabTexture;
			uniform float4 _GrabTexture_TexelSize;

			uniform fixed4 _DarkColor;
			uniform half4 _LitColor;
			uniform float _Glossiness;

			uniform float _FPower;
			uniform float _FScale;
			uniform float _FBias;

			uniform float _DissortAmt;
			uniform sampler2D _BumpMap;
			uniform float _BumpScale;

			uniform float _SpeedX;
			uniform float _SpeedY;

			void surf(Input i, inout SurfaceOutputStandard s)
			{
				// Calculate normal bump
				i.uv_BumpMap += SPEED_UV(xy);
				float3 bump = UnpackScaleNormal(TEX(_BumpMap), _BumpScale);

				// Calculate dissorted UVs
				float2 dissort = bump * pow(_DissortAmt * DISSORTION_MAX + 1, 2.0);
				i.screenPos.xy += (dissort * _GrabTexture_TexelSize.xy) * i.screenPos.z;

				// I'm not really sure of this part :/
				#ifndef UNITY_UV_STARTS_AT_TOP
					i.screenPos.y = 1 - i.screenPos.y;
				#endif

					// Calculate fresnel amount
					float fresnel;
					fresnel = 1.0 - dot(bump, i.viewDir);
					fresnel = _FScale * pow(fresnel, _FPower);
					fresnel = _FBias + (1.0 - _FBias) * saturate(fresnel);

					// Compute final fragment color
					half3 frag, emission;
					frag = lerp(tex2Dproj(_GrabTexture, i.screenPos), _DarkColor, _DarkColor.a).rgb;
					frag = lerp(frag, _LitColor, (_LitColor.a * fresnel));
					emission = _LitColor * (_LitColor.a * fresnel);

					// Feed output
					s.Albedo = frag;
					s.Normal = bump;
					s.Emission = emission;
					s.Smoothness = _Glossiness;
				}
				ENDCG
		}
			FallBack "Standard"
}