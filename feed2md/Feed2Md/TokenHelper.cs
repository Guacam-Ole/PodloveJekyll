

using Feed2Md.Feed;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Feed2Md
{
    public enum CleanMode
    {
        InTextFile,
        InPath,
        InFile
    }

    public static class TokenHelper
    {

        public static string ReplaceTokens(string pathContainingTokens, object rootObject, string prefix)
        {
            var allTokens = GetTokensinString(pathContainingTokens);
            foreach (var token in allTokens)
            {
                var replacedToken = ReplaceToken(prefix, rootObject, token);
                if (replacedToken != null) pathContainingTokens = pathContainingTokens.Replace($"[{token}]", replacedToken);
            }
            return pathContainingTokens;
        }

        public static string CleanString(this string pathContainingTokens, CleanMode cleanMode= CleanMode.InTextFile)
        {
            var cleanedString = pathContainingTokens.Replace("\\[", "[").Replace("\\]", "]");
            char[] invalidCharacters = new char[0];

            switch (cleanMode)
            {
                case CleanMode.InFile:
                    invalidCharacters = System.IO.Path.GetInvalidFileNameChars();
                    break;
                case CleanMode.InPath:
                    invalidCharacters = System.IO.Path.GetInvalidPathChars();
                    break;
            }
            foreach (char badCharacter in invalidCharacters)
            {
                cleanedString = cleanedString.Replace(badCharacter, '_');
            }

            return cleanedString;
        }

        private static List<string> GetTokensinString(string pathContainingTokens)
        {
            var foundTokens = new List<string>();
            bool inToken = false;
            string currentToken = null;
            bool ignorenext = false;
            for (int i = 0; i < pathContainingTokens.Length; i++)
            {
                if (ignorenext)
                {
                    ignorenext = false;
                    continue;
                }
                if (pathContainingTokens[i] == '\\')
                {
                    ignorenext = true;
                    continue;
                }
                if (!inToken)
                {
                    if (pathContainingTokens[i] == '[')
                    {
                        inToken = true;
                        currentToken = string.Empty;
                    }
                }
                else
                {
                    if (pathContainingTokens[i] == ']')
                    {
                        inToken = false;
                        foundTokens.Add(currentToken);
                    }
                    else
                    {
                        currentToken += pathContainingTokens[i];
                    }
                }
            }
            return foundTokens;
        }


        private static void EachLoop()
        {
            // Magic shit to be inserted here
        }

        private static string ReplaceToken(string parentPath, object parentObject, string token)
        {
            if (token.StartsWith("EACH"))
            {
                EachLoop();
                return string.Empty;
            }

            foreach (var property in parentObject.GetType().GetProperties())
            {
                if (!property.CanRead) continue;
                var value = property.GetValue(parentObject);
                string propertyPath = parentPath + property.Name;
                if (propertyPath == token)
                    return value?.ToString();
                string child = propertyPath + ".";

                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    foreach (var subelement in (IEnumerable)value)
                    {
                        var replacedToken = ReplaceToken(child + subelement.GetType().Name + ".", subelement, token);
                        if (replacedToken != null) return replacedToken;
                    }
                }
                else
                {
                    if (token.Contains(child) && value != null)
                    {
                        return ReplaceToken(child, value, token);
                    }
                }
            }
            return null;    // Nothing found
        }
    }
}