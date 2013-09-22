<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
  CodeFile="RadComboBoxClientCascadeDemo.aspx.cs" Inherits="Cormin.RadControlsExtensions.demos_RadComboBoxClientCascadeDemo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script>
  /**
   * RadComboBox 联动查询
   */
  function cascadeCombo(id) {
    if (!(this instanceof cascadeCombo))
      return new cascadeCombo(id);
    this.combo = $find(id);
  }
  cascadeCombo.prototype = {
    bindRequested: function() {
      var me = this;
      this.combo.add_itemsRequested(function(s, e) {
        var items = s.get_items(), item;
        if (items.get_count() > 0) {
          // pre-select the first item
          item = items.getItem(0);
          me.clearSelection();
          me.setValue(item);
          item.highlight();
          me.raiseChanged(item);
        }

        if (!s.get_dropDownVisible())
          s.showDropDown();
      });
    },
    request: function(val) {
      this.combo.requestItems(val, false);
    },
    setValue: function(item) {
      var s = this.combo;
      s.trackChanges();
      s.set_text(item.get_text());
      s.set_value(item.get_value());
      s.commitChanges();
    },
    setText: function(text) {
      this.combo.set_text(text);
    },
    clearSelection: function() {
      this.combo.clearSelection();
      this.sub && this.sub.clearSelection();
    },
    clearItems: function() {
      this.setText(" ");
      this.clearSelection();
      if (this.defaultItem) {
        var items = this.combo.get_items(), count = items.get_count();
        if (count > 0) {
          while (--count > 0) {
            items.removeAt(1);
          }
          items.getItem(0).select();
        }
      } else
        this.combo.clearItems();
      this.sub && this.sub.clearItems();
    },
    defaultItem: function(val) {
      this.defItem = val;
      return this;
    },
    raiseChanged: function(item) {
      var sub = this.sub;
      if (!sub || !item || this.oldVal === item.get_value()) return;
      sub.clearSelection();
      sub.setText("加载中...");

      if (!this.defItem || this.defItem && item.get_index() > 0) {
        // this will fire the ItemsRequested event of the
        // countries combobox passing the continentID as a parameter
        sub.request(item.get_value());
        //sub.raiseChanged();
      } else {
        sub.clearItems();
      }
      this.oldVal = item.get_value();
    },
    setSub: function(sub) {
      this.sub = sub;
      var me = this;
      this.combo.add_selectedIndexChanging(function(s, e) {
        var item = e.get_item();
        me.raiseChanged(item);
      });
      sub.bindRequested();
      return sub;
    }
  };
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
  <div id="qsfexWrapper">
    <telerik:RadComboBox ID="RadComboBox1" runat="server" />
    <telerik:RadComboBox ID="RadComboBox2" runat="server" EnableViewState="false" />
    <telerik:RadComboBox ID="RadComboBox3" runat="server" EnableViewState="false" />
    <telerik:RadComboBox ID="RadComboBox4" runat="server" EnableViewState="false" />
    <telerik:RadComboBox ID="RadComboBox5" runat="server" EnableViewState="false" />    
    <br />
    <span class="Button_Submit">
      <telerik:RadButton ID="Button1" runat="server" Text="Explore" OnClick="Button1_Click" />
    </span>
    <asp:Label runat="server" ID="Literal1" CssClass="Label_Result" />
  </div>
</asp:Content>
