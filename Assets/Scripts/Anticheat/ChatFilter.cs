using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Anticheat
{
    public static class ChatFilter
    {
        // Non-instantiable class for managing rich text tags
        protected class TextTag
        {
            // Record for organizing rich text tags pairs
            public record TextTagPair
            {
                // Array of all rich text tags pairs
                public static TextTagPair[] allTagPairs = { boldTag, colorTag, italicsTag, sizeTag };

                public TextTagPair(string tagName, string shortName = null, string defaultValue = null)
                {
                    OpenTag = new TextTag(tagName, TextTagType.Open, this, shortName, defaultValue);
                    CloseTag = new TextTag(tagName, TextTagType.Close, this, shortName, defaultValue);
                }

                public TextTag OpenTag { get; }
                public TextTag CloseTag { get; }
            }

            private enum TextTagType
            {
                Open,
                Close,
            }

            // Regex patterns
            private const string VALUE_PATTERN = "(=[^\\s]*?)?";
            private const string OPEN_PATTERN = "<{0}" + VALUE_PATTERN + ">";
            private const string OPEN_VALUE_LOOKAHEAD = "<{0}(?=" + VALUE_PATTERN + ">)";
            private const string OPEN_FORMAT = "<{0}>";
            private const string OPEN_VALUE_FORMAT = "<{0}={1}>";
            private const string CLOSE_FORMAT = "</{0}>";

            // Initialize text tag pairs
            public static TextTagPair boldTag = new TextTagPair("b");
            public static TextTagPair colorTag = new TextTagPair("color", shortName: "c", defaultValue: "white");
            public static TextTagPair italicsTag = new TextTagPair("i");
            public static TextTagPair sizeTag = new TextTagPair("size", shortName: "s", defaultValue: "18");

            // Array of all rich text tags
            public static TextTag[] allTags = { boldTag.OpenTag, boldTag.CloseTag, colorTag.OpenTag, colorTag.CloseTag, italicsTag.OpenTag, italicsTag.CloseTag, sizeTag.OpenTag, sizeTag.CloseTag };

            private TextTag(string tagName, TextTagType tagType, TextTagPair parentTagPair, string shortName = null, string defaultValue = null)
            {
                Name = tagName;
                ShortName = shortName;
                HasValue = !string.IsNullOrEmpty(defaultValue);
                ParentTagPair = parentTagPair;
                this.defaultValue = defaultValue;
                this.tagType = tagType;
            }

            private string defaultValue;
            private TextTagType tagType;

            public string Name { get; }
            public string ShortName { get; }
            public bool HasValue { get; }
            public TextTagPair ParentTagPair { get; }

            // Return a regex string to match this tag
            public string Pattern
            {
                get
                {
                    string pattern = isOpeningTag() ? OPEN_PATTERN : CLOSE_FORMAT;
                    return string.Format(pattern, Name);
                }
            }

            // Return a regex string to match this tag using the shortened name
            public string ShortPattern
            {
                get
                {
                    string pattern = isOpeningTag() ? OPEN_PATTERN : CLOSE_FORMAT;
                    return string.Format(pattern, ShortName);
                }
            }

            public string DefaultValue
            {
                get
                {
                    // Close tag
                    if (!isOpeningTag())
                    {
                        return string.Format(CLOSE_FORMAT, Name);
                    }

                    // Open tag
                    return HasValue ? string.Format(OPEN_VALUE_FORMAT, Name, defaultValue) : string.Format(OPEN_FORMAT, Name);

                }
            }

            // Return Regex pattern for all tags
            public static string getAllTagsPattern()
            {
                string[] patterns = allTags.Select(tag => string.Format("({0})", tag.Pattern)).ToArray();
                return string.Join("|", patterns);
            }

            // Return the TextTag corresponding to a given string
            public static TextTag getTag(string tagString)
            {
                return allTags.Where(tag => Regex.IsMatch(tagString, tag.Pattern, RegexOptions.IgnoreCase)).FirstOrDefault();
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

            // Get the complementary TextTag for the given tag
            public TextTag getMatchingTag()
            {
                return isOpeningTag() ? ParentTagPair.CloseTag : ParentTagPair.OpenTag;
            }

            public bool isMatchingTag(TextTag otherTag)
            {
                return getMatchingTag().Equals(otherTag);
            }

            // Replace occurances of the short tag in a text with the full tag
            public string expandTags(string text)
            {
                if (string.IsNullOrEmpty(ShortName))
                {
                    return text;
                }

                string openLookaheadPattern = string.Format(OPEN_VALUE_LOOKAHEAD, ShortName);
                string closePattern = string.Format(CLOSE_FORMAT, ShortName);
                string openReplace = "<" + Name;
                string closeReplace = string.Format(CLOSE_FORMAT, Name);

                text = Regex.Replace(text, openLookaheadPattern, openReplace, RegexOptions.IgnoreCase);
                text = Regex.Replace(text, closePattern, closeReplace, RegexOptions.IgnoreCase);
                return text;
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
            text = text.expandAllTags();
            text = text.FilterSizeTag();
            text = text.BalanceTags();
            return text;
        }

        public static string FilterSizeTag(this string text)
        {
            MatchCollection matches = Regex.Matches(text, TextTag.sizeTag.OpenTag.Pattern, RegexOptions.IgnoreCase);
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
                        text = text.Substring(0, pair.Key) + TextTag.sizeTag.OpenTag.DefaultValue + text.Substring(pair.Key, text.Length - pair.Key);
                    }
                }
            }
            return text;
        }


        // Replace all occurances of shortened tags in a text the full tags
        public static string expandAllTags(this string text)
        {
            TextTag.allTags.ToList().ForEach(tag => text = tag.expandTags(text));
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
