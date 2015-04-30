Imports Adjudication.DataAccess

Partial Public Class MasterPageNoNav
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private _PageTitleLabel As String = String.Empty


    Public Property PageTitleLabel()
        Get
            Return _PageTitleLabel       'Me.lblPageTitle.Text
        End Get
        Set(ByVal value)
            _PageTitleLabel = value             'Me.lblPageTitle.Text = value
        End Set
    End Property

End Class