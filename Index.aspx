<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
    .auto-style1 {
        height: 453px;
        text-align: center;
    }
    .auto-style2 {
        font-size: xx-large;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="auto-style1">
    <strong>
    <asp:Label ID="Label1" runat="server" CssClass="auto-style2" Text="ברוך הבא למערכת ההרשמה של גימנסיית יצחק נבון למועדי ב'"></asp:Label>
    </strong>
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" CssClass="auto-style2" Text="אנא התחבר עם תעודת הזהות שלך מעלה כדי להירשם למועדי ב'"></asp:Label>
    <br />
</div>
</asp:Content>

