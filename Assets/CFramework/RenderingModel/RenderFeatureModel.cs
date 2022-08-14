using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RenderFeatureModel : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        public Material material;
        public float testFloat = 1;
    }
    public Settings settings = new Settings();
    RenderModelPass renderModelPass;
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material == null)
        {
            Debug.LogError("Material null");
            return;
        }
        renderModelPass.settings = settings;
        renderModelPass.renderPassEvent = settings.renderPassEvent;
        renderer.EnqueuePass(renderModelPass);
    }

    public override void Create()
    {
        renderModelPass = new RenderModelPass();
    }
    public class RenderModelPass : ScriptableRenderPass
    {
        public RenderFeatureModel.Settings settings;
        RenderTargetIdentifier source;
        RenderTargetIdentifier destination;
        int temporaryRTId = Shader.PropertyToID("_TempRT");
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            source = renderingData.cameraData.renderer.cameraColorTarget;
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(temporaryRTId, descriptor);
            destination = new RenderTargetIdentifier(temporaryRTId);
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("RenderModel");

            Blit(cmd, source, destination, settings.material, 0);
            Blit(cmd, destination, source);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

}
