<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="Prototype.WebFormsApp.PersonalData.EditPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True"></asp:ScriptManager>

    <asp:FormView runat="server" ID="form"
        DataSourceID="PersonService"
        OnDataBound="OnFormViewDataBound" 
        OnInit="OnFormViewInitialize" 
        OnItemInserting="OnItemInsert"
        OnItemInserted="OnItemInserted" 
        OnItemUpdated="OnItemUpdated"
        DataKeyNames="UniqueId">
        <EditItemTemplate>
            <p>
                <asp:Label ID="Label1" runat="server" Text="Unique ID:" AssociatedControlID="UniqueIdTextBox"/>
                <asp:TextBox runat="server" ID="UniqueIdTextBox" ReadOnly="True" Text='<%#Bind("UniqueId") %>'/>
            </p>
    
            <p>
                <asp:Label ID="Label2" runat="server" Text="Full Name:" AssociatedControlID="FullNameTextBox"/>
                <asp:TextBox runat="server" ID="FullNameTextBox" ValidationGroup="Person" Text='<%#Bind("FullName") %>'/>
        
                <dav:DataAnnotationsValidator ID="davFullName" runat="server" 
                      ValidationGroup="Person"
                      MetadataSourceID="msPerson"
                      ControlToValidate="FullNameTextBox"
                      ObjectProperty="FullName" />
            </p>
    
            <p>
                <asp:Label ID="Label3" runat="server" Text="House Number:" AssociatedControlID="HouseNumberTextBox"/>
                <asp:TextBox runat="server" ID="HouseNumberTextBox" ValidationGroup="Person" Text='<%#Bind("HouseNumber") %>' />
        
                <dav:DataAnnotationsValidator ID="davHouseNumber" runat="server" 
                      ValidationGroup="Person"
                      MetadataSourceID="msPerson"
                      ControlToValidate="HouseNumberTextBox"
                      ObjectProperty="HouseNumber" />
            </p>
    
            <p>
                <asp:Label ID="Label4" runat="server" Text="Birth Date:" AssociatedControlID="BirthDateTextBox" />
                <asp:TextBox runat="server" ID="BirthDateTextBox" Text='<%#Bind("BirthDate") %>'/>
        
                <ajaxToolkit:CalendarExtender runat="server"
                    ID="BirthDateTextBoxCalendar"
                    TargetControlID="BirthDateTextBox"
                    />

            </p>
    
            <p>
                <asp:Label ID="Label5" runat="server" Text="Registration Date:" AssociatedControlID="RegistrationDateTextBox"/>
                <asp:TextBox runat="server" ID="RegistrationDateTextBox" ReadOnly="True" Text='<%#Eval("RegistrationDate") %>' />
            </p>
    
            <p>
                <asp:Button ID="SaveButton" runat="server" Text="Save" CommandName="Update"/>
                <a runat="server" id="CancelEditLink" href="~/PersonalData/ListPage.aspx">Cancel</a>
            </p>
        </EditItemTemplate>
    </asp:FormView>
    
    <asp:ObjectDataSource runat="server"
        ID="PersonService"
        OnObjectCreating="OnServiceCreating"
        TypeName="Prototype.WebFormsApp.Services.PersonService" 
        SelectMethod="GetPerson"
        InsertMethod="InsertPerson"
        UpdateMethod="UpdatePerson">
        <SelectParameters>
            <asp:Parameter Name="id" Type="Int64" />
        </SelectParameters>

        <%--<InsertParameters>
            <asp:Parameter Name="uniqueId" Type="Int64"/>
            <asp:Parameter Name="fullNamWe" Type="String"/>
            <asp:Parameter Name="houseNumber" Type="Int32"/>
            <asp:Parameter Name="birthDate" Type="DateTime"/>
        </InsertParameters>--%>
        
       <%-- <UpdateParameters>
            <asp:Parameter Name="uniqueId" Type="Int64"/>
            <asp:Parameter Name="fullName" Type="String"/>
            <asp:Parameter Name="houseNumber" Type="Int32"/>
            <asp:Parameter Name="birthDate" Type="DateTime"/>
        </UpdateParameters>--%>
    </asp:ObjectDataSource>
    
    <dav:MetadataSource runat="server" ID="msPerson" ObjectType="<%$ Code: typeof(Prototype.Common.Person) %>" />
</asp:Content>
