using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodeTodoList
{


	public class CTL_Exclude
	{
		public const char SEPARATOR = '?';
		List<string> mExcludedRelativePath = new List<string>();
		List<string> mOptimisedNotRelativePaths = new List<string>();



		public List<string> PathList
		{
			get { return mExcludedRelativePath; }
		}

		public void GenerateOptimisedExcludeList()
		{
			mOptimisedNotRelativePaths.Clear();
			for (int i = 0; i < mExcludedRelativePath.Count; i++)
			{
				if (!IsInsideExcludedFolder(mExcludedRelativePath[i]))
					mOptimisedNotRelativePaths.Add(CTL_Tools.GetNotRelativePath(mExcludedRelativePath[i], false));
			}
		}

		public bool IsInsideExcludedFolder(string path)
		{
			if (mExcludedRelativePath.Count == 0)
				return false;
			path = CTL_Tools.GetNotRelativePath(path, false);
			for (int i = 0; i < mExcludedRelativePath.Count; i++)
			{
				if (path.StartsWith(mExcludedRelativePath[i]) && path.Length != mExcludedRelativePath[i].Length)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsPathExcluded(string path, bool useOptimisedList = false)
		{
			List<string> pathList;
			if (useOptimisedList)
				pathList = mOptimisedNotRelativePaths;
			else
				pathList = mExcludedRelativePath;
			if (pathList == null || pathList.Count == 0)
				return false;
			path = CTL_Tools.NormalizePath(path);
			if (string.IsNullOrEmpty(path))
				return false;
			for (int i = 0; i < pathList.Count; i++)
			{
				if (path.StartsWith(pathList[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool AddExludedPath(string path)
		{
			path = CTL_Tools.NormalizePath(path);
			if (string.IsNullOrEmpty(path))
				return false;
			path = CTL_Tools.GetRelativePath(path);
			if (string.IsNullOrEmpty(path))
				return false;
			if (!mExcludedRelativePath.Contains(path))
			{
				mExcludedRelativePath.Add(path);
				Sort();
				CTL_Settings.sShouldSave = true;
				CTL_MainWindow.sNeedRefreshAuto = true;
				return true;
			}
			return false;
		}

		public void Sort()
		{
			mExcludedRelativePath.Sort();
		}

		public bool RemoveExludedPath(string path)
		{
			if (mExcludedRelativePath.Contains(path))
			{
				mExcludedRelativePath.Remove(path);
				CTL_Settings.sShouldSave = true;
				CTL_MainWindow.sNeedRefreshAuto = true;
				return true;
			}
			return false;
		}

		public static CTL_Exclude LoadFromString(string strIn)
		{
			CTL_Exclude returnObj = new CTL_Exclude();
			string[] splitted = strIn.Split(SEPARATOR);
			try
			{
				foreach (string str in splitted)
				{
					if (!string.IsNullOrEmpty(str))
					{
						returnObj.AddExludedPath(Application.dataPath + "/" + str);
					}
				}
			}
			catch (Exception) { }
			return returnObj;
		}

		public string Export()
		{
			string saveString = "";
			bool first = true;
			foreach (string path in mExcludedRelativePath)
			{
				if (!first)
					saveString += SEPARATOR;
				else
					first = false;
				saveString += path;
			}
			return saveString;
		}
	}
}
