using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace CodeTodoList
{
	public class CTL_Styles
	{
		static bool sStyleInit = false;

		public static int HEADER_ELEMENTS_HEIGHT = 22;
		public static int HEADER_HEIGHT = HEADER_ELEMENTS_HEIGHT + 4;

		public static int MAIN_TAB_ELEMENT_HEIGHT = 20;
		public static int MAIN_TAB_MCH_HEIGHT = 25;

		public static GUIStyle sStyle_Toolbar_Btn;
		public static GUIStyle sStyle_ToolbarTBtnDisabled;
		public static GUIStyle sStyle_Toolbar_BtnSelected;
		public static GUIStyle sStyle_Toolbar_SearchField;


		public static GUIStyle sStyle_Main_Comment;
		public static GUIStyle sStyle_Main_ElementLabel;


		public static GUIContent sGUIC_Settings_CaseSensitive;
		public static GUIContent sGUIC_Settings_EnableHelpTooltips;
		public static GUIContent sGUIC_Settings_AutoRefresh;
		public static GUIContent sGUIC_Settings_GroupMode;
		public static GUIContent sGUIC_Settings_ExcludeFile;
		public static GUIContent sGUIC_Settings_ExcludeFolder;
		public static GUIContent sGUIC_Settings_ExternalFile;
		public static GUIContent sGUIC_Settings_ExternalFolder;
		public static GUIContent sGUIC_Settings_CommentClickModifier_OpenFile;
		public static GUIContent sGUIC_Settings_ColorType;
		public static GUIContent sGUIC_Settings_SetRootFolder;


		public static GUIContent sGUIC_Settings_ParseCSFiles;
		public static GUIContent sGUIC_Settings_ParseJSFiles;
		public static GUIContent sGUIC_Settings_KeywordSortOrder;
		public static GUIContent sGUIC_Settings_TrimCommentOnRefresh;



		public static GUIContent sGUIC_Settings_EnableCommentTooltip;
		public static GUIContent sGUIC_Settings_DisplayKeywordInCommentTooltip;
		public static GUIContent sGUIC_Settings_DisplayFilenameAndLineInCommentTooltip;
		public static GUIContent sGUIC_Settings_DisplayTooltipModifier;

		public static GUIContent sGUIC_Main_MCH_ColumnKeyword;
		public static GUIContent sGUIC_Main_MCH_ColumnComment;
		public static GUIContent sGUIC_Main_MCH_ColumnFileName;
		public static GUIContent sGUIC_Main_MCH_ColumnLineNumber;



		public static GUIContent sGUIC_Keywords_CommentCountTooltip;
		public static GUIContent sGUIC_Keywords_VisibilityToogleON;
		public static GUIContent sGUIC_Keywords_VisibilityToogleOFF;



		public static GUIContent sGUIC_Header_TabOptions;
		public static GUIContent sGUIC_Header_TabMain;
		public static GUIContent sGUIC_Header_TabManageKeywords;
		public static GUIContent sGUIC_Header_RefreshCommentList;
		public static GUIContent sGUIC_Header_VisibilityFilter;
		public static GUIContent[] sGUIC_Header_TabsKeywordComment;


		public static GUIContent sGUIC_ManageKeyword_InvalidWarning;



		static GUIStyleState CopyStyleState(GUIStyleState styleOther)
		{
			GUIStyleState newStyle = new GUIStyleState();
			newStyle.background = styleOther.background;
			newStyle.scaledBackgrounds = styleOther.scaledBackgrounds;
			newStyle.textColor = styleOther.textColor;
			return newStyle;
		}

		public static void SetupStyles(bool force = false)
		{
			if (!force && sStyleInit)
				return;

			sGUIC_Header_TabOptions = GenerateContentIcon(new string[] { "SettingsIcon", "pane options@2x", "pane options", "ol_plus" }, "opt", "Open the options panel");
			sGUIC_Header_RefreshCommentList = GenerateContentIcon(new string[] { "RotateTool", "WaitSpin00" }, "updt", "Refresh the comment list");
			sGUIC_ManageKeyword_InvalidWarning = GenerateContentIcon(new string[] { "console.warnicon.inactive.sml", "UnityEditor.InspectorWindow" }, "Invalid", "This keyword is invalid. It may be because it is identical to another keyword, or maybe because it is empty...");
			sGUIC_Header_VisibilityFilter = GenerateContentIcon(new string[] { "scenevis_visible_hover", "animationvisibilitytoggleon", "FilterByType", "FilterByLabel" }, "vis", "Toogle visibility of different keywords");

			sGUIC_Keywords_VisibilityToogleON = GenerateContentIcon(new string[] { "scenevis_visible_hover", "animationvisibilitytoggleon", "ShurikenToggleFocusedOn" }, "vis", "Toogle visibility of this keyword");
			sGUIC_Keywords_VisibilityToogleOFF = GenerateContentIcon(new string[] { "scenevis_hidden", "animationvisibilitytoggleoff", "ShurikenToggleFocused" }, "vis", "Toogle visibility of this keyword");

			sGUIC_Header_TabMain = GenerateContent("Comments", "This is where the magic happend, visualise all your pinned comments here!");
			sGUIC_Header_TabManageKeywords = GenerateContent("Keywords", "Edit the keywords list used to detect your pinned comments");
			sGUIC_Settings_CaseSensitive = GenerateContent("Case sensitive keyword", "When activated, the keyword list generation will be case sensitive.");
			sGUIC_Settings_EnableHelpTooltips = GenerateContent("Enable help tooltips", "When activated, some buttons will have tooltip (like the one you're reading right now!). You can disable it if you find that they are sometimes anoying ;)");
			sGUIC_Settings_GroupMode = GenerateContent("Group mode:", "Select if you want to sort the comments by keyword or by file");
			sGUIC_Settings_ExcludeFile = GenerateContent("Exclude file", "You can exclude a path to a file that you don't want to be parsed by the tool");
			sGUIC_Settings_ExcludeFolder = GenerateContent("Exclude folder", "You can exclude a path to a folder that you don't want to be parsed by the tool");
			sGUIC_Settings_AutoRefresh = GenerateContent("Auto refresh", "If enabled, the tool will automatically refresh the comments list when needed (after the keywords are modified, after the project compilation etc...)");
			sGUIC_Settings_CommentClickModifier_OpenFile = GenerateContent("Open file modifier", "Select a modifier that you have to use in order to open the file at the comment's line (ex : shift+click on a comment in the main tab to jump to it)");
			sGUIC_Settings_ColorType = GenerateContent("Comment color style", "");
			sGUIC_Settings_KeywordSortOrder = GenerateContent("Keyword sort order", "Define the way the objects will be sorted in the comment section when you click on the heyword header. You can sort it using the order defined in the keyword section or alphaabetically");
			sGUIC_Settings_TrimCommentOnRefresh = GenerateContent("Trim comment", "If activated, the comment list will be generated with trimmed comments (ex: \"//todo\" and \"// todo\" will be detected with \"todo\" as keyword)\nThanks to Shminitz for the feature request, go check his \"Domiverse\" game!");

			sGUIC_Settings_ExternalFile = GenerateContent("Include external file", "");
			sGUIC_Settings_ExternalFolder = GenerateContent("Include external folder", "");
			sGUIC_Settings_SetRootFolder = GenerateContent("Set root folder", "You can specify a root folder. This has to be in the project folder.");


			sGUIC_Settings_ParseCSFiles = GenerateContent("Search in .cs files", "Enable the C# files parsing");
			sGUIC_Settings_ParseJSFiles = GenerateContent("Search in .js files", "Enable the JavaScript files parsing");


			sGUIC_Settings_EnableCommentTooltip = GenerateContent("Show comment tooltip", "Display a tooltip when you hover a comment in the comment list, containing the full comment");
			sGUIC_Settings_DisplayKeywordInCommentTooltip = GenerateContent("Show keyword", "Display in the tooltip the keyword linked to the comment you hover");
			sGUIC_Settings_DisplayFilenameAndLineInCommentTooltip = GenerateContent("Show line and filename", "Display in the tooltip the line and filename linked to the comment you hover");
			sGUIC_Settings_DisplayTooltipModifier = GenerateContent("Display modifier", "Display the comment tooltip only when this modifier is active");

			sGUIC_Main_MCH_ColumnKeyword = GenerateContent("Keyword", "Keyword used to find the comment");
			sGUIC_Main_MCH_ColumnComment = GenerateContent("Message", "Content of the comment");
			sGUIC_Main_MCH_ColumnFileName = GenerateContent("FileName", "Name of the file containing the comment");
			sGUIC_Main_MCH_ColumnLineNumber = GenerateContent("Line", "Line of the comment in the file");


			sGUIC_Keywords_CommentCountTooltip = GenerateContent("", "Comment linked to this keyword count. May need a refresh if you just added the keyword (hit the refresh button int the right up corner)");

			sStyle_Main_Comment = new GUIStyle();
			sStyle_Main_Comment.border = new RectOffset(0, 0, 0, 0);
			sStyle_Main_Comment.clipping = TextClipping.Clip;
			sStyle_Main_Comment.alignment = TextAnchor.MiddleLeft;
			sStyle_Main_Comment.contentOffset = new Vector2(4, 0);
			sStyle_Main_Comment.fixedHeight = 0;
			sStyle_Main_Comment.wordWrap = true;

			sStyle_Toolbar_Btn = new GUIStyle(EditorStyles.toolbarButton);

			sStyle_Toolbar_BtnSelected = new GUIStyle(EditorStyles.toolbarButton);
			sStyle_Toolbar_BtnSelected.normal = CopyStyleState(new GUIStyle(EditorStyles.toolbarButton).active);
			sStyle_Toolbar_BtnSelected.onNormal = CopyStyleState(new GUIStyle(EditorStyles.toolbarButton).onActive);

			sStyle_ToolbarTBtnDisabled = new GUIStyle(EditorStyles.toolbarButton);
			Color _col = sStyle_ToolbarTBtnDisabled.normal.textColor;
			_col.a = .5f;
			sStyle_ToolbarTBtnDisabled.normal.textColor = _col;
			sStyle_ToolbarTBtnDisabled.active = sStyle_ToolbarTBtnDisabled.normal;

			sStyle_Main_ElementLabel = new GUIStyle();
			sStyle_Main_ElementLabel.border = new RectOffset(0, 0, 0, 0);
			sStyle_Main_ElementLabel.clipping = TextClipping.Clip;
			sStyle_Main_ElementLabel.alignment = TextAnchor.MiddleLeft;
			sStyle_Main_ElementLabel.fixedHeight = CTL_Styles.MAIN_TAB_ELEMENT_HEIGHT;
			sStyle_Main_ElementLabel.contentOffset = new Vector2(4, 0);

			GUIStyle Base_ToolbarSeachTextField = GUI.skin.FindStyle("ToolbarSeachTextField");
			if (Base_ToolbarSeachTextField == null)
			{
				Base_ToolbarSeachTextField = GUI.skin.FindStyle("ToolbarSeachTextFieldPopup");
			}
			if (Base_ToolbarSeachTextField == null)
			{
				Base_ToolbarSeachTextField = GUI.skin.FindStyle("ToolbarSeachCancelButtonEmpty");
			}
			if (Base_ToolbarSeachTextField == null)
			{
				Base_ToolbarSeachTextField = GUI.skin.FindStyle("SearchTextField");
			}
			if (Base_ToolbarSeachTextField == null)
			{
				Base_ToolbarSeachTextField = new GUIStyle(EditorStyles.toolbarButton);
			}
			sStyle_Toolbar_SearchField = new GUIStyle(Base_ToolbarSeachTextField);


			sGUIC_Header_TabsKeywordComment = new GUIContent[] { CTL_Styles.sGUIC_Header_TabMain, CTL_Styles.sGUIC_Header_TabManageKeywords };

			sStyleInit = true;
		}

		static GUIContent GenerateContent(string contentStr, string tooltip)
		{
			GUIContent content = new GUIContent(contentStr);
			if (CTL_Settings.EnableHelpTooltips)
				content.tooltip = tooltip;
			return content;
		}

		static GUIContent GenerateContentIcon(string contentIconStr, string contentStrFallback, string tooltip)
		{
			return GenerateContentIcon(new string[] { contentIconStr }, contentStrFallback, tooltip);
		}

		static GUIContent GenerateContentIcon(string[] contentIconStr, string contentStrFallback, string tooltip)
		{
			GUIContent content;
			try
			{
				Texture2D iconTex = null;
				for (int i = 0; i < contentIconStr.Length; i++)
				{
					iconTex = EditorGUIUtility.FindTexture(contentIconStr[i]);
					if (iconTex != null)
						break;
				}
				if (iconTex != null)
					content = new GUIContent(iconTex);
				else
					content = new GUIContent(contentStrFallback);
			}
			catch (Exception)
			{
				content = new GUIContent(contentStrFallback);
			}
			if (CTL_Settings.EnableHelpTooltips)
				content.tooltip = tooltip;
			return content;
		}
	}
}
