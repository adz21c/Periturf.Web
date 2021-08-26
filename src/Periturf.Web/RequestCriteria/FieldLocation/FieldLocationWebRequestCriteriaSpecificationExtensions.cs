//
//   Copyright 2021 Adam Burton (adz21c@gmail.com)
//   
//   Licensed under the Apache License, Version 2.0 (the "License")
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//  
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//  
//

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Periturf.Web;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.FieldLocation;

namespace Periturf
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class FieldLocationWebRequestCriteriaSpecificationExtensions
    {
        /// <summary>
        /// Test the value of the specified header.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="headerName">Name of the header.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<StringValues> Header<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator, string headerName)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new HeaderWebRequestCriteriaSpecification<TWebRequestEvent>(headerName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the value of the specified cookie.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<string> Cookie<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator, string cookieName)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new CookieWebRequestCriteriaSpecification<TWebRequestEvent>(cookieName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test whether this request is HTTPS.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<bool> IsHttps<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, bool >(x => x.Request.IsHttps);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Tests the web request protocol scheme.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<string> Scheme<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, string >(x => x.Request.Scheme);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request content type.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<string> ContentType<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, string>(x => x.Request.ContentType);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request content length.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<long?> ContentLength<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, long?>(x => x.Request.ContentLength);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request HTTP method.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<string> Method<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, string>(x => x.Request.Method);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request host header.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<HostString> Host<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, HostString>(x => x.Request.Host);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request path.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<PathString> Path<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, PathString>(x => x.Request.Path);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request path base.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<PathString> PathBase<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, PathString>(x => x.Request.PathBase);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request protocol.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<string> Protocol<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, string>(x => x.Request.Protocol);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request query string.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<QueryString> QueryString<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<TWebRequestEvent, QueryString>(x => x.Request.QueryString);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Tests a web request query string parameter.
        /// </summary>
        /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="queryName">Parameter name.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<StringValues> Query<TWebRequestEvent>(this IWebRequestCriteriaConfigurator<TWebRequestEvent> configurator, string queryName)
            where TWebRequestEvent : IWebRequestEvent
        {
            var spec = new QueryWebRequestCriteriaSpecification<TWebRequestEvent>(queryName);
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }

        /// <summary>
        /// Test the web request body field.
        /// </summary>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="valueLocator">The field value locator.</param>
        /// <returns></returns>
        public static IValueConditionBuilder<TValue> Body<TBody, TValue>(this IWebRequestCriteriaConfigurator<IWebRequestEvent<TBody>> configurator, Func<TBody, TValue> valueLocator)
        {
            var spec = new SimplePropertyWebRequestCriteriaSpecification<IWebRequestEvent<TBody>, TValue>(x => valueLocator(x.Body));
            configurator.AddCriteriaSpecification(spec);
            return spec;
        }
    }
}
