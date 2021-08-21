﻿/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.Configuration.Responses.Conditional
{
    class ConditionalResponseSpecification<TWebRequestEvent> : IWebResponseSpecification<TWebRequestEvent>, IConditionalResponseConfigurator<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        private readonly List<ConditionConditionalResponseSpecification<TWebRequestEvent>> _conditionSpecifications = new List<ConditionConditionalResponseSpecification<TWebRequestEvent>>();

        public void Condition(Action<IConditionConditionalResponseConfigurator<TWebRequestEvent>> config)
        {
            var spec = new ConditionConditionalResponseSpecification<TWebRequestEvent>();
            config(spec);
            _conditionSpecifications.Add(spec);
        }

        public Func<TWebRequestEvent, IWebResponse, CancellationToken, ValueTask> BuildResponseWriter()
        {
            var conditions = _conditionSpecifications.Select(x => x.Build()).ToList();

            return async (@event, response, ct) =>
            {
                foreach (var conditon in conditions)
                    if (conditon.Condition(@event))
                    {
                        await conditon.ResponseWriter(@event, response, ct);
                        return;
                    }

                response.StatusCode = HttpStatusCode.InternalServerError;
            };
        }
    }
}
