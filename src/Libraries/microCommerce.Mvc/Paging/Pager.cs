﻿using microCommerce.Common;
using microCommerce.Ioc;
using microCommerce.Mvc.Extensions;
using microCommerce.Providers.Localizations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace microCommerce.Mvc.Paging
{
    public class Pager: IHtmlContent
    {
        /// <summary>
        /// Model
        /// </summary>
        protected readonly IPageableModel _model;
        /// <summary>
        /// ViewContext
        /// </summary>
        protected readonly ViewContext viewContext;
        /// <summary>
        /// Page query string prameter name
        /// </summary>
        protected string pageQueryName = "page";
        /// <summary>
        /// A value indicating whether to show Total summary
        /// </summary>
        protected bool showTotalSummary;
        /// <summary>
        /// A value indicating whether to show pager items
        /// </summary>
        protected bool showPagerItems = true;
        /// <summary>
        /// A value indicating whether to show the first item
        /// </summary>
        protected bool showFirst = true;
        /// <summary>
        /// A value indicating whether to the previous item
        /// </summary>
        protected bool showPrevious = true;
        /// <summary>
        /// A value indicating whether to show the next item
        /// </summary>
        protected bool showNext = true;
        /// <summary>
        /// A value indicating whether to show the last item
        /// </summary>
        protected bool showLast = true;
        /// <summary>
        /// A value indicating whether to show individual page
        /// </summary>
        protected bool showIndividualPages = true;
        /// <summary>
        /// A value indicating whether to render empty query string parameters (without values)
        /// </summary>
        protected bool renderEmptyParameters = true;
        /// <summary>
        /// Number of individual page items to display
        /// </summary>
        protected int individualPagesDisplayedCount = 5;
        /// <summary>
        /// Boolean parameter names
        /// </summary>
        protected IList<string> booleanParameterNames;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="context">ViewContext</param>
		public Pager(IPageableModel model, ViewContext context)
        {
            _model = model;
            viewContext = context;
            booleanParameterNames = new List<string>();
        }

        /// <summary>
        /// ViewContext
        /// </summary>
		protected ViewContext ViewContext
        {
            get { return viewContext; }
        }

        /// <summary>
        /// Set 
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager QueryParam(string value)
        {
            this.pageQueryName = value;
            return this;
        }
        /// <summary>
        /// Set a value indicating whether to show Total summary
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowTotalSummary(bool value)
        {
            this.showTotalSummary = value;
            return this;
        }
        /// <summary>
        /// Set a value indicating whether to show pager items
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowPagerItems(bool value)
        {
            this.showPagerItems = value;
            return this;
        }
        /// <summary>
        /// Set a value indicating whether to show the first item
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowFirst(bool value)
        {
            this.showFirst = value;
            return this;
        }
        /// <summary>
        /// Set a value indicating whether to the previous item
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowPrevious(bool value)
        {
            this.showPrevious = value;
            return this;
        }
        /// <summary>
        /// Set a  value indicating whether to show the next item
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowNext(bool value)
        {
            this.showNext = value;
            return this;
        }
        /// <summary>
        /// Set a value indicating whether to show the last item
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowLast(bool value)
        {
            this.showLast = value;
            return this;
        }
        /// <summary>
        /// Set number of individual page items to display
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowIndividualPages(bool value)
        {
            this.showIndividualPages = value;
            return this;
        }
        /// <summary>
        /// Set a value indicating whether to render empty query string parameters (without values)
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager RenderEmptyParameters(bool value)
        {
            this.renderEmptyParameters = value;
            return this;
        }
        /// <summary>
        /// Set number of individual page items to display
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager IndividualPagesDisplayedCount(int value)
        {
            this.individualPagesDisplayedCount = value;
            return this;
        }
        /// <summary>
        /// little hack here due to ugly MVC implementation
        /// find more info here: http://www.mindstorminteractive.com/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <returns>Pager</returns>
        public Pager BooleanParameterName(string paramName)
        {
            booleanParameterNames.Add(paramName);
            return this;
        }

        /// <summary>
        /// Write control
        /// </summary>
        /// <param name="writer">Writer</param>
        /// <param name="encoder">Encoder</param>
	    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            var htmlString = GenerateHtmlString();
            writer.Write(htmlString);
        }
        /// <summary>
        /// Generate HTML control
        /// </summary>
        /// <returns>HTML control</returns>
	    public override string ToString()
        {
            return GenerateHtmlString();
        }
        /// <summary>
        /// Generate HTML control
        /// </summary>
        /// <returns>HTML control</returns>
        public virtual string GenerateHtmlString()
        {
            if (_model.TotalItems == 0)
                return null;

            var localizationProvider = EngineContext.Current.Resolve<ILocalizationProvider>();

            var links = new StringBuilder();
            if (showTotalSummary && (_model.TotalPages > 0))
            {
                links.Append("<li class=\"total-summary\">");
                links.Append(string.Format(localizationProvider.GetResourceValue("Pager.CurrentPage"), _model.PageIndex + 1, _model.TotalPages, _model.TotalItems));
                links.Append("</li>");
            }

            if (showPagerItems && (_model.TotalPages > 1))
            {
                if (showFirst)
                {
                    //first page
                    if ((_model.PageIndex >= 3) && (_model.TotalPages > individualPagesDisplayedCount))
                    {
                        links.Append(CreatePageLink(1, localizationProvider.GetResourceValue("Pager.First"), "first-page"));
                    }
                }
                if (showPrevious)
                {
                    //previous page
                    if (_model.PageIndex > 0)
                    {
                        links.Append(CreatePageLink(_model.PageIndex, localizationProvider.GetResourceValue("Pager.Previous"), "previous-page"));
                    }
                }
                if (showIndividualPages)
                {
                    //individual pages
                    var firstIndividualPageIndex = GetFirstIndividualPageIndex();
                    var lastIndividualPageIndex = GetLastIndividualPageIndex();
                    for (var i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (_model.PageIndex == i)
                        {
                            links.AppendFormat("<li class=\"current-page\"><span>{0}</span></li>", (i + 1));
                        }
                        else
                        {
                            links.Append(CreatePageLink(i + 1, (i + 1).ToString(), "individual-page"));
                        }
                    }
                }
                if (showNext)
                {
                    //next page
                    if ((_model.PageIndex + 1) < _model.TotalPages)
                    {
                        links.Append(CreatePageLink(_model.PageIndex + 2, localizationProvider.GetResourceValue("Pager.Next"), "next-page"));
                    }
                }
                if (showLast)
                {
                    //last page
                    if (((_model.PageIndex + 3) < _model.TotalPages) && (_model.TotalPages > individualPagesDisplayedCount))
                    {
                        links.Append(CreatePageLink(_model.TotalPages, localizationProvider.GetResourceValue("Pager.Last"), "last-page"));
                    }
                }
            }

            var result = links.ToString();
            if (!string.IsNullOrEmpty(result))
            {
                result = "<ul>" + result + "</ul>";
            }
            return result;
        }
        /// <summary>
        /// Is pager empty (only one page)?
        /// </summary>
        /// <returns>Result</returns>
	    public virtual bool IsEmpty()
        {
            var html = GenerateHtmlString();
            return string.IsNullOrEmpty(html);
        }

        /// <summary>
        /// Get first individual page index
        /// </summary>
        /// <returns>Page index</returns>
        protected virtual int GetFirstIndividualPageIndex()
        {
            if ((_model.TotalPages < individualPagesDisplayedCount) ||
                ((_model.PageIndex - (individualPagesDisplayedCount / 2)) < 0))
            {
                return 0;
            }

            if ((_model.PageIndex + (individualPagesDisplayedCount / 2)) >= _model.TotalPages)
            {
                return (_model.TotalPages - individualPagesDisplayedCount);
            }

            return (_model.PageIndex - (individualPagesDisplayedCount / 2));
        }

        /// <summary>
        /// Get last individual page index
        /// </summary>
        /// <returns>Page index</returns>
        protected virtual int GetLastIndividualPageIndex()
        {
            var num = individualPagesDisplayedCount / 2;
            if ((individualPagesDisplayedCount % 2) == 0)
            {
                num--;
            }

            if ((_model.TotalPages < individualPagesDisplayedCount) ||
                ((_model.PageIndex + num) >= _model.TotalPages))
            {
                return (_model.TotalPages - 1);
            }

            if ((_model.PageIndex - (individualPagesDisplayedCount / 2)) < 0)
            {
                return (individualPagesDisplayedCount - 1);
            }

            return (_model.PageIndex + num);
        }
        /// <summary>
        /// Create page link
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="text">Text</param>
        /// <param name="cssClass">CSS class</param>
        /// <returns>Link</returns>
		protected virtual string CreatePageLink(int pageNumber, string text, string cssClass)
        {
            var liBuilder = new TagBuilder("li");
            if (!string.IsNullOrWhiteSpace(cssClass))
                liBuilder.AddCssClass(cssClass);

            var aBuilder = new TagBuilder("a");
            aBuilder.InnerHtml.AppendHtml(text);
            aBuilder.MergeAttribute("href", CreateDefaultUrl(pageNumber));
            liBuilder.InnerHtml.AppendHtml(aBuilder);

            return liBuilder.RenderHtmlContent();
        }

        /// <summary>
        /// Create default URL
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <returns>URL</returns>
        protected virtual string CreateDefaultUrl(int pageNumber)
        {
            var routeValues = new RouteValueDictionary();

            var parametersWithEmptyValues = new List<string>();
            foreach (var key in viewContext.HttpContext.Request.Query.Keys.Where(key => key != null))
            {
                //TODO test new implementation (QueryString, keys). And ensure no null exception is thrown when invoking ToString(). Is "StringValues.IsNullOrEmpty" required?
                var value = viewContext.HttpContext.Request.Query[key].ToString();
                if (renderEmptyParameters && string.IsNullOrEmpty(value))
                {
                    //we store query string parameters with empty values separately
                    //we need to do it because they are not properly processed in the UrlHelper.GenerateUrl method (dropped for some reasons)
                    parametersWithEmptyValues.Add(key);
                }
                else
                {
                    if (booleanParameterNames.Contains(key, StringComparer.InvariantCultureIgnoreCase))
                    {
                        //little hack here due to ugly MVC implementation
                        //find more info here: http://www.mindstorminteractive.com/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
                        if (!string.IsNullOrEmpty(value) && value.Equals("true,false", StringComparison.InvariantCultureIgnoreCase))
                        {
                            value = "true";
                        }
                    }
                    routeValues[key] = value;
                }
            }

            if (pageNumber > 1)
            {
                routeValues[pageQueryName] = pageNumber;
            }
            else
            {
                //SEO. we do not render pageindex query string parameter for the first page
                if (routeValues.ContainsKey(pageQueryName))
                {
                    routeValues.Remove(pageQueryName);
                }
            }

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            var url = webHelper.GetThisPageUrl(false);
            foreach (var routeValue in routeValues)
            {
                url = webHelper.ModifyQueryString(url, routeValue.Key + "=" + routeValue.Value, null);
            }

            if (renderEmptyParameters && parametersWithEmptyValues.Any())
            {
                foreach (var key in parametersWithEmptyValues)
                {
                    url = webHelper.ModifyQueryString(url, key + "=", null);
                }
            }

            return url;
        }

    }
}
