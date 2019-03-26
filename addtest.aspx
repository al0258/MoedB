<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="addtest.aspx.cs" Inherits="addtest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 500px">

        <table class="auto-style1">
            <tr>
                <td rowspan="2">
                    <center>
                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="276px" NextPrevFormat="ShortMonth" Width="422px" OnSelectionChanged="Calendar1_SelectionChanged">
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                        <DayStyle BackColor="#CCCCCC" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                        <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" ForeColor="White" Height="12pt" />
                        <TodayDayStyle BackColor="#999999" ForeColor="White" />
                    </asp:Calendar>
                        </center>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Style="font-weight: 700; font-size: xx-large" Text="הוסף זמן (כמו 14:00 או 07:00)"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="TextBox1" runat="server" Height="44px" Width="200px"></asp:TextBox>
                </td>
                <td>&nbsp;
                    <asp:Label ID="Label2" runat="server" Style="font-weight: 700; font-size: x-large" Text="הכנס מקצוע"></asp:Label>
                    &nbsp;&nbsp;
                    <br />
                    <br />
                    &nbsp;<asp:TextBox ID="TextBox2" runat="server" Height="38px" Width="146px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Height="36px" OnClick="Button1_Click" Text="הוסף מבחן" Width="146px" />
                </td>
            </tr>
        </table>

        <asp:Label ID="lbdate" runat="server" style="font-size: xx-large; font-weight: 700" Visible="False"></asp:Label>

    </div>
</asp:Content>

