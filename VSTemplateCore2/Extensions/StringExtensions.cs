using System.Text.RegularExpressions;

namespace VSTemplateCore2.Extensions {
	public static class StringExtensions {
		public static string CreateAlias(this string title, int maxLength) {
			var source = title.ToLower();
			var createdAlias = string.Empty;

			// Remove all punctuation from source
			source = Regex.Replace(source, @"[^A-Za-z0-9 \-/]", string.Empty);
			// Remove all common words that shouldn't be included in the alias
			source = Regex.Replace(source, @"\b(a |the |or |at |of |and |not |for |are |to )\b", string.Empty);
			// Replace the space character with a hyphen, replace multiple adjacent hyphens with one
			source = Regex.Replace(source, @"/", "-");
			source = Regex.Replace(source, @"\s", "-");
			source = Regex.Replace(source, @"\-+", "-");
			// Do not return part of a word at the end, only a whole word
			createdAlias = source.TruncateStringAtWord(maxLength, false);

			return createdAlias;
		}

		public static string TruncateStringAtWord(this string source, int maxLength, bool addEllipsis) {
			var truncatedString = string.Empty;
			const string SPACE = " ";

			var truncatePosition = maxLength;
			if (source.Length > maxLength) {
				while (source.Substring(truncatePosition, 1) != SPACE) {
					--truncatePosition;
				}
				truncatedString = source.Substring(0, truncatePosition - 1);
			} else {
				truncatedString = source;
			}
			return truncatedString;
		}
	}
}
