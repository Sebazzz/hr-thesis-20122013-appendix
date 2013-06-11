<%@ Page Title="<%$ Resources:PageTitles,ListPageTitle%>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListPage.aspx.cs" Inherits="Prototype.WebFormsApp.PersonalData.ListPage" %>
<%@ Import Namespace="Prototype.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function openEditDialog(entityId) {
            var targetPage = '<%=ResolveUrl("~/PersonalData/EditPage.aspx") %>?inDialog=true&id=' + entityId;

            $("#dialog").dialog({
                dialogClass: "no-close",
                modal: true,
                autoOpen: true,
                height: 600,
                width:450,
                close: function (event, ui) {
                    document.location.href = document.location.href;
                }
            });

            $("#dialogTarget", $("#dialog")).attr("src", targetPage);
        }
        
        function cbEditFinished(canceled) {
            if (!canceled) {
                document.location.href = document.location.href;

                $("#dialog").dialog("close");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
         <%=this.Page.Title %>
    </h2>

    <div>
        
        <asp:GridView 
            runat="server" 
            ID="PersonList"
            AutoGenerateColumns="False"
            DataSourceID="PersonSource"
            AllowSorting="True"
            ShowFooter="True"

            >
            <Columns>
                <asp:TemplateField HeaderText=" ">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" 
                                   ID="RemoveButton" 
                                   Text="(Remove)" 
                                   meta:resourcekey="RemoveButton" 
                                   NavigateUrl='<%#"~/PersonalData/RemoveConfirm.aspx?id=" + DataBinder.Eval(Container.DataItem, "UniqueId") %>'/>
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ">
                    <ItemTemplate>
                        <a runat="server" 
                           id="EditButton" 
                           href="#" 
                           onclick='<%#"openEditDialog(" + DataBinder.Eval(Container.DataItem, "UniqueId") + ")" %>'>
                             <asp:Literal runat="server" Text="<%$ Resources:EditButton.Text%>" />
                        </a>
                    </ItemTemplate>
                     <FooterTemplate>
                        &nbsp;
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="UniqueId" HeaderText="<%$ Resources:PersonList.IDHeader %>">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "UniqueId")%>
                    </ItemTemplate>
                     <FooterTemplate>
                        <%#PersonSource.Select().OfType<Person>().Count() %> items
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="FullName" HeaderText="<%$ Resources:PersonList.FullNameHeader %>">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "FullName")%>
                    </ItemTemplate>
                     <FooterTemplate>
                        <asp:HyperLink runat="server" ID="AddNewPersonLink" NavigateUrl="EditPage.aspx">New Person</asp:HyperLink>
                    </FooterTemplate>
                </asp:TemplateField>

            </Columns>
            <EmptyDataTemplate>
                <p>
                    No items found. Add a <asp:HyperLink runat="server" ID="AddNewPersonLink" NavigateUrl="EditPage.aspx">new person</asp:HyperLink>.
                </p>
            </EmptyDataTemplate>
            
        </asp:GridView>    
      
    </div>
    
    <div id="dialog" style="display: none" title="Edit">
        <iframe id="dialogTarget"></iframe>
    </div>
    
    <asp:ObjectDataSource runat="server" ID="PersonSource"
        DataObjectTypeName="Prototype.Common.Person"
        TypeName="Prototype.Common.PersonRepository"
        SelectMethod="GetAll"
        SortParameterName="sortBy"
        
        OnObjectCreating="OnCreateDataSourceObject"/>
</asp:Content>


