using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelDataLayer.Extensions;

namespace ModelDataLayer.ModelAdditional
{
    public partial class Governmentprogram: IGuidIdentifiedEntity
    {
        public Guid Id { get; set; }
    }
}
