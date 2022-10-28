using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AM;
using PX.Objects.AM.Attributes;
using PX.Objects.IN;

namespace LUMCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMProductionScrapDetails")]
    public class LUMProductionScrapDetails : IBqlTable
    {

        #region FK
        public class FK
        {
            public class ID : LUMProductionScrap.PK.ForeignKeyOf<LUMProductionScrapDetails>.By<scrapID> { }
        }
        #endregion

        #region ScrapID
        [PXDefault(typeof(LUMProductionScrap.scrapID))]
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXParent(typeof(FK.ID))]
        [PXUIField(DisplayName = "Scrap ID", Visible = false, Enabled = false)]
        public virtual string ScrapID { get; set; }
        public abstract class scrapID : PX.Data.BQL.BqlString.Field<scrapID> { }
        #endregion

        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Line Nbr", Visible = false, Enabled = false)]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion

        #region InventoryID
        [StockItem(Visibility = PXUIVisibility.SelectorVisible)]
        [PXDefault]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region InventoryDescr
        [PXString]
        [PXDefault(typeof(Search<InventoryItem.descr, Where<InventoryItem.inventoryID, Equal<Current<LUMProductionScrapDetails.inventoryID>>>>))]
        [PXUIField(DisplayName = "Inventory Description", Enabled = false)]
        public virtual string InventoryDescr { get; set; }
        public abstract class inventoryDescr : PX.Data.BQL.BqlString.Field<inventoryDescr> { }
        #endregion

        #region UOM
        [PXString]
        [PXDefault(typeof(Search<InventoryItem.baseUnit, Where<InventoryItem.inventoryID, Equal<Current<LUMProductionScrapDetails.inventoryID>>>>))]
        [PXUIField(DisplayName = "UOM", Enabled = false)]
        public virtual string UOM { get; set; }
        public abstract class uOM : PX.Data.BQL.BqlString.Field<uOM> { }
        #endregion

        #region Qty
        [PXDBDecimal(3)]
        [PXUIField(DisplayName = "Qty")]
        public virtual Decimal? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
        #endregion

        #region SiteID
        [PXDBInt]
        [PXSelector(typeof(Search<INSite.siteID>),
                    typeof(INSite.siteCD),
                    typeof(INSite.descr),
                    SubstituteKey = typeof(INSite.siteCD),
                    DescriptionField = typeof(INSite.descr))]
        [PXUIField(DisplayName = "Warehouse")]
        public virtual Int32? SiteID { get; set; }
        public abstract class siteID : PX.Data.BQL.BqlInt.Field<siteID> { }
        #endregion

        #region SerialNbr
        [PXDBString(100)]
        [PXUIField(DisplayName = "Lot/Serial Nbr")]
        public virtual string SerialNbr { get; set; }
        public abstract class serialNbr : PX.Data.BQL.BqlString.Field<serialNbr> { }
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
}