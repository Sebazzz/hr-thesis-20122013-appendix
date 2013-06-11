<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="Prototype.WebFormsApp.PersonalData.EditPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True"></asp:ScriptManager>

    <p>
        <asp:Label runat="server" Text="Unique ID:" AssociatedControlID="UniqueIdTextBox"/>
        <asp:TextBox runat="server" ID="UniqueIdTextBox" ReadOnly="True"/>
    </p>
    
    <p>
        <asp:Label runat="server" Text="Full Name:" AssociatedControlID="FullNameTextBox"/>
        <asp:TextBox runat="server" ID="FullNameTextBox" ValidationGroup="Person"/>
        
        <dav:DataAnnotationsValidator ID="davFullName" runat="server" 
              ValidationGroup="Person"
              MetadataSourceID="msPerson"
              ControlToValidate="FullNameTextBox"
              ObjectProperty="FullName" />
    </p>

    <p>
        <asp:Label runat="server" Text="House Number:" AssociatedControlID="HouseNumberTextBox"/>
        <asp:TextBox runat="server" ID="HouseNumberTextBox" ValidationGroup="Person" />
        
        <dav:DataAnnotationsValidator ID="davHouseNumber" runat="server" 
              ValidationGroup="Person"
              MetadataSourceID="msPerson"
              ControlToValidate="HouseNumberTextBox"
              ObjectProperty="HouseNumber" />
    </p>
    
    <p>
        <asp:Label runat="server" Text="Birth Date:" AssociatedControlID="BirthDateTextBox"/>
        <asp:TextBox runat="server" ID="BirthDateTextBox" />
        
        <ajaxToolkit:CalendarExtender runat="server"
            ID="BirthDateTextBoxCalendar"
            TargetControlID="BirthDateTextBox"
            />

    </p>
    
    <p>
        <asp:Label runat="server" Text="Registration Date:" AssociatedControlID="RegistrationDateTextBox"/>
        <asp:TextBox runat="server" ID="RegistrationDateTextBox" ReadOnly="True" />
    </p>
    
    <p>
        <asp:Button runat="server" Text="Save" OnClick="OnSaveEntityButtonClick"/>
        <a runat="server" id="CancelEditLink" href="~/PersonalData/ListPage.aspx">Cancel</a>
    </p>
    
    <dav:MetadataSource runat="server" ID="msPerson" ObjectType="<%$ Code: typeof(Prototype.Common.Person) %>" />
</asp:Content>
