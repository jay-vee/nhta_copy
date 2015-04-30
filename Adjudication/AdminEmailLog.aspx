<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminEmailLog.aspx.vb" Inherits="Adjudication.AdminEmailLog" Title="Email Log" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <style>
        .form-control {
            display: inline-block;
        }

        td {
            vertical-align: middle;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="UpdatePanelMain">
        <ContentTemplate>
            <table class="TableSpacing">
                <tr>
                    <td valign="top" align="left" width="100%" colspan="3">
                        <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align:left;">
                            <tr align="center">
                                <td colspan="3">
                                    <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label><asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" ForeColor="SeaGreen">Email Successfully Sent</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align: left;">
                                        <tr>
                                            <td style="width: 10%; text-align: right;">Email(s) From:
                                            </td>
                                            <td style="width: 30%;">
                                                <asp:TextBox ID="txtEmailFrom" Width="80%" runat="server" CssClass="form-control"></asp:TextBox>*
                                                <asp:ImageButton ID="ibtnClear_EmailFrom" runat="server" ImageUrl="~/Images/Icon_Clear_18px.png" ToolTip="Clear the Email From textbox" />
                                            </td>
                                            <td style="width: 15%; text-align:right;">Sent Date Range:
                                            </td>
                                            <td style="width: 15%">
                                                <cc1:CalendarExtender ID="CalendarExtender_txtStartDate" runat="server" CssClass="AjaxCalendar" PopupButtonID="imgCalendarIcon_txtStartDate" PopupPosition="BottomLeft" TargetControlID="txtStartDate" />
                                                <asp:TextBox ID="txtStartDate" runat="server" Width="100px" CssClass="form-control"></asp:TextBox>
                                                <asp:Image ID="imgCalendarIcon_txtStartDate" runat="server" ImageUrl="~/Images/Calendar_16px.png" ToolTip="Select START date from Calendar" CssClass="ImageButton" ImageAlign="Bottom" />
                                            </td>
                                            <td style="width: 15%">
                                                <cc1:CalendarExtender ID="CalendarExtender_txtEndDate" runat="server" CssClass="AjaxCalendar" PopupButtonID="imgCalendarIcon_txtEndDate" PopupPosition="BottomLeft" TargetControlID="txtEndDate" />
                                                <asp:TextBox ID="txtEndDate" runat="server" Width="100px" CssClass="form-control"></asp:TextBox>
                                                <asp:Image ID="imgCalendarIcon_txtEndDate" runat="server" ImageUrl="~/Images/Calendar_16px.png" ToolTip="Select END date from Calendar" CssClass="ImageButton" ImageAlign="Bottom" />
                                            </td>
                                            <td style="width: 10%" align="right">
                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-gold" Text="Search"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="right">Email Sent To:
                                            </td>
                                            <td style="width: 30%">
                                                <asp:TextBox ID="txtEmailTo" Style="width: 80%;" runat="server" CssClass="form-control"></asp:TextBox>*
                                                <asp:ImageButton ID="ibtnClear_EmailTo" runat="server" ImageUrl="~/Images/Icon_Clear_18px.png" ToolTip="Clear the Email To textbox" />
                                            </td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 10%; text-align:right;">
                                                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-gold" Text="Reset"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <tr>
                                    <td valign="top">
                                        <asp:GridView ID="gvMain" runat="server" Width="100%" CellPadding="2" CellSpacing="2" GridLines="Both" BorderColor="#000000" BorderStyle="Groove" HorizontalAlign="Left" AllowSorting="True" ShowFooter="False" AutoGenerateColumns="False" DataKeyNames="PK_EmailLogID" ShowHeader="False" AllowPaging="false" BorderWidth="1px" OnRowCommand="gvMain_RowCommand" OnRowEditing="gvMain_RowEditing">
                                            <AlternatingRowStyle HorizontalAlign="Left" BackColor="#d8dee2"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <table width="100%" cellpadding="1" cellspacing="1" border="1">
                                                            <tr>
                                                                <td width="100">Initiated By:
                                                                </td>
                                                                <td width="325px">
                                                                    <b>
                                                                        <%#Eval("LastUpdateByName")%>
                                                                    </b>
                                                                </td>
                                                                <td width="35px" align="right">
                                                                    <asp:ImageButton ID="ibtnEdit" CommandName="Edit" CommandArgument='<%# Eval("PK_EmailLogID")%>' runat="server" ImageAlign="Bottom" ImageUrl="~/Images/Icon_Reload_35px.png" Width="35PX" Height="35px" ToolTip="Resend this email" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100">From Address:
                                                                </td>
                                                                <td width="560px" colspan="2">
                                                                    <font color="Blue"><b>
                                                                        <%#Eval("EmailFrom")%></b></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100">Sent Date:
                                                                </td>
                                                                <td width="560px" colspan="2">
                                                                    <%# Eval("LastUpdateByDate") %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100">Sent To:
                                                                </td>
                                                                <td width="560px" colspan="2">
                                                                    <font color="Blue">
                                                                        <%# Eval("EmailTo") %></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100">
                                                                    <i>Notes:</i>
                                                                </td>
                                                                <td width="560px" colspan="2">
                                                                    <i>
                                                                        <%# Eval("EmailTechnicalNotes") %></i>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100">Subject:
                                                                </td>
                                                                <td width="560px" colspan="2">
                                                                    <%# Eval("EmailSubject") %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" colspan="3">Body:
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" colspan="3">
                                                                    <%# Eval("EmailBody") %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
