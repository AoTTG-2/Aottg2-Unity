using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Threading;
using UnityEditorInternal;

namespace CodeTodoList
{
	public class CTL_MainWindow : EditorWindow
	{
		public static CTL_MainWindow sInstance;

		const string WINDOW_NAME = "Code Todo List";

		enum eMenuPanel
		{
			MAIN,
			KEYWORD_LIST,
			SETTINGS,
		}

		eMenuPanel mCurPanel = eMenuPanel.MAIN;

		float mRepaintDeltaTime = 0;
		long mPreviousRepaintTime = 0;

		public static bool sIsRefreshing = false;
		public static bool sNeedRefreshAuto = true;
		static bool sProgressBarNeedClear = false;
		static bool sRefreshJustFinished = false;

		Thread mRefreshThread;
		CTL_RefreshWorker mRefreshWorker;
		static int sFileParsed;
		static int sFileCount;
		static string sRefreshStepName = "";

		bool mIsDragging = false;
		float mDragOffset = 0;

		bool mMouseIsInWindow = false;

		[MenuItem("Assets/" + WINDOW_NAME + "/" + "Exclude path")]
		private static void Contextual_AddExcludedPath()
		{
			if (Selection.activeObject == null)
				return;

			List<string> notRelativePaths = new List<string>();
			string path;
			foreach (UnityEngine.Object obj in Selection.objects)
			{
				path = AssetDatabase.GetAssetPath(obj);
				FileInfo fileinfo = new FileInfo(path);
				if (fileinfo.Extension.ToLower() != ".cs" && fileinfo.Extension.ToLower() != ".js")
				{
					DirectoryInfo dirInfo = new DirectoryInfo(path);
					if (!dirInfo.Exists)
						continue;
				}
				notRelativePaths.Add(CTL_Tools.GetNotRelativePath(path));
			}
			CTL_Settings.AddExludedPath(notRelativePaths.ToArray());
		}


		Vector2 mScrollPosMainTab = Vector2.zero;
		Vector2 mScrollPosKeywordsTab = Vector2.zero;
		Vector2 mScrollPosSettingsTab = Vector2.zero;

		static List<CTL_TodoFile> sFilesObjects = new List<CTL_TodoFile>();


		CTL_MCH_TodoObject mMultiColumnHeader_TodoObjects;

		public static void ClearFileObjectList()
		{
			sFilesObjects.Clear();
		}

		[MenuItem("Window/" + WINDOW_NAME)]
		public static void OpenWindow()
		{
			sInstance = (CTL_MainWindow)EditorWindow.GetWindow(typeof(CTL_MainWindow));
			sInstance.titleContent = new GUIContent(WINDOW_NAME);
			//EditorApplication.wantsToQuit += OnWantsToQuitEditor;
		}

		/*
		static bool OnWantsToQuitEditor()
		{
			return true;
		}
		*/

		void OnInspectorUpdate()
		{
			Repaint();
		}

		void OnLostFocus()
		{
			if (mIsDragging)
			{
				CTL_Settings.KeywordsList.ClearAllDraggedObjects();
				mIsDragging = false;
			}
		}		

		void OnDestroy()
		{
			CTL_Settings.SearchFieldContent = "";
			if (CTL_Settings.sShouldSave)
				CTL_Settings.Instance.Export();

			//CTL...if window closed manually, reset column size
			//CTL...if (!sWantsToQuit)
			//CTL...{
			//CTL...	CTL_Settings.ResetColumnSizePersistance();
			//CTL...}
		}

		private void Update()
		{
			//CTL...refresh window smoothly when needed
			if (sIsRefreshing || mIsDragging || mMouseIsInWindow)
			{
				Repaint();
			}
		}

		void OnGUI()
		{
			if (sInstance == null)
				OpenWindow();

			if (Event.current.type == EventType.Repaint)
			{
				mRepaintDeltaTime = (float)(CTL_Tools.ThreadSafeTimeStamp() - mPreviousRepaintTime) / 1000f;
				mPreviousRepaintTime = CTL_Tools.ThreadSafeTimeStamp();
			}
			Init();

			if (sRefreshJustFinished)
			{
				mMultiColumnHeader_TodoObjects.RefreshSorting();
				sRefreshJustFinished = false;
			}

			mMouseIsInWindow = (Event.current.mousePosition.x > 0 && 
				Event.current.mousePosition.x < position.width && 
				Event.current.mousePosition.y > 0 && 
				Event.current.mousePosition.y < position.height);

			DrawHeader();

			switch (mCurPanel)
			{
				case eMenuPanel.MAIN:
					DrawMainPanel();
					if (Event.current.type == EventType.Layout && sNeedRefreshAuto && CTL_Settings.AutoRefresh)
					{
						RefreshList();
					}
					break;
				case eMenuPanel.SETTINGS:
					DrawSettingsPanel();
					break;
				case eMenuPanel.KEYWORD_LIST:
					DrawKeywordsListPanel();
					break;
			}

			if(mIsDragging)
			{
				if(mCurPanel != eMenuPanel.KEYWORD_LIST)
				{
					CTL_Settings.KeywordsList.ClearAllDraggedObjects();
					mIsDragging = false;
				}
			}


			CTL_Settings.Instance.AutoSave();
		}



		//CTL...DRAW header
		void DrawHeader()
		{
			GUILayout.BeginHorizontal();

			//CTL...HEADER btn Main/ManageKeyword
			mCurPanel = (eMenuPanel)GUILayout.Toolbar((int)mCurPanel, CTL_Styles.sGUIC_Header_TabsKeywordComment, GUILayout.Height(CTL_Styles.HEADER_ELEMENTS_HEIGHT));

			//CTL...HEADER search field
			CTL_TodoObject.SearchTerm = GUILayout.TextField(CTL_Settings.SearchFieldContent, CTL_Styles.sStyle_Toolbar_SearchField);
			CTL_Settings.SearchFieldContent = CTL_TodoObject.SearchTerm;

			//CTL...HEADER btn toggle visibility
			if (GUILayout.Button(CTL_Styles.sGUIC_Header_VisibilityFilter, GUILayout.Width(30), GUILayout.Height(CTL_Styles.HEADER_ELEMENTS_HEIGHT)))
			{
				GenericMenu menu = new GenericMenu();
				menu.allowDuplicateNames = true;
				menu.AddItem(new GUIContent("show all"), false, () => { CTL_Settings.KeywordsList.ToggleAllVisibility(true); });
				menu.AddItem(new GUIContent("show nothing"), false, () => { CTL_Settings.KeywordsList.ToggleAllVisibility(false); });
				menu.AddSeparator("");

				for(int i = 0; i < CTL_Settings.KeywordsList.mKeywords.Count; i++)
				{
					int id = i;
					menu.AddItem(new GUIContent(CTL_Settings.KeywordsList.mKeywords[id].Key), 
						CTL_Settings.KeywordsList.mKeywords[id].Visible, 
						() => { CTL_Settings.KeywordsList.mKeywords[id].Visible = !CTL_Settings.KeywordsList.mKeywords[id].Visible; });
				}

				menu.ShowAsContext();
			}

			//CTL...HEADER btn refresh comment list
			if (!sIsRefreshing)
			{
				if (GUILayout.Button(CTL_Styles.sGUIC_Header_RefreshCommentList, GUILayout.Width(30), GUILayout.Height(CTL_Styles.HEADER_ELEMENTS_HEIGHT)))
				{
					RefreshList();
				}
			}
			else
			{
				GUIStyle style = new GUIStyle(EditorStyles.label);
				style.alignment = TextAnchor.MiddleCenter;
				GUILayout.Label(GetRefreshProgressPercent() + "%", style, GUILayout.Width(30), GUILayout.Height(CTL_Styles.HEADER_ELEMENTS_HEIGHT));
			}

			//CTL...HEADER btn settings
			bool settingsSelected = (GUILayout.Toolbar((int)mCurPanel - 2, new GUIContent[] { CTL_Styles.sGUIC_Header_TabOptions }, GUILayout.Width(30), GUILayout.Height(CTL_Styles.HEADER_ELEMENTS_HEIGHT))) == 0;
			if (settingsSelected)
				mCurPanel = eMenuPanel.SETTINGS;

			GUILayout.EndHorizontal();
		}

		//CTL...DRAW panel settings
		void DrawSettingsPanel()
		{
			mScrollPosSettingsTab = EditorGUILayout.BeginScrollView(mScrollPosSettingsTab);

			bool prevState = false;

			//CTL...SETTINGS toogle auto refresh
			CTL_Settings.AutoRefresh = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_AutoRefresh, CTL_Settings.AutoRefresh);

			//CTL...SETTINGS trim comment
			prevState = CTL_Settings.TrimCommentOnParse;
			CTL_Settings.TrimCommentOnParse = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_TrimCommentOnRefresh, CTL_Settings.TrimCommentOnParse);
			if (CTL_Settings.TrimCommentOnParse != prevState)
				sNeedRefreshAuto = true;

			//CTL...SETTINGS CS/JS parsing
			prevState = CTL_Settings.ParseCSFiles;
			CTL_Settings.ParseCSFiles = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_ParseCSFiles, CTL_Settings.ParseCSFiles);
			if (CTL_Settings.ParseCSFiles != prevState)
				sNeedRefreshAuto = true;

			prevState = CTL_Settings.ParseJSFiles;
			CTL_Settings.ParseJSFiles = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_ParseJSFiles, CTL_Settings.ParseJSFiles);
			if (CTL_Settings.ParseCSFiles != prevState)
				sNeedRefreshAuto = true;

			//CTL...SETTINGS toogle case sensitive 
			prevState = CTL_Settings.CaseSensitive;
			CTL_Settings.CaseSensitive = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_CaseSensitive, CTL_Settings.CaseSensitive);
			if (CTL_Settings.CaseSensitive != prevState)
				sNeedRefreshAuto = true;


			//CTL...SETTINGS toogle enable help tooltip
			prevState = CTL_Settings.EnableHelpTooltips;
			CTL_Settings.EnableHelpTooltips = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_EnableHelpTooltips, CTL_Settings.EnableHelpTooltips);
			if(CTL_Settings.EnableHelpTooltips != prevState)
			{
				CTL_Styles.SetupStyles(true);
			}


			EventModifiers[] modifiers = new EventModifiers[] { EventModifiers.None, EventModifiers.Shift, EventModifiers.Control, EventModifiers.Alt };
			string[] optionsModifiers = { "None", "Shift", "Control", "Alt" };
			int index = 0;
			for (int i = 0; i < modifiers.Length; i++)
			{
				if (modifiers[i] == CTL_Settings.CommentClickModifier_OpenFile)
				{
					index = i;
					break;
				}
			}
			index = EditorGUILayout.Popup(
				CTL_Styles.sGUIC_Settings_CommentClickModifier_OpenFile,
				index,
				optionsModifiers);
			CTL_Settings.CommentClickModifier_OpenFile = modifiers[index];



			CTL_Settings.eElementColorType[] colorType = new CTL_Settings.eElementColorType[] { CTL_Settings.eElementColorType.SOLID, CTL_Settings.eElementColorType.GRADIENT_LEFT, CTL_Settings.eElementColorType.GRADIENT_RIGHT};
			string[] optionsColorType = { "Solid", "Gradient left", "Gradient right" };
			index = 0;
			for (int i = 0; i < colorType.Length; i++)
			{
				if (colorType[i] == CTL_Settings.ElementColorType)
				{
					index = i;
					break;
				}
			}
			index = EditorGUILayout.Popup(
				CTL_Styles.sGUIC_Settings_ColorType,
				index,
				optionsColorType);

			if(CTL_Settings.ElementColorType != colorType[index])
			{
				CTL_Settings.ElementColorType = colorType[index];
				CTL_Settings.KeywordsList.RefreshAllTextures();
			}
			




			CTL_Settings.eKeywordSortOrder[]  keywordSortOrders = new CTL_Settings.eKeywordSortOrder[] { CTL_Settings.eKeywordSortOrder.ALPHABETICAL, CTL_Settings.eKeywordSortOrder.KEYWORD_ORDER };
			string[] optionskeywordSortOrders = { "Alphabetical", "Keyword order"};
			index = 0;
			for (int i = 0; i < keywordSortOrders.Length; i++)
			{
				if (keywordSortOrders[i] == CTL_Settings.KeywordSortOrder)
				{
					index = i;
					break;
				}
			}
			index = EditorGUILayout.Popup(
				CTL_Styles.sGUIC_Settings_KeywordSortOrder,
				index,
				optionskeywordSortOrders);
			CTL_Settings.KeywordSortOrder = keywordSortOrders[index];

			//CTL...SETTINGS toogle enable comments tooltip
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField("Comments Tooltip", EditorStyles.boldLabel);
			CTL_Settings.EnableCommentTooltip = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_EnableCommentTooltip, CTL_Settings.EnableCommentTooltip);
			if (CTL_Settings.EnableCommentTooltip)
			{
				CTL_Settings.DisplayKeywordInCommentTooltip = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_DisplayKeywordInCommentTooltip, CTL_Settings.DisplayKeywordInCommentTooltip);
				CTL_Settings.DisplayFilenameAndLineInCommentTooltip = EditorGUILayout.Toggle(CTL_Styles.sGUIC_Settings_DisplayFilenameAndLineInCommentTooltip, CTL_Settings.DisplayFilenameAndLineInCommentTooltip);

				index = 0;
				for(int i = 0; i< modifiers.Length; i++)
				{
					if(modifiers[i] == CTL_Settings.DisplayTooltipModifier)
					{
						index = i;
						break;
					}
				}
				index = EditorGUILayout.Popup(
					CTL_Styles.sGUIC_Settings_DisplayTooltipModifier,
					index,
					optionsModifiers);
				CTL_Settings.DisplayTooltipModifier = modifiers[index];
			}
			EditorGUILayout.EndHorizontal();

			//CTL...SETTINGS btn exclude file/folder
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(CTL_Styles.sGUIC_Settings_ExcludeFile))
			{
				CTL_Settings.AddExludedPath(EditorUtility.OpenFilePanel(CTL_Styles.sGUIC_Settings_ExcludeFile.text, CTL_Tools.GetAssetFolderPath(), "cs"));
			}			
			if (GUILayout.Button(CTL_Styles.sGUIC_Settings_ExcludeFolder))
			{
				CTL_Settings.AddExludedPath(EditorUtility.OpenFolderPanel(CTL_Styles.sGUIC_Settings_ExcludeFolder.text, CTL_Tools.GetAssetFolderPath(), ""));
			}
			EditorGUILayout.EndHorizontal();

			//CTL...SETTINGS btn exclude file/folder
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(CTL_Styles.sGUIC_Settings_ExternalFile))
			{
				CTL_Settings.AddExternalPath(EditorUtility.OpenFilePanel(CTL_Styles.sGUIC_Settings_ExternalFile.text, CTL_Tools.GetAssetFolderPath(), "cs"));
			}
			if (GUILayout.Button(CTL_Styles.sGUIC_Settings_ExternalFolder))
			{
				CTL_Settings.AddExternalPath(EditorUtility.OpenFolderPanel(CTL_Styles.sGUIC_Settings_ExternalFolder.text, CTL_Tools.GetAssetFolderPath(), ""));
			}
			EditorGUILayout.EndHorizontal();

			//set root folder
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Root: ASSETS" + CTL_Settings.RelativeRootFolder, EditorStyles.boldLabel);
			if (GUILayout.Button(CTL_Styles.sGUIC_Settings_SetRootFolder))
			{ 
				CTL_Settings.SetRootFolder(EditorUtility.OpenFolderPanel(CTL_Styles.sGUIC_Settings_SetRootFolder.text, CTL_Tools.GetAssetFolderPath(), ""));
			}
			EditorGUILayout.EndHorizontal();

			string excludedPath = null;

			foreach (string pathExcluded in CTL_Settings.ExcludedPaths.PathList)
			{
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("X", GUILayout.Width(30)))
				{
					excludedPath = pathExcluded;
				}
				if (GUILayout.Button("Select", GUILayout.Width(50)))
				{
					Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("ASSETS\\" + pathExcluded);
				}
				GUIStyle pathLabelStyle = new GUIStyle("label");
				if (!CTL_Tools.IsValidPath("ASSETS\\" + pathExcluded))
				{
					pathLabelStyle.normal.textColor = new Color(1f,.25f,.25f);
				}
				EditorGUILayout.LabelField(" - " +CTL_Tools.GetRelativePath(pathExcluded), pathLabelStyle);
				EditorGUILayout.EndHorizontal();
			}

			if(excludedPath != null)
			{
				CTL_Settings.RemoveExludedPath(excludedPath);
			}





			string externalPath = null;

			foreach (string pathExternal in CTL_Settings.ExternalPaths.PathList)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("X", GUILayout.Width(30)))
				{
					externalPath = pathExternal;
				}
				/*
				if (GUILayout.Button("Select", GUILayout.Width(50)))
				{
					Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("ASSETS\\" + pathExternal);
				}
				*/
				GUIStyle pathLabelStyle = new GUIStyle("label");
				/*if (!CTL_Tools.IsValidPath("ASSETS\\" + pathExternal))
				{
					pathLabelStyle.normal.textColor = new Color(1f, .25f, .25f);
				}*/
				EditorGUILayout.LabelField(" + " + pathExternal, pathLabelStyle);
				EditorGUILayout.EndHorizontal();
			}

			if (externalPath != null)
			{
				CTL_Settings.RemoveExternalPath(externalPath);
			}





			EditorGUILayout.EndScrollView();
			GUILayout.FlexibleSpace();


			if (CTL_Settings.DEBUG_MODE)
			{
				//CTL...DEBUG btn load settings
				if (GUILayout.Button("/!\\ Load settings /!\\"))
				{
					CTL_Settings.Load();
				}
				//CTL...DEBUG btn export settings
				if (GUILayout.Button("/!\\ Export settings /!\\"))
				{
					CTL_Settings.Instance.Export();
				}
				//CTL...DEBUG btn refresh styles
				if (GUILayout.Button("/!\\ Refresh Styles /!\\"))
				{
					CTL_Styles.SetupStyles(true);
				}
				//CTL...DEBUG btn delete persistant data
				if (GUILayout.Button("/!\\ DeleteAllPersistantData /!\\"))
				{
					CTL_Settings.DeleteAllPersistantData();
				}
			}

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Read the doc"))
			{
				Application.OpenURL("http://julien-foucher.com/?a=codetodolist&b=doc&s=assetsettings");
			}
			if (GUILayout.Button("Rate this asset"))
			{
				Application.OpenURL("http://julien-foucher.com/?a=codetodolist&b=store&s=assetsettings");
			}
			EditorGUILayout.EndHorizontal();
		}

		//CTL...DRAW panel keywords
		void DrawKeywordsListPanel()
		{
			CTL_KeywordObject toDelete = null;

			bool itemNeedFocus = false;


			Rect elementRect;
			List<Rect> elementsRects = new List<Rect>();
			int draggedId = 0;

			Rect draggedElementRect = new Rect();

			CTL_KeywordObject currentObject;
			CTL_KeywordObject dragedElem = null;

			
			mScrollPosKeywordsTab = EditorGUILayout.BeginScrollView(mScrollPosKeywordsTab);

			for (int i = 0; i < CTL_Settings.KeywordsList.mKeywords.Count; i++)
			{
				currentObject = CTL_Settings.KeywordsList.mKeywords[i];

				
				if (currentObject.mIsDragged)
				{
					GUI.backgroundColor = new Color(0,0,0,.1f);
					GUI.color = new Color(0, 0, 0, .1f);
					draggedId = i;
				}
				else
				{
					GUI.backgroundColor = Color.white;
					GUI.color = Color.white;
				}

				elementRect = EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
				elementsRects.Add(new Rect(elementRect));


				if (GUILayout.Button(currentObject.Visible?CTL_Styles.sGUIC_Keywords_VisibilityToogleON: CTL_Styles.sGUIC_Keywords_VisibilityToogleOFF, GUILayout.Width(30)))
				{
					currentObject.Visible = !currentObject.Visible;
				}

				currentObject.Color = EditorGUILayout.ColorField(new GUIContent(), currentObject.Color, true, true, false, GUILayout.Width(40));


				if (currentObject.Valid)
					GUI.backgroundColor = Color.white;
				else
					GUI.backgroundColor = new Color(1f, .25f, .4f, .4f);

				if (currentObject.mNeedFocus)
				{
					currentObject.mNeedFocus = false;
					GUI.SetNextControlName("itemNeedFocus");
					itemNeedFocus = true;
				}
				currentObject.Key = GUILayout.TextField(currentObject.Key, GUILayout.Width(100));
				if (itemNeedFocus)
				{
					EditorGUI.FocusTextInControl("itemNeedFocus");
				}

				GUI.backgroundColor = Color.white;

				GUILayout.Label(currentObject.Valid ? new GUIContent() : CTL_Styles.sGUIC_ManageKeyword_InvalidWarning, GUILayout.Width(30));
				if (GUILayout.Button("DELETE"))
				{
					toDelete = currentObject;
				}
				GUILayout.FlexibleSpace();

				int objLinked = CTL_TodoObject.GetObjectsForKeyword(currentObject).Count;
				CTL_Styles.sGUIC_Keywords_CommentCountTooltip.text = "[" + objLinked.ToString() + "]";
				GUILayout.Label(CTL_Styles.sGUIC_Keywords_CommentCountTooltip);

				EditorGUILayout.EndHorizontal();

				if (/*!mIsDragging && */elementRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					currentObject.mIsDragged = true;
					mDragOffset = Event.current.mousePosition.y - elementRect.y;
				}

				if (currentObject.mIsDragged)
				{
					draggedElementRect = new Rect(elementRect);
					mIsDragging = true;
					dragedElem = currentObject;
				}
			}

			EditorGUILayout.EndScrollView();

			GUI.backgroundColor = Color.white;
			GUI.color = Color.white;

			if (mIsDragging && elementsRects.Count > 0)
			{
				//CTL...move the shadow rect
				draggedElementRect.y = Event.current.mousePosition.y - mDragOffset;
				GUI.backgroundColor = Color.black;
				GUIStyle style = new GUIStyle(EditorStyles.helpBox);
				style.contentOffset = new Vector2(80,0);
				EditorGUI.LabelField(draggedElementRect, dragedElem.Key, style);
				GUI.backgroundColor = Color.white;

				//CTL...determine closest element top
				int closestId = -1;
				float closestDist = 0;
				float curDist = 0;
				float curElemMiddlePos = (draggedElementRect.y + draggedElementRect.height / 2);

				List<Rect> scrollAdaptedRect = new List<Rect>();
				for (int i = 0; i < elementsRects.Count; i++)
				{
					Rect rect = elementsRects[i];
					rect.y += CTL_Styles.HEADER_HEIGHT;
					rect.y -= mScrollPosKeywordsTab.y;
					scrollAdaptedRect.Add(rect);
				}

				for (int i = 0; i < scrollAdaptedRect.Count; i++)
				{

					//CTL...If we go up in the list, we want to use the top pos of the other element, else, the bot pos
					if (draggedId > i)
						curDist = Mathf.Abs((scrollAdaptedRect[i].y) - curElemMiddlePos);
					else
						curDist = Mathf.Abs((scrollAdaptedRect[i].y + (scrollAdaptedRect[i].height)) - curElemMiddlePos);

					if (closestId < 0 || closestDist > curDist)
					{
						closestId = i;
						closestDist = curDist;
					}
				}

				//CTL...draw dropper pos
				if (draggedId != closestId)
				{
					Rect dropperRect = new Rect(scrollAdaptedRect[closestId]);

					//CTL...If we go up in the list, we want to use the top pos of the other element, else, the bot pos
					if (draggedId < closestId)
						dropperRect.y += scrollAdaptedRect[closestId].height;

					//CTL...draw the bar with a small fade fx
					dropperRect.height = 1;
					dropperRect.y -= 0;
					EditorGUI.DrawRect(dropperRect, new Color(0, 0, 0, .25f));

					dropperRect.height = 3;
					dropperRect.y -= 1;
					EditorGUI.DrawRect(dropperRect, new Color(0, 0, 0, .25f));

					dropperRect.height = 5;
					dropperRect.y -= 1;
					dropperRect.x += 1;
					dropperRect.width -= 2;
					EditorGUI.DrawRect(dropperRect, new Color(0, 0, 0, .25f));
					
				}



				//CTL...mouse up check
				int controlId = GUIUtility.GetControlID(FocusType.Passive);
				Event evt = Event.current;
				if (evt.GetTypeForControl(controlId) == EventType.MouseDown && evt.button == 0)
				{
					GUIUtility.hotControl = controlId;
				}			
				if (evt.GetTypeForControl(controlId) == EventType.MouseUp && evt.button == 0)
				{
					GUIUtility.hotControl = 0;
					mIsDragging = false;
					CTL_KeywordObject draggedObj = CTL_Settings.KeywordsList.GetDraggedObject();
					if (draggedObj != null)
					{
						draggedObj.mIsDragged = false;
						//CTL...MOVE ITEM
						if (draggedId != closestId)
						{
							CTL_Settings.KeywordsList.MoveElementToPosInList(draggedObj, closestId);
						}
						CTL_Settings.KeywordsList.ClearAllDraggedObjects();
					}
					evt.Use();
				}
			}
			
			if (toDelete != null)
			{
				CTL_Settings.KeywordsList.RemoveKeyword(toDelete);
			}
			if (CTL_Settings.DEBUG_MODE)
			{
				if (GUILayout.Button("Add MULTIPLE Keys"))
				{
					for (int i = 0; i < 20; i++)
					{
						CTL_Settings.KeywordsList.AddKeyword("", true, true);
					}
				}
			}


			if (GUILayout.Button("Add Key"))
			{
				CTL_Settings.KeywordsList.AddKeyword("",true,true);
			}
		}
		
		float GetCommentHeight(string msg)
		{
			Rect colRectMsg = mMultiColumnHeader_TodoObjects.GetDrawRect(CTL_MCH_TodoObject.eColumnType.COMMENT);
			CTL_Styles.sStyle_Main_Comment.fixedWidth = colRectMsg.width - CTL_Styles.sStyle_Main_Comment.contentOffset.x;
			return GUILayoutUtility.GetRect(new GUIContent(msg), CTL_Styles.sStyle_Main_Comment, GUILayout.ExpandWidth(false)).height;
		}

		//CTL...DRAW single comment element
		Rect DrawElement(CTL_TodoObject obj, int startY, float windowWidth, float headerWidth)
		{
			
			float curMaxHeight = CTL_Styles.MAIN_TAB_ELEMENT_HEIGHT;
			Rect colRect;
			GUIContent guiContent = new GUIContent();

			CTL_Styles.sStyle_Main_ElementLabel.alignment = TextAnchor.MiddleLeft;
			float maxWidth = windowWidth;

			if (headerWidth > maxWidth)
				maxWidth = headerWidth;

			Rect fullElem = new Rect(0, startY, maxWidth, curMaxHeight);
			Rect drawRect = new Rect(.25f, 0, .5f, 1);

			if (obj == null)
				return fullElem;

			//GUI.DrawTexture(fullElem, obj.mLinkedKeyword.mTexture, ScaleMode.StretchToFill, true, 2f);
			GUI.DrawTextureWithTexCoords(fullElem, obj.mLinkedKeyword.GetTexture(), drawRect);

			//CTL...draw hover
			
			if (fullElem.Contains(Event.current.mousePosition))
			{				
				GUI.DrawTexture(fullElem, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1f, new Color(1,1,1,.3f), 0, 0);
			}
			//GUI.DrawTexture(new Rect(0, startY, maxWidth, 1), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1f, new Color(.5f, .5f, .5f, .5f), 0, 0);
			

			bool jumpedToLine = false;
			bool triedJumpToLine = false;
			float x = 0;

			if (mMultiColumnHeader_TodoObjects.IsColumnVisible(CTL_MCH_TodoObject.eColumnType.KEYWORD))
			{
				colRect = mMultiColumnHeader_TodoObjects.GetDrawRect(CTL_MCH_TodoObject.eColumnType.KEYWORD);
				guiContent.text = obj.mLinkedKeyword.Key;
				if (GUI.Button(new Rect(x, startY, colRect.width - CTL_Styles.sStyle_Main_ElementLabel.contentOffset.x, curMaxHeight), 
					guiContent, 
					CTL_Styles.sStyle_Main_ElementLabel) && 
					Event.current.modifiers == CTL_Settings.CommentClickModifier_OpenFile)
				{
					triedJumpToLine = true;
					jumpedToLine = obj.JumpToLine();
				}
				x += colRect.width;
			}

			if (mMultiColumnHeader_TodoObjects.IsColumnVisible(CTL_MCH_TodoObject.eColumnType.COMMENT))
			{
				colRect = mMultiColumnHeader_TodoObjects.GetDrawRect(CTL_MCH_TodoObject.eColumnType.COMMENT);
				guiContent.text = obj.mMessage;
				if (GUI.Button(new Rect(x, startY, colRect.width - CTL_Styles.sStyle_Main_ElementLabel.contentOffset.x, curMaxHeight), 
					guiContent, 
					CTL_Styles.sStyle_Main_ElementLabel) 
					 && Event.current.modifiers == CTL_Settings.CommentClickModifier_OpenFile)
				{
					triedJumpToLine = true;
					jumpedToLine = obj.JumpToLine();
				}
				x += colRect.width;
			}

			if (mMultiColumnHeader_TodoObjects.IsColumnVisible(CTL_MCH_TodoObject.eColumnType.FILE_NAME))
			{
				colRect = mMultiColumnHeader_TodoObjects.GetDrawRect(CTL_MCH_TodoObject.eColumnType.FILE_NAME);
				guiContent.text =obj.mFile.mName;
				if (GUI.Button(new Rect(x, startY, colRect.width - CTL_Styles.sStyle_Main_ElementLabel.contentOffset.x, curMaxHeight), 
					guiContent, 
					CTL_Styles.sStyle_Main_ElementLabel) && 
					Event.current.modifiers == CTL_Settings.CommentClickModifier_OpenFile)
				{
					//CTL...todo : on click item select file in project OR open menu asking if you want to exclude file
					triedJumpToLine = true;
					jumpedToLine = obj.JumpToLine();
				}
				x += colRect.width;
			}
			
			if (mMultiColumnHeader_TodoObjects.IsColumnVisible(CTL_MCH_TodoObject.eColumnType.LINE_NUMBER))
			{
				colRect = mMultiColumnHeader_TodoObjects.GetDrawRect(CTL_MCH_TodoObject.eColumnType.LINE_NUMBER);
				guiContent.text = obj.mLineNumber.ToString();
				CTL_Styles.sStyle_Main_ElementLabel.alignment = TextAnchor.MiddleRight;
				if (GUI.Button(new Rect(x, startY, colRect.width - CTL_Styles.sStyle_Main_ElementLabel.contentOffset.x, curMaxHeight), 
					guiContent, 
					CTL_Styles.sStyle_Main_ElementLabel) && 
					Event.current.modifiers == CTL_Settings.CommentClickModifier_OpenFile)
				{
					triedJumpToLine = true;
					jumpedToLine = obj.JumpToLine();
				}
				x += colRect.width;
			}

			if (triedJumpToLine && !jumpedToLine)
				Debug.LogWarning("Error opening file " + obj.mFile.mRelativePath + " at line " + obj.mLineNumber);

			return fullElem;
		}

		//CTL...DRAW tooltip comment element
		void DrawElementTooltip(Rect elementRect, Rect windowRect, CTL_TodoObject obj)
		{
			//CTL...todo : if hold modifier key : more detailed tooltip ?
			Rect drawingRect;
			float paddingRight = 20;
			string contentStr = "";
			if (CTL_Settings.DisplayKeywordInCommentTooltip)
				contentStr += obj.mLinkedKeyword.Key + "\n";
			contentStr += obj.mLineContent;
			if (CTL_Settings.DisplayFilenameAndLineInCommentTooltip)
				contentStr += "\n" + obj.mFile.mName + " (L:" + obj.mLineNumber + ")";

			GUIContent _content = new GUIContent(contentStr);
			GUIStyle _style = new GUIStyle(EditorStyles.textArea);
			_style.fixedWidth = (windowRect.width - paddingRight) * .8f;
			_style.wordWrap = true;

			float calculedHeight = _style.CalcHeight(_content, _style.fixedWidth);
			Vector2 _widthHeight = new Vector2(_style.fixedWidth, calculedHeight);
			float _topDecal = elementRect.y - _widthHeight.y;

			if (_topDecal < 0)
			{
				//CTL...tooltip hit the top, draw it under
				drawingRect = new Rect(mScrollPosMainTab.x + windowRect.width - _widthHeight.x - paddingRight, elementRect.y + elementRect.height, _widthHeight.x, _widthHeight.y);
			}
			else
			{
				//CTL...toltip displayed on top of element
				drawingRect = new Rect(mScrollPosMainTab.x + windowRect.width - _widthHeight.x - paddingRight, elementRect.y - _widthHeight.y, _widthHeight.x, _widthHeight.y);
			}

			EditorGUI.LabelField(drawingRect, _content, _style);
		}

		//CTL...DRAW panel main
		void DrawMainPanel()
		{
			if (sIsRefreshing)
			{
				string msg = sRefreshStepName;
				float progressDisplayed = CTL_RefreshWorker.GetTotalProgress();
				Rect progressBarRect = new Rect(position.width * .125f, position.height * .4f, position.width * .75f, 20);
				Rect progressBarTextRect = new Rect(position.width * .125f, position.height * .4f, position.width * .75f, 20);
				EditorGUI.ProgressBar(progressBarRect, progressDisplayed, msg);
				EditorGUI.LabelField(progressBarTextRect, GetRefreshProgressPercent() + "%", new GUIStyle("label") { alignment = TextAnchor.MiddleRight});
				return;
			}
			if (sProgressBarNeedClear)
			{
				if(Event.current.type == EventType.Repaint)
					sProgressBarNeedClear = false;
				return;
			}



			Rect headerRect = mMultiColumnHeader_TodoObjects.Draw(position, CTL_Styles.HEADER_HEIGHT, mScrollPosMainTab.x);
			GUILayout.Space(headerRect.height);

			mScrollPosMainTab = EditorGUILayout.BeginScrollView(mScrollPosMainTab);

			float headerWidth = mMultiColumnHeader_TodoObjects.GetHeaderWidth();

			CTL_TodoObject.GenerateDisplayedElementListIfNeeded();
			EditorGUILayout.LabelField("",GUILayout.Width(headerWidth-8));  //CTL...space only horizontal

			GUIStyle style = new GUIStyle();
			style.border = new RectOffset(0, 0, 0, 0);
			EditorGUILayout.BeginVertical(style);
			GUILayout.Space(CTL_Styles.MAIN_TAB_ELEMENT_HEIGHT * (CTL_TodoObject.sDisplayedElementList.Count - 1));

			int startElement = (int)mScrollPosMainTab.y / CTL_Styles.MAIN_TAB_ELEMENT_HEIGHT;
			int totalHeight = 0;

			int hoveredElementId = -1;
			Rect hoveredElementRect = new Rect();
			Rect curElemRect;


			//CTL...draw only visible elements
			for (int i = startElement; i < CTL_TodoObject.sDisplayedElementList.Count; i++)
			{
				curElemRect = DrawElement(CTL_TodoObject.sDisplayedElementList[i], i * CTL_Styles.MAIN_TAB_ELEMENT_HEIGHT, position.width, headerWidth);
				totalHeight += (int)curElemRect.height;

				if (curElemRect.Contains(Event.current.mousePosition))
				{
					hoveredElementId = i;
					hoveredElementRect = new Rect(curElemRect);
				}

				if (totalHeight > position.height - CTL_Styles.HEADER_HEIGHT - CTL_Styles.MAIN_TAB_MCH_HEIGHT)
				{
					break;
				}
			}
			
			if(hoveredElementId >= 0 && CTL_Settings.EnableCommentTooltip && Event.current.modifiers == CTL_Settings.DisplayTooltipModifier)
			{
				DrawElementTooltip(hoveredElementRect, position, CTL_TodoObject.sDisplayedElementList[hoveredElementId]);
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}

		public static void worker_WorkCompleted(List<CTL_TodoFile> todoFileList, bool cancelled)
		{
			sFilesObjects = todoFileList;
			sIsRefreshing = false;
			sProgressBarNeedClear = true;
			sRefreshJustFinished = true;
			CTL_TodoObject.sShouldRefreshSearch = true;
		}
		public static void worker_ProcessChanged(int fileParsed, int fileCount, string message)
		{
			sRefreshStepName = message;
			sFileParsed = fileParsed;
			sFileCount = fileCount;
		}

		void Init()
		{
			CTL_Styles.SetupStyles();

			if (mMultiColumnHeader_TodoObjects == null)
			{
				mMultiColumnHeader_TodoObjects = new CTL_MCH_TodoObject();
			}
		}

		void RefreshList()
		{
			//CTL...generate settings in case not generated before
			CTL_Settings.GenerateInstanceIfNotExist();

			sNeedRefreshAuto = false;
			if (sIsRefreshing)
				return;

			//CTL...clear refs
			for (int i = 0; i < sFilesObjects.Count; i++)
			{
				sFilesObjects[i].Clear();
			}
			sFilesObjects.Clear();

			//CTL...start refresh thread
			sFileParsed = 0;
			sFileCount = 0;
			sIsRefreshing = true;
			mRefreshWorker = new CTL_RefreshWorker();
			mRefreshWorker.ProcessChanged += worker_ProcessChanged;
			mRefreshWorker.WorkCompleted += worker_WorkCompleted;
			mRefreshThread = new Thread(mRefreshWorker.Work);
			mRefreshThread.Start();
		}

		int GetRefreshProgressPercent()
		{
			return (int)(CTL_RefreshWorker.GetTotalProgress() * 100);
		}
	}
}
