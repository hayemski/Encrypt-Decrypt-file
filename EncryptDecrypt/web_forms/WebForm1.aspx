<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="EncryptDecrypt.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enkripcija i dekripcija</title>

    <style>
        .m-centar { margin: 0 auto; }
        .w1000 { width: 1000px; }
        h1 { font-size: 1.5em; font-weight: bold; }
        .row { width: 100%; position: relative; /* width: 45em; */ }
            .row:after { clear: both; }

        .button1 { display: inline-block; padding: 0.35em 1.2em; border: 0.1em solid #000000; margin: 0 0.3em 0.3em 0; border-radius: 0.12em; box-sizing: border-box; text-decoration: none; font-family: 'Roboto',sans-serif; font-weight: 300; color: #000000; text-align: center; transition: all 0.2s; }
            .button1:hover { color: #FFFFFF; background-color: #000000; }
        #kljuc { height: 26px; }
        #btnFileupload { position: absolute; z-index: 10000000000; left: 48em; top: -1px; }
    </style>

    <link rel="stylesheet" type="text/css" href="..\css\reset.css" />
    <link rel="stylesheet" type="text/css" href="..\css\index.css" />
    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@100;300;400&display=swap" rel="stylesheet">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

</head>
<body>
    <form id="form1" class="m-centar w1000 centar pad-top-2 " runat="server">
        <h1>Enkripcija i dekripcija (AES/Rijndael)
            <br />
            Tehnike zaštite računarskih mreža</h1>
        <div class="break">

            <div class="pad-top-3">
                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem Text="AES" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Triple DES (3DES)" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="pad-top-3">
                <div class="fileupload-group ">
                    <button type="button" runat="server" id="btnFileupload" class="btnFileupload  button1">
                        Odabir fajla <i class="fa fa-folder-open"></i>
                        <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="True" />
                    </button>
                    <div>
                        <input type="text" runat="server" id="txtFileuploadName" class="fileupload-name bordura" readonly="readonly" />
                    </div>
                </div>
            </div>

            <div class="row pad-top-2">
                <label>Unesite vrednost ključa: </label>
                <input id="kljuc" name="kljuc" type="Text" runat="server" />
            </div>
            <div class="row pad-top-2 ">
                <asp:Button ID="btnEncrypt" CssClass="button1" Text="Enkriptuj Fajl" runat="server" OnClick="EncryptFile_Click" />
                <asp:Button ID="btnDecrypt" CssClass="button1" Text="Dekriptuj Fajl" runat="server" OnClick="DecryptFile_Click" />
            </div>
        </div>
    </form>

    <script>
        $('input[id="<%= FileUpload1.ClientID %>"]').change(function () {
            var names = [];
            var length = $(this).get(0).files.length;
            for (var i = 0; i < $(this).get(0).files.length; ++i) {
                names.push($(this).get(0).files[i].name);
            }
            if (length > 2) {
                $("#<%=txtFileuploadName.ClientID%>").attr("value", length + " selected files");
            }
            else {
                $("#<%=txtFileuploadName.ClientID%>").attr("value", names);
            }
        });


    </script>
</body>
</html>
