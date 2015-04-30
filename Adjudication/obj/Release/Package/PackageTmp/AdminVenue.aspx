<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" Title="Venue Administration" CodeBehind="AdminVenue.aspx.vb" Inherits="Adjudication.AdminVenue" %>

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
    <div>
        <asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False"></asp:Label>
        <asp:Label ID="lblSucessfulUpdate" runat="server" ForeColor="SeaGreen" Visible="False">Update Successful!</asp:Label>
    </div>
    <asp:Panel ID="pnlUserData" runat="server">
        <asp:TextBox ID="txtPK_VenueID" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
        <div class="panel panel-dark">
            <div class="panel-heading">
                Venue Administration
            </div>
            <div class="panel-body">
                <table class="TableSpacing">
                    <tr>
                        <td align="right" width="260">Venue Name</td>
                        <td align="left" width="260">
                            <asp:TextBox ID="txtVenueName" Font-Bold="True" runat="server" Width="65%" CssClass="form-control"></asp:TextBox><span style="color: red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Seating Capacity</td>
                        <td align="left">
                            <asp:TextBox ID="txtSeatingCapacity" runat="server" Width="65%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Venue Phone #</td>
                        <td align="left">
                            <asp:TextBox ID="txtPhone" runat="server" Width="35%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Physical Address</td>
                        <td align="left">
                            <asp:TextBox ID="txtAddress" runat="server" Width="65%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">City</td>
                        <td align="left">
                            <asp:TextBox ID="txtCity" runat="server" Width="35%" CssClass="form-control"></asp:TextBox>&nbsp;
                            State:<asp:TextBox ID="txtState" runat="server" Width="7%" CssClass="form-control">NH</asp:TextBox>&nbsp; 											
                            ZIP:<asp:TextBox ID="txtZIP" runat="server" Width="12%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Venue Email Address</td>
                        <td align="left">
                            <asp:TextBox ID="txtEmailAddress" runat="server" Width="65%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Venue Website</td>
                        <td align="left">
                            <asp:TextBox ID="txtWebsite" runat="server" Width="65%" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="HEIGHT: 12px" align="right" width="260">Handicapped Accessible</td>
                        <td style="HEIGHT: 12px" align="left" width="260">
                            <asp:DropDownList ID="ddlHandicappedAccessible" runat="server" Width="15%" CssClass="form-control">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right" width="260">Air Conditioned</td>
                        <td align="left" width="260">
                            <asp:DropDownList ID="ddlAirConditioned" runat="server" Width="15%" CssClass="form-control">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right" width="260">Outdoor Location</td>
                        <td align="left" width="260">
                            <asp:DropDownList ID="ddlOutdoor" runat="server" Width="15%" CssClass="form-control">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">Directions</td>
                        <td align="left">
                            <asp:TextBox ID="txtDirections" runat="server" Width="65%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">Parking</td>
                        <td align="left">
                            <asp:TextBox ID="txtParking" runat="server" Width="65%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">Venue Comments</td>
                        <td align="left">
                            <asp:TextBox ID="txtComments" runat="server" Width="65%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" width="260">Last Updated By</td>
                        <td align="left" width="260">
                            <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
											<asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                    </tr>
                    </td>
    </tr>
    </TBODY>
                </table>
            </div>
        </div>

        <div class="TextCenter">
            <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-gold" Text="Update "></asp:Button><br />
            Note: Items with a red asterisk (<span style="color: red;">*</span>) indicate Required 
						Fields.
        </div>
    </asp:Panel>
</asp:Content>
