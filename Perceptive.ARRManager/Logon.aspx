<%@ Page Language="C#" %>
<%@ Import Namespace="System.Web.Security"%>
<%@ Import Namespace="Perceptive.ARR.HelperLibrary" %>

<script runat="server">
  void Logon_Click(object sender, EventArgs e)
  {
      string userId;
      string password;
      userId = UserId.Text;
      password = UserPass.Text;
      
      RestServiceUtility utility = new RestServiceUtility();
      User authenticatedUser = utility.ProcessRequest<User>(new WebRequestInput()
      {
          Method = Constants.MethodPost,
          Uri = RestServiceUtility.AuthenticateUserUrl(),
          RequestData = new User() { UserName = userId, Password = password }
      });

      if (authenticatedUser.Role == UserRole.Unauthorized)
          Msg.Text = "Invalid credentials. Please try again.";
      else
          FormsAuthentication.RedirectFromLoginPage(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{4}{1}{4}{2}{4}{3}", authenticatedUser.UserName, 
              authenticatedUser.Role.ToString(), authenticatedUser.UserId, authenticatedUser.Password, Constants.UserProfileDelimitor), Persist.Checked);
  }
</script>
<html>
<head id="Head1" runat="server">    
  <title>Audit Record Repository Manager - Login</title>
    <base target="_self" />
    <link rel="Shortcut Icon" href="perceptive.png" /> 
    <style type="text/css">
  .Initial
  {
    display: block;
    padding: 4px 18px 4px 18px;
    float: left;
    background: url("../Images/InitialImage.png") no-repeat right top;
    color: Black;
    font-weight: bold;
  }
  .Initial:hover
  {
    color: Orange;
    background: url("../Images/SelectedButton.png") no-repeat right top;
  }
  .Clicked
  {
    float: left;
    display: block;
    background: url("../Images/SelectedButton.png") no-repeat right top;
    padding: 4px 18px 4px 18px;
    color: Black;
    font-weight: bold;
    color: Green;
  }
  </style>   
</head>
<body>
  <form id="form2" runat="server">
    <h3>
      Logon Page</h3>
    <table>
      <tr>
        <td>
          User Id:</td>
        <td>
          <asp:TextBox ID="UserId" runat="server" /></td>
        <td>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
            ControlToValidate="UserId"
            Display="Dynamic" 
            ErrorMessage="Cannot be empty." 
            runat="server" />
        </td>
      </tr>
      <tr>
        <td>
          Password:</td>
        <td>
          <asp:TextBox ID="UserPass" TextMode="Password" 
             runat="server" />
        </td>
        <td>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
            ControlToValidate="UserPass"
            ErrorMessage="Cannot be empty." 
            runat="server" />
        </td>
      </tr>
      <tr>
        <td>
          Remember me?</td>
        <td>
          <asp:CheckBox ID="Persist" runat="server" /></td>
      </tr>
      
    </table>
    <asp:Button ID="Submit1" OnClick="Logon_Click" Text="Log On" CssClass="Initial" runat="server" />
    <p>
      <asp:Label ID="Msg" ForeColor="red" runat="server" />
    </p>
  </form>
</body>
</html>