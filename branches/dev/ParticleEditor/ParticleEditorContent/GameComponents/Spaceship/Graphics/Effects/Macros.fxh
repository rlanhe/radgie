#ifndef MACROS

#define LIGHTING_PASS \
		MagFilter[0] = LINEAR; \
        MinFilter[0] = LINEAR; \
        MipFilter[0] = LINEAR; \
        AddressU[0] = WRAP; \
        AddressV[0] = WRAP; \
        MagFilter[1] = LINEAR; \
        MinFilter[1] = LINEAR; \
        MipFilter[1] = LINEAR; \
        AddressU[1] = WRAP; \
        AddressV[1] = WRAP; \
		Texture[0] = <diffuseTexture>; \
        Texture[1] = <specularTexture>; \
        VertexShader = compile vs_3_0 BasicVS(); \
		PixelShader = compile ps_3_0 MultipleLightPS();


#endif
