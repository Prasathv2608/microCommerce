using BiletKesfet.Common;
using BundlerMinifier;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Domain.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace microCommerce.Mvc.UI
{
    public class HeadBuilder : IHeadBuilder
    {
        private static readonly object s_lock = new object();

        private readonly SeoSettings _seoSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ICustomFileProvider _fileProvider;
        private BundleFileProcessor _processor;

        private readonly List<string> _titleParts;
        private readonly List<string> _metaDescriptionParts;
        private readonly Dictionary<ResourceLocation, List<ScriptReferenceMeta>> _scriptParts;
        private readonly List<StyleReferenceMeta> _styleParts;

        //in minutes
        private const int RecheckBundledFilesPeriod = 120;

        public HeadBuilder(SeoSettings seoSettings,
            IHostingEnvironment hostingEnvironment,
            IStaticCacheManager cacheManager,
            ICustomFileProvider fileProvider)
        {
            _seoSettings = seoSettings;
            _hostingEnvironment = hostingEnvironment;
            _cacheManager = cacheManager;
            _fileProvider = fileProvider;
            _processor = new BundleFileProcessor();

            _titleParts = new List<string>();
            _metaDescriptionParts = new List<string>();
            _scriptParts = new Dictionary<ResourceLocation, List<ScriptReferenceMeta>>();
            _styleParts = new List<StyleReferenceMeta>();
        }

        /// <summary>
        /// Get bundled file name
        /// </summary>
        /// <param name="parts">Parts to bundle</param>
        /// <returns>File name</returns>
        protected virtual string GetBundleFileName(string[] parts)
        {
            if (parts == null || parts.Length == 0)
                throw new ArgumentException("parts");

            //calculate hash
            var hash = string.Empty;
            using (SHA256 sha = new SHA256Managed())
            {
                // string concatenation
                var hashInput = string.Empty;
                foreach (var part in parts)
                {
                    hashInput += part;
                    hashInput += ",";
                }

                var input = sha.ComputeHash(Encoding.Unicode.GetBytes(hashInput));
                hash = WebEncoders.Base64UrlEncode(input);
            }

            //ensure only valid chars
            hash = SeoHelper.GetSeName(hash);

            return hash;
        }

        /// <summary>
        /// Add title element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Title part</param>
        public virtual void AddTitle(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Add(part);
        }

        /// <summary>
        /// Append title element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Title part</param>
        public virtual void AppendTitle(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Insert(0, part);
        }

        /// <summary>
        /// Generate all title parts
        /// </summary>
        /// <param name="addDefaultTitle">A value indicating whether to insert a default title</param>
        /// <returns>Generated string</returns>
        public virtual string RenderTitle(bool addDefaultTitle)
        {
            var result = string.Empty;

            var specificTitle = string.Join(_seoSettings.PageTitleSeparator, _titleParts.AsEnumerable().Reverse());
            if (!string.IsNullOrEmpty(specificTitle))
            {
                if (addDefaultTitle)
                {
                    result = string.Join(_seoSettings.PageTitleSeparator, specificTitle, _seoSettings.DefaultTitle);
                }
                else
                {
                    //page title only
                    result = specificTitle;
                }
            }
            else
            {
                //default title only
                result = _seoSettings.DefaultTitle;
            }

            return result;
        }

        /// <summary>
        /// Add meta description element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Meta description part</param>
        public virtual void AddMetaDescription(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaDescriptionParts.Add(part);
        }

        /// <summary>
        /// Append meta description element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Meta description part</param>
        public virtual void AppendMetaDescription(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaDescriptionParts.Insert(0, part);
        }

        /// <summary>
        /// Generate all description parts
        /// </summary>
        /// <returns>Generated string</returns>
        public virtual string RenderMetaDescription()
        {
            var metaDescription = string.Join(", ", _metaDescriptionParts.AsEnumerable().Reverse());
            var result = !string.IsNullOrEmpty(metaDescription) ? metaDescription : _seoSettings.DefaultMetaDescription;

            return result;
        }

        /// <summary>
        /// Add script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public virtual void AddScript(ResourceLocation location, string src, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            _scriptParts[location].Add(new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Src = src
            });
        }

        /// <summary>
        /// Append script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public virtual void AppendScript(ResourceLocation location, string src, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            _scriptParts[location].Insert(0, new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Src = src
            });
        }

        /// <summary>
        /// Generate all script parts
        /// </summary>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public virtual string RenderScripts(IUrlHelper urlHelper, ResourceLocation location)
        {
            if (!_scriptParts.ContainsKey(location) || _scriptParts[location] == null)
                return "";

            if (!_scriptParts.Any())
                return "";

            if (_seoSettings.EnableJsBundling)
            {
                var partsToBundle = _scriptParts[location]
                    .Where(x => !x.ExcludeFromBundle)
                    .Distinct()
                    .ToArray();

                var partsToDontBundle = _scriptParts[location]
                    .Where(x => x.ExcludeFromBundle)
                    .Distinct()
                    .ToArray();

                var result = new StringBuilder();

                //parts to  bundle
                if (partsToBundle.Any())
                {
                    //ensure \bundles directory exists
                    _fileProvider.CreateDirectory(_fileProvider.GetAbsolutePath("bundles"));

                    var bundle = new Bundle();
                    foreach (var item in partsToBundle)
                    {
                        new PathString(urlHelper.Content(item.Src))
                            .StartsWithSegments(urlHelper.ActionContext.HttpContext.Request.PathBase, out PathString path);
                        var src = path.Value.TrimStart('/');

                        //check whether this file exists, if not it should be stored into /wwwroot directory
                        if (!_fileProvider.FileExists(_fileProvider.MapPath(path)))
                            src = $"wwwroot/{src}";

                        bundle.InputFiles.Add(src);
                    }
                    //output file
                    var outputFileName = GetBundleFileName(partsToBundle.Select(x => x.Src).ToArray());
                    bundle.OutputFileName = "wwwroot/bundles/" + outputFileName + ".js";
                    //save
                    var configFilePath = _hostingEnvironment.ContentRootPath + "\\" + outputFileName + ".json";
                    bundle.FileName = configFilePath;
                    lock (s_lock)
                    {
                        //performance optimization. do not bundle and minify for each HTTP request
                        //we periodically re-check already bundles file
                        //so if we have minification enabled, it could take up to several minutes to see changes in updated resource files (or just reset the cache or restart the site)
                        var cacheKey = $"minification.shouldrebuild.scripts-{outputFileName}";
                        var shouldRebuild = _cacheManager.Get(cacheKey, RecheckBundledFilesPeriod, () => true);
                        if (shouldRebuild)
                        {
                            //process
                            _processor.Process(configFilePath, new List<Bundle> { bundle });
                            _cacheManager.Set(cacheKey, false, RecheckBundledFilesPeriod);
                        }
                    }

                    //render
                    result.AppendFormat("<script src=\"{0}\"></script>", urlHelper.Content("~/bundles/" + outputFileName + ".min.js"));
                    result.Append(Environment.NewLine);
                }

                //parts to not bundle
                foreach (var item in partsToDontBundle)
                {
                    result.AppendFormat("<script {1}src=\"{0}\"></script>", urlHelper.Content(item.Src), item.IsAsync ? "async " : "");
                    result.Append(Environment.NewLine);
                }

                return result.ToString();
            }
            else
            {
                //bundling is disabled
                var result = new StringBuilder();
                foreach (var item in _scriptParts[location].Distinct())
                {
                    result.AppendFormat("<script {1}src=\"{0}\"></script>", urlHelper.Content(item.Src), item.IsAsync ? "async " : "");
                    result.Append(Environment.NewLine);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Add CSS element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public virtual void AddStyle(string src, bool excludeFromBundle = false)
        {
            if (string.IsNullOrEmpty(src))
                return;

            _styleParts.Add(new StyleReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Src = src
            });
        }

        /// <summary>
        /// Append CSS element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public virtual void AppendStyle(string src, bool excludeFromBundle = false)
        {
            if (string.IsNullOrEmpty(src))
                return;

            _styleParts.Insert(0, new StyleReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Src = src
            });
        }

        /// <summary>
        /// Generate all CSS parts
        /// </summary>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public virtual string RenderStyles(IUrlHelper urlHelper)
        {
            if (!_styleParts.Any())
                return string.Empty;

            //use setting if no value is specified
            bool bundleFiles = _seoSettings.EnableCssBundling;

            //CSS bundling is not allowed in virtual directories
            if (urlHelper.ActionContext.HttpContext.Request.PathBase.HasValue)
                bundleFiles = false;

            if (bundleFiles)
            {
                var partsToBundle = _styleParts
                    .Where(x => !x.ExcludeFromBundle)
                    .Distinct()
                    .ToArray();

                var partsToDontBundle = _styleParts
                    .Where(x => x.ExcludeFromBundle)
                    .Distinct()
                    .ToArray();

                var result = new StringBuilder();
                //parts to  bundle
                if (partsToBundle.Any())
                {
                    //ensure \bundles directory exists
                    _fileProvider.CreateDirectory(_fileProvider.GetAbsolutePath("bundles"));

                    var bundle = new Bundle();
                    foreach (var item in partsToBundle)
                    {
                        new PathString(urlHelper.Content(item.Src))
                               .StartsWithSegments(urlHelper.ActionContext.HttpContext.Request.PathBase, out PathString path);
                        var src = path.Value.TrimStart('/');

                        //check whether this file exists, if not it should be stored into /wwwroot directory
                        if (!_fileProvider.FileExists(_fileProvider.MapPath(path)))
                            src = $"wwwroot/{src}";

                        bundle.InputFiles.Add(src);
                    }

                    //output file
                    string outputFileName = GetBundleFileName(partsToBundle.Select(x => x.Src).ToArray());
                    bundle.OutputFileName = "wwwroot/bundles/" + outputFileName + ".css";
                    //save
                    var configFilePath = _hostingEnvironment.ContentRootPath + "\\" + outputFileName + ".json";
                    bundle.FileName = configFilePath;
                    lock (s_lock)
                    {
                        //performance optimization. do not bundle and minify for each HTTP request
                        //we periodically re-check already bundles file
                        //so if we have minification enabled, it could take up to several minutes to see changes in updated resource files (or just reset the cache or restart the site)
                        var cacheKey = $"minification.shouldrebuild.styles-{outputFileName}";
                        var shouldRebuild = _cacheManager.Get(cacheKey, RecheckBundledFilesPeriod, () => true);
                        if (shouldRebuild)
                        {
                            //process
                            _processor.Process(configFilePath, new List<Bundle> { bundle });
                            _cacheManager.Set(cacheKey, false, RecheckBundledFilesPeriod);
                        }
                    }

                    //render
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" />", urlHelper.Content("~/bundles/" + outputFileName + ".min.css"));
                    result.Append(Environment.NewLine);
                }

                //parts not to bundle
                foreach (var item in partsToDontBundle)
                {
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" />", urlHelper.Content(item.Src));
                    result.Append(Environment.NewLine);
                }

                return result.ToString();
            }
            else
            {
                //bundling is disabled
                var result = new StringBuilder();
                foreach (var item in _styleParts.Distinct())
                {
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" />", urlHelper.Content(item.Src));
                    result.AppendLine();
                }

                return result.ToString();
            }
        }
    }
}