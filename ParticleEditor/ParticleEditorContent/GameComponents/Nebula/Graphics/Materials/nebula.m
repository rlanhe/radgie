﻿<?xml version="1.0" encoding="utf-8" ?>
<Material>
  <RenderState>
    <BlendState>
      <Relative>../../../../Radgie/Graphics/RenderStates/BlendStates/additiveBlend</Relative>
    </BlendState>
    <RasterizerState>
      <Relative>../../../../Radgie/Graphics/RenderStates/RasterizerStates/cullCounterClock</Relative>
    </RasterizerState>
    <SamplerState>
      <Relative>../../../../Radgie/Graphics/RenderStates/SamplerStates/linearWrap</Relative>
    </SamplerState>
    <DepthStencilState>
      <Relative>../../../../Radgie/Graphics/RenderStates/DepthStencilStates/depthRead</Relative>
    </DepthStencilState>
    <BlendFactor>
      <Color>
        <R>255</R>
        <G>255</G>
        <B>255</B>
        <A>255</A>
      </Color>
    </BlendFactor>
    <MultiSampleMask>-1</MultiSampleMask>
    <ReferenceStencil>0</ReferenceStencil>
  </RenderState>
  <Effect>
    <Relative>../Effects/nebula</Relative>
  </Effect>
  <EffectParameters>
    <EffectParameter semantic="Texture0">
      <Texture>
        <Relative>../Textures/B1_nebula01</Relative>
      </Texture>
    </EffectParameter>
  </EffectParameters>
  <DrawOrder>500</DrawOrder>
</Material>
