using ApplicationManagers;
using Events;
using Photon.Pun;
using Settings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Weather;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

namespace Map
{
    class MapLoader: MonoBehaviour
    {
        public static Dictionary<int, MapObject> IdToMapObject = new Dictionary<int, MapObject>();
        public static Dictionary<int, HashSet<int>> IdToChildren = new Dictionary<int, HashSet<int>>();
        public static Dictionary<GameObject, MapObject> GoToMapObject = new Dictionary<GameObject, MapObject>();
        public static Dictionary<string, List<MapObject>> Tags = new Dictionary<string, List<MapObject>>();
        public static List<Light> Daylight = new List<Light>();
        public static List<MapLight> MapLights = new List<MapLight>();
        private static Dictionary<string, Object> _assetCache = new Dictionary<string, Object>();
        private static Dictionary<string, List<Material>> _assetMaterialCache = new Dictionary<string, List<Material>>();
        private static Dictionary<string, List<Material>> _defaultMaterialCache = new Dictionary<string, List<Material>>();
        private static Dictionary<string, Material> _customMaterialCache = new Dictionary<string, Material>();
        private static MapLoader _instance;
        public static int HighestObjectId;
        private static MapScriptBasicMaterial _invisibleMaterial;
        public static List<string> Errors = new List<string>();
        public static bool HasWeather;
        public static WeatherSet Weather;
        private static GameObject _background;
        private static bool _hasNavMeshData;
        private static List<NavMeshBuildSource> _navMeshSources = new List<NavMeshBuildSource>();
        private static Bounds _navMeshBounds = new Bounds(Vector3.zero, Vector3.zero);
        private static Dictionary<int, NavMeshData> _navMeshData = new Dictionary<int, NavMeshData>();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            EventManager.OnPreLoadScene += OnPreLoadScene;
            _invisibleMaterial = new MapScriptBasicMaterial
            {
                Shader = "Transparent",
                Color = new Color255(126, 255, 255, 80)
            };
        }

        private static void OnPreLoadScene(SceneName sceneName)
        {
            _instance.StopAllCoroutines();
            HasWeather = false;
            Weather = null;
        }

        public static int GetNextObjectId()
        {
            HighestObjectId++;
            return HighestObjectId;
        }

        public static void StartLoadObjects(List<string> customAssets, List<MapScriptBaseObject> objects, MapScriptOptions options, WeatherSet weather, bool editor = false)
        {
            Errors.Clear();
            _customMaterialCache.Clear();
            _defaultMaterialCache.Clear();
            _assetMaterialCache.Clear();
            IdToChildren.Clear();
            IdToMapObject.Clear();
            GoToMapObject.Clear();
            Daylight.Clear();
            MapLights.Clear();
            _assetCache.Clear();
            Tags.Clear();
            HighestObjectId = 1;
            HasWeather = !editor && options != null && options.HasWeather;
            Weather = weather;
            /*
            if (options != null)
                LoadBackground(options.Background, options.BackgroundPosition, options.BackgroundRotation);
            */
            _instance.StartCoroutine(_instance.LoadObjectsCoroutine(customAssets, objects, editor));
        }

        /*
        public static void LoadBackground(string background, Vector3 position, Vector3 rotation)
        {
            if (_background != null)
                Destroy(_background);
            try
            {
                if (background == "None")
                    return;
                _background = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Map, "Background/Prefabs/" + background);
                var center = _background.transform.Find("Center").localPosition;
                _background.transform.position = position - center;
                _background.transform.rotation = Quaternion.Euler(rotation);
            }
            catch
            {
                Debug.Log("Error loading map background: " + background);
            }
        }
        */

        public static void RegisterMapLight(Light light)
        {
            MapLights.Add(new MapLight(light));
        }

        public static MapObject FindObjectFromCollider(Collider collider)
        {
            Transform current = collider.transform;
            while (true)
            {
                var go = current.gameObject;
                if (GoToMapObject.ContainsKey(go))
                    return GoToMapObject[go];
                if (current.parent == null)
                    return null;
                current = current.parent;
            }
        }

        public static MapObject LoadObject(MapScriptBaseObject scriptObject, bool editor)
        {
            GameObject go = null;
            if (scriptObject is MapScriptSceneObject)
                go = LoadSceneObject((MapScriptSceneObject)scriptObject, editor);
            MapObject mapObject = new MapObject(scriptObject.Parent, go, scriptObject);
            HighestObjectId = Mathf.Max(mapObject.ScriptObject.Id, HighestObjectId);
            if (IdToMapObject.ContainsKey(scriptObject.Id))
            {
                DebugConsole.Log("Map load error: map object with duplicate ID found (" + scriptObject.Id.ToString() + ")", true);
                return mapObject;
            }
            IdToMapObject.Add(scriptObject.Id, mapObject);
            GoToMapObject.Add(go, mapObject);
            if (scriptObject.Parent > 0)
            {
                if (IdToChildren.ContainsKey(scriptObject.Parent))
                    IdToChildren[scriptObject.Parent].Add(scriptObject.Id);
                else
                    IdToChildren.Add(scriptObject.Parent, new HashSet<int>() { scriptObject.Id });
            }
            SetTransform(mapObject);
            if (!editor && !scriptObject.Active)
            {
                mapObject.GameObject.SetActive(false);
            }
            if (!editor && scriptObject.Asset.StartsWith("Interact/Supply"))
                MinimapHandler.CreateMinimapIcon(mapObject.GameObject.transform, "Supply");
            return mapObject;
        }

        public static void SetParent(MapObject obj, MapObject parent)
        {
            if (IdToChildren.ContainsKey(obj.Parent) && IdToChildren[obj.Parent].Contains(obj.ScriptObject.Id))
                IdToChildren[obj.Parent].Remove(obj.ScriptObject.Id);
            if (parent == null)
            {
                obj.Parent = 0;
                obj.GameObject.transform.SetParent(null);
            }
            else if (obj != parent && parent.Parent != obj.ScriptObject.Id)
            {
                obj.Parent = parent.ScriptObject.Id;
                obj.GameObject.transform.SetParent(parent.GameObject.transform);
                if (IdToChildren.ContainsKey(obj.Parent))
                    IdToChildren[obj.Parent].Add(obj.ScriptObject.Id);
                else
                    IdToChildren.Add(obj.Parent, new HashSet<int>() { obj.ScriptObject.Id });
            }
        }

        public static void DeleteObject(MapObject obj)
        {
            int id = obj.ScriptObject.Id;
            DeleteObject(id);
        }

        public static void DeleteObject(int id)
        {
            var mapObject = IdToMapObject[id];
            IdToMapObject.Remove(id);
            GoToMapObject.Remove(mapObject.GameObject);
            if (IdToChildren.ContainsKey(id))
            {
                foreach (int child in new List<int>(IdToChildren[id]))
                {
                    if (IdToMapObject.ContainsKey(child))
                    {
                        SetParent(IdToMapObject[child], null);
                    }
                }
            }
            Destroy(mapObject.GameObject);
        }

        private IEnumerator LoadObjectsCoroutine(List<string> customAssets, List<MapScriptBaseObject> objects, bool editor)
        {
            foreach (string customAsset in customAssets)
            {
                string[] arr = customAsset.Split(',');
                if (arr.Length == 0)
                    continue;
                string bundle = arr[0].Trim();
                string url = string.Empty;
                if (arr.Length > 1)
                    url = arr[1].Trim();
                yield return _instance.StartCoroutine(AssetBundleManager.LoadBundle(bundle, url, editor));
                if (!AssetBundleManager.LoadedBundle(bundle))
                {
                    DebugConsole.Log("Failed to load bundle: " + customAsset, true);
                    Errors.Add("Failed to load bundle: " + customAsset);
                }
            }

            bool gamemodeNeedsNav = SettingsManager.InGameCurrent.General.GameMode.Value != "Racing"
                && SettingsManager.InGameCurrent.General.GameMode.Value != "Thunderspear PVP"
                && SettingsManager.InGameCurrent.General.GameMode.Value != "Blade PVP"
                && SettingsManager.InGameCurrent.General.GameMode.Value != "APG PVP"
                && SettingsManager.InGameCurrent.General.GameMode.Value != "AHSS PVP";

            bool willLoadNavMesh = (MapManager.NeedsNavMeshUpdate || _hasNavMeshData == false)
                && PhotonNetwork.IsMasterClient
                && SettingsManager.InGameCurrent.Titan.TitanSmartMovement.Value
                && gamemodeNeedsNav;

            int count = 0;

            float multiplier = willLoadNavMesh ? 0.25f : 0.5f;
            foreach (MapScriptBaseObject obj in objects)
            {
                LoadObject(obj, editor);
                if (count % 100 == 0 && SceneLoader.SceneName == SceneName.InGame)
                {
                    UIManager.LoadingMenu.UpdateLoading(0.5f + multiplier * ((float)count / (float)objects.Count));
                    yield return new WaitForEndOfFrame();
                }
                count++;
            }
            if (!editor)
            {
                foreach (int id in IdToMapObject.Keys)
                {
                    var mapObject = IdToMapObject[id];
                    SetParent(mapObject);
                }
            }
            if (!editor)
                Batch();

            

            if (MapManager.NeedsNavMeshUpdate || _hasNavMeshData == false)
            {
                NavMesh.RemoveAllNavMeshData();
                _hasNavMeshData = false;
                if (PhotonNetwork.IsMasterClient && SettingsManager.InGameCurrent.Titan.TitanSmartMovement.Value && gamemodeNeedsNav)
                {
                    ResetSources();

                    List<int> agentIDs = Util.GetAllTitanAgentIds();
                    List<AsyncOperation> operations = new List<AsyncOperation>();
                    foreach (int agentID in agentIDs)
                    {
                        AsyncOperation createOp = CreateNavMeshSurfaceAsyncOperation(agentID, _navMeshSources, _navMeshBounds);
                        operations.Add(createOp);
                    }

                    // Check progress of the operations and use the min progress
                    while (operations.Count > 0)
                    {
                        float minProgress = 1f;
                        for (int i = 0; i < operations.Count; i++)
                        {
                            if (operations[i].isDone)
                            {
                                operations.RemoveAt(i);
                                i--;
                            }
                            else
                                minProgress = Mathf.Min(minProgress, operations[i].progress);
                        }
                        UIManager.LoadingMenu.UpdateLoading(0.75f + 0.25f * minProgress);
                        yield return new WaitForEndOfFrame();
                    }

                    _hasNavMeshData = true;
                }
            }

            MapManager.MapLoaded = true;
        }

        static Bounds GetWorldBounds(Matrix4x4 mat, Bounds bounds)
        {
            var absAxisX = Util.Abs(mat.MultiplyVector(Vector3.right));
            var absAxisY = Util.Abs(mat.MultiplyVector(Vector3.up));
            var absAxisZ = Util.Abs(mat.MultiplyVector(Vector3.forward));
            var worldPosition = mat.MultiplyPoint(bounds.center);
            var worldSize = absAxisX * bounds.size.x + absAxisY * bounds.size.y + absAxisZ * bounds.size.z;
            return new Bounds(worldPosition, worldSize);
        }

        Bounds CalculateWorldBounds(List<NavMeshBuildSource> sources)
        {
            // Use the unscaled matrix for the NavMeshSurface
            Matrix4x4 worldToLocal = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            worldToLocal = worldToLocal.inverse;

            var result = new Bounds();
            foreach (var src in sources)
            {
                switch (src.shape)
                {
                    case NavMeshBuildSourceShape.Mesh:
                        {
                            var m = src.sourceObject as Mesh;
                            result.Encapsulate(GetWorldBounds(worldToLocal * src.transform, m.bounds));
                            break;
                        }
                    case NavMeshBuildSourceShape.Terrain:
                        {
                            var t = src.sourceObject as TerrainData;
                            result.Encapsulate(GetWorldBounds(worldToLocal * src.transform, new Bounds(0.5f * t.size, t.size)));
                            break;
/*#if NMC_CAN_ACCESS_TERRAIN
                            // Terrain pivot is lower/left corner - shift bounds accordingly
                            var t = src.sourceObject as TerrainData;
                            result.Encapsulate(GetWorldBounds(worldToLocal * src.transform, new Bounds(0.5f * t.size, t.size)));
#else
                            Debug.LogWarning("The NavMesh cannot be properly baked for the terrain because the necessary functionality is missing. Add the com.unity.modules.terrain package through the Package Manager.");
#endif
                            break;*/
                        }
                    case NavMeshBuildSourceShape.Box:
                    case NavMeshBuildSourceShape.Sphere:
                    case NavMeshBuildSourceShape.Capsule:
                    case NavMeshBuildSourceShape.ModifierBox:
                        result.Encapsulate(GetWorldBounds(worldToLocal * src.transform, new Bounds(Vector3.zero, src.size)));
                        break;
                }
            }
            // Inflate the bounds a bit to avoid clipping co-planar sources
            result.Expand(0.1f);
            return result;
        }

        private void ResetSources()
        {
            // Clear previous sources and bounds
            _navMeshSources.Clear();
            _navMeshData.Clear();
            _navMeshBounds = new Bounds(Vector3.zero, Vector3.zero);

            // Create sources and bounds
            var mask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectEntities);
            List<NavMeshBuildMarkup> modifiers = new List<NavMeshBuildMarkup>();

            // Collect sources of physics colliders, exclude components with NavMeshObstacles
            NavMeshBuilder.CollectSources(null, mask, NavMeshCollectGeometry.PhysicsColliders, 0, modifiers, _navMeshSources);
            _navMeshBounds = CalculateWorldBounds(_navMeshSources);
            _navMeshBounds.size = Vector3.Min(_navMeshBounds.size, new Vector3(15000, 15000, 15000));
        }

        /// <summary>
        /// Use NavMeshBuilder to create a navmesh surface for the given agent id.
        /// This should be called asynchronously and sync progress back to the main thread.
        /// </summary>
        /// <param name="agentID">An integer representing the agent.</param>
        private AsyncOperation CreateNavMeshSurfaceAsyncOperation(int agentID, List<NavMeshBuildSource> sources, Bounds bounds)
        {
            // Create a new navmeshsurface object
            NavMeshData data = new NavMeshData();
            NavMeshBuildSettings settings = NavMesh.GetSettingsByID(agentID);
            settings.maxJobWorkers = 3;
            settings.overrideTileSize = true;
            settings.tileSize = 256;
            settings.overrideVoxelSize = true;
            settings.voxelSize = 4f;
            settings.minRegionArea = 100;
            settings.buildHeightMesh = true;
            _navMeshData.Add(agentID, data);
            NavMesh.AddNavMeshData(data);
            return NavMeshBuilder.UpdateNavMeshDataAsync(data, settings, sources, bounds);
        }

        private async Task CreateNavMeshSurfaceAsync(int agentID, List<NavMeshBuildSource> sources, Bounds bounds)
        {
            // Create a new navmeshsurface object
            NavMeshData data = new NavMeshData();
            NavMeshBuildSettings settings = NavMesh.GetSettingsByID(agentID);
            settings.maxJobWorkers = 6;
            settings.overrideTileSize = true;
            settings.tileSize = 256;
            settings.overrideVoxelSize = true;
            settings.voxelSize = 4f;
            settings.minRegionArea = 100;
            settings.buildHeightMesh = true;
            _navMeshData.Add(agentID, data);
            NavMesh.AddNavMeshData(data);
            await NavMeshBuilder.UpdateNavMeshDataAsync(data, settings, sources, bounds);
        }

        public static async Task UpdateNavMesh()
        {
            await _instance.UpdateAllNavMeshes();
        }

        public async Task UpdateAllNavMeshes()
        {
            NavMesh.RemoveAllNavMeshData();
            _hasNavMeshData = false;
            if (PhotonNetwork.IsMasterClient)
            {
                await GenerateNavMesh();
                _hasNavMeshData = true;
            }
        }

        private async Task GenerateNavMesh()
        {
            ResetSources();
            List<int> agentIDs = Util.GetAllTitanAgentIds();
            List<Task> tasks = new List<Task>();
            foreach (int agentID in agentIDs)
            {
                tasks.Add(CreateNavMeshSurfaceAsync(agentID, _navMeshSources, _navMeshBounds));
            }
            await Task.WhenAll(tasks);
        }

        private void Batch()
        {
            Dictionary<string, GameObject> roots = new Dictionary<string, GameObject>();
            Dictionary<string, List<GameObject>> shared = new Dictionary<string, List<GameObject>>();
            Dictionary<GameObject, Transform> oldParents = new Dictionary<GameObject, Transform>();
            Dictionary<string, int> hashCounts = new Dictionary<string, int>();
            foreach (int id in IdToMapObject.Keys)
            {
                var mapObject = IdToMapObject[id];
                if (mapObject.ScriptObject.Parent > 0 || !mapObject.ScriptObject.Static)
                    continue;
                var shader = ((MapScriptSceneObject)mapObject.ScriptObject).Material.Shader;
                if (MapObjectShader.IsLegacyShader(shader) || shader == MapObjectShader.Transparent)
                    continue;
                var position = mapObject.GameObject.transform.position;
                string positionHash = ((int)(position.x / 1000f)).ToString() + "-" + ((int)(position.y / 1000f)).ToString() + "-" + ((int)(position.z / 1000f)).ToString();
                foreach (MeshFilter filter in mapObject.GameObject.GetComponentsInChildren<MeshFilter>())
                {
                    var renderer = filter.GetComponent<Renderer>();
                    if (renderer == null || renderer.sharedMaterials.Length > 1)
                        continue;
                    string hash = filter.sharedMesh.GetHashCode().ToString();
                    hash += positionHash;
                    if (renderer.enabled)
                        hash += renderer.sharedMaterial.GetHashCode().ToString();
                    else
                        hash += "disabled";
                    if (!hashCounts.ContainsKey(hash))
                        hashCounts.Add(hash, 0);
                    int meshPerHash = 65000 / filter.sharedMesh.vertexCount;
                    hashCounts[hash] += 1;
                    hash += (hashCounts[hash] / meshPerHash).ToString();
                    if (!roots.ContainsKey(hash))
                    {
                        var go = new GameObject();
                        go.layer = PhysicsLayer.MapObjectEntities;
                        roots.Add(hash, go);
                        shared.Add(hash, new List<GameObject>());
                    }
                    shared[hash].Add(filter.gameObject);
                    oldParents.Add(filter.gameObject, filter.transform.parent);
                    filter.transform.SetParent(roots[hash].transform);
                }
            }
            foreach (string hash in roots.Keys)
                CombineMeshes(roots[hash]);
            foreach (string hash in shared.Keys)
            {
                foreach (GameObject go in shared[hash])
                    go.transform.SetParent(oldParents[go]);
            }
        }

        void CombineMeshes(GameObject obj)
        {
            MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            if (meshFilters.Length == 0)
                return;
            bool rendererEnabled = meshFilters[0].GetComponent<Renderer>().enabled;
            for (int i = 0; i < meshFilters.Length; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].GetComponent<Renderer>().enabled = false;
            }
            var meshFilter = obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
            meshFilter.mesh = new Mesh();
            meshFilter.mesh.CombineMeshes(combine, true, true);
            if (rendererEnabled)
                obj.GetComponent<Renderer>().material = meshFilters[0].GetComponent<Renderer>().sharedMaterial;
            else
                obj.GetComponent<Renderer>().enabled = false;
        }

        public static void RegisterTag(string tag, MapObject obj)
        {
            if (!Tags.ContainsKey(tag))
                Tags.Add(tag, new List<MapObject>());
            Tags[tag].Add(obj);
        }

        private static GameObject LoadSceneObject(MapScriptSceneObject obj, bool editor)
        {
            GameObject go = obj.Asset == "None"
                ? new GameObject()
                : LoadPrefabCached(obj.Asset);
            
            if (editor)
            {
                int colliderCount = SetPhysics(go, MapObjectCollideMode.Physical, MapObjectCollideWith.MapEditor, obj.PhysicsMaterial);
                if (colliderCount == 0) go.AddComponent<MeshCollider>();
            }
            else
                SetPhysics(go, obj.CollideMode, obj.CollideWith, obj.PhysicsMaterial);
            SetMaterial(go, obj.Asset, obj.Material, obj.Visible, editor);
            return go;
        }

        private static void SetTransform(MapObject mapObject)
        {
            var go = mapObject.GameObject;
            var obj = mapObject.ScriptObject;
            Transform t = go.transform;
            go.isStatic = obj.Static;
            go.name = obj.Name;
            t.position = new Vector3(obj.PositionX, obj.PositionY, obj.PositionZ);
            t.rotation = Quaternion.Euler(obj.RotationX, obj.RotationY, obj.RotationZ);
            var localScale = t.localScale;
            mapObject.BaseScale = new Vector3(localScale.x, localScale.y, localScale.z);
            t.localScale = new Vector3(mapObject.BaseScale.x * obj.ScaleX, mapObject.BaseScale.y * obj.ScaleY, mapObject.BaseScale.z * obj.ScaleZ);
        }

        public static void SetParent(MapObject mapObject)
        {
            var go = mapObject.GameObject;
            var obj = mapObject.ScriptObject;
            Transform t = go.transform;
            if (obj.Parent > 0)
            {
                if (IdToMapObject.ContainsKey(obj.Parent))
                    t.SetParent(IdToMapObject[obj.Parent].GameObject.transform);
                else
                    DebugConsole.Log("Map load error: object parent id not found (" + obj.Parent.ToString() + ")", true);
            }
        }

        public static void SetDefaultTiling(string asset, Material mat, Vector2 tiling)
        {
            if (asset == "FX/WaterCube1")
            {
                mat.SetVector("_Tiling", tiling);
                mat.SetVector("_Tiling_1", tiling);
            }
            else if (asset == "FX/LavaCube1")
            {
                mat.SetVector("_BaseColorTiling", tiling);
                mat.SetVector("_EmitColorTiling", tiling);
                mat.SetVector("_Normal_Tiling", tiling);
            }
        }

        public static void SetMaterial(GameObject go, string asset, MapScriptBaseMaterial material, bool visible, bool editor)
        {
            if (asset == "None")
                return;
            var allRenderers = go.GetComponentsInChildren<Renderer>();
            var renderers = new List<Renderer>();
            if (!visible && editor)
            {
                visible = true;
                if (!asset.Contains("Editor"))
                    material = _invisibleMaterial;
            }
            foreach (var r in allRenderers)
            {
                if (r.name != "OutlineGizmo")
                    renderers.Add(r);
            }
            if (!_assetMaterialCache.ContainsKey(asset))
            {
                var assetMats = new List<Material>();
                foreach (Renderer renderer in renderers)
                {
                    assetMats.Add(renderer.material);
                }
                _assetMaterialCache.Add(asset, assetMats);
            }
            if (material.Shader == MapObjectShader.Default || material.Shader == MapObjectShader.DefaultNoTint || material.Shader == MapObjectShader.DefaultTiled)
            {
                string materialHash = asset + material.Serialize();
                if (!_defaultMaterialCache.ContainsKey(materialHash))
                {
                    var assetMats = _assetMaterialCache[asset];
                    var defaultMats = new List<Material>();
                    foreach (var assetMat in assetMats)
                    {
                        var mat = new Material(assetMat);
                        if (material.Shader != MapObjectShader.DefaultNoTint)
                            mat.color = material.Color.ToColor();
                        if (material.Shader == MapObjectShader.DefaultTiled)
                            SetDefaultTiling(asset, mat, ((MapScriptDefaultTiledMaterial)material).Tiling);
                        defaultMats.Add(mat);
                    }
                    _defaultMaterialCache.Add(materialHash, defaultMats);
                }
                var mats = _defaultMaterialCache[materialHash];
                for (int i = 0; i < renderers.Count; i++)
                {
                    renderers[i].material = mats[i];
                    renderers[i].enabled = visible;
                }
            }
            else
            {
                Material mat = null;
                string materialHash = material.Serialize();
                if (_customMaterialCache.ContainsKey(materialHash))
                    mat = _customMaterialCache[materialHash];
                else
                {
                    if (MapObjectShader.IsLegacyShader(material.Shader))
                    {
                        var legacy = (MapScriptLegacyMaterial)material;
                        mat = (Material)Instantiate(LoadAssetCached("Map/Legacy/Materials", legacy.Shader));
                        mat.SetColor("_TintColor", legacy.Color.ToColor());
                        mat.mainTextureScale = new Vector2(legacy.Tiling.x, legacy.Tiling.y);
                    }
                    else if (typeof(MapScriptBasicMaterial).IsAssignableFrom(material.GetType()))
                    {
                        var basic = (MapScriptBasicMaterial)material;
                        mat = (Material)Instantiate(LoadAssetCached("Map/Materials", basic.Shader + "Material"));
                        if (basic.Texture != "Misc/None" && basic.Texture != "None")
                        {
                            string[] textureArr = basic.Texture.Split('/');
                            string category = textureArr[0];
                            string textureName = textureArr[1];
                            bool legacy = category == "Legacy";
                            if (BuiltinMapTextures.AllTextures.ContainsKey(textureName) && (!legacy || BuiltinMapTextures.LegacyTexturePaths.ContainsKey(textureName)))
                            {
                                var reference = BuiltinMapTextures.AllTextures[textureName];
                                Texture2D texture;
                                if (legacy)
                                {
                                    string texturePath = BuiltinMapTextures.LegacyTexturePaths[textureName];
                                    texture = (Texture2D)LoadAssetCached("Map", texturePath);
                                }
                                else
                                    texture = (Texture2D)LoadAssetCached("Map/Textures/" + category, textureName + "Texture");
                                mat.mainTexture = texture;
                                mat.mainTextureScale = new Vector2(basic.Tiling.x * reference.Tiling.x, basic.Tiling.y * reference.Tiling.y);
                                mat.mainTextureOffset = basic.Offset;

                            }
                        }
                        mat.color = basic.Color.ToColor();
                        if (material is MapScriptReflectiveMaterial)
                        {
                            var reflective = (MapScriptReflectiveMaterial)material;
                            mat.SetColor("_SpecularMap", reflective.ReflectColor.ToColor());
                        }
                    }
                    _customMaterialCache.Add(materialHash, mat);
                }
                foreach (Renderer renderer in renderers)
                {
                    if (mat != null)
                        renderer.material = mat;
                    renderer.enabled = visible;
                }
            }
        }

        /// <returns>Number of colliders on <paramref name="go"/>.</returns>
        private static int SetPhysics(GameObject go, string collideMode, string collideWith, string physicsMaterial)
        {
            PhysicMaterial material = null;
            if (physicsMaterial != "Default")
                material = (PhysicMaterial)LoadAssetCached("Physics", physicsMaterial);
            int layer = GetColliderLayer(collideWith);
            Collider[] colliders = go.GetComponentsInChildren<Collider>();
            go.layer = layer;
            foreach (Collider c in colliders)
            {
                c.gameObject.layer = layer;
                c.isTrigger = collideMode == MapObjectCollideMode.Region;
                c.enabled = collideMode != MapObjectCollideMode.None;
                if (material != null)
                    c.material = material;
            }
            return colliders.Length;
        }

        public static void SetCollider(Collider c, string collideMode, string collideWith)
        {
            c.isTrigger = collideMode == MapObjectCollideMode.Region;
            c.enabled = collideMode != MapObjectCollideMode.None;
            c.gameObject.layer = GetColliderLayer(collideWith);
        }

        public static int GetColliderLayer(string collideWith)
        {
            int layer = 0;
            if (collideWith == MapObjectCollideWith.All)
                layer = PhysicsLayer.MapObjectAll;
            else if (collideWith == MapObjectCollideWith.MapObjects)
                layer = PhysicsLayer.MapObjectMapObjects;
            else if (collideWith == MapObjectCollideWith.Characters)
                layer = PhysicsLayer.MapObjectCharacters;
            else if (collideWith == MapObjectCollideWith.Projectiles)
                layer = PhysicsLayer.MapObjectProjectiles;
            else if (collideWith == MapObjectCollideWith.Entities)
                layer = PhysicsLayer.MapObjectEntities;
            else if (collideWith == MapObjectCollideWith.MapEditor)
                layer = PhysicsLayer.MapEditorObject;
            else if (collideWith == MapObjectCollideWith.Hitboxes)
                layer = PhysicsLayer.Hurtbox;
            return layer;
        }

        private static Object LoadAssetCached(string path, string asset)
        {
            string name = path + "/" + asset;
            if (!_assetCache.ContainsKey(name))
            {
                _assetCache.Add(name, ResourceManager.LoadAsset(path, asset));
            }
            return _assetCache[name];
        }

        private static GameObject LoadPrefabCached(string asset)
        {
            try
            {
                if (!_assetCache.ContainsKey(asset))
                {
                    string[] strArr = asset.Split("/");
                    if (strArr[0] == "Custom")
                    {
                        string bundleName = strArr[1];
                        int length = bundleName.Length + 8;
                        string name = asset.Substring(length);
                        _assetCache.Add(asset, AssetBundleManager.LoadAsset(bundleName, name));
                    }
                    else
                        _assetCache.Add(asset, ResourceManager.LoadAsset("Map", strArr[0] + "/Prefabs/" + strArr[1]));
                    if (asset == "Arenas/CaveMap1")
                        WeatherManager.EnableCaveMap();
                }
                return (GameObject)Instantiate(_assetCache[asset]);
            }
            catch (System.Exception e)
            {
                DebugConsole.Log("Failed to load asset: " + asset + ", " + e.Message, true);
                Errors.Add("Failed to load asset: " + asset + ", " + e.Message);
                return new GameObject();
            }
        }
    }

    static class MapObjectShader
    {
        public static string Default = "Default";
        public static string DefaultNoTint = "DefaultNoTint";
        public static string DefaultTiled = "DefaultTiled";
        // public static string Background = "Background";
        public static string Basic = "Basic";
        public static string Transparent = "Transparent";
        public static string Reflective = "Reflective";
        public static string OldBombExplode = "OldBombExplodeMat";
        public static string CannonRegionMat = "CannonRegionMat";
        public static string BombTexMat = "BombTexMat";
        public static string Smoke1Mat = "Smoke1Mat";

        public static bool IsLegacyShader(string shader)
        {
            return shader == OldBombExplode || shader == CannonRegionMat || shader == BombTexMat || shader == Smoke1Mat;
        }
    }

    static class MapObjectCollideMode
    {
        public static string Physical = "Physical";
        public static string Region = "Region";
        public static string None = "None";
    }

    static class MapObjectCollideWith
    {
        public static string All = "All";
        public static string MapObjects = "MapObjects";
        public static string Characters = "Characters";
        public static string Projectiles = "Projectiles";
        public static string Entities = "Entities";
        public static string Hitboxes = "Hitboxes";
        public static string MapEditor = "MapEditor";
    }

    static class MapObjectPhysicsMaterial
    {
        public static string Default = "Default";
        public static string Ice = "IceMaterial";
    }
}
