Shader "Charles Will Code It/PointShader"
{
	SubShader
	{
		Pass
		{

			CGPROGRAM
			#pragma target 5.0

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct data {
				float3 pos;
			};

			//The buffer containing the points we want to draw.
			StructuredBuffer<data> buf_Points;
			float3 _worldPos;

			
			//A simple input struct for our pixel shader step containing a position.
			struct ps_input 
			{
				float4 pos : SV_POSITION;
				float3 color: COLOR0;
			};

			//Our vertex function simply fetches a point from the buffer corresponding to the vertex index
			//which we transform with the view-projection matrix before passing to the pixel program.
			ps_input vert(uint id : SV_VertexID)
			{
				ps_input o;
				float3 worldPos = buf_Points[id].pos + _worldPos;
				o.pos = mul(UNITY_MATRIX_VP, float4(worldPos,1.0f));
				o.color = worldPos;
				return o;
			}

			//Pixel function returns a solid color for each point.
			float4 frag(ps_input i) : COLOR
			{
				float4 col = float4(i.color,1);

				if (i.color.r <= 0 && i.color.g <= 0 && i.color.b <= 0)
					col.rgb = float3(0, 1, 1);

				
				return col;
			}

			ENDCG

		}
	}
	Fallback Off
}
