using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace Settings
{
    class SetSettingsContainerPresetsOnly<T> : SetSettingsContainer<T> where T : BaseSetSetting, new()
    {
        protected override bool PresetsOnly => true;
    }
}
