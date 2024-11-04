Shader "TrailsFX/Effect/Color" {
Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _Color ("Color", Color) = (1,1,1,1)
    _Cull ("Cull", Int) = 2
    _ColorRamp("Color Ramp", 2D) = "white" {}
}
    SubShader
    {
        Tags { "Queue"="Transparent+101" "RenderType"="Transparent"}

        Pass
        {
			Stencil {
                Ref 2
                ReadMask 2
                Comp NotEqual
                Pass replace
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull [_Cull]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing assumeuniformscaling nolightprobe nolodfade nolightmap
            #pragma multi_compile_local _ TRAIL_INTERPOLATE
            #pragma multi_compile_local _ TRAIL_COLOR_RAMP
            #pragma multi_compile_local _ TRAIL_MASK

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                #if TRAIL_MASK
                    float2 uv : TEXCOORD0;
                #endif
                #if TRAIL_INTERPOLATE
                    float4 prevVertex : TEXCOORD1;
                #endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
            	UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 pos    : SV_POSITION;
                #if TRAIL_MASK
                    float2 uv : TEXCOORD0;
                #endif
                #if TRAIL_COLOR_RAMP
                    float3 wpos   : TEXCOORD1;
                #endif
				UNITY_VERTEX_OUTPUT_STEREO
            };

UNITY_INSTANCING_BUFFER_START(Props)
	UNITY_DEFINE_INSTANCED_PROP(fixed4, _Colors)
    #define _Colors_arr Props
    #if TRAIL_INTERPOLATE || TRAIL_COLOR_RAMP
        UNITY_DEFINE_INSTANCED_PROP(half, _SubFrameKeys)
        #define _SubFrameKeys_arr Props
    #endif
    #if TRAIL_COLOR_RAMP
	    UNITY_DEFINE_INSTANCED_PROP(float4, _RampStartPos)
        #define _RampStartPos_arr Props
	    UNITY_DEFINE_INSTANCED_PROP(fixed4, _RampEndPos)
        #define _RampEndPos_arr Props
    #endif
UNITY_INSTANCING_BUFFER_END(Props)

            sampler2D _ColorRamp;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                float4 vertex = v.vertex;

                #if TRAIL_INTERPOLATE
                    half key = UNITY_ACCESS_INSTANCED_PROP(_SubFrameKeys_arr, _SubFrameKeys);
                    vertex.xyz = lerp(v.prevVertex.xyz, vertex.xyz, key);
                #endif

                o.pos  = UnityObjectToClipPos(vertex);

                #if TRAIL_COLOR_RAMP
                    o.wpos = mul(unity_ObjectToWorld, vertex).xyz;
                #endif

                #if TRAIL_MASK
                    o.uv = TRANSFORM_TEX(v.uv, _MaskTex);
                #endif

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
            	UNITY_SETUP_INSTANCE_ID(i);
              	fixed4 col = UNITY_ACCESS_INSTANCED_PROP(_Colors_arr, _Colors);
#if TRAIL_COLOR_RAMP
                half key = UNITY_ACCESS_INSTANCED_PROP(_SubFrameKeys_arr, _SubFrameKeys);
                float3 rampStart = UNITY_ACCESS_INSTANCED_PROP(_RampStartPos_arr, _RampStartPos).xyz;
                float3 rampEnd = UNITY_ACCESS_INSTANCED_PROP(_RampEndPos_arr, _RampEndPos).xyz;
                float3 dir = rampEnd - rampStart;
                float len = length(dir);
                float3 axis = dir / len;
                float3 toWpos = i.wpos - rampStart;
                float t = dot(toWpos, axis);
                float d = t / len;
                fixed4 ramp = tex2Dlod(_ColorRamp, float4(d, key, 0, 0));
                col *= ramp;
                if (floor(d) != 0) col = 0;
#endif

                #if TRAIL_MASK
                    fixed4 mask = tex2D(_MaskTex, i.uv);
                    col.a *= mask.r;
                #endif

				return col;
            }
            ENDCG
        }

    }
}