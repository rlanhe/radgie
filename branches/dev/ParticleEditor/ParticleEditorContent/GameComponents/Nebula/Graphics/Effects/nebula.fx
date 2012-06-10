/*

% Nebula shader

*/

float4x4 WorldViewProj : WorldViewProj;
float Time : Time;
float Kx = 0.005f;
float Ky = 0.01f;

texture Texture0 : Texture0;
sampler TextureSampler0 = sampler_state
{
	Texture = <Texture0>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Mirror;
	AddressV = Mirror;
};

texture Texture1 : Texture1;
sampler TextureSampler1 = sampler_state
{
	Texture = <Texture1>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
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
	output.tex.x = input.tex.x + Time*Kx;
	output.tex.y = input.tex.y + Time*Ky;
}

float4 mainPS(in v2p input) : COLOR
{
	return tex2D(TextureSampler0, input.tex);
}

technique technique0 {
	pass p0 {
		VertexShader = compile vs_3_0 mainVS();
		PixelShader = compile ps_3_0 mainPS();
	}
}
