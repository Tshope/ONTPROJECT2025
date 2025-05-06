<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SendNotifications.aspx.cs" Inherits="ASP.NET_Web_Application__.NET_Framework_.SendNotifications" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <!-- External CSS and JS (relocated from head for this context) -->
        <link href="Content/SendingNotification.css" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" />
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>

        <h2 id="title" class="mb-4">Send Notification</h2>

        <div class="container">
            <h3 class="mt-5">Pending Notifications</h3>
            <hr />

            <asp:UpdatePanel ID="updNotifications" runat="server">
                <ContentTemplate>
                    <div class="table-responsive">
                        <asp:GridView ID="gvNotifications" runat="server"
                                      ClientIDMode="Static"
                                      AutoGenerateColumns="False"
                                      CssClass="table table-bordered table-striped table-hover"
                                      EmptyDataText="No notifications found."
                                      ShowHeaderWhenEmpty="true"
                                      UseAccessibleHeader="true"
                                      GridLines="None">
                            <HeaderStyle CssClass="thead-light" BackColor="#f8f9fa" />
                            <RowStyle CssClass="align-middle" />
                            <AlternatingRowStyle CssClass="table-light" />
                            <Columns>
                                    <asp:BoundField DataField="PatientName" HeaderText="Patient" />
                                <asp:BoundField DataField="NotificationType" HeaderText="Type" />
                                <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                 <asp:BoundField DataField="Doctor" HeaderText="Doctor" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="CreatedAt" HeaderText="Created At" 
                                                DataFormatString="{0:yyyy-MM-dd HH:mm}" ReadOnly="True" />
                           
                                <asp:BoundField DataField="AppointmentDate" HeaderText="Appointment Date" 
                               DataFormatString="{0:yyyy-MM-dd HH:mm}" ReadOnly="True" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

           <asp:UpdatePanel ID="updForm" runat="server">
    <ContentTemplate>
        <div class="row mb-3 mt-4">
            <div class="col-md-4">
                <label for="ddlAppointments" class="form-label">Select Appointment</label>
               <asp:DropDownList ID="ddlAppointments" runat="server" CssClass="form-select" AppendDataBoundItems="true">
    <asp:ListItem Text="-- Select Appointment --" Value="" />
</asp:DropDownList>

            </div>
        </div>

        <!-- Add your Button control here -->
        <asp:Button ID="btnSend" runat="server" Text="Send Notification" OnClick="btnSend_Click" CssClass="btn btn-primary" />

        <asp:Label ID="lblResult" runat="server" CssClass="d-block mt-2 fw-bold" ClientIDMode="Static" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

        </div>
    </main>
</asp:Content>
