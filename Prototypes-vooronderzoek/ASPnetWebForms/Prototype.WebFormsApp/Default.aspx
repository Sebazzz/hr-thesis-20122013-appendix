<%@ Page Title="<%$ Resources:PageTitles,HomePageTitle%>" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Prototype.WebFormsApp._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
         <%=this.Page.Title %>
    </h2>
    <p>
         <asp:Localize ID="IntroductionText" runat="server" Text="Intro Text" meta:resourcekey="IntroductionText"/>
    </p>
</asp:Content>
