using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.IN
{
    public class INSetupExt : PXCacheExtension<INSetup>
    {
        #region UsrValidStandardCostInPurchase
        [PXDBBool]
        [PXUIField(DisplayName = "Validate Standard Cost in Purchase")]
        public virtual bool? UsrValidStandardCostInPurchase { get; set; }
        public abstract class usrValidStandardCostInPurchase : PX.Data.BQL.BqlBool.Field<usrValidStandardCostInPurchase> { }
        #endregion

        #region UsrValidStandardCostInMaterials
        [PXDBBool]
        [PXUIField(DisplayName = "Validate Standard Cost in Materials")]
        public virtual bool? UsrValidStandardCostInMaterials { get; set; }
        public abstract class usrValidStandardCostInMaterials : PX.Data.BQL.BqlBool.Field<usrValidStandardCostInMaterials> { }
        #endregion

        #region UsrValidStandardCostInMove
        [PXDBBool]
        [PXUIField(DisplayName = "Validate Standard Cost in Move")]
        public virtual bool? UsrValidStandardCostInMove { get; set; }
        public abstract class usrValidStandardCostInMove : PX.Data.BQL.BqlBool.Field<usrValidStandardCostInMove> { }
        #endregion
    }
}
