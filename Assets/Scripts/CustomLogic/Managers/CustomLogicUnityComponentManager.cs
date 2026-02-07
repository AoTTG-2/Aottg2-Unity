using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace CustomLogic
{
    static class CustomLogicUnityComponentManager
    {
        private struct ComponentTypePair
        {
            public readonly Type UnityComponentType;
            public readonly Type CLComponentType;

            public ComponentTypePair(Type unityComponentType, Type clComponentType)
            {
                UnityComponentType = unityComponentType;
                CLComponentType = clComponentType;
            }
        }

        // Add more items to this object to expose more Unity component
        private static readonly Dictionary<string, ComponentTypePair> _unityComponentTypes = new Dictionary<string, ComponentTypePair>()
        {
            {CustomLogicUnityComponentEnum.VideoPlayer, new ComponentTypePair(typeof(VideoPlayer), typeof(CustomLogicVideoPlayerBuiltin))}
        };

        public static BuiltinComponentInstance GetUnityComponentByNameWithMapObjectOwner(CustomLogicMapObjectBuiltin owner, string name)
        {
            if(_unityComponentTypes.ContainsKey(name))
            {
                Type unityType = _unityComponentTypes[name].UnityComponentType;
                Type clType = _unityComponentTypes[name].CLComponentType;

                if(owner.Transform.Value.TryGetComponent(unityType, out var component))
                {
                    object[] arguments = new object[] { owner, component };
                    return (BuiltinComponentInstance) Activator.CreateInstance(clType, arguments);
                }
            }
            return null;
        }
    }
}