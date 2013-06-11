<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RemoveConfirm.aspx.cs" Inherits="Prototype.WebFormsApp.PersonalData.RemoveConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Please confirm if you want to remove the person.
    </p>
    <asp:Button runat="server" ID="RemoveConfirmButton" OnClick="OnRemoveConfirmButtonClick" Text="Confirm Remove" />
    <asp:HyperLink runat="server" ID="CancelRemoveButton" NavigateUrl="~/PersonalData/ListPage.aspx">Cancel</asp:HyperLink>
</asp:Content>
