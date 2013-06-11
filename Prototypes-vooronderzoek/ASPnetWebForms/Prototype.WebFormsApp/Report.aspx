<%@ Page Title="<%$ Resources:PageTitles,ReportPageTitle%>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Prototype.WebFormsApp.Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
         <%=this.Page.Title %>
    </h2>
    
    <asp:Repeater runat="server" ID="PersonReportRepeater" DataSourceID="PersonSource">
         <ItemTemplate>
             <table style="min-width:50%;">
                 <tr>
                     <td><%#this.GetDisplayAttributeValue(Container.DataItem, "UniqueId") %></td>
                     <td><%#DataBinder.Eval(Container.DataItem, "UniqueId") %></td>
                 </tr>
                 
                 <tr>
                     <td><%#this.GetDisplayAttributeValue(Container.DataItem, "FullName") %></td>
                     <td><%#DataBinder.Eval(Container.DataItem, "FullName") %></td>
                 </tr>
                 
                 <tr>
                     <td><%#this.GetDisplayAttributeValue(Container.DataItem, "HouseNumber")%></td>
                     <td><%#DataBinder.Eval(Container.DataItem, "HouseNumber") %></td>
                 </tr>
                 
                 <tr>
                     <td><%#this.GetDisplayAttributeValue(Container.DataItem, "BirthDate")%></td>
                     <td><%#DataBinder.Eval(Container.DataItem, "BirthDate") %></td>
                 </tr>
                 
                 <tr>
                     <td><%#this.GetDisplayAttributeValue(Container.DataItem, "RegistrationDate")%></td>
                     <td><%#DataBinder.Eval(Container.DataItem, "RegistrationDate") %></td>
                 </tr>
             </table>
         </ItemTemplate>
         <SeparatorTemplate>
             <br />
         </SeparatorTemplate>
    </asp:Repeater>

      <asp:ObjectDataSource runat="server" ID="PersonSource"
        DataObjectTypeName="Prototype.Common.Person"
        TypeName="Prototype.Common.PersonRepository"
        SelectMethod="GetAll"
        SortParameterName="sortBy"
        
        OnObjectCreating="OnCreateDataSourceObject"/>
</asp:Content>
