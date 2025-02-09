using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace CustomSkins
{
    class TextureDownloader
    {
        static readonly string[] ValidHosts = new string[]
        {
            "i.imgur.com/",
            "imgur.com/",
            "image.ibb.co/",
            "i.reddit.it/",
            "cdn.discordapp.com/attachments/",
            "media.discordapp.net/attachments/",
            "images-ext-2.discordapp.net/external/",
            "i.reddit.it/",
            "gyazo.com/",
            "puu.sh/",
            "i.postimg.cc/",
            "postimg./",
            "deviantart.com/",
            "photobucket.com/",
            "aotcorehome.files.wordpress.com/",
            "s1.ax1x.com/",
            "s27.postimg.io/",
            "1.bp.blogspot.com/",
            "tiebapic.baidu.com/",
            "s25.postimg.gg/",
            "aotcorehome.files.wordpress.com/",
            "imgse.com/"
        };
        static readonly string[] ValidFileEndings = new string[]
        {
            ".jpg",
            ".png",
            ".jpeg"
        };
        static readonly string[] URLPrefixes = new string[]
        {
            "https://",
            "http://",
            "www."
        };
        const int MaxConcurrentDownloads = 1;
        static int CurrentConcurrentDownloads = 0;

        public static void ResetConcurrentDownloads()
        {
            CurrentConcurrentDownloads = 0;
        }

        public static bool ValidTextureURL(string url)
        {
            url = url.ToLower();
            if (url == string.Empty)
                return false;
            if (url == BaseCustomSkinLoader.TransparentURL)
                return true;
            return CheckFileEnding(url) && CheckValidHost(url);
        }

        private static bool CheckFileEnding(string url)
        {
            foreach (string fileEnding in ValidFileEndings)
            {
                if (url.EndsWith(fileEnding))
                    return true;
            }
            return false;
        }

        private static bool CheckValidHost(string url)
        {
            if (url.StartsWith("file://"))
                return true;
            foreach (string prefix in URLPrefixes)
            {
                if (url.StartsWith(prefix))
                    url = url.Remove(0, prefix.Length);
            }
            foreach (string host in ValidHosts)
                if (url.StartsWith(host))
                    return true;
            return false;
        }

        public static IEnumerator DownloadTexture(MonoBehaviour obj, string url, bool mipmap, int maxSize)
        {
            // return a blank texture if an error is encountered
            Texture2D blankTexture = CreateBlankTexture(mipmap);
            yield return blankTexture;
            if (!ValidTextureURL(url))
                yield break;
            while (!CanStartTextureDownload())
                yield return blankTexture;
            OnStartTextureDownload();
            using (WWW www = new WWW(url))
            {
                yield return www;
                if (www.error != null || www.bytesDownloaded > maxSize)
                {
                    OnStopTextureDownload();
                    yield return blankTexture;
                    yield break;
                }
                OnStopTextureDownload();
                CoroutineWithData cwd = new CoroutineWithData(obj, CreateTextureFromData(obj, www, mipmap, url));
                yield return cwd.Coroutine;
                yield return cwd.Result;
            }
        }

        private static bool CanStartTextureDownload()
        {
            return CurrentConcurrentDownloads < MaxConcurrentDownloads;
        }

        private static void OnStartTextureDownload()
        {
            CurrentConcurrentDownloads++;
            CurrentConcurrentDownloads = Math.Min(CurrentConcurrentDownloads, MaxConcurrentDownloads);
        }

        private static void OnStopTextureDownload()
        {
            CurrentConcurrentDownloads--;
            CurrentConcurrentDownloads = Math.Max(CurrentConcurrentDownloads, 0);
        }

        private static bool IsPowerOfTwo(int num)
        {
            return num >= 4 && (num & (num - 1)) == 0;
        }

        private static int GetClosestPowerOfTwo(int num)
        {
            int closestPower = 4;
            num = Math.Min(num, 2047);
            while (closestPower < num)
                closestPower *= 2;
            return closestPower;
        }

        private static Texture2D CreateBlankTexture(bool mipmap)
        {
            return new Texture2D(4, 4, TextureFormat.RGBA32, mipmap);
        }

        private static FREE_IMAGE_FORMAT GetTextureFormat(string url)
        {
            if (url.EndsWith(".png"))
                return FREE_IMAGE_FORMAT.FIF_PNG;
            return FREE_IMAGE_FORMAT.FIF_JPEG;
        }

        private static IEnumerator CreateTextureFromData(MonoBehaviour obj, WWW www, bool mipmap, string url)
        {
            var importer = new TextureImporter();
            yield return obj.StartCoroutine(importer.ImportTexture(www.bytes, GetTextureFormat(url), mipmap));
            var texture = importer.texture;
            yield return texture;
        }
    }
}
