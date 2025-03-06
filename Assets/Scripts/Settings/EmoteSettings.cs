using System.Collections.Generic;

namespace Settings
{
    class EmoteSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "Emote.json"; } }
        public ListSetting<NameSetting> TextEmotes = new ListSetting<NameSetting>(new List<NameSetting>()
        {
            new NameSetting("Help", maxLength: 200, maxStrippedLength: 40),
            new NameSetting("Thanks", maxLength: 200, maxStrippedLength: 40),
            new NameSetting("Sorry", maxLength: 200, maxStrippedLength: 40),
            new NameSetting("Titan here", maxLength: 200, maxStrippedLength: 40),
            new NameSetting("Good game", maxLength: 200, maxStrippedLength: 40),
            new NameSetting("Nice hit", maxLength: 200, maxStrippedLength: 40),
            new NameSetting("Oops", maxLength: 200, maxStrippedLength: 40),
            new NameSetting("Welcome", maxLength: 200, maxStrippedLength: 40)
        });
        public ListSetting<StringSetting> EmojiEmotes = new ListSetting<StringSetting>(new List<StringSetting>()
        {
            new StringSetting("EmojiSmile", maxLength: 40),
            new StringSetting("EmojiThumbsUp", maxLength: 40),
            new StringSetting("EmojiCool", maxLength: 40),
            new StringSetting("EmojiLove", maxLength: 40),
            new StringSetting("EmojiShocked", maxLength: 40),
            new StringSetting("EmojiCrying", maxLength: 40),
            new StringSetting("EmojiAnnoyed", maxLength: 40),
            new StringSetting("EmojiAngry", maxLength: 40)
        });

        protected override bool Validate()
        {
            return TextEmotes.Value.Count == 8 && EmojiEmotes.Value.Count == 8;
        }
    }
}
