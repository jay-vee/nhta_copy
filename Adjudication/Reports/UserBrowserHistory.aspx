<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserBrowserHistory.aspx.vb" Inherits="Adjudication.UserBrowserHistory" %>
<!DOCTYPE html>
<HTML>
	<HEAD>
		<title>User Browser History</title>
		
		
		
		
		<LINK href="../Styles.css" type="text/css" rel="STYLESHEET" />
	</HEAD>
	<body >
		<form class="container" id="Form1" method="post" runat="server">
			<table   id="tblTitle" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td class="ReportHeader1000" align="center" colSpan="3" height="30">NHTA User 
						Contact List
					</td>
				</tr>
				<tr>
					<td colSpan="3"><IMG src="../Images/NHTAGradientHorizontal1000.jpg" /></td>
				</tr>
				<tr>
					<td colSpan="3">
						<table   id="UserInfo" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td align="center" colSpan="4" bgcolor="PaleGoldenrod">User Account Information
								</td>
							</TR>
							<TR>
								<TD align="right" width="200">Login ID:</TD>
								<TD align="left"><asp:label id="lblUserLoginID" runat="server" BorderStyle="None" BorderColor="White"  
										BackColor="White" Font-Bold="True"></asp:label>
									&nbsp;<asp:label id="lblActive" runat="server" BorderStyle="None" BorderColor="White"  ></asp:label></TD>
								<TD align="right">
									Access Level:</TD>
								<TD align="left">
									<asp:dropdownlist id="ddlFK_AccessLevelID" runat="server"   Enabled="False"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD align="right">Name:</TD>
								<TD align="left"><asp:label id="lblFirstName" Width="300px"   Runat="server" Font-Bold="True"></asp:label><span style="color: red;"></span></TD>
								<TD align="right">Affiliated Theater Company:</TD>
								<TD align="left">
									<asp:label id="lblCompanyName"   Font-Bold="True" Runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD align="right">Primary Contact Phone #:</TD>
								<TD align="left">
									<asp:label id="lblPhonePrimary"   Runat="server" Width="120px" Font-Bold="True"></asp:label><span style="color: red;"></span></TD>
								<TD align="right">Secondary Contact Phone #:</TD>
								<TD align="left">
									<asp:label id="lblPhoneSecondary"   Runat="server" Width="120px" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD align="right">Address:</TD>
								<TD align="left">
									<asp:label id="lblAddress"   Runat="server" Width="300px" Font-Bold="True"></asp:label>
								<TD align="right">City:</TD>
								<TD align="left">
									<asp:label id="lblCity"   Runat="server" Width="120px" Font-Bold="True"></asp:label>&nbsp;State:
									<asp:label id="lblState"   Runat="server" Width="24px" Font-Bold="True">NH</asp:label>&nbsp; 
									ZIP:
									<asp:label id="lblZIP"   Runat="server" Width="80px" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD align="right">Primary Email Address:</TD>
								<TD align="left">
									<asp:label id="lblEmailPrimary"   Runat="server" Width="300px" Font-Bold="True"></asp:label><span style="color: red;"></span></TD>
								<TD align="right">Secondary Email Address:</TD>
								<TD align="left">
									<asp:label id="lblEmailSecondary"   Runat="server" Width="300px" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD align="right">
									Last Attended Training:</TD>
								<TD align="left">
									<asp:label id="lblLastTrainingDate" runat="server" Font-Bold="True"  ></asp:label><span class="FontSmaller"></span></TD>
								<TD vAlign="top" align="right">Account Notes:</TD>
								<TD align="left">
									<asp:label id="lblUserInformation"   Runat="server" Font-Bold="True"></asp:label></TD>
							</TR>
						</table>
					</td>
				</tr>
				<tr>
					<td colSpan="3" height="20"><IMG src="../Images/NHTAGradientHorizontal1000.jpg" /></td>
				</tr>
				<TR>
					<td align="center" colSpan="3" bgcolor="PaleGoldenrod">Browser History
					</td>
				</TR>
				<tr>
					<td colspan="3">
						<asp:datagrid id="gridMain" DataKeyField="PK_BrowserDetectID" runat="server"   HorizontalAlign="Left"
							CellPadding="2" AutoGenerateColumns="False" BorderWidth="1px" AllowSorting="True" GridLines="Horizontal"
							BorderColor="Gainsboro" BorderStyle="None" width="100%">
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<ItemStyle BackColor="WhiteSmoke"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="Black" BorderColor="Black" BackColor="White"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Type" SortExpression="Type" HeaderText="Type" HeaderStyle-HorizontalAlign=Center >
									<ItemStyle HorizontalAlign="Center" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Platform" SortExpression="Platform" HeaderText="Platform" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Version" SortExpression="Version" HeaderText="Version" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="MajorVersion" SortExpression="MajorVersion" HeaderText="Major<br />Version" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="MinorVersion" SortExpression="MinorVersion" HeaderText="Minor<br />Version" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IsBeta" SortExpression="IsBeta" HeaderText="Is <br />Beta" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IsCrawler" SortExpression="IsCrawler" HeaderText="Is <br />Crawler" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IsAOL" SortExpression="IsAOL" HeaderText="Is <br />AOL" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IsWin16" SortExpression="IsWin16" HeaderText="Is <br />Win16" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IsWin32" SortExpression="IsWin32" HeaderText="Is <br />Win32" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SupportsFrames" SortExpression="SupportsFrames" HeaderText="Supports<br />Frames" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SupportsTables" SortExpression="SupportsTables" HeaderText="Supports<br />Tables" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SupportsVBScript" SortExpression="SupportsVBScript" HeaderText="Supports<br />VB<br />Script" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SupportsJavaScript" SortExpression="SupportsJavaScript" HeaderText="Supports<br />Java<br />Script" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SupportsJavaApplets" SortExpression="SupportsJavaApplets" HeaderText="Supports<br />Java<br />Applets" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDF" SortExpression="CDF" HeaderText="CDF" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="CreateByDate" HeaderText="Login Date" HeaderStyle-HorizontalAlign=Center>
									<ItemStyle HorizontalAlign="Center" Width="70px"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "CreateByDate","{0:MM/dd/yy}") %>
									</ItemTemplate>
								</asp:TemplateColumn>							
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td colSpan="3"><IMG src="../Images/NHTAGradientHorizontal1000.jpg" /></td>
				</tr>
				<tr>
					<td><asp:label id="lblSortOrder" runat="server"     BorderStyle="Dotted"
							Visible="False" Width="64px">Users.LastName, Users.FirstName</asp:label>
						<asp:label id="lblSortColumnName" runat="server"   BorderStyle="Dotted" Visible="False"
							Width="64px"></asp:label></td>
					<td align="right"  >
						Logged in as
						<asp:label id="lblLoginID" runat="server"  ></asp:label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
