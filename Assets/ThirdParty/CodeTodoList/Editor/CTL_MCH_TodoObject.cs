using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace CodeTodoList
{
	public class CTL_MCH_TodoObject
	{
		const int COLUMN_MIN_WIDTH = 10;

		public static bool sNeedInit = true;

		MultiColumnHeader mHeader;
		MultiColumnHeaderState mState;
		float mPadTop = 0;
		float mPreviousWidth = 0;

		int mFramesBeforeCanModifyAutoresize = 10;

		MultiColumnHeaderState.Column[] mColumns;
		MultiColumnHeaderState.Column mColumnKeyword;
		MultiColumnHeaderState.Column mColumnComment;
		MultiColumnHeaderState.Column mColumnFileName;
		MultiColumnHeaderState.Column mColumnLineNumber;


		public enum eColumnType
		{
			KEYWORD,
			COMMENT,
			FILE_NAME,
			LINE_NUMBER
		}

		public float GetHeaderWidth()
		{
			return mState.widthOfAllVisibleColumns;
		}

		public bool IsColumnVisible(eColumnType type)
		{
			return CTL_Settings.ColumnVisibility[(int)type];
		}

		public Rect GetDrawRect(eColumnType type)
		{
			return mHeader.GetColumnRect(mHeader.GetVisibleColumnIndex((int)type));
		}

		bool mFirstDraw = true;
		bool mAdaptedSizeChangedLastFrame = false;

		float mPreviousWindowWidth = 0;
		float mPreviousAdaptedWidth = 0;
		int mFramesBeforeCanAutoresize = 0;

		void ChangeAutoResize(bool canAutoresize)
		{
			if (mFramesBeforeCanModifyAutoresize > 0)
				return;
			mColumnComment.autoResize = canAutoresize;
		}

		public Rect Draw(Rect drawRect, int decalTop, float scrollX)
		{
			if (sNeedInit)
				Init();
			if(mFramesBeforeCanModifyAutoresize > 0)
				mFramesBeforeCanModifyAutoresize--;

			bool canChangeAutoResizeMode = true;
			if(mPreviousWindowWidth != drawRect.width)
			{
				canChangeAutoResizeMode = false;
				ChangeAutoResize(true);
			}

			mPreviousWindowWidth = drawRect.width;

			float width = drawRect.width;
			if (!mFirstDraw)
			{
				float adaptedWidth = GetHeaderWidth();
				if (canChangeAutoResizeMode)
				{
					if (adaptedWidth != mPreviousAdaptedWidth)
					{
						if (canChangeAutoResizeMode)
							ChangeAutoResize(false);
						mAdaptedSizeChangedLastFrame = true;
						mFramesBeforeCanAutoresize = 10;
					}
					else
					{
						if (mAdaptedSizeChangedLastFrame)
							mFramesBeforeCanAutoresize--;
						if (mFramesBeforeCanAutoresize < 0 && canChangeAutoResizeMode && mAdaptedSizeChangedLastFrame)
						{
							ChangeAutoResize(true);
							mAdaptedSizeChangedLastFrame = false;
						}
					}
				}

				mPreviousAdaptedWidth = adaptedWidth;


				if (adaptedWidth > width)
					width = adaptedWidth;
				if(IsColumnSizeDifferentFromSettings())
				{
					ColumnSizeChanged();
				}
			}
			Rect currentDrawPos = new Rect(new Vector2(-scrollX, mPadTop + decalTop), new Vector2(width, CTL_Styles.MAIN_TAB_MCH_HEIGHT));
			currentDrawPos.width = currentDrawPos.width;
			mHeader.OnGUI(currentDrawPos, 0f);

			mFirstDraw = false;

			mPreviousWidth = width;

			return currentDrawPos;
		}

		public void Init()
		{
			mColumnKeyword = new MultiColumnHeaderState.Column() { headerContent = CTL_Styles.sGUIC_Main_MCH_ColumnKeyword, headerTextAlignment = TextAlignment.Left, autoResize = false, minWidth = COLUMN_MIN_WIDTH };
			mColumnComment = new MultiColumnHeaderState.Column() { headerContent = CTL_Styles.sGUIC_Main_MCH_ColumnComment, headerTextAlignment = TextAlignment.Left, autoResize = true, minWidth = COLUMN_MIN_WIDTH };
			mColumnFileName = new MultiColumnHeaderState.Column() { headerContent = CTL_Styles.sGUIC_Main_MCH_ColumnFileName, headerTextAlignment = TextAlignment.Left, autoResize = false, minWidth = COLUMN_MIN_WIDTH };
			mColumnLineNumber = new MultiColumnHeaderState.Column() { headerContent = CTL_Styles.sGUIC_Main_MCH_ColumnLineNumber, headerTextAlignment = TextAlignment.Right, autoResize = false, minWidth = COLUMN_MIN_WIDTH };

			mColumns = new MultiColumnHeaderState.Column[] { mColumnKeyword, mColumnComment, mColumnFileName, mColumnLineNumber };
			mState = new MultiColumnHeaderState(mColumns);
			mHeader = new MultiColumnHeader(mState);
			mHeader.sortingChanged += SortingChanged;
			mHeader.visibleColumnsChanged += ColumnVisibilityChanged;

			mColumnKeyword.width = 58;
			mColumnFileName.width = 68;
			mColumnLineNumber.width = 38;
			if(!HasSavedHeaderSize())
				mHeader.ResizeToFit();

			SetSorting(CTL_Settings.ColumnSortIndex, CTL_Settings.ColumnSortAscending);
			SetColumnVisibility(CTL_Settings.ColumnVisibility);
			SetColumnSize(CTL_Settings.ColumnSize);

			mFirstDraw = true;
			mFramesBeforeCanModifyAutoresize = 10;
			sNeedInit = false;
		}

		bool HasSavedHeaderSize()
		{
			for(int i = 0; i< CTL_Settings.ColumnSize.Length; i++)
			{
				if (CTL_Settings.ColumnSize[i] >= 0)
					return true;
			}
			return false;
		}

		void SortingChanged(MultiColumnHeader header)
		{
			if (header.sortedColumnIndex < 0 || header.sortedColumnIndex > mColumns.Length)
				return;
			SetSorting(header.sortedColumnIndex, header.IsSortedAscending(header.sortedColumnIndex));
		}

		void ColumnVisibilityChanged(MultiColumnHeader header)
		{
			CTL_Settings.ColumnVisibility = ColumnVisibility();
		}

		void ColumnSizeChanged()
		{
			CTL_Settings.ColumnSize = ColumnSize();
		}

		public void RefreshSorting()
		{
			if (mHeader.sortedColumnIndex != CTL_Settings.ColumnSortIndex || 
				(CTL_Settings.ColumnSortIndex >= 0 && CTL_Settings.ColumnSortIndex < mColumns.Length && mHeader.IsSortedAscending(CTL_Settings.ColumnSortIndex) != CTL_Settings.ColumnSortAscending))
			{
				mHeader.SetSorting(CTL_Settings.ColumnSortIndex, CTL_Settings.ColumnSortAscending);
			}
			else
			{
				SetSorting(CTL_Settings.ColumnSortIndex, CTL_Settings.ColumnSortAscending); //CTL...force refresh
			}
		}

		public void SetColumnVisibility(bool[] columnVisibility)
		{
			List<int> visibleColumns = new List<int>();
			for (int i = 0; i < columnVisibility.Length; i++)
			{
				if (columnVisibility[i])
					visibleColumns.Add(i);
			}
			mHeader.state.visibleColumns = visibleColumns.ToArray();
		}

		public void SetColumnSize(float[] columnSizes)
		{
			for (int i = 0; i < columnSizes.Length; i++)
			{
				if (columnSizes[i] >= 0)
					mColumns[i].width = columnSizes[i];
			}
		}

		public bool[] ColumnVisibility()
		{
			bool[] toReturn = new bool[mColumns.Length];
			for(int i = 0; i<  mColumns.Length; i++)
			{
				toReturn[i] = mHeader.IsColumnVisible(i);
			}
			return toReturn;
		}

		public float[] ColumnSize()
		{
			float[] toReturn = new float[mColumns.Length];
			for (int i = 0; i < mColumns.Length; i++)
			{
				toReturn[i] = mColumns[i].width;
			}
			return toReturn;
		}

		bool IsColumnSizeDifferentFromSettings()
		{
			for (int i = 0; i < mColumns.Length; i++)
			{
				if(mColumns[i].width != CTL_Settings.ColumnSize[i])
				{
					return true;
				}
			}
			return false;
		}

		public void SetSorting(int index, bool ascending)
		{
			if (CTL_MainWindow.sIsRefreshing)
				return;
			if (index < 0 || index > mColumns.Length)
				return;

			switch ((eColumnType)index)
			{
				case eColumnType.KEYWORD:
					CTL_TodoObject.SortByKeyword(ascending);
					break;
				case eColumnType.COMMENT:
					CTL_TodoObject.SortByMessage(ascending);
					break;
				case eColumnType.FILE_NAME:
					CTL_TodoObject.SortByFileName(ascending);
					break;
				case eColumnType.LINE_NUMBER:
					CTL_TodoObject.SortByLineNumber(ascending);
					break;
			}

			CTL_Settings.SaveColumnSort(index,ascending);
		}


		public CTL_MCH_TodoObject()
		{
			Init();
		}
	}
}
