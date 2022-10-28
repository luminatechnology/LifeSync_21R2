<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM201002.aspx.cs" Inherits="Page_LM201002" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMCustomization.Graph.LUMProductionScrapMaint" PrimaryView="Document">
        <CallbackCommands>
            <%--<px:PXDSCallbackCommand Name="Report" Visible="True" CommitChanges="true" StartNewGroup="true"></px:PXDSCallbackCommand>--%>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" Caption="Production Scrap" DataMember="Document" FilesIndicator="True"
        NoteIndicator="True" LinkIndicator="True" NotifyIndicator="True" DefaultControlID="edScrapID" TabIndex="100">
        <CallbackCommands>
            <Save PostData="Self" />
        </CallbackCommands>
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="true" LabelsWidth="S" ControlSize="S" />
            <px:PXSelector runat="server" ID="edScrapID" DataField="ScrapID" CommitChanges="true" FilterByAllFields="True" Width="150px"></px:PXSelector>
            <px:PXDateTimeEdit runat="server" ID="edTrandate" DataField="Trandate"></px:PXDateTimeEdit>
            <px:PXSelector runat="server" ID="ProdOrderID" DataField="ProdOrderID" CommitChanges="true" Width="150px"></px:PXSelector>
            <px:PXTextEdit runat="server" ID="ProdOrderType" DataField="ProdOrderType"></px:PXTextEdit>
            <px:PXLayoutRule runat="server" ColumnSpan="3" ControlSize="XM" />
            <px:PXTextEdit runat="server" ID="edTransDescription" DataField="TransDescription" TextMode="MultiLine"></px:PXTextEdit>
            <px:PXLayoutRule runat="server" StartColumn="true" LabelsWidth="S" ControlSize="S" />
            <px:PXDropDown ID="edDepartment" runat="server" DataField="Department"></px:PXDropDown>
            <px:PXDropDown ID="edProdLine" runat="server" DataField="ProdLine"></px:PXDropDown>
            <px:PXDropDown ID="edReason" runat="server" DataField="Reason"></px:PXDropDown>
            <px:PXCheckBox ID="edConfirmed" runat="server" DataField="Confirmed"></px:PXCheckBox>
        </Template>
    </px:PXFormView>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="server">
    <px:PXGrid ID="edScrapDetails" SkinID="Details" runat="server" Width="100%" Height="400px" DataSourceID="ds" ActionsPosition="Top" BorderWidth="0px" SyncPosition="true">
        <Levels>
            <px:PXGridLevel DataMember="Transactions">
                <Mode InitNewRow="true" />
                <RowTemplate>
                    <px:PXSelector ID="edInventoryID" runat="server" DataField="InventoryID" CommitChanges="true"></px:PXSelector>
                    <px:PXSelector ID="edSiteID" runat="server" DataField="SiteID"></px:PXSelector>
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn DataField="ScrapID" DisplayFormat="CCCCCCCCCCCCCCCCCCCC" AutoCallBack="True" AllowDragDrop="true" Width="150px" />
                    <px:PXGridColumn DataField="LineNbr" Width="100px" />
                    <px:PXGridColumn DataField="InventoryID" Width="150px" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="InventoryDescr" Width="200px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UOM" Width="100px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Qty" Width="130px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SiteID" Width="130px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SerialNbr" Width="170px"></px:PXGridColumn>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <Mode InitNewRow="True" />
    </px:PXGrid>
</asp:Content>

