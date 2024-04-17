using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodeTodoList
{
	public class CTL_KeywordList
	{
		public const char SEPARATOR = '£'; //£ can't be used in the key as it act as separator

		bool mValidating = false;
		public List<CTL_KeywordObject> mKeywords = new List<CTL_KeywordObject>();

		public void RefreshAllTextures()
		{
			foreach(CTL_KeywordObject obj in mKeywords)
			{
				obj.RefreshTexture();
			}
		}

		public int GetKeywordId(CTL_KeywordObject obj)
		{
			return mKeywords.IndexOf(obj);
		}

		public List<CTL_KeywordObject> GetValidKeywordList()
		{
			List<CTL_KeywordObject> validKeywords = new List<CTL_KeywordObject>();
			foreach (CTL_KeywordObject keyword in mKeywords)
			{
				if (keyword.Valid)
					validKeywords.Add(keyword);
			}
			return validKeywords;
		}

		public void ToggleAllVisibility(bool visible)
		{
			foreach(CTL_KeywordObject obj in mKeywords)
			{
				obj.Visible = visible;
			}
		}

		public void ClearAllDraggedObjects()
		{
			for (int i = 0; i < mKeywords.Count; i++)
			{
				mKeywords[i].mIsDragged = false;
			}
		}

		public CTL_KeywordObject GetDraggedObject()
		{
			for (int i = 0; i < mKeywords.Count; i++)
			{
				if (mKeywords[i].mIsDragged)
					return mKeywords[i];
			}
			return null;
		}

		public void MoveElementToPosInList(CTL_KeywordObject obj, int pos)
		{
			if(mKeywords.Contains(obj))
				mKeywords.Remove(obj);
			mKeywords.Insert(pos, obj);

			CTL_Settings.sShouldSave = true;
			CTL_MainWindow.sNeedRefreshAuto = true;
		}

		public void AddKeyword(string keyword = "", bool needFocus = false, bool generateRandomColor = false)
		{
			CTL_KeywordObject newObj = new CTL_KeywordObject();
			newObj.Key = keyword;
			newObj.mNeedFocus = needFocus;

			if (generateRandomColor)
			{
				newObj.Color = CTL_Tools.GenerateRandomKeywordColor();
			}

			mKeywords.Add(newObj);
			KeywordModified(newObj, true);

			CTL_Settings.sShouldSave = true;
			CTL_MainWindow.sNeedRefreshAuto = true;
		}

		public void RemoveKeyword(CTL_KeywordObject obj)
		{
			if (mKeywords.Contains(obj))
				mKeywords.Remove(obj);
			ValidateKeywords();

			CTL_Settings.sShouldSave = true;
			CTL_MainWindow.sNeedRefreshAuto = true;
		}

		public void LoadFromString(string saveStr)
		{
			string[] splitted = saveStr.Split(SEPARATOR);
			for(int i = 0; i < splitted.Length; i++)
			{
				if(!string.IsNullOrEmpty(splitted[0]))
					mKeywords.Add(CTL_KeywordObject.LoadFromString(splitted[i]));
			}
			
			if(mKeywords.Count == 0)
			{
				CTL_KeywordObject baseKeyword = new CTL_KeywordObject();
				mKeywords.Add(baseKeyword);
				baseKeyword.Key = "TODO";
				baseKeyword.Color = new Color(.75f,1f,.75f);//CTL_Tools.GenerateRandomKeywordColor();

				baseKeyword = new CTL_KeywordObject();
				mKeywords.Add(baseKeyword);
				baseKeyword.Key = "VISUAL";
				baseKeyword.Color = new Color(1f, .6f, 1f);//CTL_Tools.GenerateRandomKeywordColor();

				baseKeyword = new CTL_KeywordObject();
				mKeywords.Add(baseKeyword);
				baseKeyword.Key = "OPTIM";
				baseKeyword.Color = new Color(.75f, .75f, 1f);//CTL_Tools.GenerateRandomKeywordColor();

				baseKeyword = new CTL_KeywordObject();
				mKeywords.Add(baseKeyword);
				baseKeyword.Key = "FIX";
				baseKeyword.Color = new Color(1f, .5f, .5f);//CTL_Tools.GenerateRandomKeywordColor();

				baseKeyword = new CTL_KeywordObject();
				mKeywords.Add(baseKeyword);
				baseKeyword.Key = "AUDIO";
				baseKeyword.Color = new Color(1f, 1f, .5f);//CTL_Tools.GenerateRandomKeywordColor();

				CTL_Settings.sShouldSave = true;
			}

		}

		public string Export()
		{
			string saveStr = "";
			for (int i = 0; i < mKeywords.Count; i++)
			{
				saveStr += mKeywords[i].Export();
				if (i < mKeywords.Count - 1)
					saveStr += SEPARATOR;
			}
			return saveStr;
		}

		public bool KeywordModified(CTL_KeywordObject modifiedObj, bool modifNeedRefreshList)
		{
			CTL_Settings.sShouldSave = true;

			if(modifNeedRefreshList)
				CTL_MainWindow.sNeedRefreshAuto = true;

			/*
			for (int i = 0; i < mKeywords.Count; i++)
			{
				if (mKeywords[i].SimilarKey(modifiedObj))
				{
					modifiedObj.Valid = false;
					break;
				}
			}*/
			ValidateKeywords();
			return modifiedObj.Valid;
		}

		public void ValidateKeywords()
		{
			if (mValidating)
				return;
			mValidating = true;
			bool valid;
			for(int i = 0; i < mKeywords.Count; i++)
			{
				valid = true;
				if (string.IsNullOrEmpty(mKeywords[i].Key))
				{
					valid = false;
				}
				else
				{
					for (int c = 0; c < mKeywords.Count; c++)
					{
						if (i == c)
							continue;
						if (mKeywords[i].SimilarKey(mKeywords[c]))
						{
							valid = false;
						}
					}
				}
				mKeywords[i].Valid = valid;
			}
			mValidating = false;
		}
	}
}
