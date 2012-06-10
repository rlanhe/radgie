/*

% Debug shadder.
% Default shadder for Radgie graphic objects.

*/

float4x4 WorldViewProj : WorldViewProj;

struct a2v
{
	float3 pos : POSITION;
	float4 color : COLOR;
};

struct v2p
{
	float4 pos : POSITION;
	float4 color : COLOR;
};

void mainVS(in a2v input, out v2p output)
{
	output.pos = mul(float4(input.pos.xyz, 1.0), WorldViewProj);
	output.color = input.color;
}

float4 mainPS(in v2p input) : COLOR
{
	return input.color;
}

technique technique0 {
	pass p0 {
		VertexShader = compile vs_3_0 mainVS();
		PixelShader = compile ps_3_0 mainPS();
	}
}
