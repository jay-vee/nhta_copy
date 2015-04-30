<%@ Page Language="vb" Title="Message to System Administrator" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="MessageToAdmin.aspx.vb" Inherits="Adjudication.MessageToAdmin" ValidateRequest="false" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <style>
        .form-control
        {
            display: inline-block;
        }

        td
        {
            vertical-align: middle;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="UpdatePanelMain">
        <ContentTemplate>

            <div class="TextCenter">
                <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label>
                <asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" ForeColor="SeaGreen">Message Successfully Logged and Emailed</asp:Label>
            </div>
            <div class="TextCenter">
                <asp:Panel ID="pnlUserData" runat="server">

                    <div class="panel panel-dark">
                        <div class="panel-heading">Message to Administrators</div>
                        <div class="panel-body">
                            <table class="TableSpacing">
                                <tr>
                                    <td style="text-align: right; width: 40%">From:
                                    </td>
                                    <td style="text-align: left; width: 60%">
                                        <asp:Label ID="lblFullName" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 40%">Email Address:
                                    </td>
                                    <td style="text-align: left; width: 60%">
                                        <asp:Label ID="lblEmailPrimary" runat="server" ForeColor="Gray"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 40%">Phone Number:
                                    </td>
                                    <td style="text-align: left; width: 60%">
                                        <asp:Label ID="lblPhonePrimary" runat="server" ForeColor="Gray"></asp:Label>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td style="text-align: right; width: 40%">To:
                                    </td>
                                    <td style="text-align: left; width: 60%">
                                        <b>System Administrator</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">Subject:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtSubject" Font-Bold="True" runat="server" CssClass="form-control" Width="100%">Message to NHTA Administrators from </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right">Message:
                                    </td>
                                    <td align="left"></td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <FTB:FreeTextBox ID="ftbMessage" runat="Server" Width="100%" OnSaveClick="btnUpdate_Click" ToolbarLayout="FontForeColorsMenu, FontBackColorsMenu, ; Bold, Italic, Underline, Strikethrough, Superscript, Subscript,; JustifyLeft, JustifyRight, JustifyCenter, JustifyFull| BulletedList,NumberedList, Indent, Outdent,; Cut, Copy, Paste, Undo, Redo, RemoveFormat,; InsertRule, InsertDate, SelectAll" GutterBackColor="whitesmoke" EnableHtmlMode="false" DesignModeCss="~\StyleSheets\NHTA.css">
                                        </FTB:FreeTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>

                <div class="TextCenter">
                    <asp:Button ID="btnUpdate" runat="server"  CssClass="btn btn-gold" Text="Send Message"></asp:Button>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script id="scptDocumentReady" type="text/javascript">
        //=========== document.ready ===========
        $(document).ready(function () {

        });
    </script>

</asp:Content>
