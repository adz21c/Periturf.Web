using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.Configuration.Responses
{
    public interface IWebResponseConfigurable<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        /// <summary>
        /// Adds a web response specification.
        /// </summary>
        /// <param name="spec">The spec.</param>
        void AddWebResponseSpecification(IWebResponseSpecification<TWebRequestEvent> spec);
    }
}
