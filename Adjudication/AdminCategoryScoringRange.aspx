<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminCategoryScoringRange.aspx.vb" Inherits="Adjudication.AdminCategoryScoringRange" Title="Administrative - Matrix Descriptions"  ValidateRequest="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <style>
        .form-control {
            display: inline-block;
            width: auto;
        }

        td {
            vertical-align: middle;
        }
    </style>
    <div class="TextCenter">
        <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label>
        <asp:TextBox ID="txtPK_ID" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
        <asp:TextBox ID="txtPK_ID_2" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
    </div>
    <div class="panel panel-dark">
        <div class="panel-heading">Matrix Descriptions</div>
        <div class="panel-body">
            <table class="TableSpacing">
                <tr>
                    <td style="width: 25%; text-align: right;">Category Name:
                    </td>
                    <td style="width: 75%; text-align: left;">
                        <asp:DropDownList ID="ddlCategoryID" CssClass="form-control" runat="server"></asp:DropDownList><span style="color: red;">*</span>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">Scoring Range:
                    </td>
                    <td style="width: 75%; text-align: left;">
                        <asp:DropDownList ID="ddlScoringRangeID" CssClass="form-control" runat="server"></asp:DropDownList><span style="color: red;">*</span>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right; vertical-align: top;">Matrix Adjectives:
                    </td>
                    <td style="width: 75%; text-align: left;">
                        <asp:TextBox ID="txtMatrixAdjectives" CssClass="form-control" runat="server" Width="100%" Rows="3" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right; vertical-align: top;">Matrix Range Description:
                    </td>
                    <td style="width: 75%; text-align: left;">
                        <FTB:FreeTextBox ID="ftbMatrixDetail" runat="Server" Width="100%" GutterBackColor="whitesmoke" ToolbarLayout="ParagraphMenu,FontSizesMenu,FontForeColorsMenu,FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker|Bold,Italic,Underline,Strikethrough,Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage|Cut,Copy,Paste,Delete;Undo,Redo,Print,Save|SymbolsMenu,StylesMenu,InsertHtmlMenu|InsertRule,InsertDate,InsertTime|InsertTable,EditTable;InsertTableRowAfter,InsertTableRowBefore,DeleteTableRow;InsertTableColumnAfter,InsertTableColumnBefore,DeleteTableColumn|InsertForm,InsertTextBox,InsertTextArea,InsertRadioButton,InsertCheckBox,InsertDropDownList,InsertButton|InsertDiv,EditStyle,InsertImageFromGallery,Preview,SelectAll"
                            OnSaveClick="btnUpdate_Click" Height="200px">
                        </FTB:FreeTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><hr /></td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">Last Updated By:
                    </td>
                    <td style="width: 75%; text-align: left;">
                        <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>
                        on
                        <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div class="TextCenter">
        <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-gold" Text="Save"></asp:Button>
        &nbsp;
		<asp:Button ID="btnCancel" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button></td>
    </div>

</asp:Content>
