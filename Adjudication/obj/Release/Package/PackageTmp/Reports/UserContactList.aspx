<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserContactList.aspx.vb" Inherits="Adjudication.UserContactList" %>

<!DOCTYPE html>
<html>
<head>
	<title>User Contact List</title>
	<link href="../Styles.css" type="text/css" rel="STYLESHEET" />
</head>
<body>
	<form class="container" id="Form1" method="post" runat="server">
		<table id="tblTitle" width="100%" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td colspan="3" class="ReportHeader1000" align="center" height="30">NHTA User Contact List
				</td>
			</tr>
			<tr>
				<td colspan="3">
					<img src="../Images/NHTAGradientHorizontal1000.jpg" /></td>
			</tr>
			<tr>
				<td colspan="3">
					<asp:DataGrid ID="gridMain" DataKeyField="PK_UserID" runat="server" HorizontalAlign="Left"
						CellPadding="2" AutoGenerateColumns="False" BorderWidth="1px" AllowSorting="True" GridLines="Horizontal"
						BorderColor="Gainsboro" BorderStyle="None" Width="100%">
						<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
						<ItemStyle BackColor="WhiteSmoke"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="Black" BorderColor="Black" BackColor="White"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="UserFullName" SortExpression="UserFullName" HeaderText="User">
								<HeaderStyle HorizontalAlign="left" Width="110px"></HeaderStyle>
								<ItemStyle HorizontalAlign="left" Width="110px"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="FullAddress" SortExpression="FullAddress" HeaderText="Address">
								<HeaderStyle HorizontalAlign="left"></HeaderStyle>
								<ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="PhonePrimary" SortExpression="PhonePrimary" HeaderText="Phone #">
								<HeaderStyle HorizontalAlign="left"></HeaderStyle>
								<ItemStyle HorizontalAlign="left" CssClass="TextSmall" Width="80"></ItemStyle>
							</asp:BoundColumn>
							<asp:TemplateColumn SortExpression="EmailPrimary" HeaderText="Email Address">
								<HeaderStyle HorizontalAlign="left"></HeaderStyle>
								<ItemStyle HorizontalAlign="left"></ItemStyle>
								<ItemTemplate>
									<a class="fontDataHyperlinkSmall" href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>'>
										<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>
									</a>
									<br />
									<a class="fontDataHyperlinkSmall" href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailSecondary") %>'>
										<%# DataBinder.Eval(Container.DataItem, "EmailSecondary") %>
									</a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn SortExpression="LastTrainingDate" HeaderText="Last Training Date">
								<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center" Width="70px" CssClass="TextSmall"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "LastTrainingDate","{0:MM/dd/yy}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn SortExpression="LastLoginTime" HeaderText="Last Login Date">
								<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center" Width="70px" CssClass="TextSmall"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "LastLoginTime","{0:MM/dd/yy}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn SortExpression="Active" HeaderText="Active">
								<HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
								<ItemTemplate>
									<%# IIF(DataBinder.Eval(Container.DataItem, "Active")=1,"Yes","No") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Affiliated Company">
								<HeaderStyle HorizontalAlign="left"></HeaderStyle>
								<ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="AccessLevelName" SortExpression="AccessLevelName" HeaderText="Access Level">
								<HeaderStyle HorizontalAlign="left"></HeaderStyle>
								<ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
							</asp:BoundColumn>
						</Columns>
					</asp:DataGrid>
				</td>
			</tr>
			<tr>
				<td colspan="3">
					<img src="../Images/NHTAGradientHorizontal1000.jpg" /></td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black"> Number of Users: 0</asp:Label></td>
				<td>
					<asp:TextBox ID="txtSortOrder" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">Users.LastName, Users.FirstName</asp:TextBox>
					<asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox></td>
				<td align="right">Logged in as
						<asp:Label ID="lblLoginID" runat="server"></asp:Label>

				</td>
			</tr>
		</table>
	</form>
</body>
</html>
