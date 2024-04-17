using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CodeTodoList
{
	public class CTL_TodoFile
	{
		public string mPath;
		public string mRelativePath;
		public string mName;
		public List<CTL_TodoObject> mTodoObjects = new List<CTL_TodoObject>();

		public CTL_TodoFile(string path)
		{
			mPath = path;
			mRelativePath = GetRelativePath();
			mName = new FileInfo(path).Name;
		}

		public void AddObject(CTL_TodoObject obj)
		{
			mTodoObjects.Add(obj);
		}

		public void Clear()
		{
			for(int i = 0; i < mTodoObjects.Count; i++)
			{
				mTodoObjects[i].Clear();
			}
			CTL_TodoObject.ClearRegisteredItems();
			mTodoObjects.Clear();
		}

		string GetRelativePath()
		{
			return "Assets" + CTL_Tools.GetRelativePath(mPath);
			/*
			string relativePath = "Assets" + CTL_Settings.RelativeRootFolder;//"Assets/";

			string[] splited = mPath.Split(new char[] { '/', '\\' });

			bool foundAssetFolder = false;
			for(int i = 0; i < splited.Length; i++)
			{
				if (foundAssetFolder)
				{
					relativePath += splited[i];
					if (i < splited.Length - 1)
						relativePath += "/";
				}
				else
				{
					foundAssetFolder = splited[i] == "Assets";
				}
			}


			return relativePath;*/
		}

		public void Print()
		{
			string str = "FILE: " + GetRelativePath() + "\n";
			foreach(CTL_TodoObject obj in mTodoObjects)
			{
				str += obj.ToString() + "\n";
			}
			Debug.Log(str);
		}
	}
}

