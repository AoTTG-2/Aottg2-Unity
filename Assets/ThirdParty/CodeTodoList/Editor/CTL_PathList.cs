using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodeTodoList
{


	public class CTL_PathList
	{
		public const char SEPARATOR = '?';
		List<string> mPaths = new List<string>();
		List<string> mOptimisedNotRelativePaths = new List<string>();



		public List<string> PathList
		{
			get { return mPaths; }
		}

		public void GenerateOptimisedList()
		{
			mOptimisedNotRelativePaths.Clear();
			for (int i = 0; i < mPaths.Count; i++)
			{
				if (!IsInsideFolder(mPaths[i]))
					mOptimisedNotRelativePaths.Add(CTL_Tools.GetNotRelativePath(mPaths[i], false));
			}
		}

		public bool IsInsideFolder(string path)
		{
			if (mPaths.Count == 0)
				return false;
			path = CTL_Tools.GetNotRelativePath(path, false);
			for (int i = 0; i < mPaths.Count; i++)
			{
				if (path.StartsWith(mPaths[i]) && path.Length != mPaths[i].Length)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainPath(string path, bool useOptimisedList = false)
		{
			List<string> pathList;
			if (useOptimisedList)
				pathList = mOptimisedNotRelativePaths;
			else
				pathList = mPaths;
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

		public bool AddPath(string path)
		{
			path = CTL_Tools.NormalizePath(path);
			if (string.IsNullOrEmpty(path))
				return false;
			path = CTL_Tools.GetRelativePath(path);
			if (string.IsNullOrEmpty(path))
				return false;
			if (!mPaths.Contains(path))
			{
				mPaths.Add(path);
				Sort();
				CTL_Settings.sShouldSave = true;
				CTL_MainWindow.sNeedRefreshAuto = true;
				return true;
			}
			return false;
		}

		public void Sort()
		{
			mPaths.Sort();
		}

		public bool RemovePath(string path)
		{
			if (mPaths.Contains(path))
			{
				mPaths.Remove(path);
				CTL_Settings.sShouldSave = true;
				CTL_MainWindow.sNeedRefreshAuto = true;
				return true;
			}
			return false;
		}

		public static CTL_PathList LoadFromString(string strIn, bool relative = true)
		{
			CTL_PathList returnObj = new CTL_PathList();
			string[] splitted = strIn.Split(SEPARATOR);
			try
			{
				foreach (string str in splitted)
				{
					if (!string.IsNullOrEmpty(str))
					{
						if(relative)
							returnObj.AddPath(Application.dataPath + "/" + str);
						else
							returnObj.AddPath(str);
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
			foreach (string path in mPaths)
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
