using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Enrichers
{
    public class DynamicEnricher : ILogEventEnricher
    {

        private string _propertyName;
        public Func<string> Fx { get; set; }
        
        public DynamicEnricher( Func<string> fx,string propertyName = "DynamicProperty") : base()
        {
            Fx = fx;
            _propertyName = propertyName;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {

            var prop = propertyFactory.CreateProperty(_propertyName, Fx());

            logEvent.AddPropertyIfAbsent(prop);


        }
    }
}
