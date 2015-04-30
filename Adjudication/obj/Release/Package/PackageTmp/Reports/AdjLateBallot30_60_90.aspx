<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdjLateBallot30_60_90.aspx.vb" Inherits="Adjudication.AdjLateBallot30_60_90" Title="Late Ballots" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="TextCenter">
        <asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Late Adjudicator Ballots: 0</asp:Label>
        <asp:TextBox ID="txtSortOrder" runat="server" Width="64px" BorderStyle="Dotted" Visible="False">
        </asp:TextBox><asp:TextBox ID="txtSortColumnName" runat="server" Width="64px" BorderStyle="Dotted" Visible="False">LastPerformanceDateTime</asp:TextBox>
    </div>
    <asp:DataGrid ID="gridMain" runat="server" Width="100%" BorderStyle="None" BorderColor="Gainsboro" GridLines="Horizontal" AllowSorting="True" BorderWidth="1px" DataKeyField="PK_ScoringID" AutoGenerateColumns="False" CellPadding="4" HorizontalAlign="Left">
        <AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
        <ItemStyle BackColor="WhiteSmoke"></ItemStyle>
        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn SortExpression="FullName" HeaderText="Assigned Adjudicator">
                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                <ItemStyle HorizontalAlign="left" Width="250"></ItemStyle>
                <ItemTemplate>
                    <b>
                        <%#DataBinder.Eval(Container.DataItem, "FullName")%>
                    </b>[<%#IIf(DataBinder.Eval(Container.DataItem, "FK_ScoringStatusID") <= 2, "<font color=#00cc66>" & DataBinder.Eval(Container.DataItem, "UserStatus") & "</font>", "<font color=red>" & DataBinder.Eval(Container.DataItem, "UserStatus") & "</font>")%>]
                                <br />
                    <span class="small">&nbsp;&nbsp;&nbsp;<%# DataBinder.Eval(Container.DataItem, "CompanyName_User") %></span>
                    <br />
                    Receive Emails:<b><%#IIf(DataBinder.Eval(Container.DataItem, "Active") = 1, "Yes", "No")%></b>
                    <br />
                    <span class="LabelSmall">Access: <font color="DarkGreen"><%#DataBinder.Eval(Container.DataItem, "AccessLevelName")%></span></span>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="ScoringStatus" HeaderText="Adjudication Status">
                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                <ItemStyle HorizontalAlign="left" CssClass="small" Width="120"></ItemStyle>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "ScoringStatus") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="EmailPrimary" HeaderText="Email Addresses">
                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                <ItemStyle HorizontalAlign="left"></ItemStyle>
                <ItemTemplate>
                    <a class="fontDataHyperlink" href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>'>
                        <%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>
                    </a>
                    <br />
                    <a class="fontDataHyperlink" href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailSecondary") %>'>
                        <%# DataBinder.Eval(Container.DataItem, "EmailSecondary") %>
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="PhonePrimary" HeaderText="Phone #'s">
                <HeaderStyle HorizontalAlign="left" Width="100px"></HeaderStyle>
                <ItemStyle HorizontalAlign="left" Width="100px"></ItemStyle>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "PhonePrimary") %>
                    <br />
                    <%# DataBinder.Eval(Container.DataItem, "PhoneSecondary") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Title" HeaderText="Assigned Production">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="250"></ItemStyle>
                <ItemTemplate>
                    <b>
                        <%# DataBinder.Eval(Container.DataItem, "Title") %>
                    </b>
                    <br />
                    <%# DataBinder.Eval(Container.DataItem, "CompanyName") %>                   
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Close Date">
                <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                    <br />
                    thru<br />
                    <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="ScoreDueDate" HeaderText="Due Date">
                <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="80px" Font-Bold="True"></ItemStyle>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "ScoreDueDate","{0:MM/dd/yy}") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Late30to60Days" HeaderText="30-60<br />Days">
                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                <ItemTemplate>
                    <%# IIF(DataBinder.Eval(Container.DataItem, "Late30to60Days") =1,"Yes","") %>
                    <br />
                    <%# IIF(DataBinder.Eval(Container.DataItem, "Late30to60Days") =1,"Late","") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Late60to90Days" HeaderText="60-90<br />Days">
                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="60px" ForeColor="red"></ItemStyle>
                <ItemTemplate>
                    <%# IIF(DataBinder.Eval(Container.DataItem, "Late60to90Days") =1,"Yes","") %>
                    <br />
                    <%# IIF(DataBinder.Eval(Container.DataItem, "Late60to90Days") =1,"<i>Suspend</i>","") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Late90DaysOrMore" HeaderText=">90<br />Days">
                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="60px" ForeColor="red"></ItemStyle>
                <ItemTemplate>
                    <%# IIF(DataBinder.Eval(Container.DataItem, "Late90DaysOrMore") =1,"<b>Yes</b>","") %>
                    <br />
                    <%# IIF(DataBinder.Eval(Container.DataItem, "Late90DaysOrMore") =1,"<b>Expel</b>","") %>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>
