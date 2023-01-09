using System;
using PX.Data;
using PX.Objects.IN;

namespace LUMCustomizations.DAC
{
    [Serializable]
    [PXCacheName("LUMStdCostUpdateLog")]
    public class LUMStdCostUpdateLog : IBqlTable
    {
        #region SequenceNumber
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Sequence Number")]
        public virtual int? SequenceNumber { get; set; }
        public abstract class sequenceNumber : PX.Data.BQL.BqlInt.Field<sequenceNumber> { }
        #endregion

        #region InventroyID
        [Inventory(DirtyRead = true, DisplayName = "Inventory ID")]
        public virtual int? InventroyID { get; set; }
        public abstract class inventroyID : PX.Data.BQL.BqlInt.Field<inventroyID> { }
        #endregion

        #region Siteid
        [Site]
        public virtual int? Siteid { get; set; }
        public abstract class siteid : PX.Data.BQL.BqlInt.Field<siteid> { }
        #endregion

        #region StdCostDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Std Cost Date")]
        public virtual DateTime? StdCostDate { get; set; }
        public abstract class stdCostDate : PX.Data.BQL.BqlDateTime.Field<stdCostDate> { }
        #endregion

        #region StdCost
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Std Cost")]
        public virtual Decimal? StdCost { get; set; }
        public abstract class stdCost : PX.Data.BQL.BqlDecimal.Field<stdCost> { }
        #endregion

        #region LastStdCost
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Last Std Cost")]
        public virtual Decimal? LastStdCost { get; set; }
        public abstract class lastStdCost : PX.Data.BQL.BqlDecimal.Field<lastStdCost> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}