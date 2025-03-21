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

        public static string HideByKey(string key, string content)
        {
            // Match <p> tags containing any of the keys
            string pattern = $@"(<p)([^>]*>)([^<]*?({key})[^<]*?)(</p>)";
            string replacement = @"$1 style=""display: none;""$2$3$5";

            string updatedHtml = Regex.Replace(content, pattern, replacement);
            return updatedHtml;
        }
    }
}

