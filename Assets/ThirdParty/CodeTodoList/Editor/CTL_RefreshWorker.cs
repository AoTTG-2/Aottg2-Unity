using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.IO;

namespace CodeTodoList
{
	public class CTL_RefreshWorker
	{
		public const string SEARCH_WORD = "//todo";

		static string DATA_PATH = Application.dataPath;

		const float PROGRESSWEIGHT_GETFILELIST = .1f;
		const float PROGRESSWEIGHT_EXCLUDEPATHS = .1f;
		const float PROGRESSWEIGHT_PROCESSFILES = .8f;

		static float sProgress_GetFileList = 0;
		static float sProgress_ExcludePaths = 0;
		static float sProgress_ProcessFiles = 0;

		static long sFilesInProjectCount = 0;
		static long sFilesProcessedCount = 0;
		static long sCommentProcessedCount = 0;
		static long sTotalLinesCount = 0;


		static StringComparison sStringComparison = StringComparison.OrdinalIgnoreCase;

		bool mCanceled = false;

		List<CTL_KeywordObject> mValidsKeywords = new List<CTL_KeywordObject>();

		public event Action<int, int, string> ProcessChanged;
		public event Action<List<CTL_TodoFile>, bool> WorkCompleted;

		public CTL_RefreshWorker()
		{
			mCanceled = false;
		}

		public void Cancel()
		{
			mCanceled = true;
		}
		
		public static float GetTotalProgress()
		{
			return sProgress_GetFileList * PROGRESSWEIGHT_GETFILELIST + sProgress_ExcludePaths * PROGRESSWEIGHT_EXCLUDEPATHS + sProgress_ProcessFiles * PROGRESSWEIGHT_PROCESSFILES;
		}

		public void Work()
		{
			mValidsKeywords = CTL_Settings.KeywordsList.GetValidKeywordList();
			sStringComparison = CTL_Settings.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

			List<CTL_TodoFile> fileObjectList = new List<CTL_TodoFile>();
			CTL_TodoFile fileObject;

			sProgress_GetFileList = 0;
			sProgress_ExcludePaths = 0;
			sProgress_ProcessFiles = 0;

			sFilesInProjectCount = 0;
			sFilesProcessedCount = 0;
			sCommentProcessedCount = 0;
			sTotalLinesCount = 0;

			long time_RealStart = CTL_Tools.ThreadSafeTimeStamp();
			long timeSpan_GetScriptFilePath = time_RealStart;
			long timeSpan_ProcessFiles = 0;
			long time_StartProcessFiles = 0;
			long timeSpan_ExcludeFiles = 0;
			long time_StartExcludeFiles = 0;

			long time_Start = time_RealStart;
			long time_Cur = time_RealStart;

			
			ProcessChanged(0, 0, "Get files paths...");
			string[] scriptFilesPath = GetScriptFilesPath();
			sProgress_GetFileList = 1;
			sFilesInProjectCount = scriptFilesPath.Length;


			if (CTL_Settings.DEBUG_MODE)
			{
				time_Cur = CTL_Tools.ThreadSafeTimeStamp();
				timeSpan_GetScriptFilePath = time_Cur - time_RealStart;
				time_StartExcludeFiles = time_Cur;
			}


			//CTL...start exclude paths
			ProcessChanged(0, 0, "Exclude paths...");
			List<string> notExcludedPath = new List<string>(scriptFilesPath.Length);
			CTL_Settings.ExcludedPaths.GenerateOptimisedList();
			for (int i = 0; i < scriptFilesPath.Length; i++)
			{
				if (!CTL_Settings.ExcludedPaths.ContainPath(scriptFilesPath[i], true))
					notExcludedPath.Add(scriptFilesPath[i]);
				sProgress_ExcludePaths = (float)i / (float)scriptFilesPath.Length;
				//Debug.Log(scriptFilesPath[i]);
			}
			sProgress_ExcludePaths = 1;

			List<string> pathsList = new List<string>();
			pathsList.AddRange(notExcludedPath);
			//pathsList.AddRange(CTL_Settings.ExternalPaths.PathList);

			scriptFilesPath = pathsList.ToArray();
			//CTL...end exclude path




			//CTL TODO... add included path
			//ex: notExcludedPath.Add("W:/PERSONAL_PROJECTS/UNITY/ASSET_STORE_PROJECTS/PUBLISHED/CODE_TODO_LIST/v1.1/TODO_PIN/Packages/test.cs");


			 


			if (CTL_Settings.DEBUG_MODE)
			{
				time_Cur = CTL_Tools.ThreadSafeTimeStamp();
				timeSpan_ExcludeFiles = time_Cur - time_StartExcludeFiles;
				time_StartProcessFiles = time_Cur;
			}


			//CTL...start process files
			ProcessChanged(0, 0, "Process files...");
			for (int i = 0; i < scriptFilesPath.Length; i++)
			{
				if (mCanceled)
					break;

				fileObject = ParseFileAtPath(scriptFilesPath[i]);
				if (fileObject != null)
					fileObjectList.Add(fileObject);

				sProgress_ProcessFiles = (float)i / (float)scriptFilesPath.Length;
			}
			sProgress_ProcessFiles = 1;
			sFilesProcessedCount = scriptFilesPath.Length;
			time_Cur = CTL_Tools.ThreadSafeTimeStamp();

			//CTL...end process  files
			if (CTL_Settings.DEBUG_MODE)
			{
				timeSpan_ProcessFiles = time_Cur - time_StartProcessFiles;
				Debug.Log("Refresh done in " + (time_Cur - time_RealStart) + 
					" -- GetScriptFilesPath: " + timeSpan_GetScriptFilePath +
					" -- ExcludeFiles: " + timeSpan_ExcludeFiles +
					" -- ProcessFiles: " + timeSpan_ProcessFiles +
					"\nScript files in project: " + sFilesInProjectCount +
					"\nScript files processed: " + sFilesProcessedCount +
					"\nComments processed: " + sCommentProcessedCount +
					"\nTotal lines count: " + sTotalLinesCount);
			}

			ProcessChanged(scriptFilesPath.Length, scriptFilesPath.Length, "COMPLETE");
			WorkCompleted.Invoke(fileObjectList, mCanceled);
		}

		string[] GetScriptFilesPath()
		{
			string[] csFiles = new string[0];
			string[] jsFiles = new string[0];
			List<string> externalPaths = new List<string>();

			string rootFolderPath = CTL_Tools.GetNotRelativePath(CTL_Settings.RelativeRootFolder, false);

			try
			{
				if (CTL_Settings.ParseCSFiles)
				{
					csFiles = Directory.GetFiles(rootFolderPath, "*.cs", SearchOption.AllDirectories);

					foreach (string path in CTL_Settings.ExternalPaths.PathList)
					{
						if (Directory.Exists(path))
						{
							externalPaths.AddRange(Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories));
						}
						else if (File.Exists(path) && path.ToLower().EndsWith(".cs"))
						{
							externalPaths.Add(path);
						}
					}
				}
			}catch (Exception ex)
			{
				Debug.LogError("Error while parsing c# files: " + ex.ToString());
			}
			try
			{
				if (CTL_Settings.ParseJSFiles)
				{
					jsFiles = Directory.GetFiles(rootFolderPath, "*.js", SearchOption.AllDirectories);

					foreach (string path in CTL_Settings.ExternalPaths.PathList)
					{
						if (Directory.Exists(path))
						{
							externalPaths.AddRange(Directory.GetFiles(path, "*.js", SearchOption.AllDirectories));
						}
						else if (File.Exists(path) && path.ToLower().EndsWith(".js"))
						{
							externalPaths.Add(path);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Error while parsing JS files: " + ex.ToString());
			}
			string[] returnFiles = new string[csFiles.Length + jsFiles.Length + externalPaths.Count];
			csFiles.CopyTo(returnFiles, 0);
			jsFiles.CopyTo(returnFiles, csFiles.Length);
			externalPaths.CopyTo(returnFiles, csFiles.Length + jsFiles.Length);
			return returnFiles;
		}

		CTL_TodoFile ParseFileAtPath(string path)
		{
			CTL_TodoFile fileObject = null;

			string line;
			int lineId = 0;
			Dictionary<int, string> potentialCommentList = new Dictionary<int, string>();
			string lowerLine;

			try
			{
				StreamReader sr = new StreamReader(path);

				//CTL...check potential comments
				
				while ((line = sr.ReadLine()) != null)
				{
					lineId++;

					if (line.Contains("//"))
					{
						potentialCommentList.Add(lineId, line);
					}
				}
				sr.Close();
				sTotalLinesCount += lineId;
			}
			catch(Exception e)
			{
				Debug.LogError(e.Message);
			}
			
			

			//CTL...analyse if real comment
			Dictionary<int, string> realCommentList = new Dictionary<int, string>();
			int stringPos;
			int escapeStringPos = 0;
			int commentPos;
			bool commentFound;
			int stringEndPos;
			string currentStr;
			foreach (KeyValuePair<int, string> keyVal in potentialCommentList)
			{
				currentStr = keyVal.Value;
				commentFound = false;
				commentPos = 0;
				escapeStringPos = 0;
				while (currentStr.Length > 0 && commentPos > -1)
				{
					escapeStringPos = 0;
					stringPos = currentStr.IndexOf("\"");
					commentPos = currentStr.IndexOf("//");

							

					//CTL...check if the comment may be in a string (ex: "teststring//Foo" )
					if (stringPos >= 0 && stringPos < commentPos)
					{
						stringEndPos = stringPos;
						escapeStringPos = stringPos + 1;
						do
						{
							stringEndPos = currentStr.IndexOf("\"", stringEndPos + 1);
							if(stringEndPos > -1)
								escapeStringPos = currentStr.IndexOf("\\\"", stringEndPos - 1);    //check if close string escaped
						}
						while (escapeStringPos > 0 && escapeStringPos < stringEndPos && stringEndPos > 0 && stringEndPos < currentStr.Length);

						if (stringEndPos > -1)
						{
							currentStr = currentStr.Substring(stringEndPos + 1);
						}
						else
						{
							break;
						}
					}
					else if (commentPos > -1)
					{
						currentStr = currentStr.Substring(commentPos);
						commentFound = true;
						break;
					}
				}
				if (commentFound)
				{
					currentStr = currentStr.Substring(2);
					if (CTL_Settings.TrimCommentOnParse)
					{
						currentStr = currentStr.Trim();
					}
					realCommentList.Add(keyVal.Key, currentStr);
				}
			}
			sCommentProcessedCount += realCommentList.Count;


			//CTL...generate object list if keyword found
			foreach (KeyValuePair<int, string> keyVal in realCommentList)
			{
				if (!CTL_Settings.CaseSensitive)
					lowerLine = keyVal.Value.ToLower();
				else
					lowerLine = keyVal.Value;

				foreach (CTL_KeywordObject obj in mValidsKeywords)
				{
					if (lowerLine.StartsWith(obj.KeySearch))
					{
						if (fileObject == null)
							fileObject = new CTL_TodoFile(path);
						fileObject.AddObject(new CTL_TodoObject(keyVal.Value, keyVal.Key, fileObject, obj));
					}
				}
			}

			return fileObject;
		}
	}
}

