using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;

namespace CodeTodoList
{
	public class CTL_TodoObject
	{
		public static bool sNeedRefreshDisplayedElementsList = false;
		public static List<CTL_TodoObject> sTodoObjects = new List<CTL_TodoObject>();
		public static List<CTL_TodoObject> sDisplayedElementList = new List<CTL_TodoObject>();

		public static bool sShouldRefreshSearch = false;

		public int mLineNumber;
		public string mLineContent;
		public string mMessage;
		public CTL_TodoFile mFile;
		public CTL_KeywordObject mLinkedKeyword;

		public bool mCanBeDisplayed = true;


		public static void GenerateDisplayedElementListIfNeeded()
		{
			if (sNeedRefreshDisplayedElementsList)
				GenerateDisplayedElementList();
			sNeedRefreshDisplayedElementsList = false;
		}

		public static void GenerateDisplayedElementList()
		{
			sDisplayedElementList.Clear();
			for (int i = 0; i < sTodoObjects.Count; i++)
			{
				if (sTodoObjects[i].mLinkedKeyword.Visible && sTodoObjects[i].CanBeDisplayed)
				{
					sDisplayedElementList.Add(sTodoObjects[i]);
				}
			}
		}

		public bool CanBeDisplayed
		{
			get { return mCanBeDisplayed; }
			set
			{
				if(mCanBeDisplayed != value)
				{
					mCanBeDisplayed = value;
					sNeedRefreshDisplayedElementsList = true;
				}
			}
		}

		static string mSearchTerm = "";
		public static string SearchTerm
		{
			get { return mSearchTerm; }
			set {
				if (value != mSearchTerm || sShouldRefreshSearch)
				{
					mSearchTerm = value;
					string termLower = mSearchTerm.ToLower();
					foreach (CTL_TodoObject obj in sTodoObjects)
					{
						obj.CanBeDisplayed = obj.mMessage.ToLower().Contains(termLower) || obj.mLinkedKeyword.Key.ToLower().Contains(termLower) || obj.mFile.mName.ToLower().Contains(termLower);
					}
					sShouldRefreshSearch = false;
				}
			}
		}

		public static void SortByKeyword(bool ascending)
		{
			if(CTL_Settings.KeywordSortOrder == CTL_Settings.eKeywordSortOrder.ALPHABETICAL)
			{
				if (ascending)
					sTodoObjects.Sort((a, b) => a.mLinkedKeyword.Key.CompareTo(b.mLinkedKeyword.Key));
				else
					sTodoObjects.Sort((b, a) => a.mLinkedKeyword.Key.CompareTo(b.mLinkedKeyword.Key));
			}
			else if(CTL_Settings.KeywordSortOrder == CTL_Settings.eKeywordSortOrder.KEYWORD_ORDER)
			{
				if (ascending)
					sTodoObjects.Sort((a, b) => CTL_Settings.KeywordsList.GetKeywordId(a.mLinkedKeyword).CompareTo(CTL_Settings.KeywordsList.GetKeywordId(b.mLinkedKeyword)));
				else
					sTodoObjects.Sort((b, a) => CTL_Settings.KeywordsList.GetKeywordId(a.mLinkedKeyword).CompareTo(CTL_Settings.KeywordsList.GetKeywordId(b.mLinkedKeyword)));
			}

			sNeedRefreshDisplayedElementsList = true;
		}
		public static void SortByMessage(bool ascending)
		{
			if (ascending)
				sTodoObjects.Sort((a, b) => a.mMessage.CompareTo(b.mMessage));
			else
				sTodoObjects.Sort((b, a) => a.mMessage.CompareTo(b.mMessage));
			sNeedRefreshDisplayedElementsList = true;
		}
		public static void SortByFileName(bool ascending)
		{
			if (ascending)
				sTodoObjects.Sort((a, b) => a.mFile.mName.CompareTo(b.mFile.mName));
			else
				sTodoObjects.Sort((b, a) => a.mFile.mName.CompareTo(b.mFile.mName));
			sNeedRefreshDisplayedElementsList = true;
		}
		public static void SortByLineNumber(bool ascending)
		{
			if (ascending)
				sTodoObjects.Sort((a, b) => a.mLineNumber.CompareTo(b.mLineNumber));
			else
				sTodoObjects.Sort((b, a) => a.mLineNumber.CompareTo(b.mLineNumber));
			sNeedRefreshDisplayedElementsList = true;
		}

		public static List<CTL_TodoObject> GetObjectsForKeyword(CTL_KeywordObject keywordObj)
		{
			List<CTL_TodoObject> toReturn = new List<CTL_TodoObject>();
			for(int i = 0; i < sTodoObjects.Count; i++)
			{
				if(sTodoObjects[i].mLinkedKeyword == keywordObj)
					toReturn.Add(sTodoObjects[i]);
			}
			return toReturn;
		}

		protected void Register()
		{
			sTodoObjects.Add(this);
		}

		protected void UnRegister()
		{
			if (sTodoObjects.Contains(this))
				sTodoObjects.Remove(this);
		}

		public static void ClearRegisteredItems()
		{
			sTodoObjects.Clear();
			sDisplayedElementList.Clear();
		}

		public void SetFile(CTL_TodoFile file)
		{
			mFile = file;
		}

		public void Clear()
		{
			mFile = null;
			UnRegister();
		}

		public CTL_TodoObject(string lineContent, int lineNb, CTL_TodoFile file, CTL_KeywordObject keyworkRef)
		{
			sNeedRefreshDisplayedElementsList = true;
			mLinkedKeyword = keyworkRef;
			mFile = file;
			mLineContent = lineContent;
			mLineNumber = lineNb;
			if (!CTL_Settings.CaseSensitive)
				mMessage = lineContent.Substring(lineContent.ToLower().IndexOf(keyworkRef.KeySearch) + keyworkRef.KeySearch.Length);
			else
				mMessage = lineContent.Substring(lineContent.IndexOf(keyworkRef.KeySearch) + keyworkRef.KeySearch.Length);

			mMessage = mMessage.Trim();
			if (mMessage.StartsWith(":"))
			{
				mMessage = mMessage.Substring(1);
				mMessage = mMessage.Trim();
			}
			if (string.IsNullOrEmpty(mMessage))
				mMessage = keyworkRef.Key;

			Register();
		}

		public bool JumpToLine()
		{
			try
			{
				string guid = AssetDatabase.AssetPathToGUID(mFile.mRelativePath);
				int assetInstanceId = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(guid)).GetInstanceID();
				return AssetDatabase.OpenAsset(assetInstanceId, mLineNumber);
			}
			catch (Exception e)
			{
				if (CTL_Settings.DEBUG_MODE)
					UnityEngine.Debug.LogError(e.Message);
			}

			FileInfo fileInfo = new FileInfo(mFile.mPath);
			if (!fileInfo.Exists)
				return false;
			//file not opened but exist : open it with default software

			try
			{
				ProcessStartInfo startInfo = new ProcessStartInfo(mFile.mPath);
				startInfo.Arguments = "/K --goto " + mFile.mPath + ":" + mLineNumber;
				Process.Start(startInfo);
				return true;
			}
			catch (Exception e)
			{
				if (CTL_Settings.DEBUG_MODE)
					UnityEngine.Debug.LogError(e.Message);
			}

			return false;
		}

		public override string ToString()
		{
			return "Line:" + mLineNumber + " Content:" + mLineContent;
		}
	}
}

