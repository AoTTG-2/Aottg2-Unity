using System;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;
using ApplicationManagers;

namespace CustomLogic
{
    /// <summary>
    /// UI element for displaying icons/images
    /// </summary>
    /// <code>
    /// # Example: Create an icon element
    /// icon = UI.CreateIcon("Icons/Game/BladeIcon")
    ///     .Width(64)
    ///     .Height(64);
    /// container.Add(icon);
    /// 
    /// # Example: Load an icon from a resource path
    /// weaponIcon = UI.CreateIcon("Icons/Game/AHSSIcon");
    /// weaponIcon.SetIcon("Icons/Game/ThunderSpearIcon"); # Change the icon dynamically
    /// </code>
    [CLType(Name = "Icon", Abstract = true)]
    partial class CustomLogicIconBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly Image _image;
        private string _currentIconPath;

        public CustomLogicIconBuiltin(Image image) : base(image)
        {
            _image = image;
            _currentIconPath = string.Empty;
        }

        /// <summary>
        /// Set the icon to display from a resource path
        /// </summary>
        /// <param name="iconPath">Path to the icon resource (e.g., "Icons/Game/BladeIcon")</param>
        /// <example>
        /// icon.SetIcon("Icons/Game/BladeIcon");
        /// icon.SetIcon("Icons/Specials/NoneSpecialIcon");
        /// icon.SetIcon("Icons/Profile/Eren1Icon");
        /// </example>
        [CLMethod]
        public CustomLogicIconBuiltin SetIcon(string iconPath)
        {
            if (string.IsNullOrEmpty(iconPath))
            {
                _image.image = null;
                _currentIconPath = string.Empty;
                return this;
            }

            var texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, iconPath, true);
            if (texture != null)
            {
                _image.image = texture;
                _currentIconPath = iconPath;
            }
            else
            {
                Debug.LogWarning($"Failed to load icon at path: {iconPath}");
            }

            return this;
        }

        /// <summary>
        /// Get the current icon path
        /// </summary>
        [CLProperty]
        public string IconPath
        {
            get => _currentIconPath;
            set => SetIcon(value);
        }

        /// <summary>
        /// Set the tint color of the icon
        /// </summary>
        [CLMethod]
        public CustomLogicIconBuiltin SetTintColor(CustomLogicColorBuiltin color)
        {
            _image.tintColor = color.Value.ToColor();
            return this;
        }

        /// <summary>
        /// Get or set the scale mode for the icon
        /// </summary>
        /// <remarks>
        /// Valid values: "ScaleAndCrop", "ScaleToFit"
        /// </remarks>
        [CLProperty]
        public string ScaleMode
        {
            get => _image.scaleMode.ToString();
            set
            {
                if (value == "ScaleAndCrop")
                    _image.scaleMode = UnityEngine.ScaleMode.ScaleAndCrop;
                else if (value == "ScaleToFit")
                    _image.scaleMode = UnityEngine.ScaleMode.ScaleToFit;
                else
                    throw new System.Exception($"Unknown scale mode: {value}. Valid values are 'ScaleAndCrop' and 'ScaleToFit'");
            }
        }
    }
}
