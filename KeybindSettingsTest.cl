class Main
{
    _settingsPanelRoot = null;
    _showSettingsPanel = false;
    _contentContainer = null;
    _keybindList = List();  # List of keybind names
    _keybindSections = Dict();  # Dict mapping keybind name to UI section
    _currentlyBinding = false;
    _bindingKeybindName = "";
    _bindingType = "";  # "Main" or "Alt"
    _currentKeybindName = "";
    _nameInput = null;
    
    function OnGameStart()
    {
        # Hide default UI elements for cleaner view
        UI.SetKDRPanelActive(false);
        UI.SetMinimapActive(false);
        UI.SetFeedPanelActive(false);

        root = UI.GetRootVisualElement();

        # Create the settings panel
        self._settingsPanelRoot = UI.VisualElement()
            .Absolute(true)
            .Width(800)
            .Height(600)
            .Top(100, false)
            .Left(50, true)
            .Right(50, true)
            .BorderRadius(10)
            .Padding(20)
            .BackgroundColor(Color(40, 40, 40, 230));

        # Create title and close button row
        titleRow = UI.VisualElement()
            .FlexDirection("Row")
            .JustifyContent("SpaceBetween")
            .AlignItems("Center")
            .MarginBottom(15);

        title = UI.Label("Keybind Settings Demo")
            .FontSize(32)
            .FontStyle("Bold")
            .Color(Color(255, 255, 255));

        closeButton = UI.Button("Close (Interact3)", self.CloseSettingsPanel);
        closeButton.Width(150);

        titleRow.Add(title).Add(closeButton);
        self._settingsPanelRoot.Add(titleRow);

        # Add new keybind section
        addSection = UI.VisualElement()
            .FlexDirection("Row")
            .AlignItems("Center")
            .MarginBottom(20)
            .Padding(10)
            .BackgroundColor(Color(30, 30, 30))
            .BorderRadius(5);

        addLabel = UI.Label("Add New Keybind:")
            .FontSize(18)
            .Color(Color(255, 255, 255))
            .MarginRight(10);

        nameInput = UI.TextField("Keybind Name");
        nameInput.RegisterValueChangedEventCallback(self.OnKeybindNameInput);
        nameInput.Width(200);
        nameInput.QueryByClassName("unity-label").Color(Color(255, 255, 255));
        self._nameInput = nameInput;

        addButton = UI.Button("Add Keybind", self.OnAddKeybind);
        addButton.Width(120);
        addButton.MarginLeft(10);

        addSection.Add(addLabel).Add(nameInput).Add(addButton);
        self._settingsPanelRoot.Add(addSection);

        # Create instructions label
        instructionsLabel = UI.Label("Click on a keybind name to select it, then press M (Main) or A (Alt) keys to bind.")
            .FontSize(14)
            .Color(Color(200, 200, 200))
            .MarginBottom(15);
        self._settingsPanelRoot.Add(instructionsLabel);

        # Create ScrollView for keybinds
        scrollView = UI.ScrollView();
        scrollView
            .Width(100, true)
            .Height(380);
        scrollView.HorizontalScrollEnabled = false;

        # Create content container inside ScrollView
        self._contentContainer = UI.VisualElement()
            .Width(100, true)
            .FlexDirection("Column");

        scrollView.Add(self._contentContainer);
        self._settingsPanelRoot.Add(scrollView);

        root.Add(self._settingsPanelRoot);

        # Start with panel hidden
        self.SetSettingsPanelVisible(false);
    }

    function OnFrame()
    {
        # Toggle settings panel with Interact3 key
        if (Input.GetKeyDown("Interaction/Interact3"))
        {
            self.SetSettingsPanelVisible(!self._showSettingsPanel);
        }

        # Only process keybind panel keys when panel is visible
        if (!self._showSettingsPanel)
        {
            return;
        }

        # Check if we're waiting for a key binding
        if (self._currentlyBinding)
        {
            self.CheckForKeyPress();
            return;
        }

        # M key to bind Main on last added keybind
        if (Input.GetKeyDown("CustomKey/M") && self._keybindList.Count > 0)
        {
            lastKeybind = self._keybindList.Get(self._keybindList.Count - 1);
            self._currentlyBinding = true;
            self._bindingKeybindName = lastKeybind;
            self._bindingType = "Main";
            Game.Print("Press any key to bind Main for '" + lastKeybind + "'");
            return;
        }

        # A key to bind Alt on last added keybind
        if (Input.GetKeyDown("CustomKey/A") && self._keybindList.Count > 0)
        {
            lastKeybind = self._keybindList.Get(self._keybindList.Count - 1);
            self._currentlyBinding = true;
            self._bindingKeybindName = lastKeybind;
            self._bindingType = "Alt";
            Game.Print("Press any key to bind Alt for '" + lastKeybind + "'");
            return;
        }

        # R key to remove last added keybind
        if (Input.GetKeyDown("CustomKey/R") && self._keybindList.Count > 0)
        {
            lastKeybind = self._keybindList.Get(self._keybindList.Count - 1);
            self.OnRemoveKeybind(lastKeybind);
            return;
        }

        # Check for keybind presses
        for (keybindName in self._keybindList)
        {
            if (self._keybindSections.Contains(keybindName))
            {
                bindData = self._keybindSections.Get(keybindName);
                mainKey = bindData.Get("mainKey");
                altKey = bindData.Get("altKey");
                
                if (mainKey != "None" && Input.GetKeyDown("CustomKey/" + mainKey))
                {
                    Game.Print(keybindName + " (Main) was pressed!");
                }
                
                if (altKey != "None" && Input.GetKeyDown("CustomKey/" + altKey))
                {
                    Game.Print(keybindName + " (Alt) was pressed!");
                }
            }
        }
    }

    function OnRemoveKeybind(keybindName)
    {
        # Remove from list
        index = -1;
        for (i in Range(self._keybindList.Count))
        {
            if (self._keybindList.Get(i) == keybindName)
            {
                index = i;
                break;
            }
        }
        
        if (index >= 0)
        {
            self._keybindList.RemoveAt(index);
        }

        # Remove UI element
        if (self._keybindSections.Contains(keybindName))
        {
            bindData = self._keybindSections.Get(keybindName);
            row = bindData.Get("row");
            row.Remove();
            self._keybindSections.Remove(keybindName);
        }
        
        Game.Print("Removed keybind: " + keybindName);
    }

    function CheckForKeyPress()
    {
        # Check for common keys
        keys = List("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", 
                    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                    "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9",
                    "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12",
                    "Space", "LeftShift", "RightShift", "LeftControl", "RightControl", "LeftAlt", "RightAlt",
                    "Tab", "CapsLock", "Escape", "Return", "Backspace",
                    "UpArrow", "DownArrow", "LeftArrow", "RightArrow",
                    "Mouse0", "Mouse1", "Mouse2", "Mouse3", "Mouse4");
        
        for (keyCode in keys)
        {
            if (Input.GetKeyDown("CustomKey/" + keyCode))
            {
                self.BindKey(keyCode);
                return;
            }
        }
    }

    function BindKey(keyCode)
    {
        bindData = self._keybindSections.Get(self._bindingKeybindName);
        
        if (self._bindingType == "Main")
        {
            bindData.Set("mainKey", keyCode);
            bindData.Get("mainLabel").Text = keyCode;
        }
        else
        {
            bindData.Set("altKey", keyCode);
            bindData.Get("altLabel").Text = keyCode;
        }
        
        Game.Print("Bound " + keyCode + " to " + self._bindingKeybindName + " (" + self._bindingType + ")");
        
        self._currentlyBinding = false;
        self._bindingKeybindName = "";
        self._bindingType = "";
    }

    function SetSettingsPanelVisible(visible)
    {
        self._showSettingsPanel = visible;
        self._settingsPanelRoot.Active(visible);

        # Enable/disable mouse and camera controls
        Camera.SetCursorVisible(visible);
        Camera.SetCameraLocked(visible);
        
        # Disable all input categories when panel is visible
        Input.SetCategoryKeysEnabled("General", !visible);
        Input.SetCategoryKeysEnabled("Human", !visible);
        Input.SetCategoryKeysEnabled("Titan", !visible);
        # Keep Interaction keys enabled so users can close the panel
    }

    function CloseSettingsPanel()
    {
        self.SetSettingsPanelVisible(false);
    }

    function OnKeybindNameInput(newValue, previousValue)
    {
        self._currentKeybindName = newValue;
    }

    function OnAddKeybind()
    {
        if (self._currentKeybindName == "" || self._currentKeybindName == null)
        {
            Game.Print("Please enter a keybind name!");
            return;
        }

        # Check if keybind already exists
        if (self._keybindSections.Contains(self._currentKeybindName))
        {
            Game.Print("Keybind '" + self._currentKeybindName + "' already exists!");
            return;
        }

        # Add to list
        self._keybindList.Add(self._currentKeybindName);
        
        # Create keybind section
        self.CreateKeybindSection(self._currentKeybindName);
        
        Game.Print("Added keybind: " + self._currentKeybindName);
        Game.Print("Press M key to bind Main, A key to bind Alt, R key to remove");
        
        # Clear input
        self._currentKeybindName = "";
        if (self._nameInput != null)
        {
            self._nameInput.Value = "";
        }
    }

    function CreateKeybindSection(keybindName)
    {
        # Create keybind row
        keybindRow = UI.VisualElement()
            .FlexDirection("Row")
            .AlignItems("Center")
            .MarginBottom(10)
            .Padding(10)
            .BackgroundColor(Color(35, 35, 35))
            .BorderRadius(5);

        # Keybind label (clickable to select)
        nameLabel = UI.Label(keybindName)
            .FontSize(18)
            .Color(Color(220, 220, 255))
            .Width(200);

        # Main label showing bound key
        mainLabel = UI.Label("Main: None")
            .FontSize(16)
            .Color(Color(180, 180, 180))
            .Width(150);

        # Alt label showing bound key
        altLabel = UI.Label("Alt: None")
            .FontSize(16)
            .Color(Color(180, 180, 180))
            .Width(150);

        keybindRow.Add(nameLabel).Add(mainLabel).Add(altLabel);
        self._contentContainer.Add(keybindRow);

        # Store reference
        bindData = Dict();
        bindData.Set("row", keybindRow);
        bindData.Set("nameLabel", nameLabel);
        bindData.Set("mainLabel", mainLabel);
        bindData.Set("altLabel", altLabel);
        bindData.Set("mainKey", "None");
        bindData.Set("altKey", "None");
        bindData.Set("name", keybindName);
        
        self._keybindSections.Set(keybindName, bindData);
    }

    function OnCharacterSpawn(character)
    {
        if (Convert.GetType(character) == "Human" && character.IsMine)
        {
            UI.SetBottomHUDActive(false);
            Game.Print("Press Interact3 to open Keybind Settings Panel");
            Game.Print("Add a keybind, then use M (Main), A (Alt), R (Remove) keys to manage it");
        }
    }
}
