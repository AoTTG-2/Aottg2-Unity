using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Anticheat
{
    static class ChatFilter
    {
        // Non-instantiable class for managing rich text tags
        protected class TextTag
        {
            // Non-instantiable record for managing rich text tags pairs
            public record TextTagPair
            {
                // Initialize text tag pairs
                public static TextTagPair boldTag = new TextTagPair(boldOpenTag, boldCloseTag);
                public static TextTagPair colorTag = new TextTagPair(colorOpenTag, colorCloseTag);
                public static TextTagPair italicsTag = new TextTagPair(italicsOpenTag, italicsCloseTag);
                public static TextTagPair sizeTag = new TextTagPair(sizeOpenTag, sizeCloseTag);

                // Array of all rich text tags pairs
                public static TextTagPair[] allTagPairs = { boldTag, colorTag, italicsTag, sizeTag };

                private TextTagPair(TextTag openTag, TextTag closeTag)
                {
                    OpenTag = openTag;
                    CloseTag = closeTag;
                }

                public TextTag OpenTag { get; }
                public TextTag CloseTag { get; }
            }

            private enum TextTagType
            {
                Open,
                Close,
            }

            // Initialize rich text tags
            public static TextTag boldOpenTag = new TextTag("<b>", "<b>", TextTagType.Open);
            public static TextTag boldCloseTag = new TextTag("</b>", "</b>", TextTagType.Close);
            public static TextTag colorOpenTag = new TextTag("<color(=[^\\s]*?)?>", "<color=white>", TextTagType.Open);
            public static TextTag colorCloseTag = new TextTag("</color>", "</color>", TextTagType.Close);
            public static TextTag italicsOpenTag = new TextTag("<i>", "<i>", TextTagType.Open);
            public static TextTag italicsCloseTag = new TextTag("</i>", "</i>", TextTagType.Close);
            public static TextTag sizeOpenTag = new TextTag("<size(=[^\\s]*?)?>", "<size=18>", TextTagType.Open);
            public static TextTag sizeCloseTag = new TextTag("</size>", "</size>", TextTagType.Close);

            // Array of all rich text tags
            public static TextTag[] allTags = { boldOpenTag, boldCloseTag, colorOpenTag, colorCloseTag, italicsOpenTag, italicsCloseTag, sizeOpenTag, sizeCloseTag };

            private TextTag(string tagPattern, string tagDefault, TextTagType tagType)
            {
                Pattern = tagPattern;
                DefaultValue = tagDefault;
                this.tagType = tagType;
            }

            public string Pattern { get; }
            public string DefaultValue { get; }
            private TextTagType tagType;

            // Return Regex pattern for all tags
            public static string getAllTagsPattern()
            {
                string[] patterns = allTags.Select(tag => "(" + tag.Pattern + ")").ToArray();
                return string.Join("|", patterns);
            }

            // Return the TextTag corresponding to a given string
            public static TextTag getTag(string tagString)
            {
                //return allTags.Where(tag => Regex.IsMatch(tagString, tag.Pattern, RegexOptions.IgnoreCase)).FirstOrDefault(null);

                foreach (TextTag tag in allTags)
                {
                    if (Regex.IsMatch(tagString, tag.Pattern, RegexOptions.IgnoreCase))
                    {
                        return tag;
                    }
                }

                return null;
            }

            // Check if a given tag is an opening tag
            public bool isOpeningTag()
            {
                return tagType.Equals(TextTagType.Open);
            }

            // Check if a given tag is a closing tag
            public bool isClosingTag()
            {
                return tagType.Equals(TextTagType.Close);
            }

            // Get the TextTagPair corresponding to the given tag
            public TextTagPair getTagPair()
            {
                //return TextTagPair.allTagPairs.Where(tagPair => tagPair.OpenTag.Equals(this) || tagPair.CloseTag.Equals(this)).FirstOrDefault(null);

                foreach (TextTagPair tagPair in TextTagPair.allTagPairs)
                {
                    if (tagPair.OpenTag.Equals(this) || tagPair.CloseTag.Equals(this))
                    {
                        return tagPair;
                    }
                }

                return null;
            }

            // Get the complementary TextTag for the given tag
            public TextTag getMatchingTag()
            {
                TextTagPair textTagPair = getTagPair();

                if (isOpeningTag())
                {
                    return textTagPair.CloseTag;
                }
                else
                {
                    return textTagPair.OpenTag;
                }
            }

            public bool isMatchingTag(TextTag otherTag)
            {
                return getMatchingTag().Equals(otherTag);
            }
        }

        private class TagOperation
        {
            public enum StringOperation
            {
                Add,
                Remove,
            }

            public TagOperation(int index, Match match, TextTag tag, StringOperation operation)
            {
                Index = index;
                Match = match;
                Tag = tag;
                Operation = operation;
            }

            public int Index { get; }
            public Match Match { get; }
            public TextTag Tag { get; }
            public StringOperation Operation { get; }

            public string applyOperation(string text)
            {
                if (Operation.Equals(StringOperation.Add))
                {
                    return text.Insert(Index, Tag.DefaultValue);
                }
                else
                {
                    return text.Remove(Index, Match.Value.Length);
                }
            }
        }

        public static string FilterText(this string text)
        {
            text = text.FilterSizeTag();
            text = text.BalanceTags();
            return text;
        }

        public static string FilterSizeTag(this string text)
        {
            MatchCollection matches = Regex.Matches(text, TextTag.sizeOpenTag.Pattern, RegexOptions.IgnoreCase);
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            foreach (Match match in matches)
            {
                if (list.Any(p => p.Key == match.Index))
                {
                    continue;
                }
                list.Add(new KeyValuePair<int, string>(match.Index, match.Value));
            }
            foreach (KeyValuePair<int, string> pair in list)
            {
                if (pair.Value.StartsWith("<size="))
                {
                    if (pair.Value.Length > 9)
                    {
                        text = text.Remove(pair.Key, pair.Value.Length);
                        text = text.Substring(0, pair.Key) + TextTag.sizeOpenTag.DefaultValue + text.Substring(pair.Key, text.Length - pair.Key);
                    }
                }
            }
            return text;
        }

        // Ensure that all tags are balanced by adding new tags to the beginning or end of the text
        public static string BalanceTags(this string text)
        {
            Stack<TextTag> openTagStack = new Stack<TextTag>();
            Stack<TagOperation> operationStack = new Stack<TagOperation>();
            Match tagMatch = Regex.Match(text, TextTag.getAllTagsPattern(), RegexOptions.IgnoreCase);

            // Parse all the tags in the text
            while (tagMatch.Success)
            {
                processTag(ref openTagStack, ref operationStack, tagMatch);
                tagMatch = tagMatch.NextMatch();
            }

            // And closing tags for any remaining tags on the stack
            while (openTagStack.Count > 0)
            {
                TextTag topTag = openTagStack.Pop();
                text = string.Concat(text, topTag.getMatchingTag().DefaultValue);
            }

            // Insert closing tags from the queue
            while (operationStack.Count > 0)
            {
                TagOperation insertOperation = operationStack.Pop();
                text = insertOperation.applyOperation(text);
            }

            return text;
        }

        private static void processTag(ref Stack<TextTag> openTagStack, ref Stack<TagOperation> operationStack, Match tagMatch)
        {
            string nextTagString = tagMatch.Value;
            int nextTagIndex = tagMatch.Index;
            TextTag nextTag = TextTag.getTag(nextTagString);

            // Ignore the match if it cannot be parsed
            if (nextTag == null)
            {
                return;
            }

            // If the next tag is an opening tag, add it to the stack
            if (nextTag.isOpeningTag())
            {
                openTagStack.Push(nextTag);
                return;
            }

            // Handle a closing tag
            while (true)
            {
                TextTag topTag;
                openTagStack.TryPeek(out topTag);

                // If there is no opening tag on the stack, push the closing tag to be removed
                if (topTag == null)
                {
                    operationStack.Push(new TagOperation(nextTagIndex, tagMatch, nextTag, TagOperation.StringOperation.Remove));
                    return;
                }

                // If the closing tag matches the top of the opening tag stack, pop the top tag
                if (nextTag.isMatchingTag(topTag))
                {
                    openTagStack.Pop();
                    return;
                }

                // If the closing tag doesn't match the top opening tag on stack, pop the top tag and keep looping until a match is found or the stack is empty
                topTag = openTagStack.Pop();
                operationStack.Push(new TagOperation(nextTagIndex, tagMatch, topTag.getMatchingTag(), TagOperation.StringOperation.Add));
            }
        }
    }
}
