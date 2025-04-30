<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SendNotifications.aspx.cs" Inherits="ASP.NET_Web_Application__.NET_Framework_.SendNotifications" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <link href="Content/SendingNotification.css" rel="stylesheet" type="text/css" />
        
        <!-- DataTables CSS -->
        <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" />

        <!-- jQuery and DataTables JS -->
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>

        <h2 id="title" class="mb-4">Send Notification</h2>

        <div class="container">
            <asp:UpdatePanel ID="updForm" runat="server">
                <ContentTemplate>
                    <div class="row mb-3">
                        <div class="col-md-4">
                            <label for="ddlNotificationType" class="form-label">Notification Type</label>
                            <asp:DropDownList ID="ddlNotificationType" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- Select --" Value="" />
                                <asp:ListItem Text="Email" Value="Email" />
                                <asp:ListItem Text="SMS" Value="SMS" />
                                <asp:ListItem Text="Push" Value="Push" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label for="txtMessage" class="form-label">Message</label>
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" Placeholder="Enter your message here..."></asp:TextBox>
                    </div>

                    <div class="row mb-3">
                        <div class="col-md-4">
                            <label for="ddlAppointments" class="form-label">Select Appointment</label>
                            <asp:DropDownList ID="ddlAppointments" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                <asp:ListItem Text="-- Select Appointment --" Value="" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <asp:Button ID="btnSend" runat="server" Text="Create Notification" CssClass="btn btn-primary" OnClientClick="this.disabled=true;this.value='Sending...';" OnClick="btnSend_Click" UseSubmitBehavior="false" />
                    <asp:Label ID="lblResult" runat="server" CssClass="d-block mt-2 fw-bold" ClientIDMode="Static" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

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
                                      GridLines="None"
                                      HeaderStyle-BackColor="#f8f9fa">
                            <HeaderStyle CssClass="thead-light" />
                            <RowStyle CssClass="align-middle" />
                            <AlternatingRowStyle CssClass="table-light" />
                            <Columns>
                                <asp:BoundField DataField="Recipient" HeaderText="Recipient" />
                                <asp:BoundField DataField="NotificationType" HeaderText="Type" />
                                <asp:BoundField DataField="Message" HeaderText="Message" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="CreatedAt" HeaderText="Created At" DataFormatString="{0:yyyy-MM-dd HH:mm}" ReadOnly="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <script type="text/javascript">
            Sys.Application.add_load(function () {
                var table = $('#gvNotifications');
                var lbl = $('#lblResult');
                var text = lbl.text().trim();
                if (text !== '') {
                    lbl.stop(true, true).fadeIn(); // Cancel previous animations, fade in
                    setTimeout(function () {
                        lbl.fadeOut('slow', function () {
                            lbl.text(''); // Clear the message after fading out
                        });
                    }, 3000);
                }

                // Destroy and reinitialize to avoid double init
                if ($.fn.dataTable.isDataTable(table)) {
                    table.DataTable().destroy();
                }
                    table.DataTable({
                        "paging": true,
                        "searching": true,
                        "ordering": true,
                        "info": true,
                        "lengthMenu": [5, 10, 25, 50],  // How many rows to show
                    });
                
            });
        </script>
    </main>
</asp:Content>
