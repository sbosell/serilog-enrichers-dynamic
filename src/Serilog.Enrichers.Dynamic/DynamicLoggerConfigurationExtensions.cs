// Copyright 2013-2016 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Serilog.Configuration;
using Serilog.Enrichers;
using Serilog.Events;

namespace Serilog
{
    /// <summary>
    /// Extends <see cref="LoggerConfiguration"/> to add enrichers for <see cref="System.Environment"/>.
    /// capabilities.
    /// </summary>
    public static class DynamicLoggerConfigurationExtensions
    {
        /// <summary>
        /// Enrich log events with a Function.
        /// </summary>
        /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration WithDynamicProperty(
          this LoggerEnrichmentConfiguration enrichmentConfiguration, string name, Func<string> valueProvider,  bool destructureObjects=false, bool enrichWhenNullOrEmptyString = false, LogEventLevel minimumLevel= LogEventLevel.Verbose)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new DynamicEnricher(name, valueProvider,destructureObjects, enrichWhenNullOrEmptyString, minimumLevel));
        }

      

    }
}