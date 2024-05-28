using System.Text.RegularExpressions;

namespace TelegramCarInsurance.Domain.Services;

/// <summary>
/// Class that is responsible for work with regex
/// </summary>
public class RegexService
{
    /// <summary>
    /// General pattern for questions
    /// </summary>
    private readonly string QuestionPattern = @"\bquestion\b|\bquestions\b|\bask\b|\byou\b|\?|\banswer\b|\btell\b|\bhow\b|\bcan\b";

    /// <summary>
    /// Method for check if command is question
    /// </summary>
    /// <param name="input">A string to regex</param>
    public bool IsQuestion(string input)
    {
        Regex regex = new Regex(QuestionPattern, RegexOptions.IgnoreCase);

        return regex.IsMatch(input);
    }
}