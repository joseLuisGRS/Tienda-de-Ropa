using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Management;
using StoreRopa.Models.Vo;
using StoreRopa.Data.Repository.Interfeces;

public class TicketPago
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Font _f8, _f8b, _f10, _f10b, _f14b;
    private readonly StringBuilder _contenidoTicket;
    private PrintDocument _pd;
    private readonly IConfiguration _configuration;
    private readonly string _printerName, _letter = "Colibri";
    private PrinterSettings _printerSettings;
    private int _longPaper, _widthPaper;
    private StringFormat _center;
    private StringFormat _right;
    private float _y = 0;
    public TicketAbonoVO _ticketAbonoVO;
    public TicketPago(IUnitOfWork unitOfWork, IConfiguration configuration, int longPaper, bool isTermic, TicketAbonoVO ticketAbonoVO)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _longPaper = longPaper * 15 + 240;
        _ticketAbonoVO = ticketAbonoVO;
        if (isTermic)
        {
            _widthPaper = int.Parse(_configuration["Configuration:WidthPaperTer"]!.ToString());
        }

        _contenidoTicket = new StringBuilder();
        _pd = new PrintDocument();
        _printerSettings = new PrinterSettings();
        _f8 = new Font(_letter, 8, FontStyle.Regular);
        _f8b = new Font(_letter, 8, FontStyle.Bold);
        _f10 = new Font(_letter, 10, FontStyle.Regular);
        _f10b = new Font(_letter, 10, FontStyle.Bold);
        _f14b = new Font(_letter, 14, FontStyle.Bold);
        _center = new StringFormat { Alignment = StringAlignment.Center };
        _right = new StringFormat { Alignment = StringAlignment.Far };
        _printerName = _configuration["Configuration:PrinterName"]!.ToString();
    }

    public void ImprimirTicket()
    {

        _printerSettings.DefaultPageSettings.PaperSize = new PaperSize("termico", _widthPaper, _longPaper);
        _pd.PrinterSettings = _printerSettings;
        if (!IsPowerPrinter())
        {
            _pd.BeginPrint += new PrintEventHandler(pd_BegingPrint);
            _pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            _pd.Print();
        }
        else
        {
            _pd.PrinterSettings.PrinterName = _printerName;
            _pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            _pd.Print();
        }
    }

    private bool IsPowerPrinter()
    {
        try
        {
            using (var searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Printer WHERE Name = '{_printerName}'"))
            {
                foreach (ManagementObject printer in searcher.Get())
                {
                    var workOffline = (bool)printer["WorkOffline"];
                    if (!workOffline)
                    {
                        return true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        return false;
    }

    private void pd_BegingPrint(object sender, PrintEventArgs e)
    {
        _pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
        _pd.PrinterSettings.PrintToFile = true;
        _pd.PrinterSettings.PrintFileName = "ticketPago.pdf";
    }
    private void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
        int leftMargin = _pd.DefaultPageSettings.Margins.Left = 5;
        int centerMargin = _pd.DefaultPageSettings.PaperSize.Width / 2;
        int rightMargin = _pd.DefaultPageSettings.PaperSize.Width;
        Graphics g = ev.Graphics;
        _y = 42;

        // Encabezado
        string logo = _configuration["Configuration:Logo"]!.ToString();
        Image logoImage = Image.FromFile(logo);
        ev.Graphics.DrawImage(logoImage, ((ev.PageBounds.Width - 150) / 2), 5, 150, 35);

        string storeName = _configuration["Configuration:StoreName"]!.ToString();
        DibujarTexto(g, storeName, _f10b, centerMargin, _y, storeName.Length, _center);
        string storeDirection = _configuration["Configuration:StoreDirection"]!.ToString();
        DibujarTexto(g, storeDirection, _f10, centerMargin, addSizeFont(_f10), storeDirection.Length ,_center);
        string storePhone = _configuration["Configuration:StorePhone"]!.ToString();
        DibujarTexto(g, storePhone, _f10, centerMargin, addSizeFont(_f10), _center);
        DibujarTexto(g, "Fecha: " + _ticketAbonoVO.FechaAbono.ToShortDateString() + " hora: " 
            + _ticketAbonoVO.FechaAbono.ToShortTimeString(), _f8, rightMargin, addSizeFont(_f8), _right);
        
        var empleado = _unitOfWork.EmpleadosRepository.GetEmpleadoPersonaById(long.Parse(_ticketAbonoVO.UsuarioAlta));
        DibujarTexto(g, "Vendedor: " + Int64.Parse(_ticketAbonoVO.UsuarioAlta).ToString("D5"), _f8, leftMargin, addSizeFont(_f8));
        string vendedor = empleado.Result.Persona.Nombres + " " + empleado.Result.Persona.ApPaterno + " " + empleado.Result.Persona.ApMaterno;
        DibujarTexto(g, vendedor, _f8, leftMargin, addSizeFont(_f8), vendedor.Length);
        var clienteVenta = _unitOfWork.ClientesRepository.GetClientePersonaById(_ticketAbonoVO.ClienteId);

        DibujarTexto(g, "Cliente:  " + _ticketAbonoVO.ClienteId.ToString("D9"), _f8, leftMargin, addSizeFont(_f8));
        string cliente = clienteVenta.Result.Persona.Nombres + " " + clienteVenta.Result.Persona.ApPaterno + " " + 
            clienteVenta.Result.Persona.ApMaterno;
        DibujarTexto(g, cliente, _f8, leftMargin, addSizeFont(_f8), cliente.Length);

        DibujarTexto(g, "", _f8, leftMargin, addSizeFont(_f8));
        DibujarTexto(g, "Articulo", _f8b, leftMargin, addSizeFont(_f8b));
        DibujarTexto(g, "Abono", _f8b, rightMargin, _y, _right);

        foreach(DetalleAbonoVO detAbono in _ticketAbonoVO.DetalleAbonos!)
        {
            DibujarTextoArticulo(g, detAbono.Articulo + " Venta no. " + detAbono.VentaId.ToString("D9"), _f8, leftMargin, addSizeFont(_f8));
            DibujarTexto(g, detAbono.Abono.ToString("C2"), _f8b, rightMargin, _y, _right);
        }
        DibujarTexto(g, "", _f8, leftMargin, addSizeFont(_f8));
        DibujarTexto(g, "A cuenta", _f8, leftMargin, addSizeFont(_f8));
        DibujarTexto(g, _ticketAbonoVO.TotalAbono.ToString("C2"), _f8, rightMargin, _y, _right);

        DibujarTexto(g, "", _f8, leftMargin, addSizeFont(_f8));
        DibujarTexto(g, "Nuevo saldo", _f8, leftMargin, addSizeFont(_f8));
        DibujarTexto(g, (_ticketAbonoVO.SaldoActual - _ticketAbonoVO.TotalAbono).ToString("C2"), _f8, rightMargin, _y, _right);

        DibujarTexto(g, "", _f8, leftMargin, addSizeFont(_f8));
        DibujarTexto(g, "Efectivo", _f8, leftMargin, addSizeFont(_f8));
        DibujarTexto(g, _ticketAbonoVO.Efectivo.ToString("C2"), _f8, rightMargin, _y, _right);
        DibujarTexto(g, "Cambio", _f8, leftMargin, addSizeFont(_f8));
        DibujarTexto(g, (_ticketAbonoVO.Efectivo - _ticketAbonoVO.TotalAbono).ToString("C2"), _f8, rightMargin, _y, _right);
        // Pie de página
        DibujarTexto(g, "¡Gracias por su pago!", _f8b, leftMargin, addSizeFont(_f8b));
       
        // Guardar contenido del ticket
        _contenidoTicket.AppendLine(ObtenerContenidoTicket());
    }

    private float addSizeFont(Font font) 
    {
        _y += font.Height;
        return _y;
    }
    private void DibujarTexto(Graphics g, string texto, Font fuente, float x, float y, StringFormat alineacion = null)
    {
        if (alineacion == null)
        {
            alineacion = new StringFormat();
            alineacion.Alignment = StringAlignment.Near;
        }
        StringFormat sf = new StringFormat { Alignment = alineacion.Alignment };
        
        g.DrawString(texto, fuente, Brushes.Black, x,y, sf);
        
        _contenidoTicket.AppendLine(texto);
    }

    private void DibujarTexto(Graphics g, string texto, Font fuente, float x, float y, int lengthText, StringFormat alineacion = null)
    {
        int charMaxRow = 36;
        if (alineacion == null)
        {
            alineacion = new StringFormat();
            alineacion.Alignment = StringAlignment.Near;
        }
        StringFormat sf = new StringFormat { Alignment = alineacion.Alignment };
        if (lengthText <= charMaxRow)
        {
            g.DrawString(texto, fuente, Brushes.Black, x, y, sf);
        }
        else 
        {
            int rango = 0;
            int inicio = 0;
            int fin = charMaxRow;
            int count = 0;
            while (rango < lengthText) 
            {
                count++;
                g.DrawString(texto.Substring(inicio, fin), fuente, Brushes.Black, x, y, sf);
                y += fuente.Height;
                int nextValue = fin + charMaxRow;
                if (nextValue <= lengthText) 
                {
                    if (count == 1)
                    {
                        inicio = charMaxRow;
                    }
                    else
                    {
                        inicio = charMaxRow * count;
                    }
                    if((inicio + charMaxRow) <= lengthText) fin = charMaxRow;
                    else fin = lengthText - (charMaxRow * count);
                    rango = charMaxRow * count;
                }
                else
                {
                    if(count == 1)
                    {
                        inicio = charMaxRow;                        
                    }
                    else 
                    { 
                        inicio = charMaxRow * (count - 1);
                    }
                    fin = lengthText - charMaxRow;
                    rango = charMaxRow * count ;
                }
            }
            _y = y - fuente.Height;
        }      

        _contenidoTicket.AppendLine(texto);
    }

    private void DibujarTextoArticulo(Graphics g, string texto, Font fuente, float x, float y)
    {
        int charMaxRow = 42;
        StringFormat sf = new StringFormat { Alignment = StringAlignment.Near };
        if (texto.Length <= charMaxRow)
        {
            int count = 0;
            bool isModifiedWord = false;
            while (count < 2)
            {
                SizeF tamañoTexto = g.MeasureString(texto, fuente);
                int chars = 0;
                string texto2 = null;
                while (tamañoTexto.Width > _pd.DefaultPageSettings.PaperSize.Width)
                {
                    texto2 = texto.Substring(0, charMaxRow - chars++);
                    tamañoTexto = g.MeasureString(texto2, fuente);
                    isModifiedWord = true;
                }
                if (texto2 != null)
                {
                    g.DrawString(texto2, fuente, Brushes.Black, x, y, sf);
                    texto = texto.Substring(texto2.Length, (texto.Length - texto2.Length));
                    y = addSizeFont(fuente);
                }
                else
                {
                    g.DrawString(texto, fuente, Brushes.Black, x, y, sf);
                }
                count = !isModifiedWord ? 2 : count +=1 ;
            }
        }
        else
        {
            int count = 0;
            bool isModifiedWord = false;
            while (count < 2)
            {
                SizeF tamañoTexto = g.MeasureString(texto, fuente);
                int chars = 0;
                string texto2 = null;
                while (tamañoTexto.Width > _pd.DefaultPageSettings.PaperSize.Width)
                {
                    texto2 = texto.Substring(0, charMaxRow - chars++);
                    tamañoTexto = g.MeasureString(texto2, fuente);
                    isModifiedWord = true ;
                }
                if (texto2 != null)
                {
                    g.DrawString(texto2, fuente, Brushes.Black, x, y, sf);
                    texto = texto.Substring(texto2.Length, (texto.Length - texto2.Length));
                    y = addSizeFont(fuente);
                }
                else
                {
                    g.DrawString(texto, fuente, Brushes.Black, x, y, sf);
                }
                count = !isModifiedWord ? 2 : count +=1;
            }
 
        }
        _contenidoTicket.AppendLine(texto);
    }

    public string ObtenerContenidoTicket()
    {
        return _contenidoTicket.ToString();
    }

}