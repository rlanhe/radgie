/*

% Basic shadder.
% Default shadder for Radgie graphic objects.

*/

float4x4 WorldViewProj : WorldViewProj;

float3 EmissiveColor : EmissiveColor;
float4 DiffuseColor : DiffuseColor;
float3 SpecularColor : SpecularColor;
float SpecularPower : SpecularPower;

texture Texture : Texture0;
sampler TextureSampler = sampler_state
{
	Texture = <Texture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct a2v
{
	float3 pos : POSITION;
	float2 tex : TEXCOORD;
};

struct v2p
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD;
};

void mainVS(in a2v input, out v2p output)
{
	output.pos = mul(float4(input.pos.xyz, 1.0), WorldViewProj);
	output.tex = input.tex;
}

float4 mainPS(in v2p input) : COLOR
{
	return tex2D(TextureSampler, input.tex) * (EmissiveColor,1.0);
}

technique technique0 {
	pass p0 {
		VertexShader = compile vs_3_0 mainVS();
		PixelShader = compile ps_3_0 mainPS();
	}
}
