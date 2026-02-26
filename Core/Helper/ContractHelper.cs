using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CMPNatural.Core.Helper
{
	public class CompanyContractHelper
	{

        public static string SignFont = "font-family: Dancing Script, cursive !important; font-size: 40px; font-weight: 700;";
        public static string ShowByKey(string key, string content ,string aditionalStyle="")
        {
            string pattern = $@"(<p[^>]*?)\s*style\s*=\s*""([^""]*?)display:\s*none;([^""]*?)""([^>]*?>)([^<]*?{Regex.Escape(key)}[^<]*?)(</p>)";
            string replacement = $@"$1 style=""$2display: block;{aditionalStyle}$3""$4$5$6";
            string updatedHtml = Regex.Replace(content, pattern, replacement);
            return updatedHtml;
        }

        //public static string HideByKey(string key, string content)
        //{
        //    // Match <p> tags containing any of the keys
        //    string pattern = $@"(<p)([^>]*>)([^<]*?({key})[^<]*?)(</p>)";
        //    string replacement = @"$1 style=""display: none;""$2$3$5";

        //    string updatedHtml = Regex.Replace(content, pattern, replacement);
        //    return updatedHtml;
        //}

        public static string HideByKey(string key, string content)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(key))
                return content;

            var escapedKey = Regex.Escape(key);
            var paragraphPattern = @"<p\b(?<attrs>[^>]*)>(?<inner>.*?)</p>";

            return Regex.Replace(content, paragraphPattern, match =>
            {
                var attrs = match.Groups["attrs"].Value;
                var inner = match.Groups["inner"].Value;

                if (!Regex.IsMatch(inner, escapedKey))
                    return match.Value;

                var stylePattern = @"\bstyle\s*=\s*""(?<style>[^""]*)""";
                if (Regex.IsMatch(attrs, stylePattern))
                {
                    attrs = Regex.Replace(attrs, stylePattern, styleMatch =>
                    {
                        var style = styleMatch.Groups["style"].Value;
                        var cleanedStyle = Regex.Replace(style, @"display\s*:\s*(none|block)\s*;?", string.Empty, RegexOptions.IgnoreCase).Trim();
                        var prefix = string.IsNullOrWhiteSpace(cleanedStyle) ? string.Empty : $"{cleanedStyle}; ";
                        return $"style=\"{prefix}display: none;\"";
                    });
                }
                else
                {
                    attrs = $"{attrs} style=\"display: none;\"";
                }

                return $"<p{attrs}>{inner}</p>";
            }, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }
    }
}
