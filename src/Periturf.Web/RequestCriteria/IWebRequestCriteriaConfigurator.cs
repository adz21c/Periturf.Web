using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria
{
    public interface IWebRequestCriteriaConfigurator
    {
        IWebRequestCriteriaConfigurator Not();

        void AddCriteriaSpecification(IWebRequestCriteriaSpecification spec);
    }
}
