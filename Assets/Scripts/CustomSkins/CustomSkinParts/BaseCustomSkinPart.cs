﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Utility;
using Settings;
using ApplicationManagers;

namespace CustomSkins
{
    class BaseCustomSkinPart
    {
        protected BaseCustomSkinLoader _loader;
        protected List<Renderer> _renderers;
        protected string _rendererId;
        protected int _maxSize;
        protected Vector2 _textureScale;
        protected readonly Vector2 _defaultTextureScale = new Vector2(1f, 1f);
        protected bool _useTransparentMaterial;

        public BaseCustomSkinPart(BaseCustomSkinLoader loader, List<Renderer> renderers, string rendererId, int maxSize, Vector2? textureScale = null,
            bool useTransparentMaterial = false)
        {
            _loader = loader;
            _renderers = renderers;
            _rendererId = rendererId;
            _maxSize = maxSize;
            if (textureScale == null)
                _textureScale = _defaultTextureScale;
            else
                _textureScale = textureScale.Value;
            _useTransparentMaterial = useTransparentMaterial;
        }
        
        // starting child coroutines skips a frame which causes skin loading delay for cached textures, 
        // so to avoid this we manually call LoadCache in the main coroutine
        public bool LoadCache(string url)
        {
            if (url.ToLower() == BaseCustomSkinLoader.TransparentURL)
            {
                DisableRenderers();
                return true;
            }
            if (!IsValidPart() || !TextureDownloader.ValidTextureURL(url))
                return true;
            if (MaterialCache.ContainsKey(_rendererId, url))
            {
                SetMaterial(MaterialCache.GetMaterial(_rendererId, url));
                return true;
            }
            return false;
        }

        public IEnumerator LoadSkin(string url)
        {
            url = url.Trim();
            if (url.Contains("http://"))
                url = url.Replace("http://", "https://");
            if (!IsValidPart())
                yield break;
            if (!TextureDownloader.ValidTextureURL(url))
                yield break;
            bool mipmap = SettingsManager.GraphicsSettings.MipmapEnabled.Value;
            CoroutineWithData cwd = new CoroutineWithData(_loader, TextureDownloader.DownloadTexture(_loader, url, mipmap, _maxSize));
            yield return cwd.Coroutine;
            if (IsValidPart())
            {
                Material newMaterial = SetNewTexture((Texture2D)cwd.Result);
                MaterialCache.SetMaterial(_rendererId, url, newMaterial);
            }
        }

        protected virtual bool IsValidPart()
        {
            if (_renderers == null || _renderers.Count == 0)
                return false;
            foreach (Renderer renderer in _renderers)
            {
                if (renderer != null)
                    return true;
            }
            return false;
        }

        protected virtual void DisableRenderers()
        {
            if (_useTransparentMaterial)
                SetMaterial(MaterialCache.TransparentMaterial);
            else
            {
                if (_renderers != null)
                {
                    foreach (Renderer renderer in _renderers)
                    {
                        if (renderer != null)
                            renderer.enabled = false;
                    }
                }
            }
        }

        protected virtual void SetMaterial(Material material)
        {
            if (_renderers != null)
            {
                foreach (Renderer renderer in _renderers)
                {
                    if (renderer != null)
                        renderer.material = material;
                }
            }
        }

        protected virtual Material SetNewTexture(Texture2D texture)
        {
            if (_renderers == null || _renderers.Count == 0 || _renderers[0] == null)
                return null;
            if (_renderers[0].material == null)
                return null;
            _renderers[0].material.mainTexture = texture;
            if (_textureScale != _defaultTextureScale)
            {
                Vector2 scale = _renderers[0].material.mainTextureScale;
                _renderers[0].material.mainTextureScale = new Vector2(scale.x * _textureScale.x, scale.y * _textureScale.y);
                _renderers[0].material.mainTextureOffset = new Vector2(0f, 0f);
            }
            SetMaterial(_renderers[0].material);
            return _renderers[0].material;
        }
        
        public string GetRendererId()
        {
            return _rendererId;
        }
        
        public void ResetToDefault()
        {
            if (!IsValidPart())
                return;
            if (_renderers == null)
                return;
            
            foreach (Renderer renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = true;
                    if (renderer.sharedMaterial != null)
                    {
                        renderer.material = renderer.sharedMaterial;
                    }
                }
            }
        }
    }
}
