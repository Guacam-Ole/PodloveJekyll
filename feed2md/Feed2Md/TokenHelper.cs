

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

        public static string ReplaceTokens(string textContainingTokens, object rootObject, string prefix)
        {
            var allTokens = GetTokensinString(textContainingTokens);
            foreach (var token in allTokens)
            {
                var replacedTokenProperty = ReplaceToken(prefix, rootObject, token, out object replacedToken);
                if (replacedTokenProperty != null) textContainingTokens = textContainingTokens.Replace($"[{token}]", replacedToken?.ToString());
            }
            return textContainingTokens;
        }

        public static string CleanString(this string pathContainingTokens, CleanMode cleanMode = CleanMode.InTextFile)
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

        //private IEnumerable DrillDownToList(string parentPath, object parentObject, string token)
        //{
        //    foreach (var property in parentObject.GetType().GetProperties())
        //    {
        //        if (!property.CanRead) continue;
        //    }

        //}


        //private static string ReplaceEachLoop(string loopContent, object rootObject, IEnumerable list, string prefix)
        //{

        //    // Magic shit to be inserted here
        //    foreach (var listitem in list)
        //    {
        //        ReplaceTokens(loopContent, listitem, prefix);
        //    }
        //}


        private static PropertyInfo ReplaceToken(string parentPath, object parentObject, string token, out object propertyContent)
        {
            foreach (var property in parentObject.GetType().GetProperties())
            {
                if (!property.CanRead) continue;

                propertyContent = null;
                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    propertyContent = property.GetValue(parentObject);

                }
                else if (property.Name != "Item") 
                {
                    propertyContent = property.GetValue(parentObject);
                } 
                string propertyPath = parentPath + property.Name;
                if (propertyPath == token)
                    return property;
                string child = propertyPath + ".";

                if (token.Contains(child) && propertyContent != null)
                {
                    return ReplaceToken(child, propertyContent, token, out propertyContent);
                }
            }

            // Nothing found:
            propertyContent = null;
            return null;    
        }

        //private static string ReplaceToken(string parentPath, object parentObject, string token)
        //{
        //    var property = GetPropertyForToken(parentPath, parentObject, token, out string replacedToken);
        //    if (property == null) return null;


        //    foreach (var property in parentObject.GetType().GetProperties())
        //    {
        //        if (!property.CanRead) continue;
        //        var value = property.GetValue(parentObject);
        //        string propertyPath = parentPath + property.Name;
        //        if (propertyPath == token)
        //            return value?.ToString();
        //        string child = propertyPath + ".";

        //        if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
        //        {
        //            foreach (var subelement in (IEnumerable)value)
        //            {
        //                var replacedToken = ReplaceToken(child + subelement.GetType().Name + ".", subelement, token);
        //                if (replacedToken != null) return replacedToken;
        //            }
        //        }
        //        else
        //        {
        //            if (token.Contains(child) && value != null)
        //            {
        //                return ReplaceToken(child, value, token);
        //            }
        //        }
        //    }
        //    return null;    // Nothing found
        //}
    }
}