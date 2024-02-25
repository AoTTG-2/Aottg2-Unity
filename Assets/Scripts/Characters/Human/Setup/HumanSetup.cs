using UnityEngine;
using SimpleJSONFixed;
using ApplicationManagers;
using Utility;
using Settings;
using CustomSkins;
using GameManagers;
using System.Globalization;

namespace Characters
{
    class HumanSetup: MonoBehaviour
    {
        public static JSONNode CostumeInfo;
        public static JSONNode HairInfo;
        public static Material WeaponTrailMaterial;
        public GameObject _mount_chest;
        public GameObject _mount_3dmg;
        public GameObject _mount_gas_l;
        public GameObject _mount_gas_r;
        public GameObject _mount_gun_mag_l;
        public GameObject _mount_gun_mag_r;
        public GameObject _mount_weapon_l;
        public GameObject _mount_weapon_r;
        public GameObject _mount_ts_l;
        public GameObject _mount_ts_r;
        public GameObject _part_3dmg;
        public GameObject _part_belt;
        public GameObject _part_gas_l;
        public GameObject _part_gas_r;
        public GameObject _part_arm_l;
        public GameObject _part_arm_r;
        public GameObject _part_blade_l;
        public GameObject _part_blade_r;
        public GameObject _part_brand_1;
        public GameObject _part_brand_2;
        public GameObject _part_brand_3;
        public GameObject _part_brand_4;
        public GameObject _part_cape;
        public GameObject _part_chest;
        public GameObject _part_chest_1;
        public GameObject _part_chest_2;
        public GameObject _part_chest_3;
        public GameObject _part_eye;
        public GameObject _part_face;
        public GameObject _part_glass;
        public GameObject _part_hair;
        public GameObject _part_hair_1;
        public GameObject _part_hand_l;
        public GameObject _part_hand_r;
        public GameObject _part_head;
        public GameObject _part_leg;
        public GameObject _part_upper_body;
        public GameObject _mount_cloth;
        public HumanSetupMeshes _meshes;
        public HumanSetupTextures _textures;
        public MeleeWeaponTrail LeftTrail;
        public MeleeWeaponTrail RightTrail;

        // loaded settings from player spawning
        public HumanCustomSet CustomSet;
        public HumanWeapon Weapon;
        public JSONNode CurrentCostume;
        public JSONNode CurrentHair;
        public bool IsDeadBody;
        public bool Deleted = false;

        // info
        public static int EyeCount;
        public static int FaceCount;
        public static int GlassCount;
        public static int CostumeMCount;
        public static int CostumeFCount;
        public static int HairMCount;
        public static int HairFCount;
        

        public static void Init()
        {
            JSONNode costume = JSON.Parse(ResourceManager.TryLoadText(ResourcePaths.Info, "CostumeInfo"));
            CostumeInfo = costume["Costume"];
            HairInfo = costume["Hair"];
            EyeCount = costume["EyeCount"].AsInt;
            FaceCount = costume["FaceCount"].AsInt;
            GlassCount = costume["GlassCount"].AsInt;
            CostumeMCount = CostumeInfo["Male"].Count;
            CostumeFCount = CostumeInfo["Female"].Count;
            HairMCount = HairInfo["Male"].Count;
            HairFCount = HairInfo["Female"].Count;
            HumanSetupMaterials.Init();
        }

        private void Awake()
        {
            _meshes = new HumanSetupMeshes(this);
            _textures = new HumanSetupTextures(this);
            _part_head = transform.Find("Armature/Core/Controller_Body/hip/spine/chest/neck/head/char_head").gameObject;
            _part_leg = transform.Find("character_leg").gameObject;
            _part_chest = transform.Find("character_chest").gameObject;
            _mount_chest = CreateMount("spine/chest");
            _mount_3dmg = CreateMount("spine/chest");
            _mount_gas_l = CreateMount("spine");
            _mount_gas_r = CreateMount("spine");
            _mount_gun_mag_l = CreateMount("thigh_L");
            _mount_gun_mag_r = CreateMount("thigh_R");
            _mount_weapon_l = CreateMount("spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
            _mount_weapon_r = CreateMount("spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
            _mount_ts_l = CreateMount("spine/chest/shoulder_L/upper_arm_L/forearm_L");
            _mount_ts_r = CreateMount("spine/chest/shoulder_R/upper_arm_R/forearm_R");
            _mount_cloth = _part_leg;
        }

        public static int GetCostumeCount(bool male)
        {
            if (male)
                return CostumeInfo["Male"].AsArray.Count;
            return CostumeInfo["Female"].AsArray.Count;
        }

        public static int GetHairCount()
        {
            return HairInfo.AsArray.Count;
        }

        public void Copy(InGameCharacterSettings settings)
        {
            var customSets = SettingsManager.HumanCustomSettings.CustomSets;
            int setIndex = settings.CustomSet.Value;
            int costumeIndex = settings.Costume.Value;
            int preCount = SettingsManager.HumanCustomSettings.Costume1Sets.Sets.GetCount();
            if (setIndex < preCount)
            {
                if (costumeIndex == 1)
                    CustomSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.Costume2Sets.Sets.GetItemAt(setIndex);
                else if (costumeIndex == 2)
                    CustomSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.Costume3Sets.Sets.GetItemAt(setIndex);
                else
                    CustomSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.Costume1Sets.Sets.GetItemAt(setIndex);
            }
            else
                CustomSet = (HumanCustomSet)customSets.Sets.GetItemAt(setIndex - preCount);
            var loadout = settings.Loadout.Value;
            if (loadout == HumanLoadout.Blades)
                Weapon = HumanWeapon.Blade;
            else if (loadout == HumanLoadout.AHSS)
                Weapon = HumanWeapon.AHSS;
            else if (loadout == HumanLoadout.APG)
                Weapon = HumanWeapon.APG;
            else if (loadout == HumanLoadout.Thunderspears)
                Weapon = HumanWeapon.Thunderspear;
        }

        public void Load(HumanCustomSet customSet, HumanWeapon weapon, bool isDeadBody = false)
        {
            CustomSet = customSet;
            Weapon = weapon;
            IsDeadBody = isDeadBody;
            if (CustomSet.Sex.Value == (int)HumanSex.Male)
            {
                if (CustomSet.Costume.Value >= CostumeMCount)
                    CustomSet.Costume.Value = 0;
                CurrentCostume = CostumeInfo["Male"][CustomSet.Costume.Value];
            }
            else
            {
                if (CustomSet.Costume.Value >= CostumeFCount)
                    CustomSet.Costume.Value = 0;
                CurrentCostume = CostumeInfo["Female"][CustomSet.Costume.Value];
            }
            string hair = CustomSet.Hair.Value;
            if (hair.StartsWith("HairM"))
                CurrentHair = HairInfo["Male"][int.Parse(hair.Substring(5))];
            else if (hair.StartsWith("HairF"))
                CurrentHair = HairInfo["Female"][int.Parse(hair.Substring(5))];
            if (CurrentCostume == null)
                DebugConsole.Log("Warning: costume does not exist in CostumeInfo JSON.");
            if (CurrentHair == null)
                DebugConsole.Log("Warning: hair does not exist in CostumeInfo JSON");
            DeleteParts();
            CreateParts();
        }

        public void DeleteDie()
        {
            if (Deleted)
                return;
            DeleteParts();
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                renderer.enabled = false;
            Deleted = true;
        }

        public void CreateParts()
        {
            var bodyMaterial = HumanSetupMaterials.GetCostumeMaterial(_textures.GetBodyMainTexture(), _textures.GetBodyMaskTexture(),
                _textures.GetBodyColorTexture(), _textures.GetBodyPantsTexture(), CustomSet.ShirtColor.Value.ToColor(), CustomSet.StrapsColor.Value.ToColor(), CustomSet.PantsColor.Value.ToColor(), CustomSet.JacketColor.Value.ToColor(), CustomSet.BootsColor.Value.ToColor());
            CreateHead();
            CreateUpperBody(bodyMaterial);
            CreateArms(bodyMaterial);
            CreateLowerBody(bodyMaterial);
            Create3dmg();
            CreateWeapon();
        }

        public void DeleteParts()
        {
            if (!IsDeadBody)
            {
                DestroyIfExists(_part_hair_1);
                DestroyIfExists(_part_cape);
                DestroyIfExists(_part_chest_3);
            }
            DestroyIfExists(_part_eye);
            DestroyIfExists(_part_face);
            DestroyIfExists(_part_glass);
            DestroyIfExists(_part_hair);
            DestroyIfExists(_part_upper_body);
            DestroyIfExists(_part_arm_l);
            DestroyIfExists(_part_arm_r);
            DestroyIfExists(_part_brand_1);
            DestroyIfExists(_part_brand_2);
            DestroyIfExists(_part_brand_3);
            DestroyIfExists(_part_brand_4);
            DestroyIfExists(_part_chest_1);
            DestroyIfExists(_part_chest_2);
            DestroyIfExists(_part_3dmg);
            DestroyIfExists(_part_belt);
            DestroyIfExists(_part_gas_l);
            DestroyIfExists(_part_gas_r);
            DestroyIfExists(_part_blade_l);
            DestroyIfExists(_part_blade_r);
        }

        public void Create3dmg()
        {
            DestroyIfExists(_part_3dmg);
            DestroyIfExists(_part_belt);
            DestroyIfExists(_part_gas_l);
            DestroyIfExists(_part_gas_r);
            Material material = HumanSetupMaterials.GetPartMaterial(_textures.Get3dmgTexture());
            _part_3dmg = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, _meshes.Get3dmgMesh(), cached: true);
            AttachToMount(_part_3dmg, _mount_3dmg);
            _part_3dmg.GetComponent<Renderer>().material = material;
            string beltMesh = _meshes.GetBeltMesh();
            if (beltMesh != string.Empty)
            {
                _part_belt = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, beltMesh, cached: true);
                _part_belt.GetComponent<Renderer>().material = material;
                AttachToMount(_part_belt, _mount_3dmg);
            }
            _part_gas_l = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, _meshes.GetGasMesh(left: true), cached: true);
            _part_gas_l.GetComponent<Renderer>().material = material;
            if (Weapon == HumanWeapon.AHSS || Weapon == HumanWeapon.APG)
                AttachToMount(_part_gas_l, _mount_gun_mag_l);
            else
                AttachToMount(_part_gas_l, _mount_gas_l);
            _part_gas_r = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, _meshes.GetGasMesh(left: false), cached: true);
            _part_gas_r.GetComponent<Renderer>().material = material;
            if (Weapon == HumanWeapon.AHSS || Weapon == HumanWeapon.APG)
                AttachToMount(_part_gas_r, _mount_gun_mag_r);
            else
                AttachToMount(_part_gas_r, _mount_gas_r);
        }

        public void CreateWeapon()
        {
            DestroyIfExists(_part_blade_l);
            DestroyIfExists(_part_blade_r);
            Material material = HumanSetupMaterials.GetPartMaterial(_textures.Get3dmgTexture());
            string weaponLMesh = _meshes.GetWeaponMesh(left: true);
            if (weaponLMesh != string.Empty)
            {
                _part_blade_l = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, weaponLMesh, cached: true);
                if (weaponLMesh.Contains("thunderspear"))
                    AttachToMount(_part_blade_l, _mount_ts_l);
                else
                {
                    AttachToMount(_part_blade_l, _mount_weapon_l);
                    _part_blade_l.GetComponent<Renderer>().material = material;
                }
                if (_part_blade_l.GetComponentInChildren<MeleeWeaponTrail>() != null)
                {
                    LeftTrail = _part_blade_l.GetComponentInChildren<MeleeWeaponTrail>();
                    LeftTrail.Emit = false;
                }

            }
            string weaponRMesh = _meshes.GetWeaponMesh(left: false);
            if (weaponRMesh != string.Empty)
            {
                _part_blade_r = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, weaponRMesh, cached: true);
                if (weaponRMesh.Contains("thunderspear"))
                    AttachToMount(_part_blade_r, _mount_ts_r);
                else
                {
                    AttachToMount(_part_blade_r, _mount_weapon_r);
                    _part_blade_r.GetComponent<Renderer>().material = material;
                }
                if (_part_blade_r.GetComponentInChildren<MeleeWeaponTrail>() != null)
                {
                    RightTrail = _part_blade_r.GetComponentInChildren<MeleeWeaponTrail>();
                    RightTrail.Emit = false;
                }
            }
        }

        public void CreateCape()
        {
            if (!IsDeadBody)
            {
                DestroyIfExists(_part_cape);
                string capeMesh = _meshes.GetCapeMesh();
                if (capeMesh != string.Empty)
                {
                    _part_cape = ClothFactory.GetCape(_mount_cloth, capeMesh, HumanSetupMaterials.GetPartMaterial(_textures.GetBrandTexture()));
                }
            }
        }

        public void CreateHair()
        {
            DestroyIfExists(_part_hair);
            if (!IsDeadBody)
                ClothFactory.DisposeObject(_part_hair_1);
            string hairMesh = _meshes.GetHairMesh();
            if (hairMesh != string.Empty)
            {
                _part_hair = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, hairMesh, cached: true);
                AttachToMount(_part_hair, _part_head);
                _part_hair.GetComponent<Renderer>().material = HumanSetupMaterials.GetHairMaterial(_textures.GetHairTexture());
                _part_hair.GetComponent<Renderer>().material.color = CustomSet.HairColor.Value.ToColor();
            }
            string hairClothMesh = _meshes.GetHairClothMesh();
            if (hairClothMesh != string.Empty && !IsDeadBody)
            {
                Material material = HumanSetupMaterials.GetHairMaterial(_textures.GetHairTexture());
                _part_hair_1 = ClothFactory.GetHair(_mount_cloth, hairClothMesh, material, CustomSet.HairColor.Value.ToColor());
            }
        }

        public void CreateEye()
        {
            DestroyIfExists(_part_eye);
            _part_eye = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, _meshes.GetEyeMesh(), cached: true);
            AttachToMount(_part_eye, _part_head);
            SetFacialTexture(_part_eye, CustomSet.Eye.Value, true);
        }

        public void CreateFace()
        {
            DestroyIfExists(_part_face);
            _part_face = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, _meshes.GetFaceMesh(), cached: true);
            AttachToMount(_part_face, _part_head);
            string face = CustomSet.Face.Value.Substring(4);
            if (face != "None")
                SetFacialTexture(_part_face, int.Parse(face), false);
            else
                SetFacialTexture(_part_face, -1, false);
        }

        public void CreateGlass()
        {
            _part_glass = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, _meshes.GetGlassMesh(), cached: true);
            AttachToMount(_part_glass, _part_head);
            string glass = CustomSet.Glass.Value.Substring(5);
            if (glass != "None")
                SetFacialTexture(_part_glass, int.Parse(glass), false);
            else
                SetFacialTexture(_part_glass, -1, false);
        }

        public void CreateHead()
        {
            CreateHair();
            CreateEye();
            CreateFace();
            CreateGlass();
        }

        public void CreateArms(Material bodyMaterial)
        {
            DestroyIfExists(_part_arm_l);
            DestroyIfExists(_part_hand_l);
            DestroyIfExists(_part_arm_r);
            DestroyIfExists(_part_hand_r);
            Material skinMaterial = HumanSetupMaterials.GetPartMaterial(_textures.GetSkinTexture());
            _part_arm_l = GenerateCloth(_meshes.GetArmMesh(left: true));
            SetMaterial(_part_arm_l.GetComponent<Renderer>(), bodyMaterial);
            _part_hand_l = GenerateCloth(_meshes.GetHandMesh(left: true));
            _part_hand_l.GetComponent<Renderer>().material = skinMaterial;
            _part_arm_r = GenerateCloth(_meshes.GetArmMesh(left: false));
            SetMaterial(_part_arm_r.GetComponent<Renderer>(), bodyMaterial);
            _part_hand_r = GenerateCloth(_meshes.GetHandMesh(left: false));
            _part_hand_r.GetComponent<Renderer>().material = skinMaterial;
        }

        private void SetMaterial(Renderer renderer, Material material)
        {
            renderer.material = material;
        }

        public void CreateLowerBody(Material bodyMaterial)
        {
            SetMaterial(_part_leg.GetComponent<SkinnedMeshRenderer>(), bodyMaterial);
        }

        public void CreateUpperBody(Material bodyMaterial)
        {
            DestroyIfExists(_part_upper_body);
            DestroyIfExists(_part_brand_1);
            DestroyIfExists(_part_brand_2);
            DestroyIfExists(_part_brand_3);
            DestroyIfExists(_part_brand_4);
            DestroyIfExists(_part_chest_1);
            DestroyIfExists(_part_chest_2);
            if (!IsDeadBody)
            {
                DestroyIfExists(_part_chest_3);
            }
            CreateCape();
            string chest1Mesh = _meshes.GetChestMesh(1);
            if (chest1Mesh != string.Empty)
            {
                _part_chest_1 = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, chest1Mesh, cached: true);
                AttachToMount(_part_chest_1, _mount_chest);
                _part_chest_1.GetComponent<Renderer>().material = HumanSetupMaterials.GetPartMaterial(_textures.GetChestTexture(1));
            }
            string chest2Mesh = _meshes.GetChestMesh(2);
            if (chest2Mesh != string.Empty)
            {
                _part_chest_2 = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, chest2Mesh, cached: true);
                AttachToMount(_part_chest_2, _mount_chest);
                _part_chest_2.GetComponent<Renderer>().material = bodyMaterial;
            }
            string chest3Mesh = _meshes.GetChestMesh(3);
            if (chest3Mesh != string.Empty && !IsDeadBody)
            {
                _part_chest_3 = ClothFactory.GetCape(_mount_cloth, chest3Mesh, bodyMaterial);
            }
            _part_upper_body = GenerateCloth(_meshes.GetBodyMesh());
            SetMaterial(_part_upper_body.GetComponent<Renderer>(), bodyMaterial);
            Material brandMaterial = HumanSetupMaterials.GetPartMaterial(_textures.GetBrandTexture());
            if (CurrentCostume["Type"].Value.StartsWith("Uniform"))
            {
                _part_brand_1 = GenerateCloth(_meshes.GetBrandMesh(1));
                _part_brand_1.GetComponent<Renderer>().material = brandMaterial;
                _part_brand_2 = GenerateCloth(_meshes.GetBrandMesh(2));
                _part_brand_2.GetComponent<Renderer>().material = brandMaterial;
                _part_brand_3 = GenerateCloth(_meshes.GetBrandMesh(3));
                _part_brand_3.GetComponent<Renderer>().material = brandMaterial;
                _part_brand_4 = GenerateCloth(_meshes.GetBrandMesh(4));
                _part_brand_4.GetComponent<Renderer>().material = brandMaterial;
            }
            Material skinMaterial = HumanSetupMaterials.GetPartMaterial(_textures.GetSkinTexture());
            _part_head.GetComponent<Renderer>().material = skinMaterial;
            _part_chest.GetComponent<Renderer>().material = skinMaterial;
        }

        private void SetFacialTexture(GameObject go, int id, bool eyes)
        {
            if (id >= 0)
            {
                float x = (int)(id / 8f) * 0.125f;
                float y = -0.125f * (id % 8);
                if (eyes)
                    y += 0.125f;
                go.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(x, y);
            }
            else
                go.GetComponent<Renderer>().material = MaterialCache.TransparentMaterial;
        }

        public void SetSkin()
        {
            Material material = HumanSetupMaterials.GetPartMaterial(_textures.GetSkinTexture());
            _part_head.GetComponent<Renderer>().material = material;
            _part_chest.GetComponent<Renderer>().material = material;
            _part_hand_l.GetComponent<Renderer>().material = material;
            _part_hand_r.GetComponent<Renderer>().material = material;
        }

        private GameObject CreateMount(string transformPath)
        {
            GameObject mount = new GameObject();
            transformPath = "Armature/Core/Controller_Body/hip/" + transformPath;
            Transform baseTransform = transform;
            mount.transform.position = baseTransform.position;
            mount.transform.rotation = Quaternion.Euler(270f, baseTransform.rotation.eulerAngles.y, 0f);
            mount.transform.parent = baseTransform.Find(transformPath);
            return mount;
        }

        private GameObject GenerateCloth(string cloth)
        {
            SkinnedMeshRenderer meshRenderer = _mount_cloth.GetComponent<SkinnedMeshRenderer>();
            if (meshRenderer == null)
                meshRenderer = _mount_cloth.AddComponent<SkinnedMeshRenderer>();
            Transform[] bones = meshRenderer.bones;
            SkinnedMeshRenderer newMeshRenderer;
            newMeshRenderer = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, cloth, cached: true).GetComponent<SkinnedMeshRenderer>();
            newMeshRenderer.gameObject.transform.parent = meshRenderer.gameObject.transform.parent;
            newMeshRenderer.transform.localPosition = Vector3.zero;
            newMeshRenderer.transform.localScale = Vector3.one;
            newMeshRenderer.bones = bones;
            newMeshRenderer.quality = SkinQuality.Bone4;
            newMeshRenderer.rootBone = meshRenderer.rootBone;
            return newMeshRenderer.gameObject;
        }

        private void AttachToMount(GameObject obj, GameObject mount)
        {
            obj.transform.SetParent(mount.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }

        private void DestroyIfExists(GameObject go)
        {
            if (go != null)
                Destroy(go);
        }
    }
}
