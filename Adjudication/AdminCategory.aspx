<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminCategory.aspx.vb" Inherits="Adjudication.AdminCategory" Title="Category" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table class="TableSpacing">
        <tr align="center">
            <td colspan="2">
                <asp:Label ID="lblErrors" runat="server"   ForeColor="red" Visible="False"></asp:Label><asp:Label ID="lblSucessfulUpdate" runat="server"   ForeColor="SeaGreen" Visible="False">Update Successful!</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="fontScoringHeader" align="center" colspan="2">
                Category
                <asp:TextBox ID="txtPK_CategoryID" runat="server" Visible="False" Width="64px"   BorderStyle="Dotted"  >0</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                Category Name:
            </td>
            <td align="left" width="460px">
                <asp:TextBox ID="txtCategoryName"   Width="400px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                Display Order:
            </td>
            <td align="left" width="460px">
                <asp:TextBox ID="txtDisplayOrder"   Width="40px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                'Score' Field Name:
            </td>
            <td align="left" width="460px">
                <asp:TextBox ID="txtScoreFieldName"   Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                'Comment' Field Name:
            </td>
            <td align="left" width="460px">
                <asp:TextBox ID="txtCommentFieldName"   Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                'Nomination' Field Name:
            </td>
            <td align="left" width="460px">
                <asp:TextBox ID="txtNominationFieldName"   Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                'Role' Field Name:
            </td>
            <td align="left" width="460px">
                <asp:TextBox ID="txtRoleFieldName"   Width="200px" runat="server"></asp:TextBox>&nbsp;<span class="FontSmaller">(only for Acting Catories)</span>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                'Gender' Field Name:
            </td>
            <td align="left" width="460px">
                <asp:TextBox ID="txtGenderFieldName"   Width="200px" runat="server"></asp:TextBox>&nbsp;<span class="FontSmaller">(only for some&nbsp;Acting Catories)</span>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                Category Active:
                <td align="left" width="460px">
                    <asp:DropDownList ID="ddlActiveCategory" runat="server"  >
                        <asp:ListItem Value="1" Selected="True">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="separatorBlack">
                    &nbsp;</div>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                Category Scoring Criteria:</span>
            </td>
            <td align="right">
                <asp:ImageButton ID="ibtnExpand" runat="server" ToolTip="Expand/Contract" ImageUrl="Images/PlusIcon.jpg" /></asp:imagebutton>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <FTB:FreeTextBox ID="ftbScoringCriteria" runat="Server" width="100%" Height="300px" BackColor="PaleGoldenrod" OnSaveClick="btnUpdate_Click" ToolbarLayout="FontFacesMenu, FontSizesMenu, FontForeColorsMenu, FontBackColorsMenu,; Bold, Italic, Underline, Strikethrough, Superscript, Subscript,; JustifyLeft, JustifyRight, JustifyCenter, JustifyFull, BulletedList, NumberedList, Indent, Outdent| Cut, Copy, Paste, Undo, Redo, RemoveFormat,; CreateLink, Unlink, StyleMenu, SymbolsMenu,; InsertHtmlMenu, InsertRule, InsertDate, InsertTime, WordClean, InsertImage, InsertTable, EditTable, InsertTableRowBefore, InsertTableRowAfter, DeleteTableRow, InsertTableColumnBefore, InsertTableColumnAfter, DeleteTableColumn, ; SelectAll" GutterBackColor="whitesmoke"     DesignModeCss="~\StyleSheets\NHTA.css">
                </FTB:FreeTextBox>
            </td>
        </tr>
        <tr>
            <td class="fontScoringHeader" align="center" colspan="2">
                Administrative Information
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="separatorBlack">
                    &nbsp;</div>
            </td>
        </tr>
        <tr>
            <td align="right" width="400px">
                Last Updated By:
            </td>
            <td align="left" width="460px">
                <asp:Label ID="lblLastUpdateByName" runat="server"   ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                <asp:Label ID="lblLastUpdateByDate" runat="server"   ForeColor="Gray"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="separatorBlack">
                    &nbsp;</div>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnUpdate" runat="server" Width="150px"   Text="Save"></asp:Button>&nbsp;
                <asp:Button ID="btnCancel" runat="server" Width="150px"   Text="Cancel"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
