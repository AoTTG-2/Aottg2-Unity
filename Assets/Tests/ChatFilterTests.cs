using NUnit.Framework;
using Anticheat;

public class TestExample
{
    /**
     * Standard tag usage. Behavior is based on how the Unity rich text parser behaves.
     * Any opening tag can have any value. The value gets ignored for bold and italics tags.
     */
    [Test]
    [TestCase("<b=123>Test</b>", "<b=123>Test</b>")]
    [TestCase("<b>Test</b>", "<b>Test</b>")]
    [TestCase("<color=#123456>Test</color>", "<color=#123456>Test</color>")]
    [TestCase("<color=#123>Test</color>", "<color=#123>Test</color>")]
    [TestCase("<color>Test</color>", "<color>Test</color>")]
    [TestCase("<i=123>Test</i>", "<i=123>Test</i>")]
    [TestCase("<i>Test</i>", "<i>Test</i>")]
    [TestCase("<size=18>Test</size>", "<size=18>Test</size>")]
    [TestCase("<size>Test</size>", "<size>Test</size>")]
    public void StandardTagTests(string expected, string text)
    {
        text = text.FilterText();
        Assert.AreEqual(expected, text);
    }

    /**
     * Size tags that are too large are filtered out and replaced with the default size.
     */
    [Test]
    [TestCase("<size=30>Test</size>", "<size=30>Test</size>")]
    [TestCase("<size=99>Test</size>", "<size=99>Test</size>")]
    [TestCase("<size=18>Test</size>", "<size=100>Test</size>")]
    public void LargeSizeTagTests(string expected, string text)
    {
        text = text.FilterText();
        Assert.AreEqual(expected, text);
    }

    /**
     * If a closing tag is missing, automatically add one.
     * If an opening tag is missing, remove the closing tag.
     */
    [Test]
    [TestCase("<b>Test</b>", "<b>Test")]
    [TestCase("Test", "Test</b>")]
    [TestCase("<color=#123>Test</color>", "<color=#123>Test")]
    [TestCase("Test", "Test</color>")]
    [TestCase("<i>Test</i>", "<i>Test")]
    [TestCase("Test", "Test</i>")]
    [TestCase("<size=18>Test</size>", "<size=#18>Test")]
    [TestCase("Test", "Test</size>")]
    public void MissingTagTests(string expected, string text)
    {
        text = text.FilterText();
        Assert.AreEqual(expected, text);
    }

    /**
     * A nested tag must have both its opening and closing tags within the parent tag.
     * Automatically close the parent tag if a nested closing tag is found.
     */
    [Test]
    [TestCase("<b><b>Test</b></b>", "<b><b>Test</b></b>")]
    [TestCase("<b><i>Test</i></b>", "<b><i>Test</b></i>")]
    [TestCase("<size=18><color=#123><b><i></i></b></color></size>", "<size=18><color=#123><b><i></size></color></b></i>")]
    public void NestedTagTests(string expected, string text)
    {
        text = text.FilterText();
        Assert.AreEqual(expected, text);
    }

    /**
     * The aliases <c/> and <s/> are expanded into <color/> and <size/>.
     */
    [Test]
    [TestCase("<color=#123456>Test</color>", "<c=#123456>Test</c>")]
    [TestCase("<color=#123>Test</color>", "<c=#123>Test</c>")]
    [TestCase("<color>Test</color>", "<c>Test</c>")]
    [TestCase("<size=18>Test</size>", "<s=18>Test</s>")]
    [TestCase("<size>Test</size>", "<s>Test</s>")]
    public void TagAliasTests(string expected, string text)
    {
        text = text.FilterText();
        Assert.AreEqual(expected, text);
    }
}
