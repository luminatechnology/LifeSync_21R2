using PX.Data;
using PX.Objects.AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.AM 
{
    public class AMWrkMatlExt: PXCacheExtension<AMWrkMatl>
    {
        [PXDBDecimal]
        [PXUIField(DisplayName = "Wizard1 Input ProdQty(TempSave)", Visible = false, Enabled = false)]
        public virtual decimal? UsrTempProdQty { get; set; }
        public abstract class usrTempProdQty : PX.Data.BQL.BqlDecimal.Field<usrTempProdQty> { }
    }
}
