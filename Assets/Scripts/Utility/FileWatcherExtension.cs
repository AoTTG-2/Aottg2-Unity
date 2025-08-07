using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utility
{
    public class FileWatcherExtension : MonoBehaviour
    {
        [Header("Watcher Settings")]
        [SerializeField] private List<string> watchPaths = new();

        [SerializeField] private bool _includeSubdirectories = true;
        [SerializeField]
        private NotifyFilters _notifyFilter =
            NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;

        [Header("Dispatch Settings")]
        [SerializeField] private int maxActionsPerFrame = 10;

        private readonly Dictionary<string, FileSystemWatcher> _watchers = new();
        private readonly ConcurrentQueue<Action> _mainThreadQueue = new();

        // Public events
        public event FileSystemEventHandler Changed;
        public event FileSystemEventHandler Created;
        public event FileSystemEventHandler Deleted;
        public event RenamedEventHandler Renamed;

        // Public properties that update all watchers when changed
        public bool IncludeSubdirectories
        {
            get => _includeSubdirectories;
            set
            {
                if (_includeSubdirectories == value)
                    return;

                _includeSubdirectories = value;

                foreach (var watcher in _watchers.Values)
                {
                    watcher.IncludeSubdirectories = value;
                }
            }
        }

        public NotifyFilters NotifyFilter
        {
            get => _notifyFilter;
            set
            {
                if (_notifyFilter == value)
                    return;

                _notifyFilter = value;

                foreach (var watcher in _watchers.Values)
                {
                    watcher.NotifyFilter = value;
                }
            }
        }

        void Awake()
        {
            foreach (var path in watchPaths)
            {
                AddWatcher(path);
            }
        }

        void Update()
        {
            int actionsProcessed = 0;

            while (actionsProcessed < maxActionsPerFrame && _mainThreadQueue.TryDequeue(out var action))
            {
                action?.Invoke();
                actionsProcessed++;
            }
        }

        void OnDestroy()
        {
            DisposeWatchers();
        }

        public void AddWatcher(string path)
        {
            if (_watchers.ContainsKey(path))
                return;

            if (!Directory.Exists(path))
            {
                Debug.LogWarning($"Directory does not exist: {path}");
                return;
            }

            var watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = IncludeSubdirectories,
                NotifyFilter = NotifyFilter,
                EnableRaisingEvents = true
            };

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;

            _watchers[path] = watcher;
        }

        public void RemoveWatcher(string path)
        {
            if (!_watchers.TryGetValue(path, out var watcher))
                return;

            watcher.EnableRaisingEvents = false;
            watcher.Changed -= OnChanged;
            watcher.Created -= OnCreated;
            watcher.Deleted -= OnDeleted;
            watcher.Renamed -= OnRenamed;
            watcher.Dispose();

            _watchers.Remove(path);
        }

        private void DisposeWatchers()
        {
            foreach (var watcher in _watchers.Values)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= OnChanged;
                watcher.Created -= OnCreated;
                watcher.Deleted -= OnDeleted;
                watcher.Renamed -= OnRenamed;
                watcher.Dispose();
            }

            _watchers.Clear();
        }

        // Internal event handlers — enqueue for main thread
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            _mainThreadQueue.Enqueue(() => Changed?.Invoke(sender, e));
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            _mainThreadQueue.Enqueue(() => Created?.Invoke(sender, e));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _mainThreadQueue.Enqueue(() => Deleted?.Invoke(sender, e));
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            _mainThreadQueue.Enqueue(() => Renamed?.Invoke(sender, e));
        }

        public IEnumerable<string> WatchedPaths => _watchers.Keys;
    }
}
