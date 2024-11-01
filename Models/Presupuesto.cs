using System.Text.Json.Serialization;

public class Presupuesto
{
    int idPresupuesto;
    string nombreDestinatario;
    DateTime fechaCreacion;
    List<PresupuestoDetalle> detalle;


    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

    public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion)
    {
        this.idPresupuesto = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
        detalle = new List<PresupuestoDetalle>();
    }

    // public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestoDetalle> detalle)
    // {
    //     this.idPresupuesto = idPresupuesto;
    //     this.nombreDestinatario = nombreDestinatario;
    //     this.fechaCreacion = fechaCreacion;
    //     this.detalle = detalle;
    // }

    public double MontoPresupuesto()
    {
        int monto = detalle.Sum(d => d.Cantidad * d.Producto.Precio);
        return monto;
    }
    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    }
    public int CantidadProductos()
    {
        return detalle.Sum(d => d.Cantidad);
    }


}