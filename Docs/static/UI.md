# UI
Inherits from object
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|TopCenter|[String](../static/String.md)|False|"TopCenter" constant|
|TopLeft|[String](../static/String.md)|False|"TopLeft" constant|
|TopRight|[String](../static/String.md)|False|"TopRight" constant|
|MiddleCenter|[String](../static/String.md)|False|"MiddleCenter" constant|
|MiddleLeft|[String](../static/String.md)|False|"MiddleLeft" constant|
|MiddleRight|[String](../static/String.md)|False|"MiddleRight" constant|
|BottomCenter|[String](../static/String.md)|False|"BottomCenter" constant|
|BottomLeft|[String](../static/String.md)|False|"BottomLeft" constant|
|BottomRight|[String](../static/String.md)|False|"BottomRight" constant|
|Popups|[List](../objects/List.md)|False|Returns a list of all popups|
## Static Methods
##### void SetLabel([String](../static/String.md) label, [String](../static/String.md) message)
- **Description:** Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".
##### void SetLabelForTime([String](../static/String.md) label, [String](../static/String.md) message, float time)
- **Description:** Sets the label for a certain time, after which it will be cleared.
##### void SetLabelAll([String](../static/String.md) label, [String](../static/String.md) message)
- **Description:** Sets the label for all players. Master client only. Be careful not to call this often.
##### void SetLabelForTimeAll([String](../static/String.md) label, [String](../static/String.md) message, float time)
- **Description:** Sets the label for all players for a certain time. Master client only.
##### [String](../static/String.md) CreatePopup([String](../static/String.md) popupName, [String](../static/String.md) title, int width, int height)
- **Description:** Creates a new popup. This popup is hidden until shown.
##### void ShowPopup([String](../static/String.md) popupName)
- **Description:** Shows the popup with given name.
##### void HidePopup([String](../static/String.md) popupName)
- **Description:** Hides the popup with given name.
##### void ClearPopup([String](../static/String.md) popupName)
- **Description:** Clears all elements in popup with given name.
##### void AddPopupLabel([String](../static/String.md) popupName, [String](../static/String.md) label)
- **Description:** Adds a text row to the popup with label as content.
##### void AddPopupButton([String](../static/String.md) popupName, [String](../static/String.md) label, [String](../static/String.md) callback)
- **Description:** Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.
##### void AddPopupBottomButton([String](../static/String.md) popupName, [String](../static/String.md) label, [String](../static/String.md) callback)
- **Description:** Adds a button to the bottom bar of the popup.
##### void AddPopupButtons([String](../static/String.md) popupName, [List](../objects/List.md) labels, [List](../objects/List.md) callbacks)
- **Description:** Adds a list of buttons in a row to the popup.
##### [String](../static/String.md) WrapStyleTag([String](../static/String.md) text, [String](../static/String.md) style, [String](../static/String.md) arg = null)
- **Description:** Returns a wrapped string given style and args.
##### [String](../static/String.md) GetLocale([String](../static/String.md) cat, [String](../static/String.md) sub, [String](../static/String.md) key)
- **Description:** Gets translated locale from the current Language.json file with given category, subcategory, and key pattern.
##### [String](../static/String.md) GetLanguage()
- **Description:** Returns the current language (e.g. "English" or "简体中文").
##### void ShowChangeCharacterMenu()
- **Description:** Shows the change character menu if main character is Human.
##### void SetScoreboardHeader([String](../static/String.md) header)
- **Description:** Sets the display of the scoreboard header (default "Kills / Deaths...")
##### void SetScoreboardProperty([String](../static/String.md) property)
- **Description:** Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default "Kills / Deaths..." display.
##### [Color](../objects/Color.md) GetThemeColor([String](../static/String.md) panel, [String](../static/String.md) category, [String](../static/String.md) item)
- **Description:** Gets the color of the specified item. See theme json for reference.
##### bool IsPopupActive([String](../static/String.md) popupName)
- **Description:** Returns if the given popup is active

---

