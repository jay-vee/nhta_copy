<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminScoringRange.aspx.vb" Inherits="Adjudication.AdminScoringRange" Title="Scoring Bands/Ranges" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
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
    <div class="TextCenter">
        <asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False"></asp:Label>
        <asp:Label ID="lblSucessfulUpdate" runat="server" ForeColor="SeaGreen" Visible="False">Update Successful!</asp:Label>
        <asp:TextBox ID="txtPK_ScoringRangeID" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
    </div>

    <div class="panel panel-dark">
        <div class="panel-heading">Scoring Bands/Ranges</div>
        <div class="panel-body">

            <table class="TableSpacing">
                <tbody>
                    <tr>
                        <td style="width: 25%; text-align: right;">Scoring Range Maximum:</td>
                        <td style="width: 75%; text-align: left;">
                            <asp:TextBox ID="txtScoringRangeMax" CssClass="form-control" Width="100px" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 25%; text-align: right;">Scoring Range Minimum:</td>
                        <td style="width: 75%; text-align: left;">
                            <asp:TextBox ID="txtScoringRangeMin" CssClass="form-control" Width="100px" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%; text-align: right;">Last Updated By:</td>
                        <td style="width: 75%; text-align: left;">
                            <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>
                            on
                            <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <div class="TextCenter">
                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-gold" Text="Save"></asp:Button>&nbsp;
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button>
            </div>
        </div>
    </div>
</asp:Content>
