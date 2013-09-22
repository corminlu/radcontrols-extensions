<%@ Page Language="C#" MasterPageFile="~/demos/MasterPage.master" AutoEventWireup="true" CodeFile="RadGridExtractValueDemo.aspx.cs" Inherits="Cormin.RadControlsExtensions.demos_RadGridExtractValueDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" Runat="Server">
  <telerik:RadAjaxManager runat="server" ID="m1" />
  <telerik:RadGrid ID="rg" runat="server" AllowMultiRowSelection="true" AutoGenerateColumns="false" OnNeedDataSource="rg_NeedDataSource" OnDeleteCommand="rg_DeleteCommand" OnItemCommand="rg_ItemCommand">
    <ClientSettings>
      <Selecting AllowRowSelect="true" />
    </ClientSettings>
    <MasterTableView CommandItemDisplay="TopAndBottom" DataKeyNames="Id">
      <CommandItemTemplate>
        <asp:Button runat="server" Text="获取选中" CommandName="GetSelected" />
      </CommandItemTemplate>
      <Columns>
        <telerik:GridClientSelectColumn />
        <telerik:GridBoundColumn HeaderText="Text" DataField="Text" />
        <telerik:GridBoundColumn HeaderText="Value" DataField="Value" />
        <telerik:GridDateTimeColumn HeaderText="Date" DataField="Date" />
        <telerik:GridTemplateColumn UniqueName="Other" HeaderText="Other">
          <ItemTemplate>
            <%#Eval("Other") %>
          </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn HeaderText="Other2">
          <ItemTemplate>
            <asp:Literal runat="server" ID="Other2" Text='<%#Eval("Other2") %>' />            
          </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn HeaderText="操作">
          <ItemTemplate>
            <asp:Button runat="server" Text="操作1" CommandName="Command1" />
            <asp:Button runat="server" Text="删除" CommandName="Delete" />
          </ItemTemplate>
        </telerik:GridTemplateColumn>
      </Columns>
    </MasterTableView>
  </telerik:RadGrid>
  <asp:PlaceHolder runat="server" ID="phResult">
    <asp:PlaceHolder runat="server" ID="phResult1">
    <fieldset>
      <legend>Id: <asp:Literal runat="server" ID="liId" /></legend>
      Text: <asp:Literal runat="server" ID="liText" /><br />
      Value: <asp:Literal runat="server" ID="liValue" /><br />
      Date: <asp:Literal runat="server" ID="liDate" /><br />
      Other: <asp:Literal runat="server" ID="liOther" /><br />
      Other2: <asp:Literal runat="server" ID="liOther2" /><br />
    </fieldset>
    </asp:PlaceHolder><br />
    <asp:Literal runat="server" ID="liSelected" />
  </asp:PlaceHolder>
</asp:Content>