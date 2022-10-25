using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AM;
using PX.Objects.AM.Attributes;
using PX.Objects.CS;
using PX.Objects.IN;

namespace LUMCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMProductionScrap")]
    public class LUMProductionScrap : IBqlTable
    {
        public class PK : PrimaryKeyOf<LUMProductionScrap>.By<scrapID>
        {
            public static LUMProductionScrap Find(PXGraph graph, string scrapID) => FindBy(graph, scrapID);
        }

        #region ScrapID
        [PXDefault("<NEW>")]
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        //[AutoNumber(typeof("SCRAPID"), typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Scrap ID", Required = true)]
        [PXSelector(typeof(Search<LUMProductionScrap.scrapID>),
                    typeof(LUMProductionScrap.trandate),
                    typeof(LUMProductionScrap.transDescription))]
        public virtual string ScrapID { get; set; }
        public abstract class scrapID : PX.Data.BQL.BqlString.Field<scrapID> { }
        #endregion

        #region Trandate
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Trandate")]
        public virtual DateTime? Trandate { get; set; }
        public abstract class trandate : PX.Data.BQL.BqlDateTime.Field<trandate> { }
        #endregion

        #region TransDescription
        [PXDBString(1024)]
        [PXUIField(DisplayName = "Description")]
        public virtual string TransDescription { get; set; }
        public abstract class transDescription : PX.Data.BQL.BqlString.Field<transDescription> { }
        #endregion

        #region ProdOrderType
        [PXDBString(2, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Production OrderType", Required = true, Enabled = false)]
        [PXDefault(typeof(SelectFrom<AMProdItem>
                         .Where<AMProdItem.prodOrdID.IsEqual<LUMProductionScrap.prodOrderID.FromCurrent>>
                         .SearchFor<AMProdItem.orderType>))]
        [PXRestrictor(typeof(Where<AMOrderType.active, Equal<True>>), PX.Objects.SO.Messages.OrderTypeInactive)]
        public virtual string ProdOrderType { get; set; }
        public abstract class prodOrderType : PX.Data.BQL.BqlString.Field<prodOrderType> { }
        #endregion

        #region ProdOrderID
        [PXDefault]
        [PXUIField(DisplayName = "Prod Order ID", Required = true)]
        [ProductionNbr]
        [PXSelector(typeof(Search<AMProdItem.prodOrdID>))]
        [PX.Data.EP.PXFieldDescription]
        public virtual string ProdOrderID { get; set; }
        public abstract class prodOrderID : PX.Data.BQL.BqlString.Field<prodOrderID> { }
        #endregion

        #region Department
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXStringList]
        [PXUIField(DisplayName = "Department")]
        public virtual string Department { get; set; }
        public abstract class department : PX.Data.BQL.BqlString.Field<department> { }
        #endregion

        #region ProdLine
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXStringList]
        [PXUIField(DisplayName = "Prod Line")]
        public virtual string ProdLine { get; set; }
        public abstract class prodLine : PX.Data.BQL.BqlString.Field<prodLine> { }
        #endregion

        #region Reason
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXStringList]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Reason")]
        public virtual string Reason { get; set; }
        public abstract class reason : PX.Data.BQL.BqlString.Field<reason> { }
        #endregion

        #region Confirmed
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Confirmed")]
        public virtual bool? Confirmed { get; set; }
        public abstract class confirmed : PX.Data.BQL.BqlBool.Field<confirmed> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
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

    public class SCRAPSequenceAttr : PX.Data.BQL.BqlString.Constant<SCRAPSequenceAttr>
    {
        public SCRAPSequenceAttr() : base("SCRAPID") { }
    }
}