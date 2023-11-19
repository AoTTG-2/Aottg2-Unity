using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using System.Collections;
using ApplicationManagers;
using Cameras;
using Characters;
using Utility;

namespace UI
{
    class SnapshotHandler : MonoBehaviour
    {
        public BaseCamera SnapshotCamera;
        public InGameCamera InGameCamera; 

        public void Awake()
        {
            SnapshotCamera = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Prefabs/Snapshot/SnapshotCamera").AddComponent<BaseCamera>();
            InGameCamera = GetComponent<InGameCamera>();
            SnapshotCamera.gameObject.SetActive(false);
        }

        public void TakeSnapshot(Vector3 position, int damage)
        {
            if (damage >= SettingsManager.GeneralSettings.SnapshotsMinimumDamage.Value)
                StartCoroutine(TakeSnapshotCoroutine(position, damage));
        }

        private IEnumerator TakeSnapshotCoroutine(Vector3 position, int damage)
        {
            yield return new WaitForEndOfFrame();
            SnapshotCamera.gameObject.SetActive(true);
            SetSnapshotPosition(position);
            Texture2D snapshot = RTImage();
            SnapshotCamera.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            snapshot.Apply();
            if (SettingsManager.GeneralSettings.SnapshotsShowInGame.Value)
                ((InGameMenu)UIManager.CurrentMenu).ShowSnapshot(snapshot);
            yield return new WaitForSeconds(0.5f);
            SnapshotManager.AddSnapshot(snapshot, damage);
            yield return new WaitForSeconds(2f);
            Destroy(snapshot);
        }

        private void SetSnapshotPosition(Vector3 position)
        {
            Transform transform = SnapshotCamera.Cache.Transform;
            Vector3 direction = (InGameCamera.Camera.transform.position - position);
            float distance = Mathf.Max(20f, direction.magnitude);
            transform.position = position + direction.normalized * distance;
            transform.LookAt(position);
            SnapshotCamera.Skybox.material = InGameCamera.Skybox.material;
        }

        private Texture2D RTImage()
        {
            var cam = SnapshotCamera.Camera;
            RenderTexture active = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;
            cam.Render();
            Texture2D textured = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);
            int num = (int)(cam.targetTexture.width * 0.04f);
            int destX = (int)(cam.targetTexture.width * 0.02f);
            try
            {
                textured.SetPixel(0, 0, Color.white);
                textured.ReadPixels(new Rect((float)num, (float)num, (float)(cam.targetTexture.width - num), (float)(cam.targetTexture.height - num)), destX, destX);
                RenderTexture.active = active;
            }
            catch
            {
                textured = new Texture2D(1, 1);
                textured.SetPixel(0, 0, Color.white);
                return textured;
            }
            return textured;
        }
    }
}
