using ApplicationManagers;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// UI element for displaying icons/images.
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

        /// <example>
        /// icon.SetIcon("Icons/Game/BladeIcon");
        /// icon.SetIcon("Icons/Specials/NoneSpecialIcon");
        /// icon.SetIcon("Icons/Profile/Eren1Icon");
        /// </example>
        /// <summary>
        /// Set the icon to display from a resource path.
        /// </summary>
        /// <param name="iconPath">Path to the icon resource (e.g., "Icons/Game/BladeIcon").</param>
        [CLMethod]
        public CustomLogicIconBuiltin SetIcon(string iconPath)
        {
            if (string.IsNullOrEmpty(iconPath))
            {
                _image.image = null;
                _currentIconPath = string.Empty;
                return this;
            }

            // Validate path to prevent path traversal attacks
            if (!Util.IsValidResourcePath(iconPath))
            {
                Debug.LogWarning($"Failed to load icon at path: {iconPath}");
                return this;
            }

            var asset = ResourceManager.LoadAsset(ResourcePaths.UI, iconPath, true);

            // Ensure only Texture2D assets are loaded, not prefabs or other types
            if (asset != null && asset is Texture2D texture)
            {
                _image.image = texture;
                _currentIconPath = iconPath;
            }
            else if (asset != null)
            {
                Debug.LogWarning($"Failed to load icon at path: {iconPath}");
            }
            else
            {
                Debug.LogWarning($"Failed to load icon at path: {iconPath}");
            }

            return this;
        }

        /// <summary>
        /// The current icon path. Setting this will load the icon from the resource path.
        /// </summary>
        [CLProperty]
        public string IconPath
        {
            get => _currentIconPath;
            set => SetIcon(value);
        }

        /// <summary>
        /// Set the tint color of the icon.
        /// </summary>
        /// <param name="color">The color to tint the icon with.</param>
        [CLMethod]
        public CustomLogicIconBuiltin SetTintColor(CustomLogicColorBuiltin color)
        {
            _image.tintColor = color.Value.ToColor();
            return this;
        }

        /// <remarks>
        /// Valid values: "ScaleAndCrop", "ScaleToFit", "StretchToFill"
        /// </remarks>
        /// <summary>
        /// The scale mode for the icon.
        /// </summary>
        [CLProperty(Enum = typeof(CustomLogicScaleModeEnum))]
        public string ScaleMode
        {
            get => _image.scaleMode.ToString();
            set
            {
                if (value == CustomLogicScaleModeEnum.ScaleAndCrop)
                    _image.scaleMode = UnityEngine.ScaleMode.ScaleAndCrop;
                else if (value == CustomLogicScaleModeEnum.ScaleToFit)
                    _image.scaleMode = UnityEngine.ScaleMode.ScaleToFit;
                else if (value == CustomLogicScaleModeEnum.StretchToFill)
                    _image.scaleMode = UnityEngine.ScaleMode.StretchToFill;
                else
                    throw new System.Exception($"Unknown scale mode: {value}. Valid values are 'ScaleAndCrop', 'ScaleToFit', and 'StretchToFill'");
            }
        }
    }
}
