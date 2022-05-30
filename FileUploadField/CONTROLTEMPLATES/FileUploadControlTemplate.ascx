<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" %>
<SharePoint:RenderingTemplate ID="FileUploadControlTemplate" runat="server">
    <Template>
        <div class="FileUploadFieldContainer">
            <asp:Panel ID="pnlUpload" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:FileUpload ID="UploadFileControl" runat="server" CssClass="ms-ButtonHeightWidth" Width="250px" /></td>

                        <td>
                            <asp:Button ID="UploadButton" runat="server" CssClass="ms-ButtonHeightWidth" CausesValidation="false" Text="Загрузить" Width="70px" /></td>

                        <td>
                            <asp:Button ID="DeleteButton" runat="server" CssClass="ms-ButtonHeightWidth" CausesValidation="false" Text="Удалить" Width="70px" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="StatusLabel" runat="server" Width="100%" />
            <asp:HiddenField ID="hdnFileName" runat="server" />
        </div>
    </Template>
</SharePoint:RenderingTemplate>
