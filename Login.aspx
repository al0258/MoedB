<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <center>
    <div style="height: 188px; ">
        <table class="style1">
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" 
                        style="font-weight: 700; color: #3399FF; font-size: xx-large" Text="התחברות"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="tbid" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label2" runat="server" 
                        style="font-weight: 700; font-size: large; color: #FF9933; text-decoration: underline" 
                        Text="תעודת זהות"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="Button1" runat="server" BorderStyle="Double" 
                        onclick="Button1_Click" 
                        style="color: #666699; font-weight: 700; font-size: large; background-color: #FF9900" 
                        Text="התחבר" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lberror" runat="server" 
                        style="font-weight: 700; color: #CC3300; font-size: x-large" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
        </center>
</asp:Content>

