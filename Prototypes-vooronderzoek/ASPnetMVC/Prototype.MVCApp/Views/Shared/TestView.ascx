<%@ Control Inherits="System.Web.Mvc.ViewPage<Prototype.Common.Person[]>" Language="C#" %>
<h1><%:Html.DisplayNameFor(m => m[0].FullName)%></h1>

<% if(Model.Any()) { %>
<ul>
    <% foreach(var p in Model){%>
    <li><%:p.FullName%></li>
    <%}%>
</ul>
<%}else{%>
    <p>No products available</p>
<%}%>


