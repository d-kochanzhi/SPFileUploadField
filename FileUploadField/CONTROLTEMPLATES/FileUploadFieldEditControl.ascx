﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="FileUploadField.FileUploadFieldEditControl" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<wssuc:InputFormSection runat="server" id="UploadControl" Title="Загрузка файлов - настройки">
    <template_inputformcontrols>

<wssuc:InputFormControl runat="server" LabelText="Библиотека документов для хранения файлов">
<Template_Control>
<asp:DropDownList ID="ddlDocLibs" runat="server" CssClass="ms-ButtonheightWidth" Width="250px" />
</Template_Control>
</wssuc:InputFormControl>

<wssuc:InputFormControl runat="server" LabelText="Использовать ИД элемента как папку(Не работает в NewForm)">
<Template_Control>
    <asp:CheckBox ID="chkUseId" runat="server"></asp:CheckBox>
</Template_Control>
</wssuc:InputFormControl>

 <wssuc:InputFormControl runat="server" LabelText="Использовать ElevatedPrivileges">
<Template_Control>
    <asp:CheckBox ID="chkElevatedPrivileges" runat="server"></asp:CheckBox>
</Template_Control>
</wssuc:InputFormControl>

<wssuc:InputFormControl runat="server" LabelText="Переименовывать файлы при загрузки на:">
<Template_Control>
     <asp:TextBox runat="server" ID="txtRename" MaxLength="255"></asp:TextBox>
</Template_Control>
</wssuc:InputFormControl>
</template_inputformcontrols>
</wssuc:InputFormSection>