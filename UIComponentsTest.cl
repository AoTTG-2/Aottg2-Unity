class Main
{
    _toggleValue = false;
    _sliderValue = 0.0;
    _sliderIntValue = 0;
    _dropdownValue = "Option 1";
    _progressValue = 0.0;
    _progressBar = null;
    _testPanelRoot = null;
    _showTestPanel = false;
    _dropdown = null;
    _scrollSpeedValue = 18.0;  # Default scroll speed
    
    # Panel behavior toggles
    _isVerticalScrollable = true;
    
    # Scrolling state
    _scrollView = null;
    _contentContainer = null;

    function OnGameStart()
    {
        # Hide default UI elements for cleaner view
        UI.SetKDRPanelActive(false);
        UI.SetMinimapActive(false);
        UI.SetFeedPanelActive(false);

        root = UI.GetRootVisualElement();

        # Create the test panel as an absolute positioned popup
        self._testPanelRoot = UI.VisualElement()
            .Absolute(true)
            .Width(700)
            .Height(700)
            .Top(50, false)
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

        title = UI.Label("UI Components Test")
            .FontSize(32)
            .FontStyle("Bold")
            .Color(Color(255, 255, 255));

        closeButton = UI.Button("Close (F)", self.CloseTestPanel);
        closeButton.Width(100);

        titleRow.Add(title).Add(closeButton);
        self._testPanelRoot.Add(titleRow);

        # Create behavior toggles section with single row
        behaviorSection = UI.VisualElement()
            .FlexDirection("Row")
            .JustifyContent("SpaceAround")
            .MarginBottom(20)
            .Padding(10)
            .BackgroundColor(Color(30, 30, 30))
            .BorderRadius(5);

        scrollYToggle = UI.Toggle("Scroll Y", self.OnScrollYToggle);
        scrollYToggle.Value = self._isVerticalScrollable;
        scrollYToggle.QueryByClassName("unity-label").Color(Color(255, 255, 255));

        # Add scroll speed slider
        scrollSpeedSlider = UI.Slider(1.0, 50.0, 0.1, "Speed", self.OnScrollSpeedChanged);
        scrollSpeedSlider.Value = self._scrollSpeedValue;
        scrollSpeedSlider.ShowInputField = true;
        scrollSpeedSlider.Width(180);
        scrollSpeedSlider.QueryByClassName("unity-label").Color(Color(255, 255, 255));

        behaviorSection.Add(scrollYToggle).Add(scrollSpeedSlider);
        self._testPanelRoot.Add(behaviorSection);

        # Create ScrollView
        self._scrollView = UI.ScrollView();
        self._scrollView
            .Width(100, true)
            .Height(550);
        
        # Disable horizontal scrolling since we don't need it
        self._scrollView.HorizontalScrollEnabled = false;

        # Create content container inside ScrollView
        self._contentContainer = UI.VisualElement()
            .Width(100, true)
            .FlexDirection("Column");

        self._scrollView.Add(self._contentContainer);
        self._testPanelRoot.Add(self._scrollView);

        # Test each component
        self.CreateToggleTest(self._contentContainer);
        self.CreateDivider(self._contentContainer);
        
        self.CreateSliderTest(self._contentContainer);
        self.CreateDivider(self._contentContainer);
        
        self.CreateSliderIntTest(self._contentContainer);
        self.CreateDivider(self._contentContainer);
        
        self.CreateDropdownTest(self._contentContainer);
        self.CreateDivider(self._contentContainer);
        
        self.CreateProgressBarTest(self._contentContainer);

        root.Add(self._testPanelRoot);

        # Start with panel hidden
        self.SetTestPanelVisible(false);
        
        # Update scrollability
        self.UpdateScrollability();
    }

    function OnTick()
    {
        # Animate progress bar
        if (self._progressBar != null)
        {
            self._progressValue = self._progressValue + 0.5;
            if (self._progressValue > 100.0)
            {
                self._progressValue = 0.0;
            }
            self._progressBar.SetPercentage(self._progressValue);
        }
    }

    function OnFrame()
    {
        # Toggle test panel with F key (Interact key)
        if (Input.GetKeyDown("Interaction/Interact"))
        {
            self.SetTestPanelVisible(!self._showTestPanel);
        }
    }

    function SetTestPanelVisible(visible)
    {
        self._showTestPanel = visible;
        self._testPanelRoot.Active(visible);

        # Enable/disable mouse and camera controls
        Camera.SetCursorVisible(visible);
        Camera.SetCameraLocked(visible);
        
        # Disable all input category keys except Interaction when panel is visible
        # Using the dynamic category method instead of individual key disabling
        Input.SetCategoryKeysEnabled("General", !visible);
        Input.SetCategoryKeysEnabled("Human", !visible);
        Input.SetCategoryKeysEnabled("Titan", !visible);
        # Keep Interaction keys enabled so users can close the panel with F
        # Input.SetCategoryKeysEnabled("Interaction", true); # Optional - already enabled by default
    }

    function CloseTestPanel()
    {
        self.SetTestPanelVisible(false);
    }

    # Behavior toggle handlers
    function OnScrollYToggle(value)
    {
        self._isVerticalScrollable = value;
        self.UpdateScrollability();
    }

    function UpdateScrollability()
    {
        if (self._scrollView != null)
        {
            # Set vertical scrolling
            self._scrollView.VerticalScrollEnabled = self._isVerticalScrollable;
            # Set initial scroll speed
            self._scrollView.MouseWheelScrollSize = self._scrollSpeedValue;
        }
    }

    function CreateToggleTest(parent)
    {
        section = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(15);

        sectionTitle = UI.Label("Toggle Component")
            .FontSize(20)
            .FontStyle("Bold")
            .Color(Color(220, 220, 255))
            .MarginBottom(10);

        section.Add(sectionTitle);

        # Create toggle
        toggleContainer = UI.VisualElement()
            .FlexDirection("Row")
            .AlignItems("Center")
            .MarginBottom(5);

        toggle = UI.Toggle("Enable Feature", self.OnToggleChanged);
        toggle.Value = self._toggleValue;
        
        # Update toggle label color to be visible
        toggle.QueryByClassName("unity-label").Color(Color(255, 255, 255));

        toggleContainer.Add(toggle);
        
        section.Add(toggleContainer);

        parent.Add(section);
    }

    function CreateSliderTest(parent)
    {
        section = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(15);

        sectionTitle = UI.Label("Float Slider Component")
            .FontSize(20)
            .FontStyle("Bold")
            .Color(Color(220, 220, 255))
            .MarginBottom(10);

        section.Add(sectionTitle);

        # Create slider with tick interval
        sliderContainer = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(5);

        slider = UI.Slider(0.0, 100.0, 0.1, "Volume", self.OnSliderChanged);
        slider.Value = 50.0;
        slider.ShowInputField = true;
        slider.Width(100, true);
        
        # Update slider label color to be visible
        slider.QueryByClassName("unity-label").Color(Color(255, 255, 255));

        sliderContainer.Add(slider);
        section.Add(sliderContainer);

        parent.Add(section);
    }

    function CreateSliderIntTest(parent)
    {
        section = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(15);

        sectionTitle = UI.Label("Integer Slider Component")
            .FontSize(20)
            .FontStyle("Bold")
            .Color(Color(220, 220, 255))
            .MarginBottom(10);

        section.Add(sectionTitle);

        # Create integer slider with tick interval
        sliderContainer = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(5);

        sliderInt = UI.SliderInt(0, 20, 5, "Player Count", self.OnSliderIntChanged);
        sliderInt.Value = 10;
        sliderInt.ShowInputField = true;
        sliderInt.Width(100, true);
        
        # Update slider label color to be visible
        sliderInt.QueryByClassName("unity-label").Color(Color(255, 255, 255));

        sliderContainer.Add(sliderInt);
        section.Add(sliderContainer);

        parent.Add(section);
    }

    function CreateDropdownTest(parent)
    {
        section = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(15);

        sectionTitle = UI.Label("Dropdown Component")
            .FontSize(20)
            .FontStyle("Bold")
            .Color(Color(220, 220, 255))
            .MarginBottom(10);

        section.Add(sectionTitle);

        # Create dropdown
        dropdownContainer = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(5);

        options = List("Option 1", "Option 2", "Option 3", "Option 4");
        dropdown = UI.Dropdown(options, 0, "Select Difficulty", self.OnDropdownChanged);
        dropdown.Width(100, true);
        
        # Update dropdown label color to be visible
        dropdown.QueryByClassName("unity-label").Color(Color(255, 255, 255));

        self._dropdown = dropdown;

        dropdownContainer.Add(dropdown);
        section.Add(dropdownContainer);

        # Test dynamic option manipulation
        buttonContainer = UI.VisualElement()
            .FlexDirection("Row")
            .MarginTop(10);

        addButton = UI.Button("Add Option", self.OnAddOption);
        addButton.MarginRight(10);

        removeButton = UI.Button("Remove Last", self.OnRemoveOption);

        buttonContainer.Add(addButton).Add(removeButton);
        section.Add(buttonContainer);

        parent.Add(section);
    }

    function CreateProgressBarTest(parent)
    {
        section = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(15);

        sectionTitle = UI.Label("Progress Bar Component")
            .FontSize(20)
            .FontStyle("Bold")
            .Color(Color(220, 220, 255))
            .MarginBottom(10);

        section.Add(sectionTitle);

        # Create progress bar
        progressContainer = UI.VisualElement()
            .FlexDirection("Column")
            .MarginBottom(5);

        progressBar = UI.ProgressBar(0.0, 100.0, "Loading...");
        progressBar.Value = 0.0;
        progressBar.Width(100, true).Height(30);
        
        # Update progress bar label color to be visible
        progressBar.QueryByClassName("unity-label").Color(Color(255, 255, 255));

        self._progressBar = progressBar;

        progressContainer.Add(progressBar);
        section.Add(progressContainer);

        # Control buttons
        buttonContainer = UI.VisualElement()
            .FlexDirection("Row")
            .MarginTop(10);

        resetButton = UI.Button("Reset", self.OnResetProgress);
        resetButton.MarginRight(10);

        set50Button = UI.Button("Set 50%", self.OnSet50Progress);
        set50Button.MarginRight(10);

        set100Button = UI.Button("Set 100%", self.OnSet100Progress);

        buttonContainer.Add(resetButton).Add(set50Button).Add(set100Button);
        section.Add(buttonContainer);

        parent.Add(section);
    }

    function CreateDivider(parent)
    {
        divider = UI.VisualElement()
            .Height(2)
            .Width(100, true)
            .BackgroundColor(Color(80, 80, 80))
            .MarginTop(10)
            .MarginBottom(10);

        parent.Add(divider);
    }

    # Event Handlers

    function OnToggleChanged(value)
    {
        self._toggleValue = value;
    }

    function OnSliderChanged(value)
    {
        self._sliderValue = value;
    }

    function OnSliderIntChanged(value)
    {
        self._sliderIntValue = value;
    }

    function OnScrollSpeedChanged(value)
    {
        self._scrollSpeedValue = value;
        if (self._scrollView != null)
        {
            self._scrollView.MouseWheelScrollSize = value;
        }
    }

    function OnDropdownChanged(value)
    {
        self._dropdownValue = value;
    }

    function OnProgressChanged(value)
    {
        percentage = self._progressBar.GetPercentage();
    }

    function OnAddOption()
    {
        if (self._dropdown != null)
        {
            currentChoices = self._dropdown.Choices;
            newIndex = currentChoices.Count + 1;
            newOption = "Option " + newIndex;
            self._dropdown.AddChoice(newOption);
        }
    }

    function OnRemoveOption()
    {
        if (self._dropdown != null)
        {
            currentChoices = self._dropdown.Choices;
            if (currentChoices.Count > 1)
            {
                lastOption = currentChoices.Get(currentChoices.Count - 1);
                self._dropdown.RemoveChoice(lastOption);
            }
        }
    }

    function OnResetProgress()
    {
        self._progressValue = 0.0;
        self._progressBar.SetPercentage(0.0);
    }

    function OnSet50Progress()
    {
        self._progressValue = 50.0;
        self._progressBar.SetPercentage(50.0);
    }

    function OnSet100Progress()
    {
        self._progressValue = 100.0;
        self._progressBar.SetPercentage(100.0);
    }

    function OnCharacterSpawn(character)
    {
        if (Convert.GetType(character) == "Human" && character.IsMine)
        {
            UI.SetBottomHUDActive(false);
            Game.Print("Press F (Interact) to open UI Components Test Panel");
        }
    }
}
