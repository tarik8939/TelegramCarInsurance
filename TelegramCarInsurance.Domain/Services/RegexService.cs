using System.Text.RegularExpressions;
using static iText.Kernel.Pdf.Colorspace.PdfSpecialCs;

namespace TelegramCarInsurance.Domain.Services;

/// <summary>
/// Class that is responsible for work with regex
/// </summary>
public class RegexService
{
    /// <summary>
    /// General pattern for questions
    /// </summary>
    private readonly string QuestionPattern = @"\bquestion\b|\bquestions\b|\bask\b|\byou\b|\?|\banswer\b|
                                \btell\b|\bhow\b|\bcan\b|\bwhich\b|\bsay\b|\bwho\b";

    /// <summary>
    /// General pattern for commands
    /// </summary>
    private readonly string CommandPattern = @"^/?(.*)$";

    /// <summary>
    /// Method for check if command is question
    /// </summary>
    /// <param name="input">A string to regex</param>
    public bool IsQuestion(string input)
    {
        Regex regex = new Regex(QuestionPattern, RegexOptions.IgnoreCase);

        return regex.IsMatch(input);
    }

    /// <summary>
    /// Method for compare commands
    /// </summary>
    /// <param name="commandName"></param>
    /// <param name="userCommand"></param>
    public bool CompareCommand(string commandName, string userCommand)
    {
        Regex regex = new Regex(CommandPattern, RegexOptions.IgnoreCase);

        string commandNameRgx = regex.Match(commandName).Groups[1].Value;
        string userCommandRgx = regex.Match(userCommand).Groups[1].Value;

        return string.Equals(commandNameRgx, userCommandRgx, StringComparison.OrdinalIgnoreCase);
    }
}