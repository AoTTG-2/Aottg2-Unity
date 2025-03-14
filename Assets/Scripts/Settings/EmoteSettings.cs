using System.Collections.Generic;

namespace Settings
{
    class EmoteSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "Emote.json"; } }
        public ListSetting<StringSetting> TextEmotes = new ListSetting<StringSetting>(new List<StringSetting>()
        {
            new StringSetting("Help", maxLength: 40),
            new StringSetting("Thanks", maxLength: 40),
            new StringSetting("Sorry", maxLength: 40),
            new StringSetting("Titan here", maxLength: 40),
            new StringSetting("Good game", maxLength: 40),
            new StringSetting("Nice hit", maxLength: 40),
            new StringSetting("Oops", maxLength: 40),
            new StringSetting("Welcome", maxLength: 40)
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
