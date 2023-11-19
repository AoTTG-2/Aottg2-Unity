using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    class PhysicsLayer
    {
        public static int UI = 5;
        public static int NoCollision = 8;
        public static int Hitbox = 9;
        public static int Human = 10;
        public static int TitanMovebox = 11;
        public static int TitanPushbox = 12;
        public static int Hurtbox = 13;
        public static int Projectile = 14;
        public static int EntityDetection = 15;
        public static int CharacterDetection = 16;
        public static int NPC = 17;
        public static int MapObjectMapObjects = 20;
        public static int MapObjectProjectiles = 21;
        public static int MapObjectCharacters = 22;
        public static int MapObjectEntities = 23;
        public static int MapObjectAll = 24;
        public static int MapEditorObject = 25;
        public static int MapEditorGizmo = 26;
        public static int MinimapIcon = 27;
        private static Dictionary<int, LayerMask> _masks = new Dictionary<int, LayerMask>();

        public static void Init()
        {
            SetLayerCollisions(NoCollision, new int[0]);
            SetLayerCollisions(Hitbox, new int[] { Human, TitanPushbox, Hurtbox });
            SetLayerCollisions(Human, new int[] { Hitbox, TitanPushbox, Projectile, EntityDetection, CharacterDetection, 
                MapObjectAll, MapObjectEntities, MapObjectCharacters});
            SetLayerCollisions(TitanMovebox, new int[] { TitanMovebox, EntityDetection, CharacterDetection, MapObjectAll, MapObjectEntities, MapObjectCharacters });
            SetLayerCollisions(TitanPushbox, new int[] { Hitbox, Human, Projectile, NPC });
            SetLayerCollisions(Projectile, new int[] { Human, TitanPushbox, EntityDetection, MapObjectEntities, MapObjectAll, MapObjectProjectiles });
            SetLayerCollisions(EntityDetection, new int[] { Human, TitanMovebox, Projectile });
            SetLayerCollisions(CharacterDetection, new int[] { Human, TitanMovebox });
            SetLayerCollisions(NPC, new int[] {TitanPushbox, MapObjectCharacters, MapObjectEntities, MapObjectAll});
            SetLayerCollisions(Hurtbox, new int[] { Hitbox });
            SetLayerCollisions(MapObjectMapObjects, new int[] { MapObjectAll, MapObjectMapObjects, MapObjectProjectiles, MapObjectCharacters, MapObjectEntities });
            SetLayerCollisions(MapObjectProjectiles, new int[] { MapObjectAll, MapObjectMapObjects, Projectile });
            SetLayerCollisions(MapObjectCharacters, new int[] { MapObjectAll, MapObjectMapObjects, Human, TitanMovebox, NPC });
            SetLayerCollisions(MapObjectEntities, new int[] { MapObjectAll, MapObjectMapObjects, TitanMovebox, Human, Projectile, NPC });
            SetLayerCollisions(MapObjectAll, new int[] { Human, TitanMovebox, Projectile, MapObjectAll, MapObjectMapObjects, MapObjectEntities, 
                MapObjectCharacters, MapObjectProjectiles, NPC});
            SetLayerCollisions(MapEditorObject, new int[0]);
            SetLayerCollisions(MapEditorGizmo, new int[0]);
        }

        public static LayerMask GetMask(params int[] layers)
        {
            if (layers.Length == 0)
                return 0;
            LayerMask layerMask = 1 << layers[0];
            for (int i = 1; i < layers.Length; i++)
                layerMask = layerMask | (1 <<layers[i]);
            return layerMask;
        }

        public static LayerMask CopyMask(int originLayer)
        {
            return _masks[originLayer];
        }

        private static void SetLayerCollisions(int layer, int[] others)
        {
            for (int i = 0; i < 32; i++)
                Physics.IgnoreLayerCollision(layer, i, true);
            foreach (int i in others)
                Physics.IgnoreLayerCollision(layer, i, false);
            _masks.Add(layer, GetMask(others));
        }
    }
}
