<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientLogin.aspx.cs" Inherits="ASP.NET_Web_Application__.NET_Framework_.PatientLogin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Patient Login</title>

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .container {
            padding-top: 50px;
        }
        .card-header {
            background-color: #007bff;
        }
        h3 {
            color:white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card login-card">
                        <div class="card-header text-center">
                            <h3>Patient Login</h3>
                        </div>
                        <div class="card-body">
                            <div class="form-group mb-3">
                                <asp:Label ID="lblEmail" runat="server" Text="Email:" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="example@domain.com"></asp:TextBox>
                            </div>
                            <div class="form-group mb-4">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
                            </div>
                            <div class="text-center">
                                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger fw-bold"></asp:Label>
                            </div>
                            <%-- Optional Registration Link
                            <div class="text-center mt-3">
                                <p>Don't have an account? <a href="PatientRegistration.aspx">Register here</a></p>
                            </div>
                            --%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS (optional for interactive components) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
