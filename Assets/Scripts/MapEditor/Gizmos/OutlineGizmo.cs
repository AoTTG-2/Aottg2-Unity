using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Map;
using UnityEngine.Rendering;

namespace MapEditor
{
    class OutlineGizmo : BaseGizmo
    {
        private Dictionary<MapObject, List<Outline>> _meshOutlines = new Dictionary<MapObject, List<Outline>>();

        public static OutlineGizmo Create()
        {
            var go = new GameObject();
            var outline = go.AddComponent<OutlineGizmo>();
            return outline;
        }

        public override void OnSelectionChange()
        {
            foreach (MapObject obj in new List<MapObject>(_meshOutlines.Keys))
            {
                if (!_gameManager.SelectedObjects.Contains(obj))
                    DestroyOutline(obj);
            }
            foreach (MapObject obj in _gameManager.SelectedObjects)
            {
                if (!_meshOutlines.ContainsKey(obj))
                    CreateOutline(obj);
            }
        }

        private void CreateOutline(MapObject obj)
        {
            var outlines = new List<Outline>();
            /*
            foreach (MeshFilter filter in obj.GameObject.GetComponentsInChildren<MeshFilter>())
            {
                var outline = new GameObject();
                outline.name = "OutlineGizmo";
                outline.transform.parent = filter.transform;
                outline.transform.localPosition = Vector3.zero;
                outline.transform.localRotation = Quaternion.identity;
                outline.transform.localScale = Vector3.one;
                outline.AddComponent<MeshFilter>();
                outline.AddComponent<MeshRenderer>();
                outline.GetComponent<MeshFilter>().mesh = filter.mesh;
                outline.GetComponent<MeshRenderer>().material = (Material)ResourceManager.LoadAsset(ResourcePaths.Map, "Materials/OutlineMaterial", true);
                outlines.Add(outline);
            }
            */
            foreach (MeshFilter filter in obj.GameObject.GetComponentsInChildren<MeshFilter>())
            {
                var outline = filter.gameObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAndLightenColor;
                outline.OutlineColor = Color.green;
                outline.OutlineWidth = 3f;
                outlines.Add(outline);
            }
            _meshOutlines.Add(obj, outlines);
        }

        private void DestroyOutline(MapObject obj)
        {
            foreach (var go in _meshOutlines[obj])
                Destroy(go);
            _meshOutlines.Remove(obj);
        }
    }
}
