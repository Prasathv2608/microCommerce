﻿using microCommerce.Domain.Directory;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;

namespace microCommerce.Mvc.Routing
{
    public static class LocalizedUrlExtenstions
    {
        public static bool IsLocalizedUrl(this string url, PathString pathBase, bool isRawPath, out string uniqueSeoCode)
        {
            uniqueSeoCode = null;
            if (string.IsNullOrEmpty(url))
                return false;

            //remove application path from raw URL
            if (isRawPath)
                url = url.RemoveApplicationPathFromRawUrl(pathBase);

            //get first segment of passed URL
            var firstSegment = url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrEmpty(firstSegment))
                return false;

            uniqueSeoCode = firstSegment;

            return true;
        }
        
        public static string RemoveApplicationPathFromRawUrl(this string rawUrl, PathString pathBase)
        {
            new PathString(rawUrl).StartsWithSegments(pathBase, out PathString result);
            return WebUtility.UrlDecode(result);
        }
        
        public static string RemoveLanguageSeoCodeFromUrl(this string url, PathString pathBase, bool isRawPath)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            //remove application path from raw URL
            if (isRawPath)
                url = url.RemoveApplicationPathFromRawUrl(pathBase);

            //get result URL
            url = url.TrimStart('/');
            var result = url.Contains('/') ? url.Substring(url.IndexOf('/')) : string.Empty;

            //and add back application path to raw URL
            if (isRawPath)
                result = pathBase + result;

            return result;
        }
        
        public static string AddLanguageSeoCodeToUrl(this string url, PathString pathBase, bool isRawPath, Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));
            
            //remove application path from raw URL
            if (isRawPath && !string.IsNullOrEmpty(url))
                url = url.RemoveApplicationPathFromRawUrl(pathBase);

            //add language code
            url = string.Format("{0}{1}", language.UniqueSeoCode, url);

            //and add back application path to raw URL
            if (isRawPath)
                url = pathBase + url;

            return url;
        }
    }
}