<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ViewAssignments.aspx.vb" Inherits="Adjudication.ViewAssignments" Title="View Adjudicator Assignments" %>

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
        <asp:Label ID="lblAdjudicationMessage" runat="server"></asp:Label>
    </div>

    <asp:Panel ID="pnlMyAdjudications" runat="server">
        <div class="panel panel-dark">
            <div class="panel-heading">Theatre Company Adjudication Assignments for<asp:Label ID="lblCompanyName" runat="server"></asp:Label></div>
            <div class="panel-body">
                <asp:DataGrid ID="gridMain" runat="server" HorizontalAlign="Left" CellPadding="2" AutoGenerateColumns="False" DataKeyField="PK_ScoringID" BorderWidth="1px" GridLines="Horizontal" BorderColor="Gainsboro" BorderStyle="None" Width="100%">
                    <HeaderStyle ForeColor="Black" BorderColor="Black" BackColor="LemonChiffon"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="PK_NominationsID" HeaderText="PK_NominationsID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FullName" HeaderText="Adjudicator Name">
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="AdjudicatorToSeeProduction" SortExpression="AdjudicatorToSeeProduction" HeaderText="Confirmed Attendance">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TotalScore" HeaderText="Ballot Sumitted">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Title">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Company">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Production Dates">
                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                                <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="FirstPerformanceDateTime" HeaderText="8"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastPerformanceDateTime" HeaderText="9"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DaysToConfirmAttendance" HeaderText="10"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DaysToWaitForScoring" HeaderText="11"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_CompanyID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="ProductionCategory"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Title"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="LastUpdateByName"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="LastUpdateByDate"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="PK_ScoringID"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Planned" HeaderText="ProductionDateAdjudicated_Planned"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignment" HeaderText="AdjudicatorRequestsReassignment"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FK_VenueID" HeaderText="FK_VenueID"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
