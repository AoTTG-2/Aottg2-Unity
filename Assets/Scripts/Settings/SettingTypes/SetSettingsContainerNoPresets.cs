using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace Settings
{
    class SetSettingsContainerNoPresets<T> : SetSettingsContainer<T> where T : BaseSetSetting, new()
    {
        protected override bool AllowPresets => false;
    }
}
