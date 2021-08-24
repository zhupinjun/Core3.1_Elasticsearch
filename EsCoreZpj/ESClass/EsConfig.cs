using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsCoreZpj.ESClass
{
    public class EsConfig : IOptions<EsConfig>
    {
        public List<string> Urls { get; set; }

        public EsConfig Value => this;
    }
}
