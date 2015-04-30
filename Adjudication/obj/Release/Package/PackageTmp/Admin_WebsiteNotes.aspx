<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Admin_WebsiteNotes.aspx.vb" Inherits="Adjudication.Admin_WebsiteNotes" Title="Website Notes & Messages" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
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
    <div style="text-align: center">
        <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label>
        <asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" ForeColor="SeaGreen" Font-Bold="True">Update Successful!</asp:Label>
    </div>

    <div class="panel panel-dark">
        <div class="panel-heading">Login Page Message</div>
        <div class="panel-body">
            <table style="width: 100%; border-spacing: 0px; border-collapse: separate;">
                <tr>
                    <td width="50%">
                        <asp:HiddenField ID="hiddenPrimaryKeyID" runat="server" />
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="ibtnMainpageApplicationDesc" runat="server" ImageUrl="Images/PlusIcon.jpg" ToolTip="Expand/Contract"></asp:ImageButton>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <FTB:FreeTextBox ID="ftbMainpageApplicationDesc" runat="Server" Width="100%" DesignModeCss="~\StyleSheets\NHTA.css" GutterBackColor="whitesmoke" ToolbarLayout="ParagraphMenu, FontFacesMenu, FontSizesMenu, FontForeColorsMenu,  FontForeColorPicker, FontBackColorsMenu, FontBackColorPicker, Bold, Italic, Underline, Strikethrough, Superscript, Subscript, InsertImageFromGallery, CreateLink, Unlink, | RemoveFormat, JustifyLeft, JustifyRight, JustifyCenter, JustifyFull, BulletedList,  NumberedList, Indent, Outdent, Cut, Copy, Paste, Delete, Undo, Redo, Print, Save, | ieSpellCheck, StyleMenu, SymbolsMenu, InsertHtmlMenu, InsertRule, InsertDate,  InsertTime, WordClean, InsertImage, InsertTable, EditTable, InsertTableRowBefore,  InsertTableRowAfter, DeleteTableRow, InsertTableColumnBefore, InsertTableColumnAfter,  DeleteTableColumn, InsertForm, InsertForm, InsertTextBox, InsertTextArea,  InsertRadioButton, InsertCheckBox, InsertDropDownList, InsertButton, InsertDiv, | InsertImageFromGallery, Preview, SelectAll, EditStyle" OnSaveClick="btnDEFAULTS_Update_Click" BackColor="PaleGoldenrod" Height="260px">
                        </FTB:FreeTextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div class="panel panel-dark">
        <div class="panel-heading">Main Menu Page Message</div>
        <div class="panel-body">
            <table style="width: 100%; border-spacing: 0px; border-collapse: separate;">
                <tr>
                    <td width="50%">&nbsp;</td>
                    <td align="right">
                        <asp:ImageButton ID="ibtnMainpageApplicationNotes" runat="server" ImageUrl="Images/PlusIcon.jpg" ToolTip="Expand/Contract"></asp:ImageButton>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <FTB:FreeTextBox ID="ftbMainpageApplicationNotes" runat="Server" Width="100%" DesignModeCss="~\StyleSheets\NHTA.css" GutterBackColor="whitesmoke" ToolbarLayout="ParagraphMenu, FontFacesMenu, FontSizesMenu, FontForeColorsMenu,  FontForeColorPicker, FontBackColorsMenu, FontBackColorPicker, Bold, Italic, Underline, Strikethrough, Superscript, Subscript, InsertImageFromGallery, CreateLink, Unlink, | RemoveFormat, JustifyLeft, JustifyRight, JustifyCenter, JustifyFull, BulletedList,  NumberedList, Indent, Outdent, Cut, Copy, Paste, Delete, Undo, Redo, Print, Save, | ieSpellCheck, StyleMenu, SymbolsMenu, InsertHtmlMenu, InsertRule, InsertDate,  InsertTime, WordClean, InsertImage, InsertTable, EditTable, InsertTableRowBefore,  InsertTableRowAfter, DeleteTableRow, InsertTableColumnBefore, InsertTableColumnAfter,  DeleteTableColumn, InsertForm, InsertForm, InsertTextBox, InsertTextArea,  InsertRadioButton, InsertCheckBox, InsertDropDownList, InsertButton, InsertDiv, | InsertImageFromGallery, Preview, SelectAll, EditStyle" OnSaveClick="btnDEFAULTS_Update_Click" BackColor="PaleGoldenrod" Height="260px">
                        </FTB:FreeTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 40%">Last Updated By:
                    </td>
                    <td align="left" style="width: 60%">
                        <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="separatorBlack" style="height: 6px">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="height: 40px;" valign="middle">
                        <asp:Button ID="btnDEFAULTS_Update" runat="server" Width="90px" Text="Update "></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
