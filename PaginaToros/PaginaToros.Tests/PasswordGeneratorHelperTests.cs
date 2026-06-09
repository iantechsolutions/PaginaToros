using PaginaToros.Shared.Helpers;

namespace PaginaToros.Tests;

public class PasswordGeneratorHelperTests
{
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void GenerateWordPassword_ReturnsUppercaseWordsSeparatedByDots(int wordCount)
    {
        var password = PasswordGeneratorHelper.GenerateWordPassword(wordCount);

        var parts = password.Split('.');

        Assert.Equal(wordCount, parts.Length);
        Assert.All(parts, part =>
        {
            Assert.False(string.IsNullOrWhiteSpace(part));
            Assert.Matches("^[A-Z]+$", part);
        });
        Assert.Equal(password, password.ToUpperInvariant());
        Assert.DoesNotContain(' ', password);
    }

    [Fact]
    public void GenerateWordPassword_UsesDefaultThreeWords()
    {
        var password = PasswordGeneratorHelper.GenerateWordPassword();

        Assert.Equal(3, password.Split('.').Length);
    }
}
