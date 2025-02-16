using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapEditor
{
    public class VirtualTreeView
    {
        public List<VirtualTreeViewItem> Items = new List<VirtualTreeViewItem>();
        private Dictionary<int, VirtualTreeViewItem> itemDictionary = new Dictionary<int, VirtualTreeViewItem>();

        public void Clear()
        {
            Items.Clear();
            itemDictionary.Clear();
        }

        // Method to add an item to the tree
        public void AddItem(VirtualTreeViewItem item)
        {
            // Set the level based on the parent's level
            if (item.ParentID == -1)
            {
                item.Level = 0; // Root level
            }
            else if (itemDictionary.TryGetValue(item.ParentID, out var parent))
            {
                item.Level = parent.Level + 2;
            }
            else
            {
                item.Level = 0; // Default to root level if parent not found
            }

            Items.Add(item);
            itemDictionary[item.ID] = item;
        }

        // Method to get the flattened tree
        public List<VirtualTreeViewItem> GetFlattenedTree()
        {
            List<VirtualTreeViewItem> flattenedTree = new List<VirtualTreeViewItem>();

            // Get all root elements
            List<VirtualTreeViewItem> rootElements = Items.Where(item => item.Level == 0).ToList();

            foreach (var item in rootElements)
            {
                if (item.Level == 0)
                {
                    flattenedTree.Add(item);
                    if (item.Expanded)
                    {
                        AddChildren(item, flattenedTree);
                    }
                }
            }
            return flattenedTree;
        }

        // Optimized method to check if the parent of an item is expanded
        private bool IsParentExpanded(VirtualTreeViewItem item)
        {
            if (item.ParentID == -1) return true; // Root items are always considered expanded
            return itemDictionary.TryGetValue(item.ParentID, out var parent) && parent.Expanded;
        }

        // Helper method to add children of an item to the flattened tree
        private void AddChildren(VirtualTreeViewItem parent, List<VirtualTreeViewItem> flattenedTree)
        {
            foreach (var item in Items)
            {
                if (item.ParentID == parent.ID)
                {
                    flattenedTree.Add(item);
                    if (item.Expanded)
                    {
                        AddChildren(item, flattenedTree);
                    }
                }
            }
        }

        public List<VirtualTreeViewItem> GetChildrenRecursive(VirtualTreeViewItem parent)
        {
            List<VirtualTreeViewItem> children = new List<VirtualTreeViewItem>();
            foreach (var item in Items)
            {
                if (item.ParentID == parent.ID)
                {
                    children.Add(item);
                    children.AddRange(GetChildrenRecursive(item));
                }
            }
            return children;
        }

        // Method to check if an item has children
        public bool HasChildren(VirtualTreeViewItem item)
        {
            foreach (var child in Items)
            {
                if (child.ParentID == item.ID)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class VirtualTreeViewItem
    {
        public int ID;
        public int ParentID;
        public int SiblingID;
        public int Level;
        public bool Expanded;
        public GameObject GameObject;
    }
}
