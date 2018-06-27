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
        private readonly Dictionary<ResourceLocation, List<BundleMeta>> _scriptParts;
        private readonly List<BundleMeta> _styleParts;

        private const string BundleFolder = "bundles";
        private const string StyleCacheKey = "minification-styles-{0}";
        private const string ScriptsCacheKey = "minification-scripts-{0}";

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
            _scriptParts = new Dictionary<ResourceLocation, List<BundleMeta>>();
            _styleParts = new List<BundleMeta>();
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

            if (!_titleParts.Contains(part))
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

            if (!_titleParts.Contains(part))
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

            if (!_metaDescriptionParts.Contains(part))
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

            if (!_metaDescriptionParts.Contains(part))
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
                _scriptParts.Add(location, new List<BundleMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (!_scriptParts[location].Any(s => s.Path == src))
            {
                _scriptParts[location].Add(new BundleMeta
                {
                    ExcludeFromBundle = excludeFromBundle,
                    Path = src
                });
            }
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
                _scriptParts.Add(location, new List<BundleMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (!_scriptParts[location].Any(s => s.Path == src))
            {
                _scriptParts[location].Insert(0, new BundleMeta
                {
                    ExcludeFromBundle = excludeFromBundle,
                    Path = src
                });
            }
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
                return string.Empty;

            if (!_scriptParts.Any())
                return string.Empty;

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
                    _fileProvider.CreateDirectory(_fileProvider.GetAbsolutePath(BundleFolder));

                    var bundle = new Bundle();
                    foreach (var item in partsToBundle)
                    {
                        string src = $"wwwroot{urlHelper.Content(item.Path)}";
                        bundle.InputFiles.Add(src);
                    }

                    //output file
                    var outputFileName = GetBundleFileName(partsToBundle.Select(x => x.Path).ToArray());
                    bundle.OutputFileName = $"wwwroot/{BundleFolder}/{outputFileName}.js";

                    //save
                    var configFilePath = $"{_hostingEnvironment.ContentRootPath}\\{outputFileName}.json";
                    bundle.FileName = configFilePath;
                    lock (s_lock)
                    {
                        var cacheKey = string.Format(ScriptsCacheKey, outputFileName);
                        var shouldRebuild = _cacheManager.Get(cacheKey, RecheckBundledFilesPeriod, () => true);
                        if (shouldRebuild)
                        {
                            //process
                            _processor.Process(configFilePath, new List<Bundle> { bundle });
                            _cacheManager.Set(cacheKey, false, RecheckBundledFilesPeriod);
                        }
                    }

                    //render
                    result.AppendFormat("<script src=\"{0}\"></script>", urlHelper.Content($"~/{BundleFolder}/" + outputFileName + ".min.js"));
                    result.Append(Environment.NewLine);
                }

                //parts to not bundle
                foreach (var item in partsToDontBundle)
                {
                    result.AppendFormat("<script src=\"{0}\"></script>", urlHelper.Content(item.Path));
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
                    result.AppendFormat("<script src=\"{0}\"></script>", urlHelper.Content(item.Path));
                    result.Append(Environment.NewLine);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Add CSS element
        /// </summary>
        /// <param name="src">Style/css path (minified version)</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this style/css from bundling</param>
        public virtual void AddStyle(string src, bool excludeFromBundle = false)
        {
            if (string.IsNullOrEmpty(src))
                return;

            if (!_styleParts.Any(s => s.Path == src))
            {
                _styleParts.Add(new BundleMeta
                {
                    ExcludeFromBundle = excludeFromBundle,
                    Path = src
                });
            }
        }

        /// <summary>
        /// Append CSS element
        /// </summary>
        /// <param name="src">Style/css path (minified version)</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this style/css from bundling</param>
        public virtual void AppendStyle(string src, bool excludeFromBundle = false)
        {
            if (string.IsNullOrEmpty(src))
                return;

            if (!_styleParts.Any(s => s.Path == src))
            {
                _styleParts.Insert(0, new BundleMeta
                {
                    ExcludeFromBundle = excludeFromBundle,
                    Path = src
                });
            }
        }

        /// <summary>
        /// Generate all CSS parts
        /// </summary>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the style/css element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle style/css elements</param>
        /// <returns>Generated string</returns>
        public virtual string RenderStyles(IUrlHelper urlHelper)
        {
            if (!_styleParts.Any())
                return "";

            if (_seoSettings.EnableJsBundling)
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
                    _fileProvider.CreateDirectory(_fileProvider.GetAbsolutePath(BundleFolder));

                    var bundle = new Bundle();
                    foreach (var item in partsToBundle)
                    {
                        string src = $"wwwroot{urlHelper.Content(item.Path)}";
                        bundle.InputFiles.Add(src);
                    }

                    //output file
                    var outputFileName = GetBundleFileName(partsToBundle.Select(x => x.Path).ToArray());
                    bundle.OutputFileName = $"wwwroot/{BundleFolder}/{outputFileName}.css";

                    //save
                    var configFilePath = $"{_hostingEnvironment.ContentRootPath}\\{outputFileName}.json";
                    bundle.FileName = configFilePath;
                    lock (s_lock)
                    {
                        var cacheKey = string.Format(StyleCacheKey, outputFileName);
                        var shouldRebuild = _cacheManager.Get(cacheKey, RecheckBundledFilesPeriod, () => true);
                        if (shouldRebuild)
                        {
                            //process
                            bool processResult = _processor.Process(configFilePath, new List<Bundle> { bundle });
                            _cacheManager.Set(cacheKey, false, RecheckBundledFilesPeriod);
                        }
                    }

                    //render
                    result.AppendFormat("<link rel=\"stylesheet\" href=\"{0}\" />", urlHelper.Content($"~/{BundleFolder}/" + outputFileName + ".min.css"));
                    result.Append(Environment.NewLine);
                }

                //parts to not bundle
                foreach (var item in partsToDontBundle)
                {
                    result.AppendFormat("<link rel=\"stylesheet\" href=\"{0}\" />", urlHelper.Content(item.Path));
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
                    result.AppendFormat("<link rel=\"stylesheet\" href=\"{0}\" />", urlHelper.Content(item.Path));
                    result.Append(Environment.NewLine);
                }

                return result.ToString();
            }
        }
    }
}