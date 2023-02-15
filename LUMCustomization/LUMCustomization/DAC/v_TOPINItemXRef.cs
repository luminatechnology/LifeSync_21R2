using System;
using PX.Data;

namespace LUMCustomizations.DAC
{
    [Serializable]
    [PXCacheName("v_TOPINItemXRef")]
    public class v_TOPINItemXRef : IBqlTable
    {
        #region Seq
        [PXDBLong()]
        [PXUIField(DisplayName = "Seq")]
        public virtual long? Seq { get; set; }
        public abstract class seq : PX.Data.BQL.BqlLong.Field<seq> { }
        #endregion

        #region InventoryID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region SubItemID
        [PXDBInt()]
        [PXUIField(DisplayName = "Sub Item ID")]
        public virtual int? SubItemID { get; set; }
        public abstract class subItemID : PX.Data.BQL.BqlInt.Field<subItemID> { }
        #endregion

        #region AlternateType
        [PXDBString(4, InputMask = "")]
        [PXUIField(DisplayName = "Alternate Type")]
        public virtual string AlternateType { get; set; }
        public abstract class alternateType : PX.Data.BQL.BqlString.Field<alternateType> { }
        #endregion

        #region BAccountID
        [PXDBInt()]
        [PXUIField(DisplayName = "BAccount ID")]
        public virtual int? BAccountID { get; set; }
        public abstract class bAccountID : PX.Data.BQL.BqlInt.Field<bAccountID> { }
        #endregion

        #region AlternateID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Alternate ID")]
        public virtual string AlternateID { get; set; }
        public abstract class alternateID : PX.Data.BQL.BqlString.Field<alternateID> { }
        #endregion

        #region Descr
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Descr")]
        public virtual string Descr { get; set; }
        public abstract class descr : PX.Data.BQL.BqlString.Field<descr> { }
        #endregion

        #region Uom
        [PXDBString(6, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Uom")]
        public virtual string Uom { get; set; }
        public abstract class uom : PX.Data.BQL.BqlString.Field<uom> { }
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