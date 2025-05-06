<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientDashboard.aspx.cs" Inherits="ASP.NET_Web_Application__.NET_Framework_.PatientDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Patient Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background-color: #f4f6f8;
        }

        .card {
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
        }

        .card-header {
            font-weight: 600;
        }

        h2, h4 {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <!-- Welcome Message -->
            <div class="row mb-4">
                <div class="col-md-12">
                    <h2>Welcome, <asp:Label ID="lblPatientName" runat="server" CssClass="text-primary fw-bold"></asp:Label></h2>
                </div>
            </div>

            <!-- Upcoming Appointments Section -->
            <div class="row mb-4">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            <h4 class="mb-0">Upcoming Appointments</h4>
                        </div>
                        <div class="card-body">
                          
                        </div>
                    </div>
                </div>
            </div>

            <!-- Recent Notifications Section -->
            <div class="row">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header bg-info text-white">
                            <h4 class="mb-0">Recent Notifications</h4>
                        </div>
                        <div class="card-body">
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS Bundle -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
