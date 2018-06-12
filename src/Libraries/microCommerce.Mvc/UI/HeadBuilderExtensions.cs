using microCommerce.Ioc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace microCommerce.Mvc.UI
{
    public static class HeadBuilderExtensions
    {
        /// <summary>
        /// Add title element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Title part</param>
        public static void AddTitle(this IHtmlHelper html, string part)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            headBuilder.AddTitle(part);
        }

        /// <summary>
        /// Append title element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Title part</param>
        public static void AppendTitle(this IHtmlHelper html, string part)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            headBuilder.AppendTitle(part);
        }

        /// <summary>
        /// Generate all title parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="addDefaultTitle">A value indicating whether to insert a default title</param>
        /// <param name="part">Title part</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent RenderTitle(this IHtmlHelper html, bool addDefaultTitle = true, string part = "")
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            html.AppendTitle(part);

            return new HtmlString(html.Encode(headBuilder.RenderTitle(addDefaultTitle)));
        }

        /// <summary>
        /// Add meta description element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Meta description part</param>
        public static void AddMetaDescription(this IHtmlHelper html, string part)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            headBuilder.AddMetaDescription(part);
        }

        /// <summary>
        /// Append meta description element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Meta description part</param>
        public static void AppendMetaDescription(this IHtmlHelper html, string part)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            headBuilder.AppendMetaDescription(part);
        }

        /// <summary>
        /// Generate all description parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Meta description part</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent RenderMetaDescription(this IHtmlHelper html, string part = "")
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            html.AppendMetaDescription(part);

            return new HtmlString(html.Encode(headBuilder.RenderMetaDescription()));
        }

        /// <summary>
        /// Add script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public static void AddScript(this IHtmlHelper html, string src,
            bool excludeFromBundle = false, bool isAsync = false)
        {
            AddScript(html, ResourceLocation.Head, src, excludeFromBundle, isAsync);
        }

        /// <summary>
        /// Add script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public static void AddScript(this IHtmlHelper html, ResourceLocation location,
            string src, bool excludeFromBundle = false, bool isAsync = false)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            headBuilder.AddScript(location, src, excludeFromBundle, isAsync);
        }

        /// <summary>
        /// Append script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public static void AppendScript(this IHtmlHelper html, string src,
            bool excludeFromBundle = false, bool isAsync = false)
        {
            AppendScript(html, ResourceLocation.Head, src, excludeFromBundle, isAsync);
        }

        /// <summary>
        /// Append script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public static void AppendScript(this IHtmlHelper html, ResourceLocation location,
            string src, bool excludeFromBundle = false, bool isAsync = false)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            headBuilder.AppendScript(location, src, excludeFromBundle, isAsync);
        }

        /// <summary>
        /// Generate all script parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent RenderScripts(this IHtmlHelper html, IUrlHelper urlHelper, ResourceLocation location)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();

            return new HtmlString(headBuilder.RenderScripts(urlHelper, location));
        }

        /// <summary>
        /// Add CSS element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public static void AddStyle(this IHtmlHelper html,
            string src, bool excludeFromBundle = false)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            headBuilder.AddStyle(src, excludeFromBundle);
        }

        /// <summary>
        /// Append CSS element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public static void AppendStyle(this IHtmlHelper html,
            string src, bool excludeFromBundle = false)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();
            headBuilder.AppendStyle(src, excludeFromBundle);
        }

        /// <summary>
        /// Generate all CSS parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="urlHelper">URL Helper</param>
        /// <param name="location">A location of the script element</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent RenderStyles(this IHtmlHelper html, IUrlHelper urlHelper)
        {
            var headBuilder = EngineContext.Current.Resolve<IHeadBuilder>();

            return new HtmlString(headBuilder.RenderStyles(urlHelper));
        }
    }
}