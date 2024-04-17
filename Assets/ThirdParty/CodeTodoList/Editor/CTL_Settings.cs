using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CodeTodoList
{
	public class CTL_Settings
	{
		public const char SEPARATOR = ':';
		public const char SEPARATOR_SECONDARY = '*';

		public static bool DEBUG_MODE = false;
		public static bool sShouldSave = false;


		static long sMinMsBetweekAutoSave = 2000;
		static long sLastSaveTime = 0;

		enum eSaveLocation
		{
			PERSISTENT_DATA,
			PROJECT_ROOT
		}

		public enum eDisplayContainerType
		{
			FILE,
			KEYWORD
		}

		public enum eKeywordSortOrder
		{
			KEYWORD_ORDER,
			ALPHABETICAL
		}

		public enum eElementColorType
		{
			SOLID,
			GRADIENT_LEFT,
			GRADIENT_RIGHT
		}

		static eSaveLocation SAVE_LOCATION = eSaveLocation.PROJECT_ROOT;
		public static string FOLDER_NAME = "Library";//CTL..."CodeTodoList";
		static string FILE_PATH = "CodeTodoList_Prefs";
		static string FILE_PATH_SECONDARY = "test";
		public static string PATH_PERSISTANT_DATA = Application.persistentDataPath;
		public static string PATH_PROJECT_ROOT = Application.dataPath;


		//CTL...PREFS
		protected bool mAutoRefresh = true;
		protected bool mCaseSensitive = false;
		protected bool mEnableHelpTooltips = true;
		protected CTL_KeywordList mKeywordsList;
		protected eDisplayContainerType mDisplayContainerType = eDisplayContainerType.KEYWORD;
		protected CTL_PathList mExcludedPaths;
		protected CTL_PathList mExternalPaths;
		protected int mColumnSortIndex = -1;
		protected bool mColumnSortAscending = true;
		protected bool[] mColumnVisible = new bool[4] { true, true, true, false};
		protected float[] mColumnSize = new float[4] { -1, -1, -1, -1 };
		protected string mSearchFieldContent = "";
		protected bool mEnableCommentTooltip = true;
		protected bool mDisplayKeywordInCommentTooltip = false;
		protected bool mDisplayFilenameAndLineInCommentTooltip = true;
		protected EventModifiers mDisplayTooltipModifier = EventModifiers.None;
		protected EventModifiers mCommentClickModifier_OpenFile = EventModifiers.None;
		protected bool mParseCSFiles = true;
		protected bool mParseJSFiles = true;
		protected eKeywordSortOrder mKeywordSortOrder = eKeywordSortOrder.KEYWORD_ORDER;
		protected bool mTrimCommentOnParse = true;
		protected eElementColorType mElementColorType = eElementColorType.SOLID;
		protected string mRelativeRootFolder = "";

		public static eElementColorType ElementColorType
		{
			get { return Instance.mElementColorType; }
			set { SetPref(ref Instance.mElementColorType, value); }
		}

		public static bool ParseCSFiles
		{
			get { return Instance.mParseCSFiles; }
			set { SetPref(ref Instance.mParseCSFiles, value); }
		}

		public static bool ParseJSFiles
		{
			get { return Instance.mParseJSFiles; }
			set { SetPref(ref Instance.mParseJSFiles, value); }
		}

		public static CTL_PathList ExcludedPaths
		{
			get
			{
				if (Instance.mExcludedPaths == null)
					Instance.mExcludedPaths = new CTL_PathList();
				return Instance.mExcludedPaths;
			}
		}

		public static CTL_PathList ExternalPaths
		{
			get
			{
				if (Instance.mExternalPaths == null)
					Instance.mExternalPaths = new CTL_PathList();
				return Instance.mExternalPaths;
			}
		}

		public static void AddExludedPath(string path, bool export = true)
		{
			bool success = ExcludedPaths.AddPath(path);
			if (success && export)
				Instance.Export();
		}

		public static void AddExludedPath(string[] paths, bool export = true)
		{
			bool success = false;
			for (int i = 0; i < paths.Length; i++)
			{
				if (ExcludedPaths.AddPath(paths[i]))
					success = true;
			}
			
			if (success && export)
				Instance.Export();
		}

		public static void AddExternalPath(string path, bool export = true)
		{
			bool success = ExternalPaths.AddPath(path);
			if (success && export)
				Instance.Export();
		}

		public static void AddExternalPath(string[] paths, bool export = true)
		{
			bool success = false;
			for (int i = 0; i < paths.Length; i++)
			{
				if (ExternalPaths.AddPath(paths[i]))
					success = true;
			}

			if (success && export)
				Instance.Export();
		}

		public static void RemoveExludedPath(string path)
		{
			bool success = ExcludedPaths.RemovePath(path);
			if(success)
				Instance.Export();
		}

		public static void RemoveExternalPath(string path)
		{
			bool success = ExternalPaths.RemovePath(path);
			if (success)
				Instance.Export();
		}

		public static bool AutoRefresh
		{
			get	{ return Instance.mAutoRefresh; }
			set	{ SetPref(ref Instance.mAutoRefresh, value); }
		}

		public static bool CaseSensitive
		{
			get { return Instance.mCaseSensitive; }
			set { SetPref(ref Instance.mCaseSensitive, value); }
		}

		public static bool EnableHelpTooltips
		{
			get { return Instance.mEnableHelpTooltips; }
			set { SetPref(ref Instance.mEnableHelpTooltips, value); }
		}

		public static CTL_KeywordList KeywordsList
		{
			get { return Instance.mKeywordsList; }
			set { SetPref(ref Instance.mKeywordsList, value); }
		}

		public static eDisplayContainerType DisplayContainerType
		{
			get { return Instance.mDisplayContainerType; }
			set { SetPref(ref Instance.mDisplayContainerType, value); }
		}

		public static int ColumnSortIndex
		{
			get { return Instance.mColumnSortIndex; }
		}

		public static bool ColumnSortAscending
		{
			get { return Instance.mColumnSortAscending; }
		}

		public static bool[] ColumnVisibility
		{
			get { return Instance.mColumnVisible; }
			set { SetPref(ref Instance.mColumnVisible, value); }
		}

		public static float[] ColumnSize
		{
			get { return Instance.mColumnSize; }
			set { SetPref(ref Instance.mColumnSize, value, false, true, true); }
		}

		public static bool EnableCommentTooltip
		{
			get { return Instance.mEnableCommentTooltip; }
			set { SetPref(ref Instance.mEnableCommentTooltip, value); }
		}

		public static bool DisplayKeywordInCommentTooltip
		{
			get { return Instance.mDisplayKeywordInCommentTooltip; }
			set { SetPref(ref Instance.mDisplayKeywordInCommentTooltip, value); }
		}

		public static bool DisplayFilenameAndLineInCommentTooltip
		{
			get { return Instance.mDisplayFilenameAndLineInCommentTooltip; }
			set { SetPref(ref Instance.mDisplayFilenameAndLineInCommentTooltip, value); }
		}

		public static EventModifiers DisplayTooltipModifier
		{
			get { return Instance.mDisplayTooltipModifier; }
			set { SetPref(ref Instance.mDisplayTooltipModifier, value); }
		}

		public static EventModifiers CommentClickModifier_OpenFile
		{
			get { return Instance.mCommentClickModifier_OpenFile; }
			set { SetPref(ref Instance.mCommentClickModifier_OpenFile, value); }
		}

		public static eKeywordSortOrder KeywordSortOrder
		{
			get { return Instance.mKeywordSortOrder; }
			set { SetPref(ref Instance.mKeywordSortOrder, value); }
		}
		 

		public static string SearchFieldContent
		{
			get { return Instance.mSearchFieldContent; }
			set { SetPref(ref Instance.mSearchFieldContent, value, false, true, true); }
		}

		public static bool TrimCommentOnParse
		{
			get { return Instance.mTrimCommentOnParse; }
			set { SetPref(ref Instance.mTrimCommentOnParse, value); }
		}

		public static string RelativeRootFolder
		{
			get	{return Instance.mRelativeRootFolder;}
			set { SetRootFolder(value); }
		}

		public static void SetRootFolder(string rootFolder)
		{
			string relativeRootFolder = CTL_Tools.GetRelativePath(CTL_Tools.NormalizePath(rootFolder));
			if (CTL_Tools.IsValidPath("ASSETS\\" + relativeRootFolder))
			{
				SetPref(ref Instance.mRelativeRootFolder, relativeRootFolder);
			}
			else
			{
				Debug.LogWarning("Root folder must be in the project!");
			}
		}

		public static void SaveColumnSort(int columnIndex, bool ascending)
		{
			bool columnModified = SetPref(ref Instance.mColumnSortIndex, columnIndex, false);
			bool ascendingModified = SetPref(ref Instance.mColumnSortAscending, ascending, false);
			if (columnModified || ascendingModified)
				Instance.Export();
		}

		static string GetColumnVisibilityExportString()
		{
			string toReturn = "";
			for(int i = 0; i < Instance.mColumnVisible.Length; i++)
			{
				if (i != 0)
					toReturn += SEPARATOR_SECONDARY;
				toReturn += Instance.mColumnVisible[i].ToString();
			}
			return toReturn;
		}

		static string GetColumnSizeExportString()
		{
			string toReturn = "";
			for (int i = 0; i < Instance.mColumnSize.Length; i++)
			{
				if (i != 0)
					toReturn += SEPARATOR_SECONDARY;
				toReturn += Instance.mColumnSize[i].ToString();
			}
			return toReturn;
		}

		static bool[] GenerateColumnVisibilityArrayFromString(string strIn)
		{
			bool[] toReturn;
			string[] splitted = strIn.Split(SEPARATOR_SECONDARY);
			if(splitted.Length == 4)
			{
				toReturn = new bool[splitted.Length];
				for (int i = 0; i < splitted.Length; i++)
				{
					bool.TryParse(splitted[i], out toReturn[i]);
				}
			}
			else
			{
				toReturn = new bool[4] { true, true, true, true };
			}

			return toReturn;
		}

		static float[] GenerateColumnSizeArrayFromString(string strIn)
		{
			float[] toReturn;
			string[] splitted = strIn.Split(SEPARATOR_SECONDARY);
			if (splitted.Length == 4)
			{
				toReturn = new float[splitted.Length];
				for (int i = 0; i < splitted.Length; i++)
				{
					float.TryParse(splitted[i], out toReturn[i]);
				}
			}
			else
			{
				toReturn = new float[4] { -1, -1, -1, -1 };
			}

			return toReturn;
		}

		public static void ResetColumnSizePersistance()
		{
			SetPref(ref Instance.mColumnSize, new float[4] { -1,-1,-1,-1}, false, true, true);
		}

		static string sPath = null;
		protected static CTL_Settings sInstance;

		public static CTL_Settings Instance
		{
			get
			{
				GenerateInstanceIfNotExist();
				return sInstance;
			}
		}

		public static void GenerateInstanceIfNotExist()
		{
			if (sInstance == null)
			{
				Load();
			}
		}

		public static void SaveOrAutoSave(bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (instantSave)
				Instance.Export();
			if (autosave)
				sShouldSave = true;
			if (refreshSaveTimer)
				sLastSaveTime = CTL_Tools.ThreadSafeTimeStamp();
		}

		private static bool SetPref(ref string pref, string newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref CTL_KeywordList pref, CTL_KeywordList newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref object pref, object newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref bool pref, bool newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref int pref, int newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref EventModifiers pref, EventModifiers newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref KeyCode pref, KeyCode newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref eDisplayContainerType pref, eDisplayContainerType newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref eKeywordSortOrder pref, eKeywordSortOrder newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref eElementColorType pref, eElementColorType newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref bool[] pref, bool[] newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;

			if (pref != null && newValue != null && pref.Length == newValue.Length)
			{
				bool different = false;
				for(int i = 0; i < pref.Length; i++)
				{
					if(pref[i] != newValue[i])
					{
						different = true;
						break;
					}
				}
				if (!different)
				{
					return false;
				}
			}
			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static bool SetPref(ref float[] pref, float[] newValue, bool instantSave = true, bool autosave = false, bool refreshSaveTimer = false)
		{
			if (pref == newValue)
				return false;

			if (pref != null && newValue != null && pref.Length == newValue.Length)
			{
				bool different = false;
				for (int i = 0; i < pref.Length; i++)
				{
					if (pref[i] != newValue[i])
					{
						different = true;
						break;
					}
				}
				if (!different)
				{
					return false;
				}
			}

			pref = newValue;
			SaveOrAutoSave(instantSave, autosave, refreshSaveTimer);
			return true;
		}

		private static string path
		{
			get
			{
				if (sPath != null)
					return sPath;
				string _path = "";
				switch (SAVE_LOCATION)
				{
					case eSaveLocation.PERSISTENT_DATA:
						_path = PATH_PERSISTANT_DATA;
						try
						{
							_path.Replace("\\", "/");
							_path = _path.Substring(0, _path.LastIndexOf("/"));
							_path = _path.Substring(0, _path.LastIndexOf("/"));
						}
						catch (Exception) { }
						break;
					case eSaveLocation.PROJECT_ROOT:
						_path = PATH_PROJECT_ROOT;
						try
						{
							_path.Replace("\\", "/");
							_path = _path.Substring(0, _path.LastIndexOf("/"));
						}
						catch (Exception) { }
						break;
				}

				_path += "/" + FOLDER_NAME + "/";
				sPath = _path;

				return sPath;
			}
		}

		public static string GetFilePath(bool createDirectory = true, bool secondary = false)
		{
			string _path = path;
			string _pathFull = _path + FILE_PATH;
			if (secondary)
				_pathFull = _path + FILE_PATH_SECONDARY;
			if (createDirectory)
			{
				if (!Directory.Exists(_path))
				{
					Directory.CreateDirectory(_path);
				}
			}
			return _pathFull;
		}

		public static void Load()
		{
			if(DEBUG_MODE)
				Debug.Log("load");
			sInstance = new CTL_Settings();

			sInstance.mKeywordsList = new CTL_KeywordList();

			string _filePath = GetFilePath();

			bool keywordListInit = false;   //CTL...at first load it should stay false

			if (File.Exists(_filePath))
			{
				StreamReader _sr = null;
				try
				{
					_sr = new StreamReader(_filePath);
					string _line = _sr.ReadLine();
					string[] _split = new string[2];
					int dataStartIndex = 0;
					while (!string.IsNullOrEmpty(_line))
					{
						dataStartIndex = _line.IndexOf(SEPARATOR) + 1;
						if(dataStartIndex > 0 && dataStartIndex < _line.Length)
						{
							_split[0] = _line.Substring(0, dataStartIndex - 1);
							_split[1] = _line.Substring(dataStartIndex);
						}
						else
						{
							Debug.LogError("cannot parse settings " + _line);
							_line = _sr.ReadLine();
							continue;
						}

						switch (_split[0])
						{
							case "a":
								sInstance.mAutoRefresh = bool.Parse(_split[1]);
								break;
							case "b":
								keywordListInit = true;
								sInstance.mKeywordsList.LoadFromString(_split[1]);
								break;
							case "c":
								sInstance.mCaseSensitive = bool.Parse(_split[1]);
								break;
							case "d":
								sInstance.mDisplayContainerType = (eDisplayContainerType)int.Parse(_split[1]);
								break;
							case "e":
								sInstance.mEnableHelpTooltips = bool.Parse(_split[1]);
								break;
							case "f":
								sInstance.mExcludedPaths = CTL_PathList.LoadFromString(_split[1]);
								break;
							case "g":
								sInstance.mColumnSortIndex = int.Parse(_split[1]);
								break;
							case "h":
								sInstance.mColumnSortAscending = bool.Parse(_split[1]);
								break;
							case "i":
								sInstance.mColumnVisible = GenerateColumnVisibilityArrayFromString(_split[1]);
								break;
							case "j":
								sInstance.mEnableCommentTooltip = bool.Parse(_split[1]);
								break;
							case "k":
								sInstance.mDisplayKeywordInCommentTooltip = bool.Parse(_split[1]);
								break;
							case "l":
								sInstance.mDisplayFilenameAndLineInCommentTooltip = bool.Parse(_split[1]);
								break;
							case "m":
								sInstance.mDisplayTooltipModifier = (EventModifiers)int.Parse(_split[1]);
								break;
							case "n":
								sInstance.mColumnSize = GenerateColumnSizeArrayFromString(_split[1]);
								break;
							case "o":
								sInstance.mSearchFieldContent = _split[1];
								break;
							case "p":
								sInstance.mCommentClickModifier_OpenFile = (EventModifiers)int.Parse(_split[1]);
								break;
							case "q":
								sInstance.mParseCSFiles = bool.Parse(_split[1]);
								break;
							case "r":
								sInstance.mParseJSFiles = bool.Parse(_split[1]);
								break;
							case "s":
								sInstance.mKeywordSortOrder = (eKeywordSortOrder)int.Parse(_split[1]);
								break;
							case "t":
								sInstance.mTrimCommentOnParse = bool.Parse(_split[1]);
								break;
							case "u":
								sInstance.mExternalPaths = CTL_PathList.LoadFromString(_split[1], false);
								break;
							case "v":
								sInstance.mElementColorType = (eElementColorType)int.Parse(_split[1]);
								break;
							case "w":
								sInstance.mRelativeRootFolder = _split[1];
								break;

							default:
								UnityEngine.Debug.LogWarning("Unknown data for " + _split[0]);
								break;
						}
						_line = _sr.ReadLine();
					}
				}
				catch (Exception e) { Debug.LogError("Fail during prefs parsing:" + e.ToString()); }
				finally { if (_sr != null) _sr.Dispose(); }
			}
			if (!keywordListInit)
			{
				sInstance.mKeywordsList.LoadFromString("");
			}

			sInstance.mKeywordsList.RefreshAllTextures();
		}

		public bool AutoSave()
		{
			if (sShouldSave && CTL_Tools.ThreadSafeTimeStamp() - sLastSaveTime > sMinMsBetweekAutoSave)
			{
				Export();
				return true;
			}
			return false;
		}

		public static void WriteToSecondaryFile(string toWrite)
		{
			string _filePath = GetFilePath(true, true);
			if (File.Exists(_filePath))
				File.Delete(_filePath);
			try
			{
				StreamWriter _sw = null;
				_sw = File.CreateText(_filePath);
				_sw.WriteLine(toWrite);
				_sw.Close();
			}
			catch (Exception) { }
		}

		public static void DeleteAllPersistantData()
		{
			string _filePath = GetFilePath();
			if (File.Exists(_filePath))
				File.Delete(_filePath);

			CTL_MainWindow.ClearFileObjectList();
			Load();
		}

		public void Export()
		{
			sLastSaveTime = CTL_Tools.ThreadSafeTimeStamp();
			sShouldSave = false;

			if (DEBUG_MODE)
				Debug.Log("export");

			StreamWriter _sw = null;
			try
			{
				string _filePath = GetFilePath();
				if (File.Exists(_filePath))
					File.Delete(_filePath);

				_sw = File.CreateText(_filePath);
				WriteDataToStreamWriter("a", mAutoRefresh.ToString(), _sw);
				WriteDataToStreamWriter("b", mKeywordsList.Export(), _sw);
				WriteDataToStreamWriter("c", mCaseSensitive.ToString(), _sw);
				WriteDataToStreamWriter("d", ((int)mDisplayContainerType).ToString(), _sw);
				WriteDataToStreamWriter("e", mEnableHelpTooltips.ToString(), _sw);
				WriteDataToStreamWriter("f", ExcludedPaths.Export(), _sw);
				WriteDataToStreamWriter("g", ColumnSortIndex.ToString(), _sw);
				WriteDataToStreamWriter("h", mColumnSortAscending.ToString(), _sw);
				WriteDataToStreamWriter("i", GetColumnVisibilityExportString(), _sw);
				WriteDataToStreamWriter("j", mEnableCommentTooltip.ToString(), _sw);
				WriteDataToStreamWriter("k", mDisplayKeywordInCommentTooltip.ToString(), _sw);
				WriteDataToStreamWriter("l", mDisplayFilenameAndLineInCommentTooltip.ToString(), _sw);
				WriteDataToStreamWriter("m", ((int)mDisplayTooltipModifier).ToString(), _sw);
				WriteDataToStreamWriter("n", GetColumnSizeExportString(), _sw);
				WriteDataToStreamWriter("o", mSearchFieldContent, _sw);
				WriteDataToStreamWriter("p", ((int)mCommentClickModifier_OpenFile).ToString(), _sw);
				WriteDataToStreamWriter("q", ParseCSFiles.ToString(), _sw);
				WriteDataToStreamWriter("r", ParseJSFiles.ToString(), _sw);
				WriteDataToStreamWriter("s", ((int)KeywordSortOrder).ToString(), _sw);
				WriteDataToStreamWriter("t", TrimCommentOnParse.ToString(), _sw);
				WriteDataToStreamWriter("u", ExternalPaths.Export(), _sw);
				WriteDataToStreamWriter("v", ((int)mElementColorType).ToString(), _sw);
				WriteDataToStreamWriter("w", mRelativeRootFolder, _sw);

			}
			catch (Exception e) { Debug.LogError("Fail during prefs export:" + e.ToString()); }
			finally { if (_sw != null) _sw.Dispose(); }
		}

		void WriteDataToStreamWriter(string key, string data, StreamWriter sw)
		{
			if (string.IsNullOrEmpty(data))
				return;
			try
			{
				sw.WriteLine(key + SEPARATOR + data);
			}
			catch (Exception)
			{
				Debug.LogError("Fail during prefs key export:" + key + SEPARATOR + data);
			}

		}
	}
}

