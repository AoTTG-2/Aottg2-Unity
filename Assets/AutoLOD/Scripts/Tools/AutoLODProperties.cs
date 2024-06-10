#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace AutoLOD.MeshDecimator
{
    public enum MeshDecimatorBackend
    {
        Fast = 0,
        HighQuality
    }

    public class AutoLODProperties : Editor
    { 

        public Renderer _target;
        public bool _customSettings = false;
        public MeshDecimatorBackend _backend = MeshDecimatorBackend.HighQuality;
        public bool _foldout = false;
        public bool _lodGroupFoldout = false;

        public int _lodLevels = 4;
        public float _reductionRate = 2f;
        public float _performance = 0.5f;
        public float _relativeHeightCulling = 0.002f;
        public bool _flatShading = false;

        public bool _writeMeshOnDisk;
        public string _filePath;

        private void OnEnable()
        {
            _filePath = (EditorPrefs.GetString("autolodDefaultExportFolder", "Assets/AutoLOD/Generated")).Replace("//", "/");
            if (_filePath.StartsWith("Assets/"))
                _filePath = _filePath.Substring(7);
        }

        public void Apply(AutoLODProperties other)
        {
            _customSettings = false;
            _backend = other._backend;
            _foldout = false;
            _lodGroupFoldout = false;
            _lodLevels = other._lodLevels;
            _reductionRate = other._reductionRate;
            _performance = other._performance;
            _relativeHeightCulling = other._relativeHeightCulling;
            _flatShading = other._flatShading;
            _writeMeshOnDisk = other._writeMeshOnDisk;
            _filePath = other._filePath;
        }
    }
}

#endif

