<?xml version="1.0" encoding="utf-8" ?>
<Material>
  <RenderState>
    <BlendState>
      <Relative>../../../../../../Radgie/Graphics/RenderStates/BlendStates/opaque</Relative>
    </BlendState>
    <RasterizerState>
      <Relative>../../../../../../Radgie/Graphics/RenderStates/RasterizerStates/cullCounterClock</Relative>
    </RasterizerState>
    <SamplerState>
      <Relative>../../../../../../Radgie/Graphics/RenderStates/SamplerStates/linearWrap</Relative>
    </SamplerState>
    <DepthStencilState>
      <Relative>../../../../../../Radgie/Graphics/RenderStates/DepthStencilStates/default</Relative>
    </DepthStencilState>
    <BlendFactor>
      <Color>
        <R>200</R>
        <G>100</G>
        <B>200</B>
        <A>255</A>
      </Color>
    </BlendFactor>
    <MultiSampleMask>-1</MultiSampleMask>
    <ReferenceStencil>0</ReferenceStencil>
  </RenderState>
  <Effect>
    <Relative>../Effects/SpaceObjectEffect</Relative>
  </Effect>
  <EffectParameters>
    <EffectParameter semantic="EmissiveColor">
      <Color>
        <R>10</R>
        <G>10</G>
        <B>10</B>
        <A>255</A>
      </Color>
    </EffectParameter>
    <EffectParameter semantic="Texture0">
      <Texture>
        <Relative>../../../Asteroid/Graphics/Textures/Asteroid/normal</Relative>
      </Texture>
    </EffectParameter>
    <EffectParameter semantic="DiffuseTextureEnabled">true</EffectParameter>
    <EffectParameter semantic="LightsEnabled">false</EffectParameter>
    <EffectParameter semantic="DiffuseLigthsEnabled">true</EffectParameter>
    <EffectParameter semantic="Texture1">
      <Texture>
        <Relative>../../../Asteroid/Graphics/Textures/Asteroid/bump</Relative>
      </Texture>
    </EffectParameter>
  </EffectParameters>
  <DrawOrder>400</DrawOrder>
</Material>
