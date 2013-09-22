<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
  CodeFile="RadTreeViewInRadComboDemo.aspx.cs" Inherits="Cormin.RadControlsExtensions.demos_RadTreeViewInRadComboDemo" %>
<%@ Import Namespace="Cormin.RadControlsExtensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script>
  /**
   * RadComboBox下拉RadTreeView
   */
  function treeCombo(comboId, treeId, multiMode) {
    if (!(this instanceof treeCombo))
      return new treeCombo(comboId, treeId, multiMode);

    this.combo = $find(comboId);
    this.tree = $find(treeId);
    this.multiMode = multiMode;
    this.emptyMessage = this.combo.get_emptyMessage() || "请选择...";
    var me = this;

    /*防止点击到空白地方时，隐藏下拉框*/
    $telerik.$(".rcbTemplate", this.combo.get_dropDownElement()).delegate("div, ul, li", "click", function(e) {
      e.stopPropagation();
      e.preventDefault();
      return false;
    });

    this.combo.add_onClientBlur(function(s, e) {
      me.showSelected();
    });

    /*this.combo.add_dropDownOpened(function(s, e) {
      var selectedNode = me.tree.get_selectedNode();
      if(selectedNode) {
        setTimeout(function() { selectedNode.get_element().scrollIntoView(); }, 10);
        selectedNode.get_element().scrollIntoView();
      }
    });*/

    this.tree.add_nodeClicking(function(s, e) {
      var node = e.get_node();
      if (!node.get_enabled()) {
        e.set_cancel(true);
        return;
      }
      me.showSelected(node);
    });

    this.showMsg();
  }
  treeCombo.prototype = {
    init: function() {
      var tree = this.tree, node = tree.get_selectedNode() || tree.get_nodes().getNode(0);
      this.showSelected(node);
    },
    showMsg: function() {
      this.combo.set_emptyMessage(this.emptyMessage);
    },
    getCheckedNodes: function() {
      var tarr = [],
        varr = [],
        ckNodes = this.tree.get_checkedNodes(),
        tmp;
      for (var i = 0, l = ckNodes.length; i < l; i++) {
        tmp = ckNodes[i];
        if (tmp.get_checkable()) {
          tarr.push(tmp.get_text());
          varr.push(tmp.get_value());
        }
      }
      return { text: tarr.join(','), value: varr.join(',') };
    },
    showSelected: function(node) {
      if (!this.multiMode && !node) {
        this.showMsg();
        return;
      }
      var chkNode = this.multiMode ? this.getCheckedNodes() : { text: node.get_text(), value: node.get_value() };
      this.combo.set_text(chkNode.text);
      this.combo.set_value(chkNode.value);
      this.combo.trackChanges();
      var item0 = this.combo.get_items().getItem(0);
      item0.set_text(chkNode.text);
      item0.set_value(chkNode.value);
      this.combo.commitChanges();
      this.combo.hideDropDown();
      if (!chkNode.text) this.showMsg();
    }
  };
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
  <telerik:RadAjaxManager Id="m1" runat="server" />
  <script>
    Sys.Application.add_load(function() {
      treeCombo("<%=RadComboBox1.ClientID%>", "<%=RadComboBox1.FindNestedTreeView().ClientID%>", false);
    });
  </script>
  <telerik:RadComboBox ID="RadComboBox1" runat="server" EmptyMessage="请选择类别" Width="250" Height="200">
    <ItemTemplate>
      <telerik:RadTreeView runat="server" ID="RadTreeView2" DataTextField="key" DataValueField="value" />
    </ItemTemplate>
    <Items>
      <telerik:RadComboBoxItem Text="" />
    </Items>
  </telerik:RadComboBox>
  <asp:PlaceHolder runat="server" ID="phSource">
    <asp:Button runat="server" ID="Button1" Text="ShowValue" OnClick="Button1_Click" />
  </asp:PlaceHolder>  
  <asp:PlaceHolder runat="server" ID="phResult">
    <br />
    Text:<asp:Literal runat="server" ID="Literal1" /><br />
    Value:<asp:Literal runat="server" ID="Literal2" />
  </asp:PlaceHolder>
  
</asp:Content>
