using ApplicationManagers;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// UI element for setting background images on visual elements.
    /// </summary>
    /// <code>
    /// # Example: Create a background image
    /// container = UI.CreateContainer();
    /// image = UI.CreateImage();
    /// image.SetImage("Icons/Game/BladeIcon");
    /// container.SetBackgroundImage(image);
    /// </code>
    [CLType(Name = "Image", Abstract = true)]
    partial class CustomLogicImageBuiltin : BuiltinClassInstance
    {
        private string _currentImagePath;
        private Texture2D _currentTexture;

        /// <summary>
        /// Creates a new Image instance.
        /// </summary>
        [CLConstructor]
        public CustomLogicImageBuiltin()
        {
            _currentImagePath = string.Empty;
            _currentTexture = null;
        }

        /// <example>
        /// image.SetImage("Icons/Game/BladeIcon");
        /// image.SetImage("Icons/Specials/NoneSpecialIcon");
        /// </example>
        /// <summary>
        /// Set the image from a resource path.
        /// </summary>
        /// <param name="imagePath">Path to the image resource (e.g., "Icons/Game/BladeIcon").</param>
        [CLMethod]
        public CustomLogicImageBuiltin SetImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                _currentImagePath = string.Empty;
                _currentTexture = null;
                return this;
            }

            // Validate path to prevent path traversal attacks
            if (!Util.IsValidResourcePath(imagePath))
            {
                Debug.LogWarning($"Failed to load image at path: {imagePath}");
                return this;
            }

            var asset = ResourceManager.LoadAsset(ResourcePaths.UI, imagePath, true);

            // Ensure only Texture2D assets are loaded, not prefabs or other types
            if (asset != null && asset is Texture2D texture)
            {
                _currentTexture = texture;
                _currentImagePath = imagePath;
            }
            else if (asset != null)
            {
                Debug.LogWarning($"Failed to load image at path: {imagePath}");
            }
            else
            {
                Debug.LogWarning($"Failed to load image at path: {imagePath}");
            }

            return this;
        }

        /// <summary>
        /// The current image path. Setting this will load the image from the resource path.
        /// </summary>
        [CLProperty]
        public string ImagePath
        {
            get => _currentImagePath;
            set => SetImage(value);
        }

        internal Texture2D GetTexture()
        {
            return _currentTexture;
        }
    }
}
